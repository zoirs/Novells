using System;
using UnityEngine;

[Serializable]

public class InventoryDto : BaseDto {
    public TubeType tubeType;
    public int rotate;
    public TubeProjectionType projection;

    public InventoryDto(TubeType tubeType, Vector2Int position, int rotate, TubeProjectionType projection) : base(position) {
        this.tubeType = tubeType;
        this.rotate = rotate;
        this.projection = projection;
    }

    public override GameObject GetPrefab(GameSettingsInstaller.PrefabSettings prefabs) {
        return tubeType.GetPrefab(prefabs);
    }
}