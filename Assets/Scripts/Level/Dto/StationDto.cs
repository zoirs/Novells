using System;
using UnityEngine;

[Serializable]
public class StationDto : BaseDto {
    public int rotate;
    public StationType stationType;

    public StationDto(StationType stationType, Vector2Int position, int itemRotate) : base(position) {
        this.stationType = stationType;
        this.rotate = itemRotate;
    }

    public override GameObject GetPrefab(GameSettingsInstaller.PrefabSettings prefabs) {
        return stationType.GetPrefab(prefabs);
    }
}