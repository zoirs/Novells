using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class AvailableCountIndicator : MonoBehaviour {
    
    [SerializeField] private Text count;

    [Inject] private SignalBus _signalBus;
    [Inject] private PriceManager _priceManager;
    [Inject] private MoneyService _moneyService;

    private void Start() {
        _signalBus.Subscribe<MoneyChangeSignal>(OnMoneyEvent);
        UpdateCounter();
    }

    private void OnDestroy() {
        _signalBus.Unsubscribe<MoneyChangeSignal>(OnMoneyEvent);
    }

    private void OnMoneyEvent(MoneyChangeSignal obj) {
        UpdateCounter();
    }

    private void UpdateCounter() {
        int availableCount = _moneyService.Balance / _priceManager.GetWagonPrice();
        gameObject.SetActive(availableCount > 0);
        count.text = (availableCount > 0 ? 1 : 0).ToString();
    }
}