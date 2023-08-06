using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class StartButtonPanelController : MonoBehaviour {
    [SerializeField] GameObject exitBtn;
    [SerializeField] GameObject volumeOnBtn;
    [SerializeField] GameObject volumeOffBtn;
    [SerializeField] GameObject languageBtn;
    
    [Inject] private MicAudioPlayer _audioMicSettings;
    [Inject] readonly YesNoConfirmDialogController.Factory confirmDialogFactory;

    private void Start() {

        exitBtn.GetComponentInChildren<Button>().onClick.AddListener(() => {
            ConfirmDialogParam confirmDialog = new ConfirmDialogParam("$dlg.quit.header",
                () => { Application.Quit(); },
                () => { });
            confirmDialogFactory.Create(confirmDialog);
        });

        volumeOnBtn.GetComponentInChildren<Button>().onClick.AddListener(() => {
            volumeOnBtn.SetActive(false);
            volumeOffBtn.SetActive(true);
            _audioMicSettings.IsOn = true;
        });
        
        volumeOffBtn.GetComponentInChildren<Button>().onClick.AddListener(() => {
            volumeOnBtn.SetActive(true);
            volumeOffBtn.SetActive(false);
            _audioMicSettings.IsOn = true;
        });
        
        languageBtn.GetComponentInChildren<Button>().onClick.AddListener(() => {
        });
    }
}