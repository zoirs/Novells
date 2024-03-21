using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class BubleSpeechController : MonoBehaviour {
    [SerializeField] private GameObject background;
    [SerializeField] private TextMeshProUGUI caption;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private TextWriter _textWriter;
    private Coroutine current;

    public void SetText(int story, string name, string speech, Action callBack) {
        if (current != null) {
            StopCoroutine(current);
            _textWriter.RemoveWriter(_text);
        }

        caption.text = name != null ? LocalisationSystem.GetLocalisedValue("$hero.name." + name, story) : "";

        current = StartCoroutine(DoClickAndAction(LocalisationSystem.GetLocalisedValue(speech, story), callBack));
    }

    IEnumerator DoClickAndAction(string text, Action callBack) {
        background.gameObject.SetActive(false);
        _text.text = "";
        yield return new WaitForSeconds(1.3f);
        background.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        Debug.Log("=== " + _text + text);
        _textWriter.AddWriter(_text, text, 0.04f, callBack);
    }
}