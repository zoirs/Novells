using System;
using UnityEngine;

[Serializable]
public class PortalDto : BaseDto {
    public int rotate;

    public PortalDto(Vector2Int position, int rotate) : base(position) {
        this.rotate = rotate;
    }

    public override GameObject GetPrefab(GameSettingsInstaller.PrefabSettings prefabs) {
        return prefabs.PortalPrefab;
    }
}