using System;
using Main;
using Map;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Object = System.Object;

namespace MenuSystemWithZenject.Elements {
    public class MenuButtonController : MonoBehaviour {

        [SerializeField] private Text text;
        [SerializeField] private Button button;
        [SerializeField] private GameObject Lock;
        [SerializeField] private GameObject Complete;

        [Inject] private GameController controller;
        [Inject] private GameSettingsInstaller.GameSetting setting;


        private void Start() {
            button.onClick.AddListener(() => {
                Debug.Log("===================");
                controller.StartGame(null);
            });
        }

        public void Init(Object levelData) {
        }

        private void InitBtn(Object levelDataState) {
        }

        

        public class Pool : MonoMemoryPool<MenuButtonController> { }

        public class Factory : PlaceholderFactory<Object, MenuButtonController> { }

    }
}