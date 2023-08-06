using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;
using Zenject;

namespace Hint {
    public class HintButtonController : MonoBehaviour {
        [Inject] private CountHintManager _countHintManager;
        [Inject] private HintManager _hintManager;
        [Inject] private SignalBus _signalBus;
        
        [SerializeField] 
        private Text count;
        [SerializeField] 
        Button helpBtn;

        private void Start() {
            _signalBus.Subscribe<PurchaseHintSignal>(OnPurchase);

            UpdateCounter();

            helpBtn.onClick.AddListener(() => {
                bool executed = _hintManager.GetOutHint();
                if (executed) {
                    UpdateCounter();
                }
                _signalBus.Fire(new HintExecutedSignal(executed));
            });
        }

        private void OnDestroy() {
            _signalBus.Unsubscribe<PurchaseHintSignal>(OnPurchase);
        }

        private void UpdateCounter() {
            count.text = _countHintManager.HintCount().ToString();
        }
        
        private void OnPurchase(PurchaseHintSignal obj) {
            UpdateCounter();
        }
    }
}