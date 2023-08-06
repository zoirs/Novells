using System;
using Level;
using Zenject;
using MenuSystemWithZenject.Elements;
using UnityEngine;

public class StartMenuOneLevel : StartMenu
{
    [SerializeField] private CurrentMenuButtonController _button;

    [Inject]
    private GameMenu.Factory _gameFactory;
    [Inject] 
    private LevelLoadManager levelManager;

    private void Start() {
        LevelBtnParam param = levelManager.FindFirstNotLocked(LevelPackage.SIMPLE);
        _button.Init(new LevelPath(param.FileName, LevelPackage.SIMPLE)); // todo package
    }

    public void OnStartClick()
    {
        _gameFactory.Create().Open();
    }
}
