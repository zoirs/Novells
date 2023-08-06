using System;
using System.Collections.Generic;
using MenuSystemWithZenject;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class NovellSceneMenu : Menu<NovellSceneMenu> {
    [SerializeField] private Image back;
    [SerializeField] private Image heroLeft;
    [SerializeField] private Image heroRight;
    [SerializeField] private BubleSpeechController _bubleSpeech;
    [SerializeField] private Animator animator;
    [SerializeField] private HorizontalLayoutGroup answerButtons;

    [Inject] private SceneryManager _sceneryManager;
    [Inject] private NextFrameButtonController.Factory _factory;


    private void Start() {
        LoadNext(1,1,0, true);
    }

    private void LoadNext(int room, int scene, int frameIndex, bool isNewScene) {
        foreach (NextFrameButtonController buttonController in answerButtons.GetComponentsInChildren<NextFrameButtonController>()) {
            Destroy(buttonController.gameObject);
        }
        Debug.Log("Load room " + room + "; scene: " + scene + "; frame: " +frameIndex);
        Tuple<string,Frame> tuple = _sceneryManager.next(room, scene, frameIndex);
        Frame frame = tuple.Item2;
        string place = tuple.Item1;

        if (isNewScene) {
            back.sprite = Resources.Load<Sprite>("Place/" + place);
        }

        if (frame.sceneType == SceneType.LEFT) {
            heroLeft.sprite = Resources.Load<Sprite>("Hero/" + frame.hero.name + "/Base");
        }

        if (frame.sceneType == SceneType.RIGHT) {
            heroRight.sprite = Resources.Load<Sprite>("Hero/" + frame.hero.name + "/Base");
        }

        if (frame.sceneType == SceneType.CENTER) {
        }

        animator.SetTrigger(frame.sceneType.GetTrigger());
        _bubleSpeech.SetText(frame.hero.name, frame.text, () => {ShowButtons(room, scene, frameIndex,frame.buttons);});
    }

    private void ShowButtons(int room, int scene, int frameIndex, List<ButtonDto> frameButtons) {
        foreach (ButtonDto button in frameButtons) {
            if ("End".Equals(button.type)) {
                // NextFrameButtonController nextFrameButtonController = _factory.Create(new NextFrameBtnParam(frame., 1, 1));
            }

            if ("Frame".Equals(button.type)) {
                _factory.Create(new NextFrameBtnParam(() => LoadNext(room, scene, frameIndex + 1, true)));
            }

            if (button.type.StartsWith("room_") && button.type.Contains("_scene_")) {
                int parseSceneIndex = button.type.IndexOf("_scene_", StringComparison.Ordinal);
                string roomIndex = button.type.Substring("room_".Length, parseSceneIndex - "room_".Length);
                string sceneIndex = button.type.Substring(parseSceneIndex + "_scene_".Length);
                string caption = button.caption;
                if (button.price > 0) {
                    caption = caption + " (\uD83D\uDC8E" + button.price + ")";
                }

                _factory.Create(new NextFrameBtnParam(() =>
                    LoadNext(Convert.ToInt32(roomIndex), Convert.ToInt32(sceneIndex), 0, true)));
            }
        }
    }
}