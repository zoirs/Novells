
using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;

public class BubleSpeechController : MonoBehaviour {
    [SerializeField] private GameObject background;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private TextWriter _textWriter;

    public void SetText(string name, string speech) {
        string text;
        if (name == null) {
            text = speech;
        } else {
            text = name + ":\n" + speech;
        }
        StartCoroutine(DoClickAndAction(text));

    }
    
    IEnumerator DoClickAndAction(string text) {
        background.gameObject.SetActive(false);
        _text.text = "";
        yield return new WaitForSeconds(1.3f);
        background.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        _textWriter.AddWriter(_text, text, 0.04f, () => { background.gameObject.SetActive(false);});
    }
}