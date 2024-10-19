using Main;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LevelPackageBtn : MonoBehaviour {
    [SerializeField] private Button startBtn;

    [Inject] private GameController controller;
    [Inject] private DialogManager dialogManager;
    [Inject] private SignalBus _signalBus;

    private void Start() {
        startBtn.onClick.AddListener(() => {
                controller.StartGame(null);
        });
    }
}