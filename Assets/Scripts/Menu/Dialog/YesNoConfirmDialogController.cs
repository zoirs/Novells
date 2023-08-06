using Zenject;

public class YesNoConfirmDialogController : ConfirmDialogController {
   
    public class Factory : PlaceholderFactory<ConfirmDialogParam, YesNoConfirmDialogController> { }
}