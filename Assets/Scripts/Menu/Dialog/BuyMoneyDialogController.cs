using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BuyMoneyDialogController : MonoBehaviour {
    
    [SerializeField]
    private Button close;
    
    [Inject] private SignalBus _signalBus;

    private void Start() {
        _signalBus.Subscribe<BuyMoneySignal>(OnMoneyBuy);
        
        close.GetComponent<Button>().onClick.AddListener(() => {
            Destroy(gameObject);
        });
    }

    private void OnMoneyBuy() {
        Destroy(gameObject);
    }

    private void OnDestroy() {
        _signalBus.Unsubscribe<BuyMoneySignal>(OnMoneyBuy);
    }

    public class Factory : PlaceholderFactory<BuyHintDialogParam, BuyMoneyDialogController> {
        
    }
}