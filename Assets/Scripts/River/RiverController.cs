using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using Zenject;

public class RiverController : MonoBehaviour {
 
    [SerializeField]
    private RiverType riverType;
    
    [Inject] private GameSettingsInstaller.GameSetting _setting;

    private int _rotate = 0;

    public Vector3Int GetVector() {
        Vector3 p = transform.position;
        return new Vector3Int((int) Math.Round(p.x, 0),(int) Math.Round(p.y, 0),(int) Math.Round( p.z, 0));
    }
    
    public class Factory : PlaceholderFactory<RiverCreateParam, RiverController> { }

    public RiverType RiverType => riverType;
    
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
            RiverController riverController = hit.transform.GetComponent<RiverController>();
            return riverController == this;
        }

        return false;
    }

}