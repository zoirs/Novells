using System;
using UnityEngine;

public enum StationType {
    SIMPLE
}

public static class StationTypeExtension {
    public static GameObject GetPrefab(this StationType stationType, GameSettingsInstaller.PrefabSettings prefabs) {
        Debug.Log(stationType + " ");
        switch (stationType) {
            case StationType.SIMPLE:
                return prefabs.Tube2StationPrefab;
            default:
                throw new ArgumentOutOfRangeException(nameof(stationType), stationType, null);
        }
    }

    public static Texture GetTexture(this StationType homeType, GameSettingsInstaller.TubeButtonSettings textures) {
        switch (homeType) {
            case StationType.SIMPLE:
                return textures.Station;
            default:
                throw new ArgumentOutOfRangeException(nameof(homeType), homeType, null);
        }
    }
}