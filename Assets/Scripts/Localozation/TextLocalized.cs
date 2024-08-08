using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextLocalized : MonoBehaviour {
    
    public TextMeshProUGUI _textField;

    private void Awake() {
        _textField = GetComponent<TextMeshProUGUI>();
        Debug.Log(" ss " + _textField);
    }

    private void Start() {
        string textValue = _textField.text;
        if (textValue == null || !textValue.StartsWith("$")) {
            return;
        }

        string value = LocalisationSystem.GetLocalisedValue(textValue, 0);
        if (value != null) {
            _textField.text = value;
        }
    }

    public void SetText(string key) {
        if (key != null && key.StartsWith("$")) {
            string value = LocalisationSystem.GetLocalisedValue(key, 0);// todo передавать номер истории
            if (value != null) {
                _textField.text = value;
                return;
            }
        }
        _textField.text = key;
    }
}