using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class PortalController : MonoBehaviour {
    private static readonly Color WATER_COLOR = new Color(0.8f, 0.8f, 1.0f, 1f);
    private static readonly Color BASE_COLOR = Color.white;

    [SerializeField] private Renderer _quad;

    [Inject] private GameSettingsInstaller.GameSetting _setting;

    private int _rotate = 0;

    private bool _hasWater;
    
    private ConnectorController portalConnector;
    private ConnectorController tubeConnector;

    private void Start() {
        ConnectorController[] _ends = GetComponentsInChildren<ConnectorController>();
        foreach (ConnectorController connectorController in _ends) {
            if (connectorController.GetDirection() == Direction.PORTAL) {
                portalConnector = connectorController;
            } else {
                tubeConnector = connectorController;
            }
        }
    }

    public void MarkWater() {
        SetMaterial(WATER_COLOR);
        _hasWater = true;
    }

    public bool HasWater => _hasWater;

    public Vector3Int GetVector() {
        Vector3 p = transform.position;
        return new Vector3Int((int) Math.Round(p.x, 0), (int) Math.Round(p.y, 0), (int) Math.Round(p.z, 0));
    }
    
    private void SetMaterial(Color color) {
       // _quad.material.SetColor("_Color", color);
    }
    
    public class Factory : PlaceholderFactory<PortalCreateParam, PortalController> { }

    public void Clear() {
        SetMaterial(BASE_COLOR);
        _hasWater = false;
        TubeConnector.UnConnect();
        PortalConnector.UnConnect();
    }

    private void Update() {
        if (!_setting.isDebug) {
            return;
        }
        if (Input.GetMouseButtonUp(0) && ClickOnCurrent()) {
            RotatePortal();
        }
    }

    private void RotatePortal() {
        transform.Rotate(Vector3.forward, 90);
        _rotate++;
        if (_rotate >= 4) {
            _rotate = _rotate % 4;
        }
    }
    
    public int Rotate {
        get => _rotate;
        set {
            //только для инициализации при загрузке уровня
            _rotate = value;
            transform.Rotate(Vector3.forward, _rotate * 90);
        }
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
            PortalController tubeController = hit.transform.GetComponent<PortalController>();
            return tubeController == this;
        }

        return false;
    }

    public ConnectorController PortalConnector => portalConnector;

    public ConnectorController TubeConnector => tubeConnector;
}