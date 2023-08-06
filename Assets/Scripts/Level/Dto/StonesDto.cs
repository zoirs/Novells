using System;
using UnityEngine;

[Serializable]
public class StonesDto : BaseDto {
    public StoneType stoneType;

    public StonesDto(StoneType stoneType, Vector2Int position) : base(position) {
        this.stoneType = stoneType;
    }

    public override GameObject GetPrefab(GameSettingsInstaller.PrefabSettings prefabs) {
        return stoneType.GetPrefab(prefabs);
    }
}