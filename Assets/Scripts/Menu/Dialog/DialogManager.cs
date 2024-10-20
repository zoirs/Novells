using Main;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class DialogManager {
    [Inject] readonly YesNoConfirmDialogController.Factory confirmDialogFactory;
    // [Inject] readonly CongradulationDialogController.Factory congradulationDialogFactory;
    // [Inject] readonly PurchaseDialogController.Factory purchaseDialogFactory;
    [Inject] readonly OptionDialogController.Factory optionDialogFactory;
    [Inject] readonly InfoDialogController.Factory infoDialogController;
    [Inject] readonly PrivacyDialogController.Factory privacyDialogController;
    [Inject] readonly GameSettingsInstaller.GameSetting _setting;
    [Inject] readonly GameController gameController;

    public void OpenOptionDialog() {
        optionDialogFactory.Create(new OptionDialogParam());
    }
    public void OpenInfoDialog() {
        infoDialogController.Create(new InfoDialogParam("$dlg.info.body", () => { }));
    }
    
    public void CollectPrivacyDialog(UnityAction agree, UnityAction read)
    {
        privacyDialogController.Create(new PrivacyDialogParam("$dlg.privacy.text", agree, read));
    }
    
    public void OpenAboutDialog() {
        infoDialogController.Create(new InfoDialogParam("$dlg.info.about", () => {gameController.GoToMenu(); }));
    }

    public void CongradulationDialog(int wagonCount, int railwayLength, int reward) {
        CongradulationDialogParam congradulationDialogParam;
        if (_setting.isDebug) {
            congradulationDialogParam = new CongradulationDialogParam("$dlg.congradulation.header", railwayLength, wagonCount, reward,
                () => { gameController.State = GameStates.LevelStart; },
                () => { gameController.State = GameStates.LevelStart; });
        } else {
            bool hasAvailableNewPackage = false;
            string body = hasAvailableNewPackage ? "$dlg.congradulation.body.available.new.package" : "";
            congradulationDialogParam = new CongradulationDialogParam("$dlg.congradulation.body.level.complete", railwayLength, wagonCount, reward, body,
                () => { gameController.LoadNext(); },
                () => { gameController.GoToMenu(); }, "$dlg.congradulation.btn.next", "$dlg.congradulation.btn.menu");
        }
                    
       
    }

    public void OpenBuyMoneyDialog() {
    }

    public void NoMoneyDialog() {
        CreateYesNoDialog(null, "$dlg.no.money.body.text", "$dlg.no.money.btn.yes", "$dlg.no.money.btn.no", OpenBuyMoneyDialog);
    }

    public void CreateYesNoDialog(string caption, string body, string yesTextKey, string noTextKey, UnityAction yes) {
        CreateYesNoDialog(caption, body, yesTextKey, noTextKey, yes, () => { });
    }

    public void CreateYesNoDialog(string caption, string body, string yesTextKey, string noTextKey, UnityAction yes,
        UnityAction no) {
        ConfirmDialogParam buyMoneyQuestion = new ConfirmDialogParam(caption, body,
            yes,
            no,
            yesTextKey, noTextKey);
        confirmDialogFactory.Create(buyMoneyQuestion);
    }
}