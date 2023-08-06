using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class NextFrameButtonController : MonoBehaviour {

    [SerializeField] private Button _button;
    private NextFrameBtnParam _nextFrameBtnParam;

    public void Init(NextFrameBtnParam createParam) {
        _nextFrameBtnParam = createParam;
        _button.onClick.AddListener(() => {
            _nextFrameBtnParam.CallBack();
            // Destroy(gameObject);
        });
    }

    public NextFrameBtnParam NextFrameBtnParam => _nextFrameBtnParam;

    public class Factory : PlaceholderFactory<NextFrameBtnParam, NextFrameButtonController> { }
}