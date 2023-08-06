using UnityEngine;

public class TubeCreateParam : CreateParam {
    private readonly int rotation;
    private readonly TubeProjectionType projection;

    public TubeCreateParam(GameObject prefab, int rotation, Vector2Int position, TubeProjectionType projection) : base(prefab, position) {
        this.rotation = rotation;
        this.projection = projection;
    }

    public int Rotation => rotation;

    public TubeProjectionType Projection => projection;
}