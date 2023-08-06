using System.Collections.Generic;
using UnityEngine;

public class PathParam {
    private List<Vector2> points;

    public PathParam(List<Vector2> points) {
        this.points = points;
    }

    public List<Vector2> Points => points;
}