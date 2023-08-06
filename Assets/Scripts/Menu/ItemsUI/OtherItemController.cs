using MenuSystemWithZenject.ItemsUI;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using Zenject;

public class OtherItemController : MonoBehaviour {
    [SerializeField] private RawImage texture;

    [Inject] GameSettingsInstaller.GameSetting _setting;
    [Inject] GameSettingsInstaller.OtherItemTextureSettings _textures;
    [Inject] GameSettingsInstaller.TubeButtonSettings _buttonSettings;
    [Inject] RiverManager _riverManager;
    [Inject] StoneManager _stoneManager;
    [Inject] PortalManager _portalManager;
    [Inject] StationManager _stationManager;

    private ScrollRect _scroll;
    private SwipeDetector _swipeDetector;

    private void Start() {
        Assert.IsTrue(_setting.isDebug);
    }

    private void Init() {
        _scroll = GetComponentInParent<ScrollRect>();
        _swipeDetector = GetComponent<SwipeDetector>();
    }

    public void InitStone(StoneType stoneType) {
        Init();
        texture.texture = stoneType.GetTexture(_textures);
        _swipeDetector.Callback = () => {
            _scroll.enabled = false;
            _stoneManager.CreateDebug(stoneType);
        };
    }

    public void InitRiver(RiverType riverType) {
        Init();
        texture.texture = riverType.GetTexture(_textures);
        _swipeDetector.Callback = () => {
            _scroll.enabled = false;
            _riverManager.CreateDebug(riverType);
        };
    }

    public void InitPortal() {
        Init();
        texture.texture = _textures.Portal;
        _swipeDetector.Callback = () => {
            _scroll.enabled = false;
            _portalManager.CreateDebug();
        };
    }

    public void InitStation() {
        Init();
        texture.texture = _buttonSettings.Station;
        _swipeDetector.Callback = () => {
            _scroll.enabled = false;
            _stationManager.CreateDebug();
        };
    }

    public class Factory : PlaceholderFactory<OtherItemController> { }
}