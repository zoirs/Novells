using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class HintManager : TubeManager {

    [Inject] private DialogManager _dialogManager;
    [Inject] private CountHintManager _countHintManager;

    private List<InventoryDto> availableHints;

    public HintManager(GameMapService gameMapService,
        GameSettingsInstaller.GameSetting setting,
        GameSettingsInstaller.PrefabSettings prefabSettings, TubeController.Factory factoryTube, DiContainer container)
        : base(gameMapService, setting, prefabSettings, factoryTube, container) {
        
    }

    public void Unload(List<InventoryDto> inventoryDtos) {
        availableHints = inventoryDtos;
        base.Reload(new List<InventoryDto>());
    }

    public bool GetOutHint() {
        if (_countHintManager.HintCount() == 0 || availableHints.Count == 0) {
            _dialogManager.BuyHintDialog();
            return false;
        }

        int index = Random.Range(0, availableHints.Count);
        InventoryDto possibleHint = availableHints[index];
        availableHints.Remove(possibleHint);
        _countHintManager.DecHintCount();
        Create(new InventoryDto(possibleHint.tubeType, possibleHint.position, possibleHint.rotate, TubeProjectionType.HINT));
        return true;
    }
}