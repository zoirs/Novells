using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using Random = System.Random;

namespace Main {
    public class CheckerService {
        [Inject] readonly TubeManager tubeManager;
        [Inject] readonly StationManager stationManager;
        [Inject] readonly PortalManager portalManager;
        [Inject] readonly GameController gameController;
        Random rand = new Random();

        private Dictionary<PointInfo, ConnectorController> all = new Dictionary<PointInfo, ConnectorController>();

        public void Check() {
            bool allTubeCorrect = tubeManager.Objects.All(controller => controller.State == TubeState.SettedCorrect);
            if (!allTubeCorrect) {
                return;
            }

            tubeManager.Clear();
            stationManager.Clear();
            portalManager.Clear();
            
            bool result = true;
            StationController station = stationManager.GetStartStation();
            //Debug.Log("Стартовая станция:" + station.name);
            result &= Find(station.GetStartConnector());
            if (result) {
                bool allTubeConnected = tubeManager.Objects.All(controller => controller.HasWater);
                //Debug.Log("allTubeConnected " + allTubeConnected);
                if (allTubeConnected) {
                    gameController.State = GameStates.GoTrain;
                }
            }
        }

        private bool Find(ConnectorController connectorController) {
            //Debug.Log("Начальный коннкетор " + connectorController.name + " " + "; pos:" + connectorController.GetVector()+" "+rand.Next());
            bool isPortalConnected = false;
            if (!connectorController.IsPortal()) {
                foreach (PortalController portal in portalManager.Objects) {
                    if (portal.TubeConnector.TryConnect(connectorController)) {
                        isPortalConnected = true;
                        break;
                    }
                }
            }

            if (isPortalConnected) {
                PortalController connectedPortal = connectorController.next.GetComponent<PortalController>();
                foreach (PortalController otherPortal in portalManager.Objects) {
                    if (connectedPortal == otherPortal) {
                        //Debug.Log("Текущий портал ");
                        continue;
                    }

                    if (otherPortal.PortalConnector.TryConnect(connectedPortal.PortalConnector)) {
                        //Debug.Log("портал  соединен");

                        otherPortal.MarkWater();
                        ConnectorController point = otherPortal.TubeConnector;
                        //Debug.Log("Соединяем портал с ");
                        bool result = Find(point);
                        if (!result) {
                            //Debug.Log("Кажется, есть утечка из второго портала");
                        }
                        return result;
                    }
                    else {
                        //Debug.Log("портал  НЕ соединен");
                        return false;
                    }
                }

                return true;
            }
            //Debug.Log("=================== " + tubeManager.Objects.Count);
            foreach (TubeController tube in tubeManager.Objects) {
                //Debug.Log("=================== " + tube.name);
                if (connectorController.GetParentTube() != null && connectorController.GetParentTube() == tube) {
                    continue;
                }

                if (tube.State != TubeState.SettedCorrect) {
                    return false;
                }
                
                if (tube.Projection == TubeProjectionType.HINT || tube.Projection == TubeProjectionType.DECORATE) {
                    continue;
                }

                List<ConnectorController> freeConnecters = tube.GetFreeConnecter();
                //Debug.Log("Берем трубу " + tube.name +"; свободных коннектов " + freeConnecters.Count +  "; " + rand.Next());
                foreach (ConnectorController freeConnecter in freeConnecters) {
                    //Debug.Log("Берем коонкетор трубы " + tube.name + "; поворот " +tube.Rotate + " " + rand.Next());
                    if (freeConnecter.TryConnect(connectorController)) {
                        //Debug.Log("труба подошла " + tube.name + rand.Next());

                        List<ConnectorController> otherConnecters = tube.GetFreeConnecter();
                        bool allOtherEndsConnected = true;
                        foreach (ConnectorController endController in otherConnecters) {
                            //Debug.Log("Берем другой коннкетор трубы " + tube.name + rand.Next());
                            bool result = Find(endController);
                            if (!result) {
                                //Debug.Log("Кажется, есть утечка " + endController.GetParentTube());
                            }

                            allOtherEndsConnected = allOtherEndsConnected & result;
                        }

                        return allOtherEndsConnected;
                    }
                }
            }

            // Если это труба от колодца, то не учитываем что ничего не нашли
            // return connectorController.GetParentTube() == null;
            StationController finishStation = stationManager.GetFinishStation();
            List<ConnectorController> connecters = finishStation.Tube.GetFreeConnecter();
            foreach (ConnectorController freeConnecter in connecters) {
                if (freeConnecter.TryConnect(connectorController)) {
                    finishStation.MarkAsFinish();
                    //Debug.Log("++++++++!!");
                    return true;
                }
            }

            //Debug.Log("не соединилось");
            return false;
        }
    }
}