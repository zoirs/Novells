using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class InfoDialogController : MonoBehaviour {
    [SerializeField] private Button close;

    public class Factory : PlaceholderFactory<HelpDialogParam, InfoDialogController> { }

    public void Init(HelpDialogParam createParam) {
        close.GetComponent<Button>().onClick.AddListener(Close);
    }

    public void Close() {
        // if (needReload) {
        //     ReloadScene();
        // }

        Destroy(gameObject);
    }
}