using System;

public class NextFrameBtnParam {
    private readonly int room;
    private readonly int scene;
    private readonly int frame;

    public NextFrameBtnParam(int room, int scene, int frame) {
        this.room = room;
        this.scene = scene;
        this.frame = frame;
    }

    public int Room => room;

    public int Scene => scene;

    public int Frame => frame;
    
    public String getFileName() {
        return "room_" + room + "_scene_" + scene;
    }
}