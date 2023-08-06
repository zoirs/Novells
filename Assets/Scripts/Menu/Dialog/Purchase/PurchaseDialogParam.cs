using UnityEngine;
using UnityEngine.Events;

public class PurchaseDialogParam : ConfirmDialogParam {
    private string purchaseName;
    private string purchaseDesc;
    private Texture purchaseImage ;
    private int price;

    public PurchaseDialogParam(string purchaseName, string purchaseDesc, Texture purchaseImage, int price,
        UnityAction yes, UnityAction no, string yesText, string noText)
        : base("$dlg.buy.package.header", null, yes, no, yesText, noText) {
        this.price = price;
        this.purchaseName = purchaseName;
        this.purchaseDesc = purchaseDesc;
        this.purchaseImage = purchaseImage;
    }

    public string PurchaseName => purchaseName;

    public string PurchaseDesc => purchaseDesc;

    public Texture PurchaseImage => purchaseImage;

    public int Price => price;
}