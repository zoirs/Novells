public class PlayerPrefsUtils {
    
    public static string LANGUAGE = "lang";
    public static string BALANCE = "balance";
    public static string WAGON_COUNT = "wagon_count";
    public static string SOUNDS = "sounds";
    public static string MUSIC = "music";

    public static string LevelKey(LevelPath levelPath) {
        return LevelKey(levelPath.Package, levelPath.Number);
    }

    public static string LevelKey(LevelPackage levelPathPackage, int levelPathNumber) {
        return levelPathPackage + "_" + levelPathNumber;
    }

    public static string PurchasePackageKey(LevelPackage levelPathPackage) {
        return levelPathPackage.ToString();
    }
}