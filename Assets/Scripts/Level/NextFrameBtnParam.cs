using System;

public class NextFrameBtnParam {

    private Action _callBack;
    private string _caption;

    public NextFrameBtnParam(Action callBack, string caption) {
        _caption = caption;
        _callBack = callBack;
    }

    public string Caption => _caption;

    public Action CallBack => _callBack;

    
}