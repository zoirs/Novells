using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PurchaseDialogController : ConfirmDialogController {
    
    [SerializeField] private TextLocalized PurchaseName;
    [SerializeField] private TextLocalized PurchaseDesc;
    [SerializeField] private RawImage purchaseImage;
    [SerializeField] private Text Price;

    public void Init(PurchaseDialogParam param) {
        base.Init(param);
        Price.text = param.Price.ToString();
        PurchaseName.SetText(param.PurchaseName);
        PurchaseDesc.SetText(param.PurchaseDesc);
        purchaseImage.texture = param.PurchaseImage;
    }

    public class Factory : PlaceholderFactory<PurchaseDialogParam, PurchaseDialogController> { }
}