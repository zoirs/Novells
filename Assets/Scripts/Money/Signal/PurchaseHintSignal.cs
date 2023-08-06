public class PurchaseHintSignal : BasePurchaseSignal {
    private int count;

    public PurchaseHintSignal(int count) {
        this.count = count;
    }

    public int Count => count;
}