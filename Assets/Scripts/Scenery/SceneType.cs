using System;

public enum SceneType {
    NONE,
    CENTER,
    LEFT,
    RIGHT,
    CHOOSE
}

public static class SceneTypeExtension {
    public static string GetTrigger(this SceneType tuneType) {
        switch (tuneType) {
            case SceneType.LEFT:
                return "Left";
            case SceneType.RIGHT:
                return "Right";
            case SceneType.CENTER:
                return "Base";
            default:
                throw new ArgumentOutOfRangeException(nameof(tuneType),
                    tuneType, null);
        }
    }
}