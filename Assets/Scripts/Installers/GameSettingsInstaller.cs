using System;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

[CreateAssetMenu(fileName = "GameSettingsInstaller", menuName = "Installers/GameSettingsInstaller")]
public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
{
    public UIPrefabs UIPrefab;
    public AudioMicSettings audioMicSetting;
    public GameSetting Setting;
    
    [Serializable]
    public class UIPrefabs {
        public GameObject ConfirmDialog;
        public GameObject OptionDialog;
        public GameObject InfoDialog;
        public GameObject PrivacyDialog;
        public GameObject AboutMeDialog;

        public GameObject nextFramePrefab;
    }   
    
    [Serializable]
    public class GameSetting {
        public bool isDebug;
    }
    
    [Serializable]
    public class AudioMicSettings {
        // public AudioClip startMove;
        // public AudioClip levelComplete;
        public bool isOn;
    }
    
    
    public override void InstallBindings()
    {
        Container.BindInstance(Setting);
        Container.BindInstance(UIPrefab);
        Container.BindInstance(audioMicSetting);
    }
}
