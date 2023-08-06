using System;
using Zenject;

public class PriceManager {
    [Inject] readonly GameSettingsInstaller.PriceSetting _priceSetting;
    [Inject] private TrainManager _trainManager;

    public int GetWagonPrice() {
        return TrainType.VAGON.Price(_priceSetting) * (int) Math.Pow(2, _trainManager.WagonCount);
    }

    public int GetLevelPackagePrice(LevelPackage levelPackage) {
        return levelPackage.Price(_priceSetting);
    }

    public int GetHintPrice(int count) {
        return count * _priceSetting.Hint;
    }
}