using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Level {
    public class LevelLoadManager : IInitializable {

        [Inject] private GameSettingsInstaller.GameSetting setting; 
        
        private Dictionary<LevelPackage, List<LevelBtnParam>> levels = new Dictionary<LevelPackage, List<LevelBtnParam>>();

        public void Initialize() {
            Debug.Log(Application.systemLanguage);

            foreach (LevelPackage levelType in Enum.GetValues(typeof(LevelPackage))) {
                levels[levelType] = new List<LevelBtnParam>();
            }

            Debug.Log("LevelLoadManager Initialize");
            if (setting.loadLevelFromResources) {
                foreach (LevelPackage levelType in Enum.GetValues(typeof(LevelPackage))) {
                    TextAsset[] textAssets = Resources.LoadAll<TextAsset>("Levels/" + levelType);
                    foreach (TextAsset f in textAssets) {
                        AddLevelData(levelType, int.Parse(f.name));
                    }    
                }
            }
            else {
                foreach (LevelPackage levelType in Enum.GetValues(typeof(LevelPackage))) {
                    DirectoryInfo dir = new DirectoryInfo(Application.persistentDataPath + "/" + levelType);
                    Debug.Log(dir);
                    FileInfo[] info = dir.GetFiles("*.txt");

                    foreach (FileInfo f in info) {
                        AddLevelData(levelType, int.Parse(Path.GetFileNameWithoutExtension(f.Name)));
                    }
                }
            }

            foreach (LevelPackage levelType in Enum.GetValues(typeof(LevelPackage))) {
                levels[levelType].Sort((p1, p2) => p1.FileName.CompareTo(p2.FileName));
            }
            Debug.Log("LevelLoadManager Initialize complete");
        }

        private void AddLevelData(LevelPackage levelPackage, int fileName) {
            LevelState state;
            string levelKey = PlayerPrefsUtils.LevelKey(levelPackage, fileName);
            if (PlayerPrefs.HasKey(levelKey)) {
                state = (LevelState) PlayerPrefs.GetInt(levelKey);
                Debug.Log(levelKey + " " + state + " " + fileName);
            }
            else {
                bool isPreviewsComplete = levels[levelPackage].Count == 0 || levels[levelPackage][levels[levelPackage].Count - 1].State == LevelState.COMPLETE;
                Debug.Log(levelKey + " isPreviewsComplete " + isPreviewsComplete + " " + fileName);
                state = isPreviewsComplete ? LevelState.ACTIVE : LevelState.LOCKED;
            }
            Debug.Log(fileName + " " + state);

            levels[levelPackage].Add(new LevelBtnParam(fileName, levelPackage, state));
        }

        public LevelBtnParam FindFirstNotLocked(LevelPackage levelPackage) {
            if (levels[levelPackage].Count == 0) {
                return null;
            }

            if (levels[levelPackage][0].State != LevelState.COMPLETE) {
                return levels[levelPackage][0];
            }

            for (int i = 0; i < levels[levelPackage].Count; i++) {
                if (levels[levelPackage][i].State == LevelState.COMPLETE) {
                    continue;
                }
                return levels[levelPackage][i];
            }
            return levels[levelPackage][Random.Range(0, levels[levelPackage].Count)];
        }

        public Dictionary<LevelPackage, List<LevelBtnParam>> Levels => levels;

        public void Complete(LevelPath currentLevel) {
            PlayerPrefs.SetInt(PlayerPrefsUtils.LevelKey(currentLevel), (int) LevelState.COMPLETE);
            levels[currentLevel.Package][currentLevel.Number].State = LevelState.COMPLETE;
        }
    }
}