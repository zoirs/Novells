using System;
using Zenject;

public class StoneManager : ObjectManager<StoneController, StonesDto, StoneCreateParam> {

    public StoneManager(StoneController.Factory factory, GameMapService gameMapService,
        GameSettingsInstaller.GameSetting setting, GameSettingsInstaller.PrefabSettings prefabSettings, DiContainer container) : base(
        gameMapService, setting, prefabSettings, factory, container) {
    }

    public override StoneCreateParam Convert(StonesDto dto) {
        return new StoneCreateParam(dto.GetPrefab(prefabs), dto.position);
    }

    public void CreateDebug(StoneType stoneType) {
        if (!_setting.isDebug) {
            throw new Exception("Unsupport");
        }

        Create(new StonesDto(stoneType, Constants.CREATE_POSITION));
    }
}