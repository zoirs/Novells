public class SceneData {
    private SceneType sceneType;
    private string leftHero;
    private string rightHero;
    private string text;
    private string place;

    private SceneData(SceneType sceneType, string leftHero, string rightHero, string text, string place) {
        this.sceneType = sceneType;
        this.leftHero = leftHero;
        this.rightHero = rightHero;
        this.text = text;
        this.place = place;
    }

    public static SceneData SpeakLeft(string text, string hero) {
        return new SceneData(SceneType.LEFT, hero, null, text, null);
    }

    public static SceneData SpeakRight(string text, string hero) {
        return new SceneData(SceneType.RIGHT, null, hero, text, null);
    }

    public static SceneData Description(string text, string place) {
        return new SceneData(SceneType.CENTER, null, null, text, place);
    }

    public SceneType SceneType => sceneType;

    public string HeroName {
        get {
            if (sceneType == SceneType.LEFT) {
                return leftHero;
            }

            if (sceneType == SceneType.RIGHT) {
                return rightHero;
            }

            return null;
        }
    }

    public string Text => text;

    public string Place => place;
}