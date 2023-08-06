using System.Collections.Generic;

public class TutorialParams {
    private List<TutorialStepDto> _levelTutorialSteps = new List<TutorialStepDto>();

    public void Add(TutorialStepDto tutorialStep) {
        _levelTutorialSteps.Add(tutorialStep);
    }
    
    public List<TutorialStepDto> LevelTutorialSteps => _levelTutorialSteps;
}