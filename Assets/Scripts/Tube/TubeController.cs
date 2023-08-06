using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Main;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class TubeController : MonoBehaviour {
    public static int TUBE_Z = -1;
    public static int TUBE_HINT_Z = -0;
    
    [SerializeField] private TubeType _tubeType;
    [SerializeField] private GameObject _image;
    [SerializeField] private ConnectorController[] _connectors;
    
    [SerializeField] private Renderer _moveProjection;
    [SerializeField] private GameObject _staticProjection;
    [SerializeField] private GameObject _hintProjection;
    [SerializeField] private GameObject _decorateProjection;

    [Inject] private GameMapService _gameMapService;
    [Inject] private CheckerService _checkerService;
    [Inject] private GameSettingsInstaller.GameSetting _setting;
    [Inject] private MicAudioPlayer _audioPlayer;
    [Inject] private InventoryManager _inventoryManager;
    [Inject] private GameController _gameController;

    private TubeState _positionState = TubeState.SettedCorrect;

    private bool _hasWater;

    private int _rotate = 0;
    
    private TubeProjectionType _projection = TubeProjectionType.SIMPLE;

    Vector3 tapPosition = Vector3.zero;
    Vector3 finishPosition = Vector3.zero;
    Vector3 startPosition = Vector3.zero;
    private PointController[] _points;

    private static readonly Color MOVE_COLOR = new Color(0.7f, 0.7f, 0.7f, 1f);
    private static readonly Color ERROR_POSITION_COLOR = new Color(1f, 0.7f, 0.7f, 1f);
    // private static readonly Color WATER_COLOR = new Color(0.8f, 0.8f, 1.0f, 1f);
    private static readonly Color BASE_COLOR = Color.white;

    protected void Start() {
        _points = GetComponentsInChildren<PointController>();

        if (Input.GetMouseButton(0)) {
            tapPosition = Input.mousePosition;
            Vector3 screenToWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position =  new Vector3(screenToWorldPoint.x, -1f, TUBE_Z);
            startPosition = transform.position;
            State = TubeState.Move;
        } else {
            PositionState positionState = _gameMapService.Check(_points);
            if (positionState == PositionState.CORRECT) {
                State = TubeState.SettedCorrect;
                if (_projection != TubeProjectionType.HINT) {
                    _gameMapService.SetToMap(_points, MapBusyWeight.BUSY);
                }
                if (!_setting.isDebug) {
                    StartCoroutine(Check());
                }
            } else if (positionState == PositionState.RIVER && _tubeType == TubeType.BRIDGE) {
                State = TubeState.SettedCorrect;
                if (_projection != TubeProjectionType.HINT) {
                    _gameMapService.SetToMap(_points, MapBusyWeight.BUSY);
                }
                if (!_setting.isDebug) {
                    StartCoroutine(Check());
                }
            } else {
                State = TubeState.SettedWrong;
            }
        }
    }

    public List<ConnectorController> GetFreeConnecter() {
        return _connectors.Where(tubeEndController => !tubeEndController.IsConnected()).ToList();
    }

    private void Update() {
        if (_projection.NotMovable() && !_setting.isDebug) {
            return;
        }

        if (_gameController.State == GameStates.GoTrain) {
            return;
        }

        switch (_positionState) {
            case TubeState.Inventory:
                break;
            case TubeState.Wait:
            case TubeState.SettedCorrect:
            case TubeState.SettedWrong:
                if (Input.GetMouseButtonDown(0) && ClickOnCurrent()) {
                    tapPosition = Input.mousePosition;
                    startPosition = transform.position;
                    if (_positionState == TubeState.SettedCorrect) {
                        _gameMapService.Free(_points);
                    }

                    State = TubeState.Move;
                }

                break;
            case TubeState.Move:
                if (Input.GetMouseButton(0)) {
                    finishPosition = Input.mousePosition;
                    LeftMouseDrag();
                }

                if (Input.GetMouseButtonUp(0)) {
                    if (Vector2.Distance(startPosition, transform.position) < 0.2f) {
                        _audioPlayer.DoRotate();
                        RotateTube();
                    } else {
                        _audioPlayer.DoMove();
                    }

                    PositionState positionState = _gameMapService.Check(_points);
                    if (positionState == PositionState.CORRECT) {
                        State = TubeState.SettedCorrect;
                        _gameMapService.SetToMap(_points, MapBusyWeight.BUSY);
                        if (!_setting.isDebug) {
                            StartCoroutine(Check());
                        }
                    } else if (positionState == PositionState.RIVER && _tubeType == TubeType.BRIDGE) {
                        Debug.Log("Мост на реке");
                        State = TubeState.SettedCorrect;
                        _gameMapService.SetToMap(_points, MapBusyWeight.BRIDGE_ON_RIVER);
                        if (!_setting.isDebug) {
                            StartCoroutine(Check());
                        }
                    } else if (positionState == PositionState.OUTSIDE_FULL) {
                        _inventoryManager.PutToInventory(this);
                    }
                    else {
                        State = TubeState.SettedWrong;
                    }
                }

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public TubeState State {
        get => _positionState;
        set {
            _positionState = value;
            if (_positionState == TubeState.Move) {
                _image.transform.localScale = _image.transform.localScale + new Vector3(0.1f,0.1f,0);
                _moveProjection.gameObject.SetActive(true);
                SetMaterial(MOVE_COLOR);
            } else if (_positionState == TubeState.SettedWrong) {
                _image.transform.localScale = new Vector3(1f,1f,0);
                _image.transform.localPosition = new Vector3(0,0,-0.5f);;// тут можно анимацию сделать
                _moveProjection.gameObject.SetActive(false);
                SetMaterial(ERROR_POSITION_COLOR);
            } else {
                _image.transform.localScale = new Vector3(1f,1f,0);
                _image.transform.localPosition = new Vector3(0,0,-0.5f);;// тут можно анимацию сделать
                _moveProjection.gameObject.SetActive(false);
                SetMaterial(BASE_COLOR);
            }
        }
    }

    private void RotateTube() {
        if (_positionState == TubeState.Move) {
            transform.Rotate(Vector3.forward, 90);
            _rotate++;
            if (_rotate >= 4) {
                if (_setting.isDebug) {
                    if (_projection == TubeProjectionType.SIMPLE) {
                        Projection = TubeProjectionType.STATIC;
                    } else if (_projection == TubeProjectionType.STATIC) {
                        Projection = TubeProjectionType.DECORATE;
                    } else if (_projection == TubeProjectionType.DECORATE) {
                        Projection = TubeProjectionType.SIMPLE;
                    }
                }

                _rotate = _rotate % 4;
            }
        }
    }

    public class Factory : PlaceholderFactory<TubeCreateParam, TubeController> { }

    public void MarkWater(int i) {
        // SetMaterial(WATER_COLOR);
        _hasWater = true;
        Array.Sort(_connectors, (q1, q2) => q1.pathStepNumber.CompareTo(q2.pathStepNumber));
    }

    public bool HasWater => _hasWater;

    public void Clear() {
        SetMaterial(_positionState == TubeState.SettedWrong ? ERROR_POSITION_COLOR : BASE_COLOR);
        _hasWater = false;
        foreach (ConnectorController tubeEndController in _connectors) {
            tubeEndController.UnConnect();
        }
    }

    private void SetMaterial(Color color) {
        foreach (Renderer r in _image.GetComponentsInChildren<Renderer>()) {
            r.material.SetColor("_Color", color);
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
        if (Vector2.Distance(position, transform.position) > 0.5f) {
            transform.position = new Vector3Int((int) Math.Round(position.x), (int) Math.Round(position.y), TUBE_Z);
            PositionState positionState = _gameMapService.Check(_points);
            if (positionState == PositionState.CORRECT) {
                SetMaterial(MOVE_COLOR);
                _moveProjection.gameObject.SetActive(true);
            } else if (positionState == PositionState.RIVER && _tubeType == TubeType.BRIDGE) {
                SetMaterial(MOVE_COLOR);
                _moveProjection.gameObject.SetActive(true);
            } else {
                SetMaterial(ERROR_POSITION_COLOR);
                _moveProjection.gameObject.SetActive(false);
            }
        }
        position.z = -1.6f;
        _image.transform.position = position;
    }
    
    public List<ConnectorController> SortedConnecter() {
        if (!_hasWater) {
            Debug.LogError("Try get sort connector but tube not connected");
        }

        Array.Sort(_connectors, (c1, c2) => (c1.GetNext() != null).CompareTo(c2.GetNext() != null));
        return new List<ConnectorController>(_connectors);
    }

    private bool ClickOnCurrent() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

#if UNITY_EDITOR
        if (EventSystem.current.IsPointerOverGameObject()) {
            return false;
        }
#else
        if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) {
            return false;
        }
#endif

        if (Physics.Raycast(ray, out hit)) {
            TubeController tubeController = hit.transform.GetComponent<TubeController>();
            return tubeController == this;
        }

        return false;
    }

    public TubeType TubeType => _tubeType;

    public int Rotate {
        get => _rotate;
        set {
            //только для инициализации при загрузке уровня
            _rotate = value;
            transform.Rotate(Vector3.forward, _rotate * 90);
        }
    }

    public ConnectorController[] Connectors => _connectors;

    public TubeProjectionType Projection {
        set {
            _projection = value;
            _staticProjection.gameObject.SetActive(value == TubeProjectionType.STATIC);
            _hintProjection.gameObject.SetActive(value == TubeProjectionType.HINT);
            if (_decorateProjection != null) {
                _decorateProjection.gameObject.SetActive(value == TubeProjectionType.DECORATE);
            }
        }
        get {
            return _projection;
        }
    }

    public Vector3Int GetVector() {
        Vector3 p = transform.position;
        return new Vector3Int((int) Math.Round(p.x, 0), (int) Math.Round(p.y, 0), (int) Math.Round(p.z, 0));
    }
    
    IEnumerator Check() {
        yield return 0;
        _checkerService.Check();
    }
}