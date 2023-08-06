using UnityEngine;
using Zenject;

public class CountHintManager : IInitializable {

    private const string HINT_COUNT = "hint_count";

    public void Initialize() {
        if (!PlayerPrefs.HasKey(HINT_COUNT)) {
            PlayerPrefs.SetInt(HINT_COUNT, 3);
        }
    }

    public void DecHintCount() {
        if (PlayerPrefs.HasKey(HINT_COUNT)) {
            int count = PlayerPrefs.GetInt(HINT_COUNT);
            PlayerPrefs.SetInt(HINT_COUNT, count - 1);
        }
        else {
            PlayerPrefs.SetInt(HINT_COUNT, 0);
        }
    }

    public int HintCount() {
        if (PlayerPrefs.HasKey(HINT_COUNT)) {
            return PlayerPrefs.GetInt(HINT_COUNT);
        }

        return 0;
    }

    public void AddHint(int addCount) {
        int currentCount = 0;
        if (PlayerPrefs.HasKey(HINT_COUNT)) {
            currentCount = PlayerPrefs.GetInt(HINT_COUNT);
        }
        PlayerPrefs.SetInt(HINT_COUNT, currentCount + addCount);
    }

    public void ResetHint() {
        PlayerPrefs.SetInt(HINT_COUNT, 1);
    }
}