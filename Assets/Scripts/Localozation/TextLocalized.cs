using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextLocalized : MonoBehaviour {
    
    private Text _textField;

    private void Awake() {
        _textField = GetComponent<Text>();
    }

    private void Start() {
        string textValue = _textField.text;
        if (textValue == null || !textValue.StartsWith("$")) {
            return;
        }

        string value = LocalisationSystem.GetLocalisedValue(textValue);
        if (value != null) {
            _textField.text = value;
        }
    }

    public void SetText(string key) {
        if (key != null && key.StartsWith("$")) {
            string value = LocalisationSystem.GetLocalisedValue(key);
            if (value != null) {
                _textField.text = value;
                return;
            }
        }
        _textField.text = key;
    }
}