using UnityEngine;

public abstract class BaseDto {
    public Vector2Int position;

    protected BaseDto(Vector2Int position) {
        this.position = position;
    }
    public abstract GameObject GetPrefab(GameSettingsInstaller.PrefabSettings prefabs);
}