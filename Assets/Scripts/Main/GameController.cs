using System;
using System.Collections.Generic;
using System.Linq;
using MenuSystemWithZenject;
using UnityEngine;
using Zenject;
using UnityEngine.Analytics;
using Object = System.Object;

namespace Main {
    public enum GameStates {
        WaitingToStart,
        LevelStart,
        GoTrain,
        LevelComplete,
        GameOver
    }

    public class GameController : IInitializable, IDisposable {
        
        [Inject] private Menu<StartMenu>.Factory _startMenuFactory;
        [Inject] private Menu<NovellSceneMenu>.Factory _novellSceneFactory;
        
        GameStates _state = GameStates.WaitingToStart;

        private StartMenu startMenu;
        private GameMenu gameMenu;
        private NovellSceneMenu _novellSceneMenu;

        public void Initialize() {
            Debug.Log("GameController");
            startMenu = _startMenuFactory.Create();
            startMenu.Open();
        }

        // private void OnStartLevel(LevelStartSignal obj) {
            // StartGame(Int32.Parse(obj.Level));
        // }

        public void Dispose() {
        }

        public void StartGame(Object levelPath) {
            // PlayerPrefs.DeleteKey(PlayerPrefsUtils.LevelKey(LevelPackage.TUTORIAL, 0));
            // PlayerPrefs.DeleteKey(PlayerPrefsUtils.LevelKey(LevelPackage.BRIDGE, 7));
            
            if (_novellSceneMenu == null) {
                _novellSceneMenu = _novellSceneFactory.Create();
            }
            // if (gameMenu == null) {
                // gameMenu = _gameFactory.Create();
            // }

            _novellSceneMenu.Open();
            // currentLevel = levelPath;
            // _levelManager.LoadLevel(currentLevel);
            // gameMenu.LoadLevel(currentLevel);
            // _state = GameStates.LevelStart;
            // AnalyticsEvent.LevelStart(currentLevel.GetAnalyticNumber());
        }
        
        public void RestartGame() {
            StartGame(null);
        }
        
        // public void ResetProgress() {
            // currentLevel = 0;
        // }
        

        public GameStates State {
            get => _state;
            set {
                _state = value;
                if (_state == GameStates.GoTrain) {
                    // trainManager.Run();
                }

                if (_state == GameStates.LevelComplete) {
                }
            }
        }

        public void LoadNext() {
            StartGame(null);
        }

        public void GoToMenu() {
            gameMenu.OnBackPressed();
            // _levelManager.ClearLevel();
            _state = GameStates.WaitingToStart;
        }
    }
}