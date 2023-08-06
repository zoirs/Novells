using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using Object = UnityEngine.Object;

public class CSVLoader {
    private TextAsset csvFile;
    private char lineSeparator = '\n';
    private char surround = '"';
    private String[] fieldSeparator = {"\",\""};

    public void LoadCSV() {
        Debug.Log("load ==== ");
        csvFile = Resources.Load<TextAsset>("Localisation/Localization");
        Object o = Resources.Load("Localisation/Localization");
        Debug.Log("load ==== " + o);
        // Object load = Resources.Load("Sprite/move");
        // Debug.Log("load ==== " + load);
    }

    public Dictionary<string, string> GetDictionaryValues(string attributeId) {
        Debug.Log(csvFile);
        Debug.Log(csvFile.text);
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        string[] lines = csvFile.text.Split(lineSeparator);
        int attributeIndex = -1;
        string[] headers = lines[0].Split(fieldSeparator, StringSplitOptions.None);
        for (int i = 0; i < headers.Length; i++) {
            if (headers[i].Contains(attributeId)) {
                attributeIndex = i;
                break;
            }
        }
        
        Regex csvParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
        for (int i = 0; i < lines.Length; i++) {
            string line = lines[i];
            string[] fields = csvParser.Split(line);
            for (int j = 0; j < fields.Length; j++) {
                fields[j] = fields[j].TrimStart(' ', surround);
                fields[j] = fields[j].TrimEnd(surround);
            }

            if (fields.Length > attributeIndex) {
                string key = fields[0];
                if (dictionary.ContainsKey(key)) { continue;}

                string value = fields[attributeIndex];
                dictionary.Add(key, value);
            }
        }

        return dictionary;
    }
}
