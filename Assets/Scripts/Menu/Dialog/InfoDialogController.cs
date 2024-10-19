using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class InfoDialogController : MonoBehaviour {
    [SerializeField] private Button close;
    [SerializeField] private TextLocalized body;

    public class Factory : PlaceholderFactory<InfoDialogParam, InfoDialogController> { }
    public class FactoryPrivacy : PlaceholderFactory<InfoDialogParam, InfoDialogController> { }

    public void Init(InfoDialogParam param) {
        if (body != null)
        {
            body.SetText(param.Body);
        }

        close.onClick.AddListener(() =>
        {
            param.Close.Invoke();
            Close();
        });
    }

    public void Close() {
        Destroy(gameObject);
    }
}