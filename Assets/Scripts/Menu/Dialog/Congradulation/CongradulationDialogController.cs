using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CongradulationDialogController : ConfirmDialogController {
    
    [SerializeField] private Text RailwayLenght;
    [SerializeField] private Text WagonCount;
    [SerializeField] private Text Reward;

    public void Init(CongradulationDialogParam param) {
        base.Init(param);
        RailwayLenght.text = param.RailwayCount.ToString();
        WagonCount.text = param.WagonCount.ToString();
        Reward.text = param.Reward.ToString();
    }

    public class Factory : PlaceholderFactory<CongradulationDialogParam, CongradulationDialogController> { }
}