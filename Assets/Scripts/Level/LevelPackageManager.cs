using System;
using UnityEngine;
using Zenject;

public class LevelPackageManager {
    
    [Inject] private MoneyService _moneyService;
    [Inject] private GameSettingsInstaller.PriceSetting _priceSetting;

    public void Buy(LevelPackage levelPackage) {
        PlayerPrefs.SetInt(PlayerPrefsUtils.PurchasePackageKey(levelPackage), (int) LevelPachageStatus.OPEN);
    }

    public LevelPachageStatus GetStatus(LevelPackage levelPackage) {
        LevelPachageStatus actualStatus = (LevelPachageStatus) PlayerPrefs.GetInt(PlayerPrefsUtils.PurchasePackageKey(levelPackage));
        return actualStatus != LevelPachageStatus.NONE ? actualStatus : levelPackage.DefaultStatus();
    }

    public bool HasAvailableNewPackage() {
        foreach (LevelPackage levelPackage in Enum.GetValues(typeof(LevelPackage))) {
            LevelPachageStatus levelPachageStatus = GetStatus(levelPackage);
            if (levelPachageStatus == LevelPachageStatus.NEED_BUY_FOR_VIRTUAL_MONEY) {
                if (_moneyService.Balance > levelPackage.Price(_priceSetting)) {
                    return true;
                }
            }
        }

        return false;
    }
}