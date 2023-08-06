using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

public class InventoryManager {
    [Inject] private GameSettingsInstaller.GameSetting setting;
    [Inject] private TubeManager _tubeManager;
    [Inject] private GameSettingsInstaller.GameSetting _setting;
    [Inject] private InventoryItemController.Factory inventoryItemFactory;
    [Inject] private OtherItemController.Factory otherItemController;

    private Dictionary<TubeType, int> inventory = new Dictionary<TubeType, int>();
    private List<InventoryItemController> uiObjects = new List<InventoryItemController>();

    public void Unload(List<InventoryDto> inventoryDtos) {
        foreach (InventoryItemController element in uiObjects.Where(element => element != null && element.gameObject != null)) {
            Object.Destroy(element.gameObject);
        }
        uiObjects.Clear();
        inventory.Clear();
        if (_setting.isDebug) {
            foreach (TubeType tubeType in Enum.GetValues(typeof(TubeType))) {
                if (tubeType == TubeType.NONE) {
                    continue;
                }

                InventoryItemController element = inventoryItemFactory.Create(tubeType, 50);
                uiObjects.Add(element);
            }
            otherItemController.Create().InitStone(StoneType.ROCK);
            otherItemController.Create().InitStone(StoneType.STONE);
            otherItemController.Create().InitStone(StoneType.TREE);
            otherItemController.Create().InitRiver(RiverType.ONE);
            otherItemController.Create().InitRiver(RiverType.ANGEL);
            otherItemController.Create().InitPortal();
            otherItemController.Create().InitStation();
            return;            
        }

        foreach (InventoryDto inventoryDto in inventoryDtos) {
            int count = 0;
            if (inventory.ContainsKey(inventoryDto.tubeType)) {
                count = inventory[inventoryDto.tubeType];
            }

            count = count + 1;
            inventory[inventoryDto.tubeType] = count;
        }
        
        foreach (KeyValuePair<TubeType,int> item in inventory) {
            if (item.Value > 0) {
                InventoryItemController element = inventoryItemFactory.Create(item.Key, item.Value);
                uiObjects.Add(element);
            }
        }
    }

    public void GetOutOfInventory(TubeType tubeType) {
        if (!setting.isDebug) {
            if (!inventory.ContainsKey(tubeType)) {
                Debug.LogError("No in inventory " + tubeType);
            }
        
            int count = inventory[tubeType];
            if (count == 0) {
                Debug.LogError("No in inventory " + tubeType);
            }
        
            int currentCount = count - 1;
            inventory[tubeType] = currentCount;
            foreach (InventoryItemController element in uiObjects) {
                if (element.TubeType == tubeType) {
                    element.UpdateCount(currentCount);
                    if (currentCount == 0) {
                        uiObjects.Remove(element);
                    }
                    break;
                }
            }
        }
    
        _tubeManager.Create(new InventoryDto(tubeType, Constants.CREATE_POSITION, 0, TubeProjectionType.SIMPLE));
    }

    public void PutToInventory(TubeController tubeController) {
        if (!setting.isDebug) {
            int currentCount = inventory[tubeController.TubeType];
            int nextCount = currentCount + 1;
            if (currentCount == 0) {
                InventoryItemController element = inventoryItemFactory.Create(tubeController.TubeType, nextCount);
                uiObjects.Add(element);
            }
            else {
                InventoryItemController item = uiObjects.First(i => i.TubeType == tubeController.TubeType);
                item.UpdateCount(nextCount);
            }

            inventory[tubeController.TubeType] = nextCount;
        }

        _tubeManager.Remove(tubeController.GetComponent<Removable>());
    }
}