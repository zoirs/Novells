using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PrivacyDialogController : MonoBehaviour {
    [SerializeField] private Button agree;
    [SerializeField] private Button read;
    [SerializeField] private TextLocalized body;

    public class Factory : PlaceholderFactory<PrivacyDialogParam, PrivacyDialogController> { }

    public void Init(PrivacyDialogParam param) {
        if (body != null)
        {
            body.SetText(param.Body);
        }

        agree.onClick.AddListener(() =>
        {
            param.Agree.Invoke();
            Close();
        });
        read.onClick.AddListener(() =>
        {
            param.Read.Invoke();
        });
    }

    public void Close() {
        Destroy(gameObject);
    }
}