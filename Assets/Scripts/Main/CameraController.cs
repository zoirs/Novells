using UnityEngine;

public class CameraController : MonoBehaviour {
    private void Start() {
        Camera camera = GetComponent<Camera>();
        float baseOrthographicWidthSize = 9 / 2080f * 1040f;
        float newOrthographicSize = baseOrthographicWidthSize / Screen.width * Screen.height;
        camera.orthographicSize = newOrthographicSize;
    }
}