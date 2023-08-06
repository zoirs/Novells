using Base;
using UnityEngine;
using UnityEngine.Purchasing;
using Zenject;

namespace Hint {
    public class MoneyPurchaseController : MonoBehaviour {

        [Inject] private SignalBus _signalBus;
        [Inject] private MoneyService _moneyService;
        
        public void OnPurchaseAdsOffFail(Product args, PurchaseFailureReason reason) {
            Debug.Log("Callback purchasing " + reason);
            _signalBus.Fire(new BuyMoneySignal(null));
        }

        public void OnPurchaseAdsOffSuccess(Product args) {
            Debug.Log("Callback purchasing " + args);
            string definitionID = args.definition.id;
            Debug.Log("Callback purchasing " + definitionID);
            
            if (definitionID == IAPcatalog.money_100) {
                _moneyService.Plus(100);
            }
            if (definitionID ==  IAPcatalog.money_250) {
                _moneyService.Plus(250);
            }
            if (definitionID ==  IAPcatalog.money_1000) {
                _moneyService.Plus(1000);
            }

            _signalBus.Fire(new BuyMoneySignal(definitionID));
        }
    }
}