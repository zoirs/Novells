using System;
using Zenject;

public class RiverManager : ObjectManager<RiverController, RiverDto, RiverCreateParam> {

    public RiverManager(RiverController.Factory factory, GameMapService gameMapService,
        GameSettingsInstaller.GameSetting setting, GameSettingsInstaller.PrefabSettings prefabSettings, DiContainer container) : base(
        gameMapService, setting, prefabSettings, factory, container) {
    }

    public override RiverCreateParam Convert(RiverDto dto) {
        return new RiverCreateParam(dto.GetPrefab(prefabs), dto.position, dto.rotate);
    }

    public void CreateDebug(RiverType stoneType) {
        if (!_setting.isDebug) {
            throw new Exception("Unsupport");
        }

        Create(new RiverDto(stoneType, Constants.CREATE_POSITION, 0));
    }
}