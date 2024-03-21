using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Button))]
public class ClueController : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI caption;

    [Inject] private DialogManager _dialogManager;

    private void Start() {
        GetComponent<Button>().onClick.AddListener(() => { _dialogManager.OpenInfoDialog(); });

    }

    public void SetText(int current, int wait) {
        string localisedValue = LocalisationSystem.GetLocalisedValue("$clue", 0);
        caption.text = localisedValue.Substring(0, localisedValue.Length - 1) + " " + current + "/" + wait;
    }

}