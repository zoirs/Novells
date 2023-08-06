using Main;
using UnityEngine;
using Zenject;
using MenuSystemWithZenject;
using MenuSystemWithZenject.Elements;
using UnityEngine.UI;


public class GlobalInstaller : MonoInstaller<GlobalInstaller> {
    // public GameObject startMenu;
    // public GameObject gameMenu;
    // public GameObject buttonMenu;


    public override void InstallBindings() {
        Debug.Log("GlobalInstaller Global Binding.");

        // Bind MenuSystemWithZenject
        // Container.Bind<MenuManager>().AsSingle();
        //
        // Container.Bind<GameObject>().FromInstance(startMenu).WhenInjectedInto<StartMenu.CustomMenuFactory>();
        // Container.BindFactory<StartMenu, StartMenu.Factory>().FromFactory<StartMenu.CustomMenuFactory>();
        //
        // Container.Bind<GameObject>().FromInstance(gameMenu).WhenInjectedInto<GameMenu.CustomMenuFactory>();
        // Container.BindFactory<GameMenu, GameMenu.Factory>().FromFactory<GameMenu.CustomMenuFactory>();

        // InstallSignals();
    }
    
    void InstallSignals() {
        // Every scene that uses signals needs to install the built-in installer SignalBusInstaller
        // Or alternatively it can be installed at the project context level (see docs for details)
        // SignalBusInstaller.Install(Container);

        // Signals can be useful for game-wide events that could have many interested parties
        // Container.DeclareSignal<LevelStartSignal>();
    }
}