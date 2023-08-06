using PathCreation;
using UnityEngine;

namespace Train {
    public class FollowerController : MonoBehaviour {
        private PathCreator path;
        public float speed = 0.5f;
        private float distancetraveled;
        private int index;
        public float delta = 1.2f;

        public void Add(PathCreator p, int index) {
            this.index = index;
            this.path = p;
        }

        private void Update() {
            if (path == null) {
                return;
            }

            distancetraveled += speed * Time.deltaTime;

            transform.position = path.path.GetPointAtDistance(distancetraveled - index * delta);
            transform.rotation = path.path.GetRotationAtDistance(distancetraveled - index * delta);
        }
    }
}