using System;
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
    [Inject] private GameController controller;
    [Inject] private DialogManager _dialogManager;

    // private int nextLevel;

    private void Start() {
        // nextLevel = param.FileName;
        startBtn.onClick.AddListener(() => { animator.SetTrigger("change"); });
        backBtn.onClick.AddListener(() => { animator.SetTrigger("change"); });
        optionBtn.onClick.AddListener(() => { _dialogManager.OpenOptionDialog(); });
        // exitGameBtn.onClick.AddListener(() => { _dialogManager.OpenOptionDialog(); });
    }
}