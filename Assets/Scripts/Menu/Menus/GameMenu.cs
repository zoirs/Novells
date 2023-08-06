using MenuSystemWithZenject;
using MenuSystemWithZenject.Elements;
using UnityEngine;
using Zenject;

public class GameMenu : Menu<GameMenu> {
    [SerializeField] private LevelNameController levelName;

    [Inject] private TutorialController.Factory _tutorialFactory;

    private TutorialController _tutorial;

    public void LoadLevel(LevelPath levelPath) {
        levelName.LoadLevel(levelPath);

        if (levelPath.Package == LevelPackage.TUTORIAL && _tutorial == null) {
            LoadTutorial(levelPath);
        }
    }

    private void LoadTutorial(LevelPath levelPath) {
        string path = "Tutorial/" + levelPath.GetPath();
        Debug.Log(path);
        TextAsset levelasset = Resources.Load<TextAsset>(path);
        if (levelasset == null) {
            return;
        }

        LevelTutorialDto levelTutorialDto = JsonUtility.FromJson<LevelTutorialDto>(levelasset.text);
        if (levelTutorialDto == null || levelTutorialDto.steps == null || levelTutorialDto.steps.Count == 0) {
            return;
        }

        TutorialParams tutorialParams = new TutorialParams();
        foreach (TutorialStepDto stepDto in levelTutorialDto.steps) {
            tutorialParams.Add(stepDto);
        }

        _tutorial = _tutorialFactory.Create(tutorialParams);
    }
}