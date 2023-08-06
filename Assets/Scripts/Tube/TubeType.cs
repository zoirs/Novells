using System;
using System.Collections.Generic;
using UnityEngine;

public enum TubeType {
    NONE,
    LINE1,
    LINE2,
    LINE3,
    LINE4,
    ANGEL,
    BRIDGE,
}

//    3
//    _
// 4 |_| 2 
//    1
public static class TubeTypeExtension {
    
    public static GameObject GetPrefab(this TubeType tuneType, GameSettingsInstaller.PrefabSettings prefabs) {
        switch (tuneType) {
            case TubeType.LINE1:
                return prefabs.Tube1Prefab;
            case TubeType.LINE2:
                return prefabs.Tube2Prefab;
            case TubeType.LINE3:
                return prefabs.Tube3Prefab;
            case TubeType.LINE4:
                return prefabs.Tube4Prefab;
            // case TubeType.STATION:
                // return prefabs.Tube2StationPrefab;
            case TubeType.ANGEL:
                return prefabs.TubeAngelPrefab;
            case TubeType.BRIDGE:
                return prefabs.Tube1BridgePrefab;
            default:
                throw new ArgumentOutOfRangeException(nameof(tuneType), tuneType, null);
        }
    }
    public static Texture GetTexture(this TubeType tuneType, GameSettingsInstaller.TubeButtonSettings textures) {
        switch (tuneType) {
            case TubeType.LINE1:
                return textures.Line1;
            case TubeType.LINE2:
                return textures.Line2;
            case TubeType.LINE3:
                return textures.Line3;
            case TubeType.LINE4:
                return textures.Line4;
            // case TubeType.STATION:
                // return textures.Station;
            case TubeType.ANGEL:
                return textures.Angle;
            case TubeType.BRIDGE:
                return textures.Bridge;
            default:
                throw new ArgumentOutOfRangeException(nameof(tuneType), tuneType, null);
        }
    }
    
    public static int GetLenght(this TubeType tuneType) {
        switch (tuneType) {
            case TubeType.LINE1:
                return 1;
            case TubeType.LINE2:
                return 2;
            case TubeType.LINE3:
                return 3;
            case TubeType.LINE4:
                return 4;
            case TubeType.ANGEL:
                return 1;
            case TubeType.BRIDGE:
                return 1;
            default:
                throw new ArgumentOutOfRangeException(nameof(tuneType), tuneType, null);
        }
    }
}