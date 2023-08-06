using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class WagonCountController : MonoBehaviour {
    [SerializeField] private Text count;
    [SerializeField] private Button buy;

    [Inject] private TrainManager _trainManager;
    [Inject] private SignalBus _signalBus;
    [Inject] private DialogManager _dialogManager;

    private void Start() {
        buy.onClick.AddListener(() => _dialogManager.BuyDialog(TrainType.VAGON));
        count.text = _trainManager.WagonCount.ToString();
        _signalBus.Subscribe<PurchaseWagonSignal>(OnPurchase);
    }

    private void OnDestroy() {
        _signalBus.Unsubscribe<PurchaseWagonSignal>(OnPurchase);
    }

    private void OnPurchase(PurchaseWagonSignal obj) {
        count.text = _trainManager.WagonCount.ToString();
    }
}