using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using Object = UnityEngine.Object;

public class LocalizationLoader {
    // private TextAsset csvFile;
    private char lineSeparator = '\n';
    private String[] fieldSeparator = {"\",\""};
    private TextAsset[] _all;

    public void LoadAll() {
        Debug.Log("load ==== ");
        // csvFile = Resources.Load<TextAsset>("Localisation/Localization");
        _all = Resources.LoadAll<TextAsset>("Localisation/");
        Debug.Log("all: " + _all);
        // Object o = Resources.Load("Localisation/Localization");
        // Debug.Log("load ==== " + o);
        // Object load = Resources.Load("Sprite/move");
        // Debug.Log("load ==== " + load);
    }

    public Dictionary<string, string> GetDictionaryValues(string attributeId) {
        // Debug.Log(csvFile);
        // Debug.Log(csvFile.text);
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        foreach (TextAsset asset in _all) { }

        TextAsset textAsset = _all.First(q => q.name.Equals("Localization_" + attributeId));

        string[] lines = textAsset.text.Split(lineSeparator);
        // int attributeIndex = -1;
        foreach (string line in lines) {
            if (string.IsNullOrWhiteSpace(line)) {
                continue;
            }

            string[] strings = line.Split("=", 2);
            Debug.Log("was loaded : " + strings + "; " + line);
            Debug.Log("was loaded : " + strings[0]);
            Debug.Log("was loaded : " + strings[1]);
            dictionary.Add(strings[0], strings[1]);
        }

        // if (true) {
        //     return dictionary;
        // }
        //
        // // string[] headers = lines[0].Split(fieldSeparator, StringSplitOptions.None);
        // for (int i = 0; i < headers.Length; i++) {
        //     if (headers[i].Contains(attributeId)) {
        //         attributeIndex = i;
        //         break;
        //     }
        // }
        //
        // Regex csvParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
        // for (int i = 0; i < lines.Length; i++) {
        //     string line = lines[i];
        //     string[] fields = csvParser.Split(line);
        //     for (int j = 0; j < fields.Length; j++) {
        //         fields[j] = fields[j].TrimStart(' ', surround);
        //         fields[j] = fields[j].TrimEnd(surround);
        //     }
        //
        //     if (fields.Length > attributeIndex) {
        //         string key = fields[0];
        //         if (dictionary.ContainsKey(key)) { continue;}
        //
        //         string value = fields[attributeIndex];
        //         dictionary.Add(key, value);
        //     }
        // }

        return dictionary;
    }
}