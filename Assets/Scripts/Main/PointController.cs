using System;
using UnityEngine;

    public class PointController : MonoBehaviour {
 
        public Vector3Int GetVector() {
            Vector3 p = transform.position;
            return new Vector3Int((int) Math.Round(p.x, 0),(int) Math.Round(p.y, 0),(int) Math.Round( p.z, 0));
        }
}