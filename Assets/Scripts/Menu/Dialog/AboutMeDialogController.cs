using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class AboutMeDialogController : MonoBehaviour {
    [SerializeField] private Button close;
    [SerializeField] private Button rate;
    [SerializeField] private TextLocalized body;

    public class Factory : PlaceholderFactory<AboutMeDialogParam, AboutMeDialogController> { }

    public void Init(AboutMeDialogParam param) {
        if (body != null)
        {
            body.SetText(param.Body);
        }

        close.onClick.AddListener(() =>
        {
            ResetProgress();
            param.Close.Invoke();
            Close();
        });
        rate.onClick.AddListener(() =>
        {
            param.Rate.Invoke();
            Close();
        });
    }

    public void Close() {
        Destroy(gameObject);
    }
    
    private void ResetProgress() {
        for (int i = 0; i < 10; i++)
        {
            PlayerPrefs.DeleteKey(PlayerPrefsUtils.STORY_PREFIX + i);
        }
        PlayerPrefs.DeleteKey(PlayerPrefsUtils.CLUE);
    }
}