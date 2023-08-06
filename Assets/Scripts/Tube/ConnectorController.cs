using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ConnectorController : MonoBehaviour {
    [SerializeField] private Direction direction;
    [SerializeField] private GameObject parent;
    
    // следующий по направлению движения, на предыдущий ссылки нет
    public GameObject next;

    private ConnectorController connected;
    public int pathStepNumber = 0;
    private static int i = 0; // временная переменная, кажтеся можно убрать
    
    public Direction GetDirection(int rotate) {
        return direction.Rotate(rotate);
    }

    public Direction GetDirection() {
        if (parent != null) {
            TubeController tubeController = parent.GetComponent<TubeController>();
            if (tubeController != null) {
                //Debug.Log("Направление " + direction + "; направление " + tubeController.Rotate + " " + Random.Range(1,1000));
                return direction.Rotate(tubeController.Rotate);
            }
            PortalController portalController = parent.GetComponent<PortalController>();
            if (portalController != null) {
                return direction.Rotate(portalController.Rotate);
            }
        }

        return direction.Rotate(0);
    }

    public Vector2 GetPathPoint() {
        Vector2Int vector2Int = GetDirection().GetVector();
        return transform.position + new Vector3(vector2Int.x / 4f + Random.Range(-0.03f,0.03f), vector2Int.y / 4f + Random.Range(-0.03f,0.03f));
    }

    public Vector2Int GetVector() {
        Vector3 p = transform.position;
        return new Vector2Int((int) Math.Round(p.x, 0),(int) Math.Round(p.y, 0));
    }
    
    public void SetConnect(ConnectorController controller) {
        connected = controller;
    }
    
    public bool TryConnect(ConnectorController connectorController) {
        if (IsConnectorsConnected(this, connectorController)) {
            //Debug.Log("try connect " + i + "; prev=" + connectorController+"; next="+this);
            this.SetConnect(connectorController);
            connectorController.next = parent;
            i++;
            connectorController.pathStepNumber = i;
            i++;
            this.pathStepNumber = i;
            connectorController.SetConnect(this);
            TubeController tubeController = parent.GetComponent<TubeController>();
            if (tubeController != null) {
                tubeController.MarkWater(i);
            }
            PortalController portalController = parent.GetComponent<PortalController>();
            if (portalController != null) {
                portalController.MarkWater();
            }
            return true;
        }

        return false;
    }

    private static bool IsConnectorsConnected(ConnectorController first, ConnectorController second) {
        bool isPortalToPortal = first.GetDirection() == Direction.PORTAL && second.GetDirection() == Direction.PORTAL;
        if (isPortalToPortal) {
            return true;
        }

        if (first.GetDirection() != second.GetDirection().Invert() ||
            first.GetDirection().Invert() != second.GetDirection()) {
            //Debug.Log("коннекторы не подошли тк разнонапрвленны "+ Random.Range(1,1000));
            return false;
        }

        //Debug.Log("Проверяем коннекторы 1-2 "+ Random.Range(1,1000));
        
        Vector2Int connectPosition = first.GetVector() + first.GetDirection().GetVector();
        Direction connectDirection = first.GetDirection().Invert();
        //Debug.Log("First " +  connectPosition + " " + connectDirection + " ;" + Random.Range(1,1000));
        //Debug.Log("Second " +  second.GetVector() + " " + second.GetDirection()  + " ;" + Random.Range(1,1000));

        if (connectPosition == second.GetVector() && (connectDirection == second.GetDirection())) {
            //Debug.Log("коннекторы 1-2 подошли"+ Random.Range(1,1000));
            return true;
        }
        //Debug.Log("Проверяем коннекторы 2-1 "+ Random.Range(1,1000));
        
        connectPosition = second.GetVector() + second.GetDirection().GetVector();
        connectDirection = second.GetDirection().Invert();

        //Debug.Log("Second " +  connectPosition + " " + connectDirection + " ;" + Random.Range(1,1000));
        //Debug.Log("first " +  first.GetVector() + " " + first.GetDirection() + " ;" + Random.Range(1,1000));

        if (connectPosition == first.GetVector() && (connectDirection == first.GetDirection())) {
            //Debug.Log("коннекторы 2-1 подошли"+ Random.Range(1,1000));
            return true;
        }
        //Debug.Log("коннекторы не подошли"+ Random.Range(1,1000));
        return false;
    }

    public void UnConnect() {
        connected = null;
        next = null;
    }

    public bool IsConnected() {
        return connected != null;
    }
    
    public TubeController GetParentTube() {
        return parent != null ? parent.GetComponent<TubeController>() : null;
    }


    public bool IsStartPortal() {
        bool b = IsPortal();
        if (b) {
            //Debug.Log("Это портал; след: " + next);
        }

        return b
               && next != null && next.GetComponent<PortalController>() != null;
    }

    public bool IsPortal() {
        return parent != null && parent.GetComponent<PortalController>() != null;
    }

    public GameObject GetNext() {
        return next;
    }
    
    public ConnectorController GetConnected() {
        return connected;
    }
}