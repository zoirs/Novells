using System;
using Zenject;

public class StationManager : ObjectManager<StationController, StationDto, TubeCreateParam> {

    [Inject] private GameSettingsInstaller.PrefabSettings prefabs;

    public StationManager(GameMapService gameMapService,
        GameSettingsInstaller.GameSetting setting,
        GameSettingsInstaller.PrefabSettings prefabSettings, StationController.Factory factoryTube, DiContainer container)
        : base(gameMapService, setting, prefabSettings, factoryTube, container) {
    }

    public bool IsExist() {
        return Objects != null && Objects.Count > 0;
    }

    public StationController GetStartStation() {
        StationController stationController = Objects[0];
        return stationController;
    }

    public StationController GetFinishStation() {
        StationController stationController = Objects[1];
        return stationController;
    }

    public override TubeCreateParam Convert(StationDto dto) {
        return new TubeCreateParam(dto.GetPrefab(prefabs), dto.rotate, dto.position, TubeProjectionType.STATIC);
    }

    public void CreateDebug() {
        if (!_setting.isDebug) {
            throw new Exception("Unsupport");
        }

        Create(new StationDto(StationType.SIMPLE, Constants.CREATE_POSITION, 0));
    }

    public void Clear() {
        foreach (StationController controller in Objects) {
            controller.Tube.Clear();
        }
    }
}