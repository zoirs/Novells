using UnityEngine;
using Zenject;

public class Removable : MonoBehaviour {
    [Inject] private GameMapService _gameMapService;

    [Inject] private TubeManager _tubeManager;
    [Inject] private StationManager _stationManager;
    [Inject] private StoneManager _stoneManager;
    [Inject] private TrainManager _trainManager;
    [Inject] private RiverManager _riverManager;

    private PointController[] _points;

    private TubeController tubeController;
    private StationController stationController;
    private StoneController stoneController;
    private TrainController trainController;
    private RiverController riverController;

    private void Start() {
        _points = GetComponentsInChildren<PointController>();

        stationController = GetComponent<StationController>();
        tubeController = GetComponent<TubeController>();
        stoneController = GetComponent<StoneController>();
        trainController = GetComponent<TrainController>();
        riverController = GetComponent<RiverController>();
    }

    //ObjectManager<MonoBehaviour, BaseDto, object> objectManager
    private void Update() {
        CheckRemove();
    }

    private void CheckRemove() {
        if (Input.GetMouseButtonDown(1) && ClickOnCurrent()) {
            _gameMapService.Free(_points);

            if (tubeController != null) {
                _tubeManager.Remove(this);
            }
            
            if (stationController != null) {
                _stationManager.Remove(this);
            }

            if (stoneController != null) {
                _stoneManager.Remove(this);
            }

            if (trainController != null) {
                _trainManager.Remove(this);
            }
            
            if (riverController != null) {
                _riverManager.Remove(this);
            }
        }
    }

    private bool ClickOnCurrent() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            Removable controller = hit.transform.GetComponent<Removable>();
            return controller == this;
        }

        return false;
    }
}