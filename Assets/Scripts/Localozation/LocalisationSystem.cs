﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LocalisationSystem {
    public enum Language {
        UNKNOWN,
        ENGLISH,
        RUSSIAN,
        FRENCH
    }

    public static Language language;

    private static Dictionary<string, string> localisedEn;
    private static Dictionary<string, string> localisedRu;
    private static Dictionary<string, string> localisedFr;

    public static bool isInit;

    public static void Init() {
        RefreshLanguage();
        
        LocalizationLoader localizationLoader = new LocalizationLoader();
        localizationLoader.LoadAll();

        localisedEn = localizationLoader.GetDictionaryValues("en");
        localisedRu = localizationLoader.GetDictionaryValues("ru");
        localisedFr = localizationLoader.GetDictionaryValues("fr");

        isInit = true;
    }

    public static void RefreshLanguage() {
        int lang = PlayerPrefs.GetInt(PlayerPrefsUtils.LANGUAGE);
        if (lang == (int) Language.UNKNOWN) {
            if (Application.systemLanguage == SystemLanguage.Russian) {
                lang = (int) Language.RUSSIAN;
            } else {
                lang = (int) Language.ENGLISH;
            }
            PlayerPrefs.SetInt(PlayerPrefsUtils.LANGUAGE, lang);
        }

        language = Enum.GetValues(typeof(Language))
            .Cast<Language>()
            .ToList()[lang];
    }

    public static string GetLocalisedValue(string key) {
        if (!isInit) {
            Init();
        }

        string value = key;

        switch (language) {
            case Language.ENGLISH:
                localisedEn.TryGetValue(key.ToLower(), out value);
                break;
            case Language.RUSSIAN:
                localisedRu.TryGetValue(key.ToLower(), out value);
                break;
            case Language.FRENCH:
                localisedFr.TryGetValue(key.ToLower(), out value);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return value;
    }
}