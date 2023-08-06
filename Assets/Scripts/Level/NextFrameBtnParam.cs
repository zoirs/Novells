using System;

public class NextFrameBtnParam {

    private Action _callBack;

    public NextFrameBtnParam(Action callBack) {

        _callBack = callBack;
    }

    public Action CallBack => _callBack;

    
}