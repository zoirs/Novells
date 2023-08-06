using UnityEngine;
using UnityEngine.UI;

public class LevelTypeMoney : MonoBehaviour {
    [SerializeField] private Image icon;
    [SerializeField] private Text price;

    public void SetPrice(int value) {
        price.text = value.ToString();
    }
}