using UnityEngine;
using Zenject;

namespace Plane {
    public class PlaneController : MonoBehaviour {
        
        
        public class Factory : PlaceholderFactory<PlaneParam, PlaneController> { }
    }
}