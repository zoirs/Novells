using Main;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameButtonPanelController : MonoBehaviour {
    [SerializeField] Button exitBtn;
    [SerializeField] Button restartBtn;

    [Inject] private GameController _gameController;
    [Inject] readonly YesNoConfirmDialogController.Factory confirmDialogFactory;

    private void Start() {
        restartBtn.onClick.AddListener(() => {
            ConfirmDialogParam confirmDialog = new ConfirmDialogParam("$dlg.level.reset.header.text","$dlg.level.reset.body.text",
                () => { _gameController.RestartGame(); },
                () => { }, "$dlg.level.reset.btn.yes", "$dlg.level.reset.btn.no");
            confirmDialogFactory.Create(confirmDialog);
        });

        exitBtn.onClick.AddListener(() => {
            ConfirmDialogParam confirmDialog = new ConfirmDialogParam("$dlg.level.leave.header.text", "$dlg.level.leave.body.text", 
                () => { _gameController.GoToMenu(); },
                () => { }, "$dlg.level.leave.btn.yes", "$dlg.level.leave.btn.no");
            confirmDialogFactory.Create(confirmDialog);
        });
    }
}