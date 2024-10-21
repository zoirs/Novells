using Main;
using MenuSystemWithZenject;
using UnityEngine;
using Zenject;

// документация https://github.com/modesttree/Zenject
public class GameInstaller : MonoInstaller {
    
    [Inject] GameSettingsInstaller.UIPrefabs prefabsUI;
    [Inject] GameSettingsInstaller.GameSetting _setting;

    // public GameObject startMoreLevelsMenu;
    public GameObject startOneButtonMenu;
    // public GameObject gameMenu;
    public GameObject novellSceneMenu;

    private static int lastTubeNumber = -1;
    private static int lastInventoryNumber = -1;

    public override void InstallBindings() {
        Debug.Log("GameInstaller");
        Container.Bind<ScreenService>().AsSingle();

        Container.Bind<MenuManager>().AsSingle();

        if (_setting.isDebug) {
            // Container.Bind<GameObject>().FromInstance(startMoreLevelsMenu)
                // .WhenInjectedInto<Menu<StartMenu>.CustomMenuFactory>();
            Container.BindFactory<StartMenu, Menu<StartMenu>.Factory>()
                .FromFactory<Menu<StartMenu>.CustomMenuFactory>();
        }
        else {
            Container.Bind<GameObject>().FromInstance(startOneButtonMenu)
                .WhenInjectedInto<Menu<StartMenu>.CustomMenuFactory>();
            Container.BindFactory<StartMenu, Menu<StartMenu>.Factory>()
                .FromFactory<Menu<StartMenu>.CustomMenuFactory>();
        }

        // Container.Bind<GameObject>().FromInstance(gameMenu).WhenInjectedInto<Menu<GameMenu>.CustomMenuFactory>();
        Container.BindFactory<GameMenu, Menu<GameMenu>.Factory>().FromFactory<Menu<GameMenu>.CustomMenuFactory>();
       
        Container.Bind<GameObject>().FromInstance(novellSceneMenu).WhenInjectedInto<Menu<NovellSceneMenu>.CustomMenuFactory>();
        Container.BindFactory<NovellSceneMenu, Menu<NovellSceneMenu>.Factory>().FromFactory<Menu<NovellSceneMenu>.CustomMenuFactory>();

        // Container.Bind<GameObject>().FromSubContainerResolve().ByNewPrefabInstaller<InventoryItemInstaller>(gameMenu);

        // Container.BindInterfacesAndSelfTo<PurchaseManager>().FromComponentInHierarchy().AsSingle();


        Container.BindInterfacesAndSelfTo<GameController>().AsSingle();
        // Container.BindInterfacesAndSelfTo<MapService>().AsSingle();
        Container.BindInterfacesAndSelfTo<DialogManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<GameMapService>().AsSingle();
        Container.BindInterfacesAndSelfTo<CountHintManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<SceneryManager>().AsSingle();
            //Container.BindInterfacesAndSelfTo<Purchaser>().AsSingle();
        Container.Bind<MicAudioPlayer>().AsSingle();


        // Container.BindFactory<StoneController, StoneController.Factory>()
        //     .FromComponentInNewPrefab(_prefabs.StonePrefab)
        //     .WithGameObjectName("Stone")
        //     .UnderTransformGroup("Stones");

        // Container.BindMemoryPool<MenuButtonController, MenuButtonController.Pool>()
        //     // .WithInitialSize(50)
        //     .FromComponentInNewPrefab(prefabsUI.menuButtonPrefab)
        //     .UnderTransform(GameObject.Find("StartMoreLevelsMenu(Clone)").transform);
        

        Container.BindFactory<ConfirmDialogParam, YesNoConfirmDialogController, YesNoConfirmDialogController.Factory>()
            .FromMethod(CreateConfirmDialog);
        
        Container.BindFactory<NextFrameBtnParam, NextFrameButtonController, NextFrameButtonController.Factory>()
            .FromMethod(CreateNextFrameBtn);
        

        Container.BindFactory<OptionDialogParam, OptionDialogController, OptionDialogController.Factory>()
            .FromMethod(CreateOptionDialog);


        Container.BindFactory<InfoDialogParam, InfoDialogController, InfoDialogController.Factory>()
            .FromMethod(CreateHelpDialog);
        Container.BindFactory<AboutMeDialogParam, AboutMeDialogController, AboutMeDialogController.Factory>()
            .FromMethod(CreateAboutMeDialog);
        Container.BindFactory<PrivacyDialogParam, PrivacyDialogController, PrivacyDialogController.Factory>()
            .FromMethod(CreatePrivacyDialog);


        InstallSignals();
        InstallUI();
    }

    private void InstallUI() { }
    
    YesNoConfirmDialogController CreateConfirmDialog(DiContainer subContainer, ConfirmDialogParam createParam) {
        GameObject gameMenu = GameObject.Find("GameMenu(Clone)");
        GameObject startMenu = GameObject.Find("StartMenu(Clone)");
        YesNoConfirmDialogController controller =
            subContainer.InstantiatePrefabForComponent<YesNoConfirmDialogController>(prefabsUI.ConfirmDialog,
                startMenu != null ? startMenu.transform : gameMenu.transform);
        controller.Init(createParam);
        return controller;
    }
    
    NextFrameButtonController CreateNextFrameBtn(DiContainer subContainer, NextFrameBtnParam createParam) {
        GameObject menu = GameObject.Find("CurrentSceneButtons");
        NextFrameButtonController controller =
            subContainer.InstantiatePrefabForComponent<NextFrameButtonController>(prefabsUI.nextFramePrefab,
                menu.transform);
        controller.Init(createParam);
        return controller;
    }
    
    
    OptionDialogController CreateOptionDialog(DiContainer subContainer, OptionDialogParam createParam) {
        GameObject startMenu = GameObject.Find("StartMenu(Clone)");
        OptionDialogController controller =
            subContainer.InstantiatePrefabForComponent<OptionDialogController>(prefabsUI.OptionDialog,
                startMenu.transform);
        controller.Init(createParam);
        return controller;
    }    
    
    InfoDialogController CreateHelpDialog(DiContainer subContainer, InfoDialogParam createParam) {
        GameObject startMenu = GameObject.Find("NovellScene(Clone)");
        InfoDialogController controller =
            subContainer.InstantiatePrefabForComponent<InfoDialogController>(prefabsUI.InfoDialog,
                startMenu.transform);
        controller.Init(createParam);
        return controller;
    }
    AboutMeDialogController CreateAboutMeDialog(DiContainer subContainer, AboutMeDialogParam createParam) {
        GameObject startMenu = GameObject.Find("NovellScene(Clone)");
        AboutMeDialogController controller =
            subContainer.InstantiatePrefabForComponent<AboutMeDialogController>(prefabsUI.AboutMeDialog,
                startMenu.transform);
        controller.Init(createParam);
        return controller;
    }
    
    PrivacyDialogController CreatePrivacyDialog(DiContainer subContainer, PrivacyDialogParam createParam) {
        GameObject startMenu = GameObject.Find("StartMenu(Clone)");
        PrivacyDialogController controller =
            subContainer.InstantiatePrefabForComponent<PrivacyDialogController>(prefabsUI.PrivacyDialog,
                startMenu.transform);
        controller.Init(createParam);
        return controller;
    }



    void InstallSignals() {
        // Every scene that uses signals needs to install the built-in installer SignalBusInstaller
        // Or alternatively it can be installed at the project context level (see docs for details)
        SignalBusInstaller.Install(Container);

        // Signals can be useful for game-wide events that could have many interested parties
        // Container.DeclareSignal<AddProductSignal>();
        Container.DeclareSignal<HintExecutedSignal>();
    }
    
    public static int LastTubeNumber => lastTubeNumber;

    public static int LastInventoryNumber => lastInventoryNumber;
}