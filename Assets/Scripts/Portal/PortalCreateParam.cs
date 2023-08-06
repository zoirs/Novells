using UnityEngine;

public class PortalCreateParam : CreateParam {

    private readonly int rotation;

    public PortalCreateParam(GameObject prefab, Vector2Int position, int rotation) : base(prefab, position) {
        this.rotation = rotation;
    }

    public int Rotation => rotation;
}