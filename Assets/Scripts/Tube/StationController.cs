using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(TubeController))]
public class StationController : MonoBehaviour {
    
    [SerializeField] private StationType stationType;
    [SerializeField] private TubeController tube;

    private int _rotate = 0;

    public void Start() {
        InitLater();
    }

    void InitLater() {
        // yield return new WaitUntil(() => tube.QuadMaterial != null);
        tube.MarkWater(-1);
    }

    private void OnValidate() {
        if (GetComponent<TubeController>().TubeType != TubeType.LINE2) {
            Debug.LogError("Incorrect tube type");
        }
    }

    public ConnectorController GetStartConnector() {
        // хак, берем дальний коннектор
        return tube.Connectors.First(c => c.name == "PointConnector1");
    }

    public ConnectorController[] GetConnectors() {
        return tube.Connectors;
    }

    public void MarkAsFinish() {
        foreach (ConnectorController connectorController in tube.Connectors) {
            if (!connectorController.IsConnected()) {
                connectorController.pathStepNumber = int.MaxValue;
            }
        }
    }
    
    public Vector3Int GetVector() {
        Vector3 p = transform.position;
        return new Vector3Int((int) Math.Round(p.x, 0),(int) Math.Round(p.y, 0),(int) Math.Round( p.z, 0));
    }

    public StationType StationType => stationType;

    public TubeController Tube => tube;

    public int Rotate {
        get => tube.Rotate;
        set {
            //только для инициализации при загрузке уровня
            tube.Rotate = value;
            // _rotate = value;
            // transform.Rotate(Vector3.forward, _rotate * 90);
        }
    }

    public class Factory : PlaceholderFactory<TubeCreateParam, StationController> { }
}