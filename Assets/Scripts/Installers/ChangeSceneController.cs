using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace.Installers {
    public class ChangeSceneController : MonoBehaviour {
        private void Start() {
            Debug.Log("============");
            SceneManager.LoadScene("MainScene");

        }
    }
}