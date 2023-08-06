using System;
using Level;
using Main;
using Zenject;
using MenuSystemWithZenject.Elements;
using UnityEngine;
using UnityEngine.UI;

public class StartOptionMenu : StartMenu {
    [SerializeField] private Button startBtn;
    [SerializeField] private Button optionBtn;
    [SerializeField] private Button backBtn;
    [SerializeField] private Button exitGameBtn;
    [SerializeField] private Animator animator;

    [Inject] private GameMenu.Factory _gameFactory;
    [Inject] private LevelLoadManager levelManager;
    [Inject] private GameController controller;
    [Inject] private DialogManager _dialogManager;
    [Inject] private SignalBus _signalBus;

    private int nextLevel;

    private void Start() {
        LevelPackage levelPackage = LevelPackage.SIMPLE; // todo
        LevelBtnParam param = levelManager.FindFirstNotLocked(levelPackage);
        nextLevel = param.FileName;
        startBtn.onClick.AddListener(() => { animator.SetTrigger("change"); });
        backBtn.onClick.AddListener(() => { animator.SetTrigger("change"); });
        optionBtn.onClick.AddListener(() => { _dialogManager.OpenOptionDialog(); });
        // exitGameBtn.onClick.AddListener(() => { _dialogManager.OpenOptionDialog(); });
        _signalBus.Subscribe<LevelCompleteSignal>(OnCompleteLevel);
        _signalBus.Subscribe<ResetProgressSignal>(OnResetProgress);
    }

    private void OnResetProgress() {
        nextLevel = 0;
    }

    private void OnCompleteLevel(LevelCompleteSignal obj) {
        nextLevel++;
    }

    private void OnDestroy() {
        _signalBus.Unsubscribe<LevelCompleteSignal>(OnCompleteLevel);
        _signalBus.Unsubscribe<ResetProgressSignal>(OnResetProgress);
    }
}