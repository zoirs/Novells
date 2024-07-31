using Main;
using MenuSystemWithZenject;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Button))]
    public class ExitController : MonoBehaviour {
        
        [Inject] private GameController controller;

        private void Start() {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                controller.GoToMenu();
            });
        }
}