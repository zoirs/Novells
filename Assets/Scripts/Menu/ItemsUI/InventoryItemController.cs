using MenuSystemWithZenject.ItemsUI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class InventoryItemController : MonoBehaviour  {
    [SerializeField] private Text count;
    [SerializeField] private RawImage texture;

    [Inject] private GameSettingsInstaller.TubeButtonSettings textures;
    [Inject] private InventoryManager _inventoryManager;

    private TubeType tubeType;
    
    private void Start() {
        ScrollRect scroll = GetComponentInParent<ScrollRect>();
        SwipeDetector swipeDetector = GetComponent<SwipeDetector>();
        swipeDetector.Callback = () => {
            scroll.enabled = false;
            _inventoryManager.GetOutOfInventory(tubeType);
        };
    }

    public void Init(TubeType tubeType, int inventoryCount) {
        this.tubeType = tubeType;
        texture.texture = tubeType.GetTexture(textures);
        UpdateCount(inventoryCount);
    }

    public void UpdateCount(int updatedCount) {
        count.text = "* " + updatedCount;
        if (updatedCount == 0) {
            Destroy(gameObject);
        }
    }

    public TubeType TubeType => tubeType;

    public class Factory : PlaceholderFactory<TubeType, int, InventoryItemController> { }
}