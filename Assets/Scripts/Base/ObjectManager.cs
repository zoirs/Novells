using System.Collections.Generic;
using UnityEngine;
using Zenject;

public abstract class ObjectManager<CONTROLLER, DTO, CREATE>
    where CONTROLLER : MonoBehaviour
    where DTO : BaseDto {
    private GameMapService _gameMapService;
    protected GameSettingsInstaller.GameSetting _setting;

    private List<CONTROLLER> objects = new List<CONTROLLER>();
    protected GameSettingsInstaller.PrefabSettings prefabs;
    private PlaceholderFactory<CREATE, CONTROLLER> _factory;
    private DiContainer _container;

    protected ObjectManager(GameMapService gameMapService, GameSettingsInstaller.GameSetting setting,
        GameSettingsInstaller.PrefabSettings prefabSettings, PlaceholderFactory<CREATE, CONTROLLER> factory,
        DiContainer container) {
        _gameMapService = gameMapService;
        _setting = setting;
        prefabs = prefabSettings;
        _factory = factory;
        _container = container;
    }

    public void Reload(List<DTO> createParams) {
        foreach (CONTROLLER o in objects) {
            Object.Destroy(o.gameObject);
        }

        objects.Clear();

        foreach (DTO paramDto in createParams) {
            bool isTube = typeof(CONTROLLER) == typeof(TubeController);

            if (isTube) {
                Debug.Log(paramDto);
                if (_setting.isDebug || (paramDto as InventoryDto).projection == TubeProjectionType.STATIC ||  (paramDto as InventoryDto).projection == TubeProjectionType.DECORATE) {
                    //трубы создаем только в дебаг режиме
                    Create(paramDto);
                }
            } else {
                CONTROLLER go = Create(paramDto);
                bool isStation = typeof(CONTROLLER) == typeof(StationController);
                if (!isStation) {
                    bool isRiver = typeof(CONTROLLER) == typeof(RiverController);
                    _gameMapService.SetToMap(go.GetComponentsInChildren<PointController>(),
                        isRiver ? MapBusyWeight.RIVER : MapBusyWeight.BUSY);
                }
            }
        }
    }

    public CONTROLLER Create(DTO paramDto) {
        CONTROLLER item = _factory.Create(Convert(paramDto));
        objects.Add(item);
        
        bool isTube = typeof(CONTROLLER) == typeof(TubeController);
        GameObject gameObject = item.gameObject;

        if (_setting.isDebug) {
            _container.InstantiateComponent<Removable>(gameObject);
            if (!isTube) {
                _container.InstantiateComponent<DebugMoveController>(gameObject);
            }
        } else {
            if (isTube) {
                _container.InstantiateComponent<Removable>(gameObject);
            }
        }

        return item;
    }

    public abstract CREATE Convert(DTO dto);

    public List<CONTROLLER> Objects => objects;

    // public abstract CONTROLLER Create(DTO dto);
    public void Remove(Removable removable) {
        CONTROLLER find = Objects.Find(controller => controller.gameObject == removable.gameObject);
        Debug.Log("remove " + find);
        if (find != null) {
            Objects.Remove(find);
            Object.Destroy(find.gameObject);
        }
    }
}