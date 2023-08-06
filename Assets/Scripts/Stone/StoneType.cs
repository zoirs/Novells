using System;
using UnityEngine;

public enum StoneType {
    ROCK,
    STONE,
    TREE
}

public static class StoneTypeExtension {
    
    public static GameObject GetPrefab(this StoneType homeType, GameSettingsInstaller.PrefabSettings prefabs) {
        Debug.Log(homeType + " ");
        switch (homeType) {
            case StoneType.ROCK:
                return prefabs.RockPrefab;
            case StoneType.TREE:
                return prefabs.TreePrefab;
            case StoneType.STONE:
                return prefabs.StonePrefab;
            default:
                throw new ArgumentOutOfRangeException(nameof(homeType), homeType, null);
        }
    }
    
    public static Texture GetTexture(this StoneType homeType, GameSettingsInstaller.OtherItemTextureSettings textures) {
        switch (homeType) {
            case StoneType.ROCK:
                return textures.Rock;
            case StoneType.TREE:
                return textures.Tree;
            case StoneType.STONE:
                return textures.Stone;
            default:
                throw new ArgumentOutOfRangeException(nameof(homeType), homeType, null);
        }
    }
}
