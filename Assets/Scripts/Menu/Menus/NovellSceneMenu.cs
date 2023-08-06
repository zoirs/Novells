using System;
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

    [Inject] private SceneryManager _sceneryManager;
    [Inject] private NextFrameButtonController.Factory _factory;


    private void Start() {
        LoadNext(new NextFrameBtnParam(1,1,0), true);
    }

    private void LoadNext(NextFrameBtnParam nextFrameBtnParam, bool isNewScene) {
        Tuple<string,Frame> tuple = _sceneryManager.next(nextFrameBtnParam);
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
        _bubleSpeech.SetText(frame.hero.name, frame.text);
        foreach (ButtonDto button in frame.buttons) {
            
            if ("End".Equals(button.type)) {
                // NextFrameButtonController nextFrameButtonController = _factory.Create(new NextFrameBtnParam(frame., 1, 1));

            }

            NextFrameButtonController nextFrameButtonController;
            Button componentInChildren;
            if ("Frame".Equals(button.type)) {
                NextFrameBtnParam frameBtnParam = new NextFrameBtnParam(nextFrameBtnParam.Room, nextFrameBtnParam.Scene, nextFrameBtnParam.Frame + 1);
                nextFrameButtonController = _factory.Create(frameBtnParam);
                 componentInChildren = nextFrameButtonController.GetComponentInChildren<Button>();
                componentInChildren.onClick.AddListener(() => {
                    LoadNext(frameBtnParam, false);
                    Destroy(componentInChildren.gameObject);
                });
            }
            if (button.type.StartsWith("room_") && button.type.Contains("_scene_")) {
                int parseSceneIndex = button.type.IndexOf("_scene_", StringComparison.Ordinal);
                Debug.Log(parseSceneIndex - "room_".Length);
                string roomIndex = button.type.Substring("room_".Length, parseSceneIndex - "room_".Length);
                string sceneIndex = button.type.Substring(parseSceneIndex + "_scene_".Length);
                string caption = button.caption;
                if (button.price > 0) {
                    caption = caption + " (\uD83D\uDC8E" + button.price + ")";
                }
                Debug.Log(button.type+" ; " +roomIndex + " ; " + sceneIndex);
                // componentInChildren.na
                NextFrameBtnParam frameBtnParam = new NextFrameBtnParam(Convert.ToInt32(roomIndex), Convert.ToInt32(sceneIndex), 0);
                nextFrameButtonController = _factory.Create(frameBtnParam);
                componentInChildren = nextFrameButtonController.GetComponentInChildren<Button>();
                componentInChildren.onClick.AddListener(() => {
                    LoadNext(frameBtnParam, true);
                    Destroy(componentInChildren.gameObject);
                });

            }
            
            
            // = _factory.Create(new NextFrameBtnParam(frame., 1, 1));
            // Button componentInChildren = nextFrameButtonController.GetComponentInChildren<Button>();
            // componentInChildren.onClick.AddListener(() => {
                // LoadNext(nextFrameButtonController.NextFrameBtnParam);
                // Destroy(componentInChildren.gameObject);
            // });   
        }
    }
}