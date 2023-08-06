using System;
using UnityEngine;

public enum LevelPackage {
    TUTORIAL,
    SIMPLE,
    STATIC,
    PORTAL,
    BRIDGE,
    CROSSROADS,
    SMALL,
}

public static class LevelBlockExtension {

    public static int Price(this LevelPackage levelPackage, GameSettingsInstaller.PriceSetting gameSetting) {
        switch (levelPackage) {
            case LevelPackage.TUTORIAL:
                return 0;
            case LevelPackage.SIMPLE:
                return gameSetting.LevelPackageSimple;
            case LevelPackage.STATIC:
                return gameSetting.LevelPackageStatic;
            case LevelPackage.PORTAL:
                return gameSetting.LevelPackagePortal;
            case LevelPackage.CROSSROADS:
                return gameSetting.LevelPackageCrossroad;
            case LevelPackage.BRIDGE:
                return gameSetting.LevelPackageBridge;
            case LevelPackage.SMALL:
                return gameSetting.LevelPackageSmall;
            default:
                throw new ArgumentOutOfRangeException(nameof(levelPackage), levelPackage, null);
        }
    }
    
    public static LevelPachageStatus DefaultStatus(this LevelPackage levelPackage) {
        switch (levelPackage) {
            case LevelPackage.TUTORIAL:
                return LevelPachageStatus.OPEN;
            case LevelPackage.SIMPLE:
                return LevelPachageStatus.NEED_BUY_FOR_VIRTUAL_MONEY;
            case LevelPackage.STATIC:
                return LevelPachageStatus.NEED_BUY_FOR_VIRTUAL_MONEY;
            case LevelPackage.PORTAL:
                return LevelPachageStatus.NEED_BUY_FOR_REAL_MONEY;
            case LevelPackage.CROSSROADS:
                return LevelPachageStatus.IN_DEVELOP;
            case LevelPackage.BRIDGE:
                return LevelPachageStatus.NEED_BUY_FOR_VIRTUAL_MONEY;
            case LevelPackage.SMALL:
                return LevelPachageStatus.IN_DEVELOP;
            default:
                throw new ArgumentOutOfRangeException(nameof(levelPackage), levelPackage, null);
        }
    } 
    
    public static Texture GetTexture(this LevelPackage levelPackage, GameSettingsInstaller.PackagesTextureSettings textureSettings) {
        switch (levelPackage) {
            case LevelPackage.SIMPLE:
                return textureSettings.Simple;
            case LevelPackage.STATIC:
                return textureSettings.Static;
            case LevelPackage.PORTAL:
                return textureSettings.Portal;
            case LevelPackage.BRIDGE:
                return textureSettings.Bridge;
            default:
                throw new ArgumentOutOfRangeException(nameof(levelPackage), levelPackage, null);
        }
    }
}