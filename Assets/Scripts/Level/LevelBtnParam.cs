using System;

public class LevelBtnParam {
    private readonly int fileName;
    private readonly LevelPackage levelPackage;

    public LevelBtnParam(int fName, LevelPackage levelPackage, LevelState levelState) {
        this.fileName = fName;
        this.levelPackage = levelPackage;
        this.State = levelState;
    }

    public int FileName => fileName;

    public LevelState State { get; set; }

    public LevelPackage LevelPackage => levelPackage;
}