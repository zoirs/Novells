
using System;
using System.Collections.Generic;
using Level;
using Main;
using MenuSystemWithZenject.Elements;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class StartMenuMoreLevel : StartMenu {
    [SerializeField] private Dropdown levelType;
    // [SerializeField] private VerticalLayoutGroup content;
    
    [Inject] private LevelLoadManager _levelLoadManager;
    [Inject] private MenuButtonController.Factory pool;
        
    [Inject] private GameSettingsInstaller.GameSetting setting;

    private List<MenuButtonController> all = new List<MenuButtonController>();
    private void Start() {
        // Debug.Log("MenuButtonsManager initialize " + controller.Levels.Count);
        ;
        // LevelType current = (LevelType)Enum.Parse(typeof(LevelType), levelType.itemText.text);
        levelType.onValueChanged.AddListener(v => Init(v));
        levelType.value = 0;
        // Init();
    }

    private void Init(int v) {
        for (var i = all.Count - 1; i >= 0; i--) {
            MenuButtonController menuButtonController = all[i];
            all.Remove(menuButtonController);
            Destroy(menuButtonController.gameObject);
        }
        LevelPackage currentPackage = (LevelPackage) v;
        Debug.Log(currentPackage);
        foreach (LevelBtnParam level in _levelLoadManager.Levels[currentPackage]) {
            Debug.Log(currentPackage +" " + level.FileName);
            MenuButtonController menuButtonController = pool.Create(level);
            int index = level.FileName;
            menuButtonController.transform.SetSiblingIndex(index);
            menuButtonController.Init(level);
            all.Add(menuButtonController);
        }

        if (setting.isDebug) {
            LevelBtnParam levelBtnParam = new LevelBtnParam(int.MinValue,  currentPackage, LevelState.ACTIVE);
            MenuButtonController menuButtonController = pool.Create(levelBtnParam);
            menuButtonController.transform.SetSiblingIndex(0);
            menuButtonController.Init(levelBtnParam);
            all.Add(menuButtonController);
        }
    }
}