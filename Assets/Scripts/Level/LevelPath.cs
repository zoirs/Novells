public class LevelPath {
    private int number;
    private LevelPackage package;

    public LevelPath(int number, LevelPackage package) {
        this.number = number;
        this.package = package;
    }

    public bool IsSame(LevelPath levelPath) {
        return levelPath != null
               && levelPath.number == number
               && levelPath.package == package;
    }

    public int Number => number;

    public LevelPackage Package => package;

    public string GetPath() {
        return package + "/" + number;
    }

    public LevelPath Next() {
        return new LevelPath(number + 1, package);
    }

    public int GetAnalyticNumber() {
        return (int) package * 1000 + number;
    }
}