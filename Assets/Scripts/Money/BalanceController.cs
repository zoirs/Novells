using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BalanceController : MonoBehaviour {
    [SerializeField] private Text balance;
    [SerializeField] private Button buy;

    [Inject] private MoneyService _moneyService;
    [Inject] private SignalBus _signalBus;  
    [Inject] private DialogManager _dialogManager;

    private void Start() {
        buy.onClick.AddListener(() => _dialogManager.OpenBuyMoneyDialog());
        balance.text = _moneyService.Balance.ToString();
        _signalBus.Subscribe<MoneyChangeSignal>(OnMoneyChange);
    }

    private void OnDestroy() {
        _signalBus.Unsubscribe<MoneyChangeSignal>(OnMoneyChange);
    }

    private void OnMoneyChange(MoneyChangeSignal obj) {
        balance.text = _moneyService.Balance.ToString();
    }
}