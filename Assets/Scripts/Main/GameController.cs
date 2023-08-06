using System;
using System.Collections.Generic;
using System.Linq;
using Level;
using MenuSystemWithZenject;
using UnityEngine;
using Zenject;
using UnityEngine.Analytics;

namespace Main {
    public enum GameStates {
        WaitingToStart,
        LevelStart,
        GoTrain,
        LevelComplete,
        GameOver
    }

    public class GameController : IInitializable, IDisposable {
        
        [Inject] private LevelManager _levelManager;
        [Inject] private LevelLoadManager _levelLoadManager;
        
        
        [Inject] private Menu<StartMenu>.Factory _startMenuFactory;
        [Inject] private Menu<GameMenu>.Factory _gameFactory;
        [Inject] private Menu<NovellSceneMenu>.Factory _novellSceneFactory;
        
        
        
        [Inject] private MicAudioPlayer _audioPlayer;
        [Inject] private DialogManager _dialogManager;
        [Inject] readonly TrainManager trainManager;
        [Inject] readonly TubeManager _tubeManager;
        [Inject] readonly MoneyService _moneyService;
        [Inject] readonly GameSettingsInstaller.PriceSetting _priceSetting;
        [Inject] private SignalBus _signalBus;

        GameStates _state = GameStates.WaitingToStart;

        private LevelPath currentLevel;
        private StartMenu startMenu;
        private GameMenu gameMenu;
        private NovellSceneMenu _novellSceneMenu;

        public void Initialize() {
            startMenu = _startMenuFactory.Create();
            startMenu.Open();
        }

        // private void OnStartLevel(LevelStartSignal obj) {
            // StartGame(Int32.Parse(obj.Level));
        // }

        public void Dispose() {
        }

        public void StartGame(LevelPath levelPath) {
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
            StartGame(currentLevel);
        }
        
        // public void ResetProgress() {
            // currentLevel = 0;
        // }
        
        public LevelPath CurrentLevel => currentLevel;

        public GameStates State {
            get => _state;
            set {
                _state = value;
                if (_state == GameStates.GoTrain) {
                    trainManager.Run();
                }

                if (_state == GameStates.LevelComplete) {
                    Debug.Log("LevelComplete " + currentLevel.Package + " " + currentLevel.Number);
                    int wagonCount = trainManager.WagonCount + 1;
                    int railwayCount = _tubeManager.Objects
                        .FindAll(q => q.Projection == TubeProjectionType.SIMPLE)
                        .Select(q=> q.TubeType.GetLenght()).Sum();
                    int reward = wagonCount * railwayCount;
                    _dialogManager.CongradulationDialog(wagonCount, railwayCount, reward);
                    _audioPlayer.DoComplete();
                    _moneyService.Plus(reward);
                    _levelLoadManager.Complete(currentLevel);
                    _signalBus.Fire(new LevelCompleteSignal(CurrentLevel));
                    AnalyticsEvent.LevelComplete(currentLevel.GetAnalyticNumber());
                    AnalyticsEvent.Custom(currentLevel.Package.ToString(), new Dictionary<string, object> {{"level_index", currentLevel.Number}});
                }
            }
        }

        public void LoadNext() {
            LevelBtnParam next = _levelLoadManager.FindFirstNotLocked(currentLevel.Package);
            Debug.Log("Стартуем следующий " +  currentLevel.Package + " " + next.FileName);
            StartGame(new LevelPath(next.FileName, currentLevel.Package));
        }

        public void GoToMenu() {
            gameMenu.OnBackPressed();
            _levelManager.ClearLevel();
            _state = GameStates.WaitingToStart;
        }
    }
}