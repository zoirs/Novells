using System.Collections.Generic;
using PathCreation;
using UnityEngine;
using Zenject;

public class TrainManager : ObjectManager<TrainController, TrainDto, TrainCreateParam> {

    [Inject] readonly TubeManager tubeManager;
    [Inject] readonly PortalManager portalManager;
    [Inject] readonly StationManager stationManager;
    [Inject] readonly PathFactory _pathFactory;

    private int wagonCount;
    
    private List<PathCreator> pathObjects = new List<PathCreator>();
    private readonly GameSettingsInstaller.PrefabSettings prefabs;
    
    public TrainManager(GameMapService gameMapService,
        GameSettingsInstaller.GameSetting setting,
        TrainController.Factory factory,
        GameSettingsInstaller.PrefabSettings prefabs, DiContainer container)
        : base(gameMapService, setting, prefabs, factory, container) {
        this.prefabs = prefabs;
        this.wagonCount = PlayerPrefs.GetInt(PlayerPrefsUtils.WAGON_COUNT);
    }

    public override TrainCreateParam Convert(TrainDto dto) {
        return new TrainCreateParam(dto.GetPrefab(prefabs), dto.position);
    }

    public List<TrainDto> CreatePositions() {
        if (!stationManager.IsExist()) {
            return new List<TrainDto>();
        }
        StationController stationController = stationManager.GetStartStation();
        ConnectorController connectorController = stationController.GetStartConnector();
        TrainDto train = new TrainDto(TrainType.SIMPLE, connectorController.GetVector());
        List<TrainDto> trainDtos = new List<TrainDto>();
        trainDtos.Add(train);

        for (int i = 0; i < wagonCount; i++) {
            TrainDto vagon = new TrainDto(TrainType.VAGON,    connectorController.GetVector() - connectorController.GetDirection().GetVector() * (i +1));
            trainDtos.Add(vagon);
        }

        return trainDtos;
    }

    public void Run() {
        List<ConnectorController> list = new List<ConnectorController>();
        foreach (TubeController tubeManagerObject in tubeManager.Objects) {
            if (tubeManagerObject.Projection == TubeProjectionType.DECORATE) {
                continue;
            }
            // todo нужно ли проверять что труба - это подсказка?
            list.AddRange(tubeManagerObject.Connectors);
        }
        foreach (PortalController portalController in portalManager.Objects) {
            list.Add(portalController.TubeConnector);
            list.Add(portalController.PortalConnector);
        }
        foreach (StationController stationController in stationManager.Objects) {
            ConnectorController[] connectorControllers = stationController.Tube.Connectors;
            list.AddRange(connectorControllers);
        }
        Run(list);
    }

    public void Reload(List<TrainDto> createParams) {
        for (var i = pathObjects.Count - 1; i >= 0; i--) {
            PathCreator forRemove = pathObjects[i];
            pathObjects.Remove(forRemove);
            Object.Destroy(forRemove);
        }
        base.Reload(createParams);
    }

    private void Run(List<ConnectorController> allConnectors) {
        List<List<Vector2>> paths = new List<List<Vector2>>();
        int pathCount = 1;
        paths.Add(new List<Vector2>());
        allConnectors.Sort((c1, c2) => c1.pathStepNumber.CompareTo(c2.pathStepNumber));
        foreach (ConnectorController connectorController in allConnectors) {
                // Debug.Log("добавляем точку в " + (pathCount - 1) + "; " + connectorController.GetPathPoint());
                paths[pathCount - 1].Add(connectorController.GetPathPoint());
                if (connectorController.IsStartPortal()) {
                    // Debug.Log("встретили портал ");
                    pathCount++;
                    paths.Add(new List<Vector2>());
                }

        }

        List<PathCreator> pathsCreators = new List<PathCreator>(pathCount);
        // Debug.Log("============= всего штук: " + paths.Count + " |||||" + Random.Range(1,100));

        foreach (List<Vector2> path in paths) {
            // Debug.Log("Исходные точки пути: " + path.Count + " |||||" + Random.Range(1,100));
            // foreach (Vector2 vector3 in path) {
                // Debug.Log(vector3 + " ;" + Random.Range(1,100));
            // }
            // Debug.Log("Созданный путь: " + Random.Range(1,100));

            PathCreator pathCreator = _pathFactory.Create(new PathParam(path));
            pathObjects.Add(pathCreator);
            // foreach (Vector3 pathLocalPoint in pathCreator.path.localPoints) {
                // Debug.Log(pathLocalPoint + " ;" + Random.Range(1,100));
            // }
            pathsCreators.Add(pathCreator);
        }

        for (int index = 0; index < Objects.Count; index++) {
            TrainController trainController = Objects[index];
            TrainController next = index + 1 < Objects.Count ? Objects[index + 1] : null; 
            trainController.Run(pathsCreators, index, next);
        }
    }

    public int WagonCount => wagonCount;

    public void AddWagon() { // todo разделить класс на несколько
        wagonCount++;
        PlayerPrefs.SetInt(PlayerPrefsUtils.WAGON_COUNT, wagonCount);
        Reload(CreatePositions());
    }
}