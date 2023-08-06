using UnityEngine;

public class RiverCreateParam : CreateParam {
    
    private readonly int rotation;

    public RiverCreateParam(GameObject prefab, Vector2Int position, int rotation) : base(prefab, position) {
        this.rotation = rotation;
    }
    public int Rotation => rotation;
}