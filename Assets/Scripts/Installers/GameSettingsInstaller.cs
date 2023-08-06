using System;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

[CreateAssetMenu(fileName = "GameSettingsInstaller", menuName = "Installers/GameSettingsInstaller")]
public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
{
    public PrefabSettings Prefabs;
    public PriceSetting PriceSettings;
    public TubeButtonSettings TubeButton;
    public PackagesTextureSettings TextureSettings;
    public MenuButtonSpritesSettings MenuButtonSprites;
    public BuyItemTextureSettings BuyItemsTextureSettings;
    public OtherItemTextureSettings OtherItemTexture;
    public UIPrefabs UIPrefab;
    public AudioMicSettings audioMicSetting;
    public GameSetting Setting;
    
    [Serializable]
    public class PrefabSettings
    {
        public GameObject Tube1Prefab;
        public GameObject Tube2Prefab;
        public GameObject Tube3Prefab;
        public GameObject Tube4Prefab;
        public GameObject TubeAngelPrefab;
        public GameObject Tube1BridgePrefab;

        public GameObject Tube2StationPrefab;

        public GameObject RockPrefab;
        public GameObject TreePrefab;
        public GameObject StonePrefab;
        
        public GameObject PortalPrefab;
        
        public GameObject PlanePrefab;
        public GameObject TrainPrefab;
        public GameObject VagonPrefab;
        
        public GameObject pathPrefab;
        
        public GameObject riverPrefab;
        public GameObject riverAngelPrefab;
    }
    
    [Serializable]
    public class TubeButtonSettings
    {
        public Texture Line1;
        public Texture Line2;
        public Texture Line3;
        public Texture Line4;
        public Texture Station;
        public Texture Angle;
        public Texture Bridge;
    }
    
    [Serializable]
    public class OtherItemTextureSettings
    {
        public Texture Rock;
        public Texture Tree;
        public Texture Stone;
        
        public Texture Portal;
        public Texture Train;
        
        public Texture River;
        public Texture RiverAngel;
    }
    
    [Serializable]
    public class BuyItemTextureSettings
    {
        public Texture Wagon;
        public Texture Hint;
    }
    
    [Serializable]
    public class PackagesTextureSettings
    {
        public Texture Portal;
        public Texture Simple;
        public Texture Static;
        public Texture Bridge;
    }    
    
    [Serializable]
    public class MenuButtonSpritesSettings
    {
        public Sprite openButton;
        public Sprite needBuyVirtualMoneyButton;
        public Sprite needBuyRealMoneyButton;
        public Sprite developButton;
    }
    
    [Serializable]
    public class UIPrefabs {
        public GameObject InventoryItem;
        public GameObject OtherItem;
        public GameObject ConfirmDialog;
        public GameObject PurchaseDialog;
        public GameObject CongradulationDialog; // todo проверить, вроде не используется
        public GameObject BuyHintDialog;
        public GameObject OptionDialog;
        public GameObject Tutorial;

        public GameObject menuButtonPrefab;
        public GameObject nextFramePrefab;
    }   
    
    [Serializable]
    public class GameSetting {
        public bool isDebug;
        public bool loadLevelFromResources;
    }
    
    [Serializable]
    public class AudioMicSettings {
        public AudioClip rotate;
        public AudioClip startMove;
        public AudioClip levelComplete;
        public bool isOn;
    }
    
    [Serializable]
    public class PriceSetting {
        public int Hint;
        public int Reward;
        public int Wagon;
        
        public int LevelPackageSimple;
        public int LevelPackageStatic;
        public int LevelPackagePortal;
        public int LevelPackageBridge;
        public int LevelPackageCrossroad;
        public int LevelPackageSmall;
    }
    
    public override void InstallBindings()
    {
        Container.BindInstance(Prefabs);
        Container.BindInstance(PriceSettings);
        Container.BindInstance(TubeButton);
        Container.BindInstance(Setting);
        Container.BindInstance(UIPrefab);
        Container.BindInstance(OtherItemTexture);
        Container.BindInstance(audioMicSetting);
        Container.BindInstance(TextureSettings);
        Container.BindInstance(MenuButtonSprites);
        Container.BindInstance(BuyItemsTextureSettings);
    }
}
