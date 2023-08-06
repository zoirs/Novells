using System;

[Serializable]
public class TutorialStepDto {
    public TutorialCursorPoint first;
    public TutorialActionType actionType;
    public TutorialCursorPoint second;
    public TutorialStepComplectionCondition completionCondition ;
}