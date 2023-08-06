using UnityEngine;

public class FingerMoveController : MonoBehaviour {
    public bool _isChecked { get; private set; }

    Vector3 tapPosition = Vector3.zero;
    Vector3 finishPosition = Vector3.zero;
    Vector3 startPosition = Vector3.zero;

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            tapPosition = Input.mousePosition;
            startPosition = transform.position;
        }

        if (Input.GetMouseButton(0)) {
            finishPosition = Input.mousePosition;
            LeftMouseDrag();
        }
    }

    void LeftMouseDrag() {
        // вектор направлениея движения в мире в плоскости экрана
        Vector3 direction = Camera.main.ScreenToWorldPoint(finishPosition) -
                            Camera.main.ScreenToWorldPoint(tapPosition);
        if (direction == Vector3.zero) {
            return;
        }

        Vector3 position = startPosition + direction;
        transform.position = position;
        enabled = false;
    }
    
}