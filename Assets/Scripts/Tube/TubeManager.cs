using Zenject;

public class TubeManager : ObjectManager<TubeController, InventoryDto, TubeCreateParam> {
    [Inject] private GameSettingsInstaller.PrefabSettings prefabs;

    public TubeManager(GameMapService gameMapService,
        GameSettingsInstaller.GameSetting setting,
        GameSettingsInstaller.PrefabSettings prefabSettings, TubeController.Factory factoryTube, DiContainer container)
        : base(gameMapService, setting, prefabSettings, factoryTube, container) { }

    public void Clear() {
        foreach (TubeController controller in Objects) {
            controller.Clear();
        }
    }

    public override TubeCreateParam Convert(InventoryDto dto) {
        return new TubeCreateParam(dto.GetPrefab(prefabs), dto.rotate, dto.position, dto.projection);
    }
}