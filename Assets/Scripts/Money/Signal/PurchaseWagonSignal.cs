public class PurchaseWagonSignal:BasePurchaseSignal {
    private TrainType wagon;

    public PurchaseWagonSignal(TrainType wagon) {
        this.wagon = wagon;
    }

    public TrainType Wagon => wagon;
}