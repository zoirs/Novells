using System;
using Main;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class OptionDialogController : MonoBehaviour {
    
    [SerializeField]
    private Toggle music;
    [SerializeField]
    private Toggle sounds;
    [SerializeField]
    private Button rate;
    [SerializeField]
    private Button resetProgressBtn;
    [SerializeField]
    private Dropdown language;
    [SerializeField]
    private Button close;
    
    [Inject] private MicAudioPlayer _audioMicSettings;
    [Inject] private DialogManager _dialogManager;
    [Inject] private SignalBus _signalBus;
    
    private static readonly string ON_KEY = "$dlg.setting.btn.on";
    private static readonly string OFF_KEY = "$dlg.setting.btn.off";

    private bool needReload;
    public class Factory : PlaceholderFactory<OptionDialogParam, OptionDialogController> { }

    public void Init(OptionDialogParam createParam) {
        if (!PlayerPrefs.HasKey(PlayerPrefsUtils.MUSIC)) {
            PlayerPrefs.SetInt(PlayerPrefsUtils.MUSIC, 1);
        }
        if (!PlayerPrefs.HasKey(PlayerPrefsUtils.SOUNDS)) {
            PlayerPrefs.SetInt(PlayerPrefsUtils.SOUNDS, 1);
        }

        bool isMusicOn = PlayerPrefs.GetInt(PlayerPrefsUtils.MUSIC) > 0;
        music.SetIsOnWithoutNotify(isMusicOn);
        music.GetComponentInChildren<TextMeshProUGUI>().text = LocalisationSystem.GetLocalisedValue(isMusicOn ? ON_KEY : OFF_KEY, 1);
        music.onValueChanged.AddListener(v => {
            music.GetComponentInChildren<TextMeshProUGUI>().text = LocalisationSystem.GetLocalisedValue(v ? ON_KEY : OFF_KEY, 1);
            PlayerPrefs.SetInt(PlayerPrefsUtils.MUSIC, v ? 1 : 0);
        });

        bool isSoundOn = PlayerPrefs.GetInt(PlayerPrefsUtils.SOUNDS) > 0;
        sounds.GetComponentInChildren<TextLocalized> ().SetText(isSoundOn ? ON_KEY : OFF_KEY);
        sounds.SetIsOnWithoutNotify(isSoundOn);
        sounds.onValueChanged.AddListener(v => {
            sounds.GetComponentInChildren<TextLocalized> ().SetText(v ? ON_KEY : OFF_KEY);
            PlayerPrefs.SetInt(PlayerPrefsUtils.SOUNDS, v ? 1 : 0);
            _audioMicSettings.IsOn = v;
        });
        
        int lang = PlayerPrefs.GetInt(PlayerPrefsUtils.LANGUAGE);
        language.SetValueWithoutNotify(lang - 1);
        language.onValueChanged.AddListener(value => {
            PlayerPrefs.SetInt(PlayerPrefsUtils.LANGUAGE, value + 1);
            LocalisationSystem.RefreshLanguage();
            needReload = true;
            Close();
        });

        resetProgressBtn.onClick.AddListener(() => {
            _dialogManager.CreateYesNoDialog("$dlg.reset.header.text", "$dlg.reset.body.text", "$dlg.reset.btn.yes", "$dlg.reset.btn.no", () => {
                ResetProgress();
                ReloadScene();
            });
        });

        rate.onClick.AddListener(() => {
            _dialogManager.CreateYesNoDialog("$dlg.rate.header", "$dlg.rate.desc", "$dlg.rate.yes", "$dlg.rate.tomenu",
                () => { Application.OpenURL("market://details?id=" + Application.identifier); });
        });

        close.GetComponent<Button>().onClick.AddListener(Close);
    }

    public void Close() {
        if (needReload) {
            ReloadScene();
        }

        Destroy(gameObject);
    }

    private void ResetProgress() {
        for (int i = 0; i < 10; i++)
        {
            PlayerPrefs.DeleteKey(PlayerPrefsUtils.STORY_PREFIX + i);
        }
    }
    
    public void ReloadScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}