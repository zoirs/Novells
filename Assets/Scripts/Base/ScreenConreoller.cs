using UnityEngine;

namespace Base {
    public class ScreenConreoller : MonoBehaviour {

        private void Update() {
            if (Input.GetKeyDown("space")) {
                string filename = "SomeLevel_"+ UnityEngine.Random.Range(1,10000) +".png";
                ScreenCapture.CaptureScreenshot(filename);
                Debug.Log(Application.persistentDataPath + "/" + filename);
            }
        }
    }
}