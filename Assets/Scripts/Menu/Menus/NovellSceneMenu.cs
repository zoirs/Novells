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
    [SerializeField] private ClueController clue;
    [SerializeField] private Animator animator;
    [SerializeField] private LayoutGroup answerButtons;

    [Inject] private SceneryManager _sceneryManager;
    [Inject] private NextFrameButtonController.Factory _factory;
    
    private static readonly string STORY_PREFIX = "Story_";
    private int current = 3;
    private int lastScene;


    private void Start() {
        // LoadNext(2,1,0, true);
        LoadNext(2,18,0, true);
    }

    private void LoadNext(int story, int scene, int frameIndex, bool isNewScene) {
        foreach (NextFrameButtonController buttonController in answerButtons.GetComponentsInChildren<NextFrameButtonController>()) {
            Destroy(buttonController.gameObject);
        }
        Debug.Log("Load room " + story + "; scene: " + scene + "; frame: " +frameIndex);
        Tuple<string,Frame> tuple = _sceneryManager.next(story, scene, frameIndex);
        Frame frame = tuple.Item2;
        string place = tuple.Item1;

        if (isNewScene) {
            back.sprite = Resources.Load<Sprite>(STORY_PREFIX + story + "/Place/" + place);
        }

        string heroName = frame.hero?.name;
        string heroEmotion = frame.hero?.emotion;
        if (frame.sceneType == SceneType.LEFT && heroName != null && heroEmotion != null) {
            heroLeft.sprite = Resources.Load<Sprite>(STORY_PREFIX + story + "/Hero/" + heroName + "/" + heroEmotion);
        }

        if (frame.sceneType == SceneType.RIGHT && heroName != null && heroEmotion != null) {
            heroRight.sprite = Resources.Load<Sprite>(STORY_PREFIX + story + "/Hero/" + heroName + "/" + heroEmotion);
        }

        if (frame.sceneType == SceneType.CENTER) {
        }

        animator.SetTrigger(frame.sceneType.GetTrigger());
        Debug.Log("heroName " + heroName + " speech: " + frame.text);
        _bubleSpeech.SetText(story, heroName, frame.text, () => {ShowButtons(story, scene, frameIndex,frame.buttons);});
        if (lastScene != scene) {
            clue.SetText(current, 4);
            lastScene = scene;
        }
    }

    private void ShowButtons(int room, int scene, int frameIndex, List<ButtonDto> frameButtons) {
        foreach (ButtonDto button in frameButtons) {
            if ("End".Equals(button.type)) {
                // NextFrameButtonController nextFrameButtonController = _factory.Create(new NextFrameBtnParam(frame., 1, 1));
            }

            string buttonCaption = LocalisationSystem.GetLocalisedValue(button.caption, room);
            if ("Frame".Equals(button.type)) {
                _factory.Create(new NextFrameBtnParam(() => LoadNext(room, scene, frameIndex + 1, true),buttonCaption));
            }

            string nextFrame = "ClueFrame".Equals(button.type) ? button.clueFrame[current] : button.type;
            
            if (nextFrame.StartsWith("room_") && nextFrame.Contains("_scene_")) {
                int parseSceneIndex = nextFrame.IndexOf("_scene_", StringComparison.Ordinal);
                string roomIndex = nextFrame.Substring("room_".Length, parseSceneIndex - "room_".Length);
                string sceneIndex = nextFrame.Substring(parseSceneIndex + "_scene_".Length);
                string caption = buttonCaption;
                if (button.price > 0) {
                    caption = caption + " (\uD83D\uDC8E" + button.price + ")";
                }

                _factory.Create(new NextFrameBtnParam(() => {
                    LoadNext(Convert.ToInt32(roomIndex), Convert.ToInt32(sceneIndex), 0, true);
                    current = current + button.clue;
                }, buttonCaption));
            }
        }
    }
}