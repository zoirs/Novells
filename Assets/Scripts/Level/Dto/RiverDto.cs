using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class RiverDto : BaseDto {
    
    public RiverType riverType;
    public int rotate;

    public RiverDto(RiverType riverType, Vector2Int position, int rotate) : base(position) {
        this.riverType = riverType;
        this.rotate = rotate;
    }

    public override GameObject GetPrefab(GameSettingsInstaller.PrefabSettings prefabs) {
        return riverType.GetPrefab(prefabs);
    }
}