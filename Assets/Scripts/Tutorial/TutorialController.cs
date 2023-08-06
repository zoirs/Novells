using System;
using System.Collections;
using Main;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class TutorialController : MonoBehaviour {
    [SerializeField] private GameObject finger;
    [SerializeField] private Sprite simple;
    [SerializeField] private Sprite click;

    [Inject] private GameMapService _gameMapService;
    [Inject] private GameController _gameController;
    [Inject] private CountHintManager _hintManager;
    [Inject] private SignalBus _signalBus;

    private Image _imageComponent;
    private LTDescr _tween;
    private Vector3 _inventoryItemPosition;
    private TutorialCurrentStepInfo _currentStep;

    private void Start() {
        _signalBus.Subscribe<LevelCompleteSignal>(OnCompleteLevel);
        _signalBus.Subscribe<HintExecutedSignal>(OnHintExecuted);

        _imageComponent = finger.GetComponent<Image>();
    }

    private void DoTutorialAnimation() {
        if (_currentStep == null) {
            return;
        }

        finger.transform.position = _currentStep.FirstPosition();
        _imageComponent.sprite = simple;
        _tween = LeanTween.move(finger, _currentStep.SecondPosition(), 1f)
            .setDelay(0.5f)
            .setEase(LeanTweenType.easeOutQuad)
            .setOnComplete(OnCompleteTutorialAnim);
    }

    private void OnCompleteTutorialAnim() {
        if (_currentStep.ActionType() == TutorialActionType.MOVE) {
            _imageComponent.sprite = click;
        }

        _tween = LeanTween.move(finger, _currentStep.FirstPosition(), 1f)
            .setDelay(0.5f)
            .setEase(LeanTweenType.easeOutQuad)
            .setOnComplete(() => {
                if (_currentStep.ActionType() == TutorialActionType.CLICK) {
                    StartCoroutine(DoClickAndAction(DoTutorialAnimation));
                }

                if (_currentStep.ActionType() == TutorialActionType.MOVE) {
                    DoTutorialAnimation();
                    _imageComponent.sprite = simple;
                }
            });
    }


    IEnumerator DoClickAndAction(Action onCompleteAction) {
        yield return new WaitForSeconds(0.2f);
        _imageComponent.sprite = click;
        yield return new WaitForSeconds(0.5f);
        _imageComponent.sprite = simple;
        onCompleteAction();
    }

    public void Init(TutorialParams createParam) {
        _currentStep = new TutorialCurrentStepInfo(createParam.LevelTutorialSteps);
        NextStep();
    }

    private void Update() {
        if (_currentStep == null) {
            return;
        }

        if (_gameController.State == GameStates.GoTrain) {
            _currentStep = null;
            StopAllCoroutines();
            LeanTween.cancel(finger, _tween.id);
            Destroy(finger);
            return;
        }

        TutorialStepComplectionCondition condition = _currentStep.TutorialStepComplectionCondition();
        // if (condition.levelCompletion) {
        //     if (_gameController.State == GameStates.GoTrain) {
        //         LeanTween.cancel(finger, _tween.id);
        //         incStep();
        //         DoTutorialAnim();
        //         return;
        //     }
        // }

        if (condition.busyPoint != Vector3.zero) {
            if (_gameMapService.CheckPoint(condition.busyPoint)) {
                NextStep();
                return;
            }
        }
    }

    private void NextStep() {
        if (finger != null && _tween != null) {
            LeanTween.cancel(finger, _tween.id);
        }

        StopAllCoroutines();
        StartCoroutine(StartNextStep());
    }


    private IEnumerator StartNextStep() {
        TutorialCurrentStepInfo previewStep = _currentStep;
        _currentStep = null;
        yield return new WaitForSeconds(0.1f);
        _currentStep = previewStep.NextStep();
        TutorialStepComplectionCondition condition = _currentStep.TutorialStepComplectionCondition();
        if (condition.clickBtn == "HelpBtn") {
            _hintManager.AddHint(1);
        }

        DoTutorialAnimation();
    }


    private void OnCompleteLevel(LevelCompleteSignal obj) {
        Destroy(gameObject);
    }

    private void OnHintExecuted(HintExecutedSignal obj) {
        if (_currentStep == null) {
            return;
        }

        TutorialStepComplectionCondition condition = _currentStep.TutorialStepComplectionCondition();
        if (condition.clickBtn == "HelpBtn") {
            NextStep();
        }
    }

    private void OnDestroy() {
        StopAllCoroutines();
        _signalBus.Unsubscribe<LevelCompleteSignal>(OnCompleteLevel);
        _signalBus.Unsubscribe<HintExecutedSignal>(OnHintExecuted);
    }

    public class Factory : PlaceholderFactory<TutorialParams, TutorialController> { }
}