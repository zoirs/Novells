using DefaultNamespace;
using Main;
using MenuSystemWithZenject;
using MenuSystemWithZenject.Elements;
using PathCreation;
using Plane;
using UnityEngine;
using Zenject;
using Object = System.Object;

// документация https://github.com/modesttree/Zenject
public class GameInstaller : MonoInstaller {
    
    [Inject] GameSettingsInstaller.UIPrefabs prefabsUI;
    [Inject] GameSettingsInstaller.PrefabSettings prefabs;
    [Inject] GameSettingsInstaller.GameSetting _setting;

    public GameObject startMoreLevelsMenu;
    public GameObject startOneButtonMenu;
    public GameObject gameMenu;
    public GameObject novellSceneMenu;

    private static int lastTubeNumber = -1;
    private static int lastInventoryNumber = -1;

    public override void InstallBindings() {
        Container.Bind<ScreenService>().AsSingle();

        Container.Bind<MenuManager>().AsSingle();

        if (_setting.isDebug) {
            Container.Bind<GameObject>().FromInstance(startMoreLevelsMenu)
                .WhenInjectedInto<Menu<StartMenu>.CustomMenuFactory>();
            Container.BindFactory<StartMenu, Menu<StartMenu>.Factory>()
                .FromFactory<Menu<StartMenu>.CustomMenuFactory>();
        }
        else {
            Container.Bind<GameObject>().FromInstance(startOneButtonMenu)
                .WhenInjectedInto<Menu<StartMenu>.CustomMenuFactory>();
            Container.BindFactory<StartMenu, Menu<StartMenu>.Factory>()
                .FromFactory<Menu<StartMenu>.CustomMenuFactory>();
        }

        Container.Bind<GameObject>().FromInstance(gameMenu).WhenInjectedInto<Menu<GameMenu>.CustomMenuFactory>();
        Container.BindFactory<GameMenu, Menu<GameMenu>.Factory>().FromFactory<Menu<GameMenu>.CustomMenuFactory>();
       
        Container.Bind<GameObject>().FromInstance(novellSceneMenu).WhenInjectedInto<Menu<NovellSceneMenu>.CustomMenuFactory>();
        Container.BindFactory<NovellSceneMenu, Menu<NovellSceneMenu>.Factory>().FromFactory<Menu<NovellSceneMenu>.CustomMenuFactory>();

        // Container.Bind<GameObject>().FromSubContainerResolve().ByNewPrefabInstaller<InventoryItemInstaller>(gameMenu);

        // Container.BindInterfacesAndSelfTo<PurchaseManager>().FromComponentInHierarchy().AsSingle();


        Container.BindInterfacesAndSelfTo<GameController>().AsSingle();
        // Container.BindInterfacesAndSelfTo<MapService>().AsSingle();
        Container.BindInterfacesAndSelfTo<DialogManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<GameMapService>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlaneManager>().AsSingle();
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
        
        Container.BindFactory<PathParam, PathCreator, PathFactory>()
            .FromMethod(CreatePath);
        Container.BindFactory<PlaneParam, PlaneController, PlaneController.Factory>()
            .FromMethod(CreatePlane);

        
        
        Container.BindFactory<NextFrameBtnParam, NextFrameButtonController, NextFrameButtonController.Factory>()
            .FromMethod(CreateNextFrameBtn);
        

        Container.BindFactory<ConfirmDialogParam, YesNoConfirmDialogController, YesNoConfirmDialogController.Factory>()
            .FromMethod(CreateConfirmDialog);
        
        Container.BindFactory<PurchaseDialogParam, PurchaseDialogController, PurchaseDialogController.Factory>()
            .FromMethod(CreatePurchaseDialog);
        
        Container.BindFactory<CongradulationDialogParam, CongradulationDialogController, CongradulationDialogController.Factory>()
            .FromMethod(CreateCongradulationDialog);
        
        
        Container.BindFactory<OptionDialogParam, OptionDialogController, OptionDialogController.Factory>()
            .FromMethod(CreateOptionDialog);


        InstallSignals();
        InstallUI();
    }

    private void InstallUI() { }







    PathCreator CreatePath(DiContainer subContainer, PathParam createParam) {
        PathCreator pathController =
            subContainer.InstantiatePrefabForComponent<PathCreator>(prefabs.pathPrefab,
                GameObject.Find("Paths").transform);
        BezierPath bezierPath = new BezierPath(createParam.Points, false, PathSpace.xy);
        pathController.bezierPath = bezierPath;
        return pathController;
    }

    PlaneController CreatePlane(DiContainer subContainer, PlaneParam createParam) {
        PlaneController controller =
            subContainer.InstantiatePrefabForComponent<PlaneController>(createParam.Prefab,
                GameObject.Find("Planes").transform);
        controller.transform.position = new Vector3(createParam.Position.x, createParam.Position.y, 7f);
        return controller;
    }

    YesNoConfirmDialogController CreateConfirmDialog(DiContainer subContainer, ConfirmDialogParam createParam) {
        GameObject gameMenu = GameObject.Find("GameMenu(Clone)");
        GameObject startMenu = GameObject.Find("StartMenu(Clone)");
        YesNoConfirmDialogController controller =
            subContainer.InstantiatePrefabForComponent<YesNoConfirmDialogController>(prefabsUI.ConfirmDialog,
                startMenu != null ? startMenu.transform : gameMenu.transform);
        controller.Init(createParam);
        return controller;
    }
    
    PurchaseDialogController CreatePurchaseDialog(DiContainer subContainer, PurchaseDialogParam createParam) {
        GameObject gameMenu = GameObject.Find("GameMenu(Clone)");
        GameObject startMenu = GameObject.Find("StartMenu(Clone)");
        PurchaseDialogController controller =
            subContainer.InstantiatePrefabForComponent<PurchaseDialogController>(prefabsUI.PurchaseDialog,
                startMenu != null ? startMenu.transform : gameMenu.transform);
        controller.Init(createParam);
        return controller;
    }
    
    CongradulationDialogController CreateCongradulationDialog(DiContainer subContainer, CongradulationDialogParam createParam) {
        GameObject gameMenu = GameObject.Find("GameMenu(Clone)");
        GameObject startMenu = GameObject.Find("StartMenu(Clone)");
        CongradulationDialogController controller =
            subContainer.InstantiatePrefabForComponent<CongradulationDialogController>(prefabsUI.CongradulationDialog,
                startMenu != null ? startMenu.transform : gameMenu.transform);
        controller.Init(createParam);
        return controller;
    }
    
    MenuButtonController CreateMenuBtn(DiContainer subContainer, Object createParam) {
        GameObject gameMenu = GameObject.Find("Content");
        MenuButtonController controller =
            subContainer.InstantiatePrefabForComponent<MenuButtonController>(prefabsUI.menuButtonPrefab,
                gameMenu.transform);
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