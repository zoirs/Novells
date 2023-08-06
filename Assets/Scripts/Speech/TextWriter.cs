using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class TextWriter : MonoBehaviour {
    private List<TextWriterSingle> _textWriterSingles;

    private void Awake() {
        _textWriterSingles = new List<TextWriterSingle>();
    }

    public void AddWriter(TextMeshProUGUI textMeshProUGUI, string textToWrite, float timePerCharacter, Action unknown) {
        _textWriterSingles.Add(new TextWriterSingle(textMeshProUGUI, textToWrite, timePerCharacter, unknown));
    }

    public void RemoveWriter(TextMeshProUGUI textMeshProUGUI) {
        TextWriterSingle find = _textWriterSingles.Find(single => single.Uitext == textMeshProUGUI);
        _textWriterSingles.Remove(find);
    }

    private void Update() {
        for (int i = 0; i < _textWriterSingles.Count; i++) {
            bool destroyInstance = _textWriterSingles[i].Update();
            if (destroyInstance) {
                // _textWriterSingles[i].ONSuccess.Invoke();
                _textWriterSingles.RemoveAt(i);
                i--;
            }
        }
    }


    public class TextWriterSingle {
        private TextMeshProUGUI _uitext;
        private string _textToWrite;
        private float timePerCharacter;
        private float _timer;
        private int _characterIndex;
        private Action _onSuccess;

        public TextWriterSingle(TextMeshProUGUI textMeshProUGUI, string textToWrite, float timePerCharacter,
            Action onSuccess) {
            _uitext = textMeshProUGUI;
            _textToWrite = textToWrite;
            this.timePerCharacter = timePerCharacter;
            _onSuccess = onSuccess;
        }


        public bool Update() {
            _timer -= Time.deltaTime;
            while (_timer <= 0) {
                _timer += timePerCharacter;
                _characterIndex++;
                string text = _textToWrite.Substring(0, _characterIndex);
                _uitext.text = text;
                
                if (_characterIndex >= _textToWrite.Length) {
                    return true;
                }
            }

            return false;
        }

        public Action ONSuccess => _onSuccess;

        public TextMeshProUGUI Uitext => _uitext;
    }
}