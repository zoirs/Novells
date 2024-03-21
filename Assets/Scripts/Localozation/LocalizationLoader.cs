using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using Object = UnityEngine.Object;

public class LocalizationLoader {
    // private TextAsset csvFile;
    private char lineSeparator = '\n';
    private String[] fieldSeparator = {"\",\""};
    private Dictionary<int, TextAsset[]> _all = new Dictionary<int, TextAsset[]>();
    private static readonly string STORY_PREFIX = "Story_";
    private static readonly string COMMON_PREFIX = "Common";

    public void LoadAll() {
        string resourcsPath = Application.dataPath + "/Resources";

        string[] fileNames = Directory.GetDirectories(resourcsPath).ToArray();
        foreach (string fileName in fileNames) {
            Debug.Log(fileName);
            string folder = fileName.Substring(resourcsPath.Length + 1);
            if (folder.StartsWith(STORY_PREFIX)) {
                short storyNumber = Int16.Parse(folder.Substring(STORY_PREFIX.Length));
                TextAsset[] textAssets = Resources.LoadAll<TextAsset>(folder +"/Localisation/");
                _all.Add(storyNumber, textAssets);
            }
            if (folder.StartsWith(COMMON_PREFIX)) {
                short storyNumber = 0;
                TextAsset[] textAssets = Resources.LoadAll<TextAsset>(folder +"/Localisation/");
                _all.Add(storyNumber, textAssets);
            }
        }
    }

    public Dictionary<int, Dictionary<string, string>> GetDictionaryValues(string attributeId) {
        Dictionary<int, Dictionary<string, string>> result = new Dictionary<int, Dictionary<string, string>>();
        foreach (var keyValuePair in _all) {
            result.Add(keyValuePair.Key, loadStory(attributeId, keyValuePair.Key));
        }

        return result;
    }

    private Dictionary<string, string> loadStory(string attributeId, int story) {
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        Debug.Log(attributeId+ " 11 " + story);
        TextAsset textAsset = _all[story].First(q => q.name.Equals("Localization_" + attributeId));

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

        return dictionary;
    }
}