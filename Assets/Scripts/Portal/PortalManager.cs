using System;
using Zenject;

public class PortalManager : ObjectManager<PortalController, PortalDto, PortalCreateParam> {
    
    public PortalManager(GameMapService gameMapService, GameSettingsInstaller.GameSetting setting,
        GameSettingsInstaller.PrefabSettings prefabSettings,
        PortalController.Factory factory, DiContainer container) : base(gameMapService,
        setting, prefabSettings, factory, container) { }

    public override PortalCreateParam Convert(PortalDto dto) {
        return new PortalCreateParam(dto.GetPrefab(prefabs), dto.position, dto.rotate);
    }

    
    public void CreateDebug() {
        if (!_setting.isDebug) {
            throw new Exception("Unsupport");
        }

        Create(new PortalDto(Constants.CREATE_POSITION, 0));
    }

    public void Clear() {
        foreach (PortalController controller in Objects) {
            controller.Clear();
        }
    }
}