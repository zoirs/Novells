using System;
using UnityEngine;
using Zenject;

public class StoneController : MonoBehaviour {
 
    [SerializeField]
    private StoneType stoneType;
    
    public Vector3Int GetVector() {
        Vector3 p = transform.position;
        return new Vector3Int((int) Math.Round(p.x, 0),(int) Math.Round(p.y, 0),(int) Math.Round( p.z, 0));
    }
    
    public class Factory : PlaceholderFactory<StoneCreateParam, StoneController> { }

    public StoneType StoneType => stoneType;
}