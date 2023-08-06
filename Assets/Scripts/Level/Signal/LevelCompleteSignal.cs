public class LevelCompleteSignal {
    private LevelPath level;

    public LevelCompleteSignal(LevelPath level) {
        this.level = level;
    }

    public LevelPath Level => level;
}