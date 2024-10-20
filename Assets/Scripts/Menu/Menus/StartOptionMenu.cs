using Main;
using MenuSystemWithZenject;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class StartOptionMenu : StartMenu
{
    [SerializeField] private Button startBtn;
    [SerializeField] private Button optionBtn;
    [SerializeField] private Button backBtn;
    [SerializeField] private Button exitGameBtn;
    [SerializeField] private Animator animator;

    [Inject] private Menu<GameMenu>.Factory _gameFactory;
    [Inject] private GameController controller;
    [Inject] private DialogManager _dialogManager;

    // private int nextLevel;

    private void Start()
    {
        // nextLevel = param.FileName;
        startBtn.onClick.AddListener(() => { animator.SetTrigger("change"); });
        backBtn.onClick.AddListener(() => { animator.SetTrigger("change"); });
        optionBtn.onClick.AddListener(() => { _dialogManager.OpenOptionDialog(); });
        // exitGameBtn.onClick.AddListener(() => { _dialogManager.OpenOptionDialog(); });
        if (!PlayerPrefs.HasKey(PlayerPrefsUtils.PRIVACY_AGREE))
        {
            Debug.Log("Show privacy dialog");
            UnityServices.InitializeAsync();
            _dialogManager.CollectPrivacyDialog(() =>
            {
                Debug.Log("Agree privacy dialog");
                AnalyticsService.Instance.StartDataCollection();
                PlayerPrefs.SetInt(PlayerPrefsUtils.PRIVACY_AGREE, 1);
            }, () =>
            {
                Application.OpenURL("market://details?id=" + Application.identifier);
                Debug.Log("read");
                
            });
        }
    }
}