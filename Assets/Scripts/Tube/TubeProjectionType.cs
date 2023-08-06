using System;

public enum TubeProjectionType {
    SIMPLE,
    STATIC,
    HINT,
    DECORATE,
}

public static class TubeProjectionTypeExtension {
    public static bool NotMovable(this TubeProjectionType tuneType) {
        switch (tuneType) {
            case TubeProjectionType.SIMPLE:
                return false;
            case TubeProjectionType.STATIC:
                return true;
            case TubeProjectionType.DECORATE:
                return true;
            case TubeProjectionType.HINT:
                return true;
            default:
                throw new ArgumentOutOfRangeException(nameof(tuneType), tuneType, null);
        }
    }
}