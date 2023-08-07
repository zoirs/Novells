using System.Collections;
using Main;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;
using Zenject;

public class LevelPackageBtn : MonoBehaviour {
    [SerializeField] private GameObject locked;
    [SerializeField] private Button startBtn;
    [SerializeField] private IAPButton iapButton;

    [Inject] private GameController controller;
    [Inject] private DialogManager dialogManager;
    [Inject] private SignalBus _signalBus;


    private void Start() {

        
        Init();

        startBtn.onClick.AddListener(() => {
                controller.StartGame(null);
        });
    }


    private void Init() {
        bool isForRealMoney = false;
        if (iapButton != null) {
            if (isForRealMoney) {
                iapButton.gameObject.SetActive(isForRealMoney);
                if (iapButton.gameObject.activeSelf) {
                    // iapButton.GetComponent<Image>().sprite = ButtonImageSprite();
                }   
            } else {
                // todo Так делать не корректно. Если была куплена одноразовая покупка, 
                //  а потом в какой либо версий эта кнопка исчезнет, то будем ловить 
                //  purchase not correctly processed for product
                Destroy(iapButton.gameObject);
            }
        }

    }
    
    IEnumerator WaitAndInit() {
        yield return 0;
        Init();
    }

}