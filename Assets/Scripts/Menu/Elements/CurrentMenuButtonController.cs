using Main;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MenuSystemWithZenject.Elements {
    
    //todo вроде бы более не испольлзуется
    public class CurrentMenuButtonController : MonoBehaviour {
        [SerializeField] private Text text;
        [SerializeField] private Button button;

        private bool hideLevelNumber;
        
        [Inject] private GameController controller;
        [Inject] private SignalBus _signalBus;

        private LevelPath level;

        private void Start() {
            button.onClick.AddListener(() => {
                Debug.Log(level);
                controller.StartGame(level);
            });
            _signalBus.Subscribe<LevelCompleteSignal>(OnCompleteLevel);
        }

        public void Init(LevelPath levelPath) {
            level = levelPath;
            if (!hideLevelNumber) {
                text.text = "Уровень " + levelPath.Package + " "+ levelPath.Number;
            }
        }
        
        private void OnCompleteLevel(LevelCompleteSignal obj) {
            Init(obj.Level.Next());
        }

        private void OnDestroy() {
            _signalBus.Unsubscribe<LevelCompleteSignal>(OnCompleteLevel);
        }
    }
}