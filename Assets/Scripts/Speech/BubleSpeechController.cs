
using System;
using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;

public class BubleSpeechController : MonoBehaviour {
    [SerializeField] private GameObject background;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private TextWriter _textWriter;
    private Coroutine current;

    public void SetText(string name, string speech, Action callBack) {
        if (current != null) {
            StopCoroutine(current);
            _textWriter.RemoveWriter(_text);
        }

        string text;
        if (name == null) {
            text = speech;
        } else {
            text = name + ":\n" + speech;
        }

        current = StartCoroutine(DoClickAndAction(text, callBack));
    }
    
    IEnumerator DoClickAndAction(string text, Action callBack) {
        background.gameObject.SetActive(false);
        _text.text = "";
        yield return new WaitForSeconds(1.3f);
        background.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        
        _textWriter.AddWriter(_text, text, 0.04f, callBack);
    }
}