using MenuSystemWithZenject;
using MenuSystemWithZenject.Elements;
using UnityEngine;
using Zenject;
using Object = System.Object;

public class GameMenu : Menu<GameMenu> {
    [SerializeField] private LevelNameController levelName;


    public void LoadLevel(Object levelPath) {
        levelName.LoadLevel(levelPath);

    }

}