public class PurchasePackageSignal:BasePurchaseSignal {
    private LevelPackage _levelPackage;

    public PurchasePackageSignal(LevelPackage levelPackage) {
        this._levelPackage = levelPackage;
    }

    public LevelPackage LevelPackage => _levelPackage;
}