using DefaultNamespace;
using Level;
using Main;
using MenuSystemWithZenject;
using MenuSystemWithZenject.Elements;
using PathCreation;
using Plane;
using UnityEngine;
using Zenject;

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


        Container.BindInterfacesAndSelfTo<LevelLoadManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<PurchaseManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<LevelPackageManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<GameController>().AsSingle();
        Container.BindInterfacesAndSelfTo<CheckerService>().AsSingle();
        // Container.BindInterfacesAndSelfTo<MapService>().AsSingle();
        Container.BindInterfacesAndSelfTo<TubeManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<HintManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<TrainManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<PriceManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<MoneyService>().AsSingle();
        Container.BindInterfacesAndSelfTo<DialogManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<GameMapService>().AsSingle();
        Container.BindInterfacesAndSelfTo<InventoryManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlaneManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<LevelManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<StoneManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<RiverManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<PortalManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<StationManager>().AsSingle();
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
        
        Container.BindFactory<PortalCreateParam, PortalController, PortalController.Factory>()
            .FromMethod(CreatePortal);
        Container.BindFactory<StoneCreateParam, StoneController, StoneController.Factory>()
            .FromMethod(CreateStone);
        Container.BindFactory<RiverCreateParam, RiverController, RiverController.Factory>()
            .FromMethod(CreateRiver);
        Container.BindFactory<TubeCreateParam, TubeController, TubeController.Factory>()
            .FromMethod(CreateTube);
        Container.BindFactory<TubeCreateParam, StationController, StationController.Factory>()
            .FromMethod(CreateStation);
        Container.BindFactory<TrainCreateParam, TrainController, TrainController.Factory>()
            .FromMethod(CreateTrain);
        Container.BindFactory<PathParam, PathCreator, PathFactory>()
            .FromMethod(CreatePath);
        Container.BindFactory<PlaneParam, PlaneController, PlaneController.Factory>()
            .FromMethod(CreatePlane);

        
        Container.BindFactory<LevelBtnParam, MenuButtonController, MenuButtonController.Factory>()
            .FromMethod(CreateMenuBtn);
        
        Container.BindFactory<NextFrameBtnParam, NextFrameButtonController, NextFrameButtonController.Factory>()
            .FromMethod(CreateNextFrameBtn);
        
        Container.BindFactory<TubeType, int, InventoryItemController, InventoryItemController.Factory>()
            .FromMethod(CreateInventorItem);

        Container.BindFactory<OtherItemController, OtherItemController.Factory>()
            .FromMethod(CreateOtherItem);

        Container.BindFactory<ConfirmDialogParam, YesNoConfirmDialogController, YesNoConfirmDialogController.Factory>()
            .FromMethod(CreateConfirmDialog);
        
        Container.BindFactory<PurchaseDialogParam, PurchaseDialogController, PurchaseDialogController.Factory>()
            .FromMethod(CreatePurchaseDialog);
        
        Container.BindFactory<CongradulationDialogParam, CongradulationDialogController, CongradulationDialogController.Factory>()
            .FromMethod(CreateCongradulationDialog);
        
        Container.BindFactory<BuyHintDialogParam, BuyMoneyDialogController, BuyMoneyDialogController.Factory>()
            .FromMethod(CreateBuyHintDialog);
        
        Container.BindFactory<OptionDialogParam, OptionDialogController, OptionDialogController.Factory>()
            .FromMethod(CreateOptionDialog);

        Container.BindFactory<TutorialParams, TutorialController, TutorialController.Factory>()
            .FromMethod(CreateTutorial);

        InstallSignals();
        InstallUI();
    }

    private void InstallUI() { }

    private InventoryItemController CreateInventorItem(DiContainer subContainer, TubeType tubeType, int count) {
        InventoryItemController item =
            subContainer.InstantiatePrefabForComponent<InventoryItemController>(prefabsUI.InventoryItem,
                GameObject.Find("InventoryContent").transform);
        item.Init(tubeType, count);
        item.name = "Inv_" + item.TubeType + "_" + ++lastInventoryNumber;
        return item;
    }

    private OtherItemController CreateOtherItem(DiContainer subContainer) {
        OtherItemController item =
            subContainer.InstantiatePrefabForComponent<OtherItemController>(prefabsUI.OtherItem,
                GameObject.Find("InventoryContent").transform);
        return item;
    }

    private StoneController CreateStone(DiContainer subContainer, StoneCreateParam createParam) {
        StoneController stone =
            subContainer.InstantiatePrefabForComponent<StoneController>(createParam.Prefab,
                GameObject.Find("Stones").transform);
        stone.transform.position = new Vector3(createParam.Position.x, createParam.Position.y, 0f);
        return stone;
    }

    private RiverController CreateRiver(DiContainer subContainer, RiverCreateParam createParam) {
        RiverController river =
            subContainer.InstantiatePrefabForComponent<RiverController>(createParam.Prefab,
                GameObject.Find("Rivers").transform);
        river.transform.position = new Vector3(createParam.Position.x, createParam.Position.y, 0f);
        river.Rotate = createParam.Rotation;
        return river;
    }

    private PortalController CreatePortal(DiContainer subContainer, PortalCreateParam createParam) {
        PortalController portal =
            subContainer.InstantiatePrefabForComponent<PortalController>(createParam.Prefab,
                GameObject.Find("Portals").transform);
        portal.transform.position = new Vector3(createParam.Position.x, createParam.Position.y, -1.5f);
        portal.Rotate = createParam.Rotation;
        return portal;
    }

    TubeController CreateTube(DiContainer subContainer, TubeCreateParam createParam) {
        TubeController tube =
            subContainer.InstantiatePrefabForComponent<TubeController>(createParam.Prefab,
                GameObject.Find("Tubes").transform);
        int zPos = createParam.Projection == TubeProjectionType.HINT ? TubeController.TUBE_HINT_Z : TubeController.TUBE_Z;
        tube.transform.position = new Vector3(createParam.Position.x, createParam.Position.y, zPos);
        tube.Rotate = createParam.Rotation;
        tube.Projection = createParam.Projection;
        tube.name = "Obj_" + tube.TubeType + "_" + ++lastTubeNumber; 
        return tube;
    }

    StationController CreateStation(DiContainer subContainer, TubeCreateParam createParam) {
        StationController stationController =
            subContainer.InstantiatePrefabForComponent<StationController>(createParam.Prefab,
                GameObject.Find("Tubes").transform);
        stationController.transform.position =
            new Vector3(createParam.Position.x, createParam.Position.y, 0);
        stationController.Rotate = createParam.Rotation;
        stationController.Tube.Projection = TubeProjectionType.STATIC;
        stationController.name = "St_" + stationController.StationType + "_" + ++lastTubeNumber; 
        return stationController;
    }

    TrainController CreateTrain(DiContainer subContainer, TrainCreateParam createParam) {
        TrainController controller =
            subContainer.InstantiatePrefabForComponent<TrainController>(createParam.Prefab,
                GameObject.Find("Trains").transform);
        controller.transform.position = new Vector3(createParam.Position.x, createParam.Position.y, 0f);
        return controller;
    }    
    
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
    
    MenuButtonController CreateMenuBtn(DiContainer subContainer, LevelBtnParam createParam) {
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
    
    BuyMoneyDialogController CreateBuyHintDialog(DiContainer subContainer, BuyHintDialogParam createParam) {
        GameObject gameMenu = GameObject.Find("GameMenu(Clone)");
        GameObject startMenu = GameObject.Find("StartMenu(Clone)");
        BuyMoneyDialogController controller =
            subContainer.InstantiatePrefabForComponent<BuyMoneyDialogController>(prefabsUI.BuyHintDialog,
                (gameMenu != null ? gameMenu : startMenu).transform);
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

    TutorialController CreateTutorial(DiContainer subContainer, TutorialParams createParam) {
        GameObject gameMenu = GameObject.Find("GameMenu(Clone)");
        GameObject startMenu = GameObject.Find("StartMenu(Clone)");
        TutorialController controller =
            subContainer.InstantiatePrefabForComponent<TutorialController>(prefabsUI.Tutorial,
                (gameMenu != null ? gameMenu : startMenu).transform);
        controller.Init(createParam);
        return controller;
    }


    void InstallSignals() {
        // Every scene that uses signals needs to install the built-in installer SignalBusInstaller
        // Or alternatively it can be installed at the project context level (see docs for details)
        SignalBusInstaller.Install(Container);

        // Signals can be useful for game-wide events that could have many interested parties
        // Container.DeclareSignal<AddProductSignal>();
        Container.DeclareSignal<LevelStartSignal>();
        Container.DeclareSignal<ResetProgressSignal>();
        Container.DeclareSignal<LevelCompleteSignal>();
        Container.DeclareSignal<HintExecutedSignal>();
        Container.DeclareSignal<BuyMoneySignal>();
        Container.DeclareSignal<PurchasePackageSignal>();
        Container.DeclareSignal<PurchaseWagonSignal>();
        Container.DeclareSignal<PurchaseHintSignal>();
        Container.DeclareSignal<MoneyChangeSignal>();
    }
    
    public static int LastTubeNumber => lastTubeNumber;

    public static int LastInventoryNumber => lastInventoryNumber;
}