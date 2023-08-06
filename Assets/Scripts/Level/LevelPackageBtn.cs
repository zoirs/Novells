using System.Collections;
using Base;
using Level;
using Main;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;
using Zenject;

public class LevelPackageBtn : MonoBehaviour {
    [SerializeField] private GameObject locked;
    [SerializeField] private LevelTypeMoney levelTypeMoneyVirtual; 
    [SerializeField] private LevelTypeMoney levelTypeMoneyReal; 
    [SerializeField] private LevelPackage levelPackage;
    [SerializeField] private Button startBtn;
    [SerializeField] private IAPButton iapButton;

    [Inject] private GameSettingsInstaller.PriceSetting _priceSetting;
    [Inject] private LevelLoadManager levelLoadManager;
    [Inject] private GameController controller;
    [Inject] private DialogManager dialogManager;
    [Inject] private LevelPackageManager _levelPackageManager;
    [Inject] private PurchaseManager _purchaseManager;
    [Inject] private GameSettingsInstaller.MenuButtonSpritesSettings buttonSprites;
    [Inject] private SignalBus _signalBus;

    private LevelPachageStatus _status;

    private void Start() {
        _signalBus.Subscribe<PurchasePackageSignal>(OnPurchase);
        _signalBus.Subscribe<ResetProgressSignal>(OnReset);

        _status = _levelPackageManager.GetStatus(levelPackage);
        
        Init();

        startBtn.onClick.AddListener(() => {
            Debug.Log(_status);
            if (_status == LevelPachageStatus.OPEN) {
                // LevelBtnParam param = levelLoadManager.FindFirstNotLocked(levelPackage);
                // if (param == null) {
                    // return;
                // }
                controller.StartGame(null);
            }

            if (_status == LevelPachageStatus.NEED_BUY_FOR_VIRTUAL_MONEY) {
                dialogManager.BuyDialog(levelPackage);
            }
        });
    }


    private void Init() {
        bool isForRealMoney = _status == LevelPachageStatus.NEED_BUY_FOR_REAL_MONEY;
        Debug.Log("init " + _status + " " + levelPackage + " forRealMoney == " + isForRealMoney);
        if (iapButton != null) {
            if (isForRealMoney) {
                iapButton.gameObject.SetActive(isForRealMoney);
                if (iapButton.gameObject.activeSelf) {
                    iapButton.GetComponent<Image>().sprite = ButtonImageSprite();
                }   
            } else {
                // todo Так делать не корректно. Если была куплена одноразовая покупка, 
                //  а потом в какой либо версий эта кнопка исчезнет, то будем ловить 
                //  purchase not correctly processed for product
                Destroy(iapButton.gameObject);
            }
        }

        startBtn.gameObject.SetActive(!isForRealMoney);
        if (startBtn.gameObject.activeSelf) {
            startBtn.GetComponent<Image>().sprite = ButtonImageSprite();
        }

        startBtn.interactable = _status != LevelPachageStatus.NONE && _status != LevelPachageStatus.IN_DEVELOP;
        locked.SetActive(_status == LevelPachageStatus.IN_DEVELOP);
        levelTypeMoneyVirtual.gameObject.SetActive(_status == LevelPachageStatus.NEED_BUY_FOR_VIRTUAL_MONEY);
        if (levelTypeMoneyVirtual.gameObject.activeSelf) {
            levelTypeMoneyVirtual.SetPrice(levelPackage.Price(_priceSetting));
        }
        levelTypeMoneyReal.gameObject.SetActive(_status == LevelPachageStatus.NEED_BUY_FOR_REAL_MONEY);
    }

    private Sprite ButtonImageSprite() {
        if (_status == LevelPachageStatus.OPEN) {
            return buttonSprites.openButton;
        }
        if (_status == LevelPachageStatus.NEED_BUY_FOR_VIRTUAL_MONEY) {
            return buttonSprites.needBuyVirtualMoneyButton;
        }
        if (_status == LevelPachageStatus.NEED_BUY_FOR_REAL_MONEY) {
            return buttonSprites.needBuyRealMoneyButton;
        }
        return buttonSprites.developButton;
    }

    private void OnReset(ResetProgressSignal obj) {
        _status = levelPackage.DefaultStatus();
        Init();
    }

    private void OnPurchase(PurchasePackageSignal obj) {
        Debug.Log("was buy " + obj.LevelPackage);
        if (levelPackage == obj.LevelPackage) {
            Debug.Log("update status ");
            _status = LevelPachageStatus.OPEN;
            StartCoroutine(WaitAndInit());
        }
    }
    
    public void OnPurchaseAdsOffFail(Product args, PurchaseFailureReason reason) {
        Debug.Log("OnPurchaseAdsOffFail " + args + ", " + reason);
    }

    public void OnPurchaseAdsOffSuccess(Product args) {
        Debug.Log("Callback purchasing " + args);
        string definitionID = args.definition.id;
        Debug.Log("Callback purchasing " + definitionID);
        
        if (definitionID == IAPcatalog.LEVEL_BRIGE) {
            _purchaseManager.BuyLevelBlockForReal(levelPackage);
        }
        if (definitionID == IAPcatalog.LEVEL_TUNNEL) {
            _purchaseManager.BuyLevelBlockForReal(levelPackage);
        }
    }
    
    IEnumerator WaitAndInit() {
        yield return 0;
        Init();
    }

    private void OnDestroy() {
        _signalBus.Unsubscribe<PurchasePackageSignal>(OnPurchase);
        _signalBus.Unsubscribe<ResetProgressSignal>(OnReset);
    }
}