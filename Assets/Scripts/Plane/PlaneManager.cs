using UnityEngine;
using Zenject;

namespace Plane {
    public class PlaneManager {
        [Inject] 
        private GameSettingsInstaller.PrefabSettings _prefabSettings;
        [Inject]
        private PlaneController.Factory _planeFactory;

        private PlaneController plane;

        public void CreateIfNotExist() {
            if (plane == null) {
                plane = _planeFactory.Create(new PlaneParam(_prefabSettings.PlanePrefab ,new Vector2Int(4,4)));
            }
        }

        public void Clear() {
            if (plane != null) {
                Object.Destroy(plane.gameObject);
            }
        }
    }
}