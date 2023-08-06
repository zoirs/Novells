using System.Collections.Generic;
using UnityEngine;

public class TutorialCurrentStepInfo {
    private int currentStepIndex = -1;
    private TutorialStep currentStep;
    private List<TutorialStepDto> steps;

    public TutorialCurrentStepInfo(List<TutorialStepDto> steps) {
        this.steps = steps;
    }

    public TutorialCurrentStepInfo NextStep() {
        currentStepIndex++;
        if (steps.Count <= currentStepIndex) {
            return null;
        }

        Vector3 first = GetCursorPoint(steps[currentStepIndex].first);
        Vector3 second = GetCursorPoint(steps[currentStepIndex].second);
        TutorialStep step = new TutorialStep(first, second, steps[currentStepIndex].actionType,
            steps[currentStepIndex].completionCondition);
        currentStep = step;
        return this;
    }

    private static Vector3 GetCursorPoint(TutorialCursorPoint tutorialCursorPoint) {
        if (tutorialCursorPoint.objectName != null) {
            string objectName = tutorialCursorPoint.objectName
                .Replace("%LI", GameInstaller.LastInventoryNumber.ToString())
                .Replace("%LO", GameInstaller.LastTubeNumber.ToString());
            Vector3 transformPosition = GameObject.Find(objectName).transform.position;

            if (objectName.Contains("Obj_")) {
                Vector3 worldToScreenPoint = Camera.main.WorldToScreenPoint(transformPosition);
                return worldToScreenPoint;
            }

            return transformPosition;
        }

        if (tutorialCursorPoint.movePosition != null) {
            return Camera.main.WorldToScreenPoint(tutorialCursorPoint.movePosition);
        }

        return Vector3.zero;
    }

    public Vector3 FirstPosition() {
        return currentStep.FirstPosition;
    }

    public Vector3 SecondPosition() {
        return currentStep.SecondPosition;
    }

    public TutorialActionType ActionType() {
        return currentStep.ActionType;
    }

    public TutorialStepComplectionCondition TutorialStepComplectionCondition() {
        return currentStep.TutorialStepComplectionCondition;
    }
}