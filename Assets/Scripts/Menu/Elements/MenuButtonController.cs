using System;
using Main;
using Map;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MenuSystemWithZenject.Elements {
    public class MenuButtonController : MonoBehaviour {

        [SerializeField] private Text text;
        [SerializeField] private Button button;
        [SerializeField] private GameObject Lock;
        [SerializeField] private GameObject Complete;

        [Inject] private GameController controller;
        [Inject] private GameSettingsInstaller.GameSetting setting;
        [Inject] private SignalBus _signalBus;

        private LevelPath level;

        private void Start() {
            button.onClick.AddListener(() => {
                Debug.Log(level);
                controller.StartGame(level);
            });
            _signalBus.Subscribe<LevelCompleteSignal>(OnCompleteLevel);
        }

        public void Init(LevelBtnParam levelData) {
            level = new LevelPath(levelData.FileName, levelData.LevelPackage);
            if (levelData.FileName == int.MinValue) {
                text.text = "Создать новый";
            } else {
                Debug.Log(text + " " + levelData);
                text.text = "Уровень " + levelData.FileName;
            }

            InitBtn(levelData.State);
        }

        private void InitBtn(LevelState levelDataState) {
            button.enabled = levelDataState != LevelState.LOCKED || setting.isDebug;
            Lock.SetActive(levelDataState == LevelState.LOCKED);
            Complete.SetActive(levelDataState == LevelState.COMPLETE);
        }

        private void OnCompleteLevel(LevelCompleteSignal completedLevel) {
            if (completedLevel.Level.IsSame(level)) {
                InitBtn(LevelState.COMPLETE);
            }

            int nextLevel = completedLevel.Level.Number + 1;
            if (level.Package == completedLevel.Level.Package && level.Number == nextLevel) {
                InitBtn(LevelState.ACTIVE);
            }
        }

        private void OnDestroy() {
            _signalBus.Unsubscribe<LevelCompleteSignal>(OnCompleteLevel);
        }

        public class Pool : MonoMemoryPool<MenuButtonController> { }

        public class Factory : PlaceholderFactory<LevelBtnParam, MenuButtonController> { }

    }
}