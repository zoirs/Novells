using UnityEngine;

public class TutorialStep {
    private Vector3 firstPosition;
    private Vector3 secondPosition;
    private TutorialActionType actionType;
    private TutorialStepComplectionCondition _tutorialStepComplectionCondition;

    public TutorialStep(Vector3 firstPosition, Vector3 secondPosition, TutorialActionType actionType,
        TutorialStepComplectionCondition tutorialStepComplectionCondition) {
        this.firstPosition = firstPosition;
        this.secondPosition = secondPosition;
        this.actionType = actionType;
        _tutorialStepComplectionCondition = tutorialStepComplectionCondition;
    }

    public Vector3 FirstPosition => firstPosition;

    public Vector3 SecondPosition => secondPosition;

    public TutorialActionType ActionType => actionType;

    public TutorialStepComplectionCondition TutorialStepComplectionCondition => _tutorialStepComplectionCondition;
}