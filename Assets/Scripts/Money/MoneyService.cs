using UnityEngine;
using Zenject;

public class MoneyService : IInitializable {
    private int balance;

    [Inject] private SignalBus _signalBus;

    public void Initialize() {
        balance = PlayerPrefs.GetInt(PlayerPrefsUtils.BALANCE);
    }

    public void Minus(int money) {
        balance = balance - money;
        PlayerPrefs.SetInt(PlayerPrefsUtils.BALANCE, balance);
        _signalBus.Fire(new MoneyChangeSignal(balance));
    }

    public void Plus(int money) {
        balance = balance + money;
        PlayerPrefs.SetInt(PlayerPrefsUtils.BALANCE, balance);
        _signalBus.Fire(new MoneyChangeSignal(balance));
    }

    public int Balance {
        get => balance;
    }

    public bool CheckSum(int price) {
        return balance >= price;
    }
}