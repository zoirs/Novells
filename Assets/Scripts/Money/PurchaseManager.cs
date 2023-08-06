using Zenject;

public class PurchaseManager {
    [Inject] private MoneyService _moneyService;
    [Inject] private TrainManager _trainManager;
    [Inject] private CountHintManager _hintManager;
    [Inject] private SignalBus _signalBus;
    [Inject] private LevelPackageManager _levelPackageManager;
    [Inject] private DialogManager _dialogManager;

    public void BuyLevelBlockForVirtual(LevelPackage levelPackage, int price) {
        if (!_moneyService.CheckSum(price)) {
            _dialogManager.NoMoneyDialog();
            return;
        }

        _levelPackageManager.Buy(levelPackage);
        _moneyService.Minus(price);
        _signalBus.Fire(new PurchasePackageSignal(levelPackage));
    }

    public void BuyLevelBlockForReal(LevelPackage levelPackage) {
        _levelPackageManager.Buy(levelPackage);
        _signalBus.Fire(new PurchasePackageSignal(levelPackage));
    }

    public void BuyWagon(TrainType type, int price) {
        if (!_moneyService.CheckSum(price)) {
            _dialogManager.NoMoneyDialog();
            return;
        }

        _moneyService.Minus(price);
        _trainManager.AddWagon();
        _signalBus.Fire(new PurchaseWagonSignal(type));
    }

    public void BuyHint(int count, int price) {
        if (!_moneyService.CheckSum(price)) {
            _dialogManager.NoMoneyDialog();
            return;
        }

        _moneyService.Minus(price);
        _hintManager.AddHint(count);
        _signalBus.Fire(new PurchaseHintSignal(count));
    }
}