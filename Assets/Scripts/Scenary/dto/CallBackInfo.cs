
public class CallBackInfo {
    private int room;
    private int scene;
    private int frameIndex;

    private CallBackInfo() {
    }

    public CallBackInfo(int room, int scene, int frameIndex) {
        this.room = room;
        this.scene = scene;
        this.frameIndex = frameIndex;
    }

    public int getRoom() {
        return room;
    }

    public int getScene() {
        return scene;
    }

    public int getFrameIndex() {
        return frameIndex;
    }

    public string getFileName() {
        return "room_" + room + "_scene_" + scene + ".json";
    }
}
