    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Main;
    using Plane;
    using UnityEngine;
    using UnityEngine.Assertions;
    using Zenject;

    public class LevelManager {
 
        [Inject]
        private TrainManager _trainManager;
        [Inject]
        private StationManager _stationManager;
        [Inject] 
        private TubeManager _tubeManager;
        [Inject] 
        private StoneManager _stoneManager;
        [Inject] 
        private RiverManager _riverManager;
        [Inject] 
        private PortalManager _portalManager;
        [Inject] 
        private InventoryManager _inventoryManager;
        [Inject] 
        private HintManager _hintManager;
        [Inject] 
        private PlaneManager _planeManager;
        [Inject]
        private GameSettingsInstaller.GameSetting _setting; 
        [Inject]
        private GameController _gameController; 
        [Inject]
        private GameMapService _gameMapService;

        public void UpdateLevel() {
            Assert.IsTrue(_gameController.CurrentLevel != null && _gameController.CurrentLevel.Number != int.MinValue); 
            SaveOrUpdateLevel(_gameController.CurrentLevel);
        }

        public void SaveLevel(LevelPackage packageValue) {
            SaveOrUpdateLevel(new LevelPath(Int32.MinValue, packageValue));
        }

        private void SaveOrUpdateLevel(LevelPath levelPath) {
            Debug.Log(levelPath.GetPath());
            TubeLevel level = new TubeLevel();
            foreach (TubeController tubeController in _tubeManager.Objects) {
                InventoryDto inventoryDto = new InventoryDto(tubeController.TubeType, (Vector2Int) tubeController.GetVector(), tubeController.Rotate, tubeController.Projection);
                level.inventory.Add(inventoryDto);
            }
            foreach (TrainController item in _trainManager.Objects) {
                TrainDto trainDto = new TrainDto(item.TrainType, (Vector2Int) item.GetVector());
                level.trains.Add(trainDto);
            }
            foreach (StationController item in _stationManager.Objects) {
                StationDto stationDto = new StationDto(item.StationType, (Vector2Int) item.GetVector(), item.Rotate);
                level.stations.Add(stationDto);
            }
            foreach (StoneController item in _stoneManager.Objects) {
                StonesDto stonesDto = new StonesDto(item.StoneType, (Vector2Int) item.GetVector());
                level.stones.Add(stonesDto);
            }
            foreach (RiverController item in _riverManager.Objects) {
                RiverDto riverDto = new RiverDto(item.RiverType, (Vector2Int) item.GetVector(), item.Rotate);
                level.rivers.Add(riverDto);
            }
            foreach (PortalController portal in _portalManager.Objects) {
                PortalDto portalDto = new PortalDto((Vector2Int) portal.GetVector(), portal.Rotate);
                level.portals.Add(portalDto);
            }

            string json = JsonUtility.ToJson(level);
            Debug.Log(json);

            if (levelPath.Number == int.MinValue) {
                DirectoryInfo dir = new DirectoryInfo(Application.persistentDataPath + "/" + levelPath.Package);
                int levelNumber = dir.GetFileSystemInfos("*.txt").Length;
                Debug.Log("подсчет уровня для "+ Application.persistentDataPath + "/" + levelPath.Package + ", итог " +levelNumber);
                levelPath = new LevelPath(levelNumber, levelPath.Package);
            }

            string filepath = Application.persistentDataPath + "/" + levelPath.GetPath() + ".txt";

            StreamWriter writer = new StreamWriter(filepath, false);
            writer.WriteLine(JsonUtility.ToJson(level));
            writer.Close();
        }

        public void LoadLevel(LevelPath levelPath) {
            TubeLevel level = Load(levelPath);
            _gameMapService.Reload();
            _stoneManager.Reload(level.stones);
            _riverManager.Reload(level.rivers);
            _portalManager.Reload(level.portals);
            _stationManager.Reload(level.stations);
            _trainManager.Reload(_trainManager.CreatePositions());
            _tubeManager.Reload(level.inventory); // последним
            List<InventoryDto> inventoryDtos = level.inventory
                .Where(q1 => q1.projection != TubeProjectionType.STATIC)
                .Where(q1 => q1.projection != TubeProjectionType.DECORATE)
                .ToList();
            _inventoryManager.Unload(inventoryDtos); //todo наверное стоит переименовать инвентарь в трубы, тк не передвигаемые трубы не являются инвентарем
            _hintManager.Unload(inventoryDtos);
            _planeManager.CreateIfNotExist();
        }

        public void ClearLevel() {
            _gameMapService.Reload();
            _stoneManager.Reload(new List<StonesDto>());
            _portalManager.Reload(new List<PortalDto>());
            _tubeManager.Reload(new List<InventoryDto>());
            _trainManager.Reload(new List<TrainDto>());
            _inventoryManager.Unload(new List<InventoryDto>());
            _hintManager.Unload(new List<InventoryDto>());
            _planeManager.Clear();
        }

        private TubeLevel Load(LevelPath levelPath) {
            if (levelPath.Number == Int32.MinValue) {
                return new TubeLevel();
            }

            if (_setting.loadLevelFromResources) {
                TextAsset levelasset = Resources.Load<TextAsset>("Levels/" + levelPath.GetPath());
                return JsonUtility.FromJson<TubeLevel>(levelasset.text);
            }
            else {
                string filepath = Application.persistentDataPath + "/" + levelPath.GetPath() + ".txt";

                StreamReader reader = new StreamReader(filepath);

                string readToEnd = reader.ReadToEnd();
                reader.Close();
                return JsonUtility.FromJson<TubeLevel>(readToEnd);
            }
        }
    }