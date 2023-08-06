using System;
using UnityEngine;
using Zenject;

public class DebugMoveController : MonoBehaviour {
    
    [Inject] private GameMapService _gameMapService;

    private PointController[] _points;

    Vector3 tapPosition = Vector3.zero;
    Vector3 finishPosition = Vector3.zero;
    Vector3 startPosition = Vector3.zero;
    private bool inMove = false;

    private void Start() {
        _points = gameObject.GetComponentsInChildren<PointController>();

        if (Input.GetMouseButton(0)) {
            tapPosition = Input.mousePosition;
            Vector3 screenToWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position =  new Vector3(screenToWorldPoint.x, -1f, -1);
            startPosition = transform.position;
            inMove = true;
        }
    }
    
    private void Update() {
        if (Input.GetMouseButtonDown(0) && ClickOnCurrent()) {
            inMove = true;
            tapPosition = Input.mousePosition;
            startPosition = transform.position;
            _gameMapService.Free(_points);
        }

        if (Input.GetMouseButton(0) && inMove) {
            finishPosition = Input.mousePosition;
            LeftMouseDrag();
        }

        if (Input.GetMouseButtonUp(0) && inMove) {
            inMove = false;
            bool isRiver = GetComponent<RiverController>();
            _gameMapService.SetToMap(_points, isRiver ? MapBusyWeight.RIVER : MapBusyWeight.BUSY);
        }
    }

    // временно, для отладки
    void LeftMouseDrag() {
        // вектор направлениея движения в мире в плоскости экрана
        Vector3 direction = Camera.main.ScreenToWorldPoint(finishPosition) -
                            Camera.main.ScreenToWorldPoint(tapPosition);
        if (direction == Vector3.zero) {
            return;
        }

        Vector3 position = startPosition + direction;
        if (Vector2.Distance(position, transform.position) > 1f) {
            transform.position = new Vector3Int((int) position.x, (int) position.y, 0);
        }
    }

    // временно, для отладки
    private bool ClickOnCurrent() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            DebugMoveController controller = hit.transform.GetComponent<DebugMoveController>();
            return controller == this;
        }

        return false;
    }
}