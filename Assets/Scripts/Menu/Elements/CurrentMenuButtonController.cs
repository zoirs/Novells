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


        private void Start() {
            button.onClick.AddListener(() => {
                controller.StartGame(null);
            });
        }

        public void Init(Object levelPath) {
        }
        
    }
}