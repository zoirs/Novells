using Main;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class DialogManager {
    [Inject] readonly YesNoConfirmDialogController.Factory confirmDialogFactory;
    [Inject] readonly CongradulationDialogController.Factory congradulationDialogFactory;
    [Inject] readonly PurchaseDialogController.Factory purchaseDialogFactory;
    [Inject] readonly OptionDialogController.Factory optionDialogFactory;
    [Inject] readonly BuyMoneyDialogController.Factory _buyMoneyDialogFactory;
    [Inject] readonly GameSettingsInstaller.GameSetting _setting;
    [Inject] readonly PriceManager _priceManager;
    [Inject] readonly GameController gameController;
    [Inject] readonly PurchaseManager _purchaseManager;
    [Inject] private GameSettingsInstaller.PackagesTextureSettings textureSettings;
    [Inject] private GameSettingsInstaller.BuyItemTextureSettings buyItemTextureSettings;
    [Inject] private LevelPackageManager _levelPackageManager;

    public void OpenOptionDialog() {
        optionDialogFactory.Create(new OptionDialogParam());
    }

    public void CongradulationDialog(int wagonCount, int railwayLength, int reward) {
        CongradulationDialogParam congradulationDialogParam;
        if (_setting.isDebug) {
            congradulationDialogParam = new CongradulationDialogParam("$dlg.congradulation.header", railwayLength, wagonCount, reward,
                () => { gameController.State = GameStates.LevelStart; },
                () => { gameController.State = GameStates.LevelStart; });
        } else {
            bool hasAvailableNewPackage = _levelPackageManager.HasAvailableNewPackage();
            string body = hasAvailableNewPackage ? "$dlg.congradulation.body.available.new.package" : "";
            congradulationDialogParam = new CongradulationDialogParam("$dlg.congradulation.body.level.complete", railwayLength, wagonCount, reward, body,
                () => { gameController.LoadNext(); },
                () => { gameController.GoToMenu(); }, "$dlg.congradulation.btn.next", "$dlg.congradulation.btn.menu");
        }
                    
        if (gameController.CurrentLevel.Package == LevelPackage.SIMPLE && gameController.CurrentLevel.Number == 3) {
            CreateYesNoDialog("$dlg.rate.header", "$dlg.rate.desc", "$dlg.rate.yes", "$dlg.rate.tomenu", () => {
                Application.OpenURL("market://details?id=" + Application.identifier);
                gameController.LoadNext();
            },
                () => { gameController.GoToMenu(); });
        } else {
            congradulationDialogFactory.Create(congradulationDialogParam);
        }
    }

    public void OpenBuyMoneyDialog() {
        _buyMoneyDialogFactory.Create(new BuyHintDialogParam());
    }

    public void BuyDialog(LevelPackage levelPackage) {
        string prefix = "$dlg.buy.package.";
        int price = _priceManager.GetLevelPackagePrice(levelPackage);
        PurchaseDialogParam confirmDialog = new PurchaseDialogParam(prefix + levelPackage + ".name", prefix + levelPackage + ".desc",
            levelPackage.GetTexture(textureSettings), price,
            () => { _purchaseManager.BuyLevelBlockForVirtual(levelPackage, price); },
            () => { }, "$dlg.buy.yes", "$dlg.buy.no");
        purchaseDialogFactory.Create(confirmDialog);
    }
    
    public void BuyDialog(TrainType trainType) {
        string prefix = "$dlg.buy.";
        int price = _priceManager.GetWagonPrice();
        PurchaseDialogParam confirmDialog = new PurchaseDialogParam(prefix + trainType + ".name", prefix + trainType + ".desc", 
            buyItemTextureSettings.Wagon , price,
            () => { _purchaseManager.BuyWagon(trainType, price); },
            () => {  }, "$dlg.buy.yes", "$dlg.buy.no");
        purchaseDialogFactory.Create(confirmDialog);
    }
    
    public void BuyHintDialog() {
        int count = 10;
        int price = _priceManager.GetHintPrice(count);
        PurchaseDialogParam confirmDialog = new PurchaseDialogParam("$dlg.buy.hint.name", "$dlg.buy.hint.desc", 
            buyItemTextureSettings.Hint , price,
            () => { _purchaseManager.BuyHint(count, price); },
            () => {  }, "$dlg.buy.hint.yes", "$dlg.buy.hint.no");
        purchaseDialogFactory.Create(confirmDialog);
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