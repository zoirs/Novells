using UnityEngine;
using UnityEngine.Purchasing;
using Zenject;

namespace DefaultNamespace {
    public class Purchaser {//: IStoreListener, IInitializable {
        
        
    //     private static IStoreController _storeController;
    //     private static IExtensionProvider _storeExtensionProvider;
    //     
    //     public void OnInitializeFailed(InitializationFailureReason error) {
    //         Debug.Log("OnInitializeFailed ==" + error);
    //     }
    //
    //     public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent) {
    //         Debug.Log("ProcessPurchase ==");
    //         return PurchaseProcessingResult.Complete;
    //     }
    //
    //     public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason) {
    //         Debug.Log("OnPurchaseFailed ==");
    //     }
    //
    //     public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    //     {
    //         Debug.Log("OnInitialized: PASS");
    //
    //         _storeController = controller;
    //         _storeExtensionProvider = extensions;
    //     }
    //
    //     public bool IsInitialized()
    //     {
    //         return _storeController != null && _storeExtensionProvider != null;
    //     }
    //     
    //     public void Initialize() {
    //         // Debug.Log("start Initialize");
    //         // var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
    //         // // builder.AddProduct(
    //         // //     "Brige",
    //         // //     ProductType.Consumable,
    //         // //     new IDs { { "Brige", GooglePlay.Name } });
    //         // UnityPurchasing.Initialize(this, builder);
    //
    //     }
    //     
    //     public void BuyBrige()
    //     {
    //         BuyProductID("Brige");
    //     }
    //
    //     void BuyProductID(string productId)
    //     {
    //         Debug.Log("BuyProductID");
    //
    //         if (IsInitialized())
    //         {
    //             Debug.Log("инициализированно");
    //             // Fire signal in the buy method
    //             // _signalBus.Fire<IAPSignal>();
    //
    //             Product product = _storeController.products.WithID(productId);
    //             Debug.Log(" продукт " + product);
    //
    //             if (product != null && product.availableToPurchase)
    //             {
    //                 Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
    //                 _storeController.InitiatePurchase(product);
    //             }
    //             else
    //             {
    //                 Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
    //             }
    //         }
    //         else
    //         {
    //             Debug.Log("BuyProductID FAIL. Not initialized.");
    //         }
    //     }
    //     
    }
}