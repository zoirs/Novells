using UnityEngine;
using Zenject;

public class NextFrameButtonController : MonoBehaviour {
    private NextFrameBtnParam _nextFrameBtnParam;

    public void Init(NextFrameBtnParam createParam) {
        _nextFrameBtnParam = createParam;
    }

    public NextFrameBtnParam NextFrameBtnParam => _nextFrameBtnParam;

    public class Factory : PlaceholderFactory<NextFrameBtnParam, NextFrameButtonController> { }
}