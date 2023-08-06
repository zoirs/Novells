using System;
using UnityEngine;

[Serializable]
public class TrainDto : BaseDto {
    public TrainType trainType;

    public TrainDto(TrainType trainType, Vector2Int position) : base(position) {
        this.trainType = trainType;
    }

    public override GameObject GetPrefab(GameSettingsInstaller.PrefabSettings prefabs) {
       return trainType.GetPrefab(prefabs);
    }
}