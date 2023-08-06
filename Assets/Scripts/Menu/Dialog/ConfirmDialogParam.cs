using UnityEngine.Events;

public class ConfirmDialogParam {
    private string caption;
    private string body;
    private UnityAction yes;
    private UnityAction no;
    
    private string yesText;
    private string noText;
    
    public ConfirmDialogParam(string body, UnityAction yes, UnityAction no) {
        this.body = body;
        this.yes = yes;
        this.no = no;
    }    
    public ConfirmDialogParam(string caption, string body, UnityAction yes, UnityAction no, string yesText , string noText) {
        this.caption = caption;
        this.body = body;
        this.yes = yes;
        this.no = no;
        this.yesText = yesText;
        this.noText = noText;
    }

    public string Body => body;

    public string Caption => caption;

    public UnityAction Yes => yes;

    public UnityAction No => no;
    
    public string NoText => noText;
    public string YesText => yesText;
    
    
}