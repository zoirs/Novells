using UnityEngine;

public abstract class CreateParam {
    private GameObject prefab;
    private Vector2Int position;

    public CreateParam(GameObject prefab, Vector2Int position) {
        this.prefab = prefab;
        this.position = position;
    }

    public GameObject Prefab => prefab;

    public Vector2Int Position => position;
}