using System;
using UnityEngine;

public enum RiverType {
    ONE,
    ANGEL
}

public static class RiverTypeExtension {
    
    public static GameObject GetPrefab(this RiverType riverType, GameSettingsInstaller.PrefabSettings prefabs) {
        Debug.Log(riverType + " ");
        switch (riverType) {
            case RiverType.ONE:
                return prefabs.riverPrefab;
            case RiverType.ANGEL:
                return prefabs.riverAngelPrefab;
            default:
                throw new ArgumentOutOfRangeException(nameof(riverType), riverType, null);
        }
    }
    
    public static Texture GetTexture(this RiverType riverType, GameSettingsInstaller.OtherItemTextureSettings textures) {
        switch (riverType) {
            case RiverType.ONE:
                return textures.River;
            case RiverType.ANGEL:
                return textures.RiverAngel;
            default:
                throw new ArgumentOutOfRangeException(nameof(riverType), riverType, null);
        }
    }
}
