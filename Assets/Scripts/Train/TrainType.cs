using System;
using UnityEngine;

public enum TrainType {
    SIMPLE,
    VAGON,
}

public static class TrainTypeExtension {
    
    
    public static GameObject GetPrefab(this TrainType trainType, GameSettingsInstaller.PrefabSettings prefabs) {
        switch (trainType) {
            case TrainType.SIMPLE:
                return prefabs.TrainPrefab;
            case TrainType.VAGON:
                return prefabs.VagonPrefab;
            default:
                throw new ArgumentOutOfRangeException(nameof(trainType), trainType, null);
        }
    }
    
    public static Texture GetTexture(this TrainType trainType, GameSettingsInstaller.OtherItemTextureSettings textures) {
        switch (trainType) {
            case TrainType.SIMPLE:
                return textures.Train;
            case TrainType.VAGON:
                return textures.Train;
            default:
                throw new ArgumentOutOfRangeException(nameof(trainType), trainType, null);
        }
    }
    
    public static int Price(this TrainType trainType, GameSettingsInstaller.PriceSetting gameSetting) {
        switch (trainType) {
            case TrainType.VAGON:
                return gameSetting.Wagon;
            default:
                throw new ArgumentOutOfRangeException(nameof(trainType), trainType, null);
        }
    }
}