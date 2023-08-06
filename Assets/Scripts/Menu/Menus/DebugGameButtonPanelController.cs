using System.Collections;
using Main;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DebugGameButtonPanelController : MonoBehaviour {
    [SerializeField] GameObject checkBtn;
    [SerializeField] GameObject updateBtn;
    [SerializeField] Dropdown package;
    [SerializeField] GameObject saveBtn;

    [Inject] private GameSettingsInstaller.GameSetting setting;
    [Inject] private LevelManager _levelManager;
    [Inject] private CheckerService _checkerService;
    [Inject] private GameController _gameController;

    private void Start() {
        gameObject.SetActive(setting.isDebug);
        if (!gameObject.activeSelf) {
            return;
        }

        checkBtn.GetComponent<Button>().onClick.AddListener(() => {
            _checkerService.Check();
            // StartCoroutine(Check());
        });
        
        saveBtn.GetComponent<Button>().onClick.AddListener(() => {
            LevelPackage packageValue = (LevelPackage) package.value;
            Debug.Log(packageValue);
            _levelManager.SaveLevel(packageValue);
        });

        updateBtn.SetActive(_gameController.CurrentLevel.Number != int.MinValue);
        if (updateBtn.activeSelf) {
            updateBtn.GetComponent<Button>().onClick.AddListener(() => { _levelManager.UpdateLevel(); });
        }
    }
    
    IEnumerator Check() {
        yield return 0;
        _checkerService.Check();
    }
}