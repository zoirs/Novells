using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

[Serializable]
public class TubeLevel {
        public int wight;
        public int height;
        [FormerlySerializedAs("levelBlock")] public LevelPackage levelPackage;
        public List<InventoryDto> inventory = new List<InventoryDto>();
        public List<StationDto> stations = new List<StationDto>();
        public List<StonesDto> stones = new List<StonesDto>();
        public List<TrainDto> trains = new List<TrainDto>();
        public List<PortalDto> portals = new List<PortalDto>();
        public List<RiverDto> rivers = new List<RiverDto>();
}