﻿using System;
using System.Collections.Generic;
using MenuSystemWithZenject;
using UnityEngine;
using UnityEngine.Analytics;
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
    [Inject] private DialogManager _dialogManager;
    
    private static readonly string STORY_PREFIX = "Story_";
    private int clueCount = 0;
    private int lastScene;


    private void Start() {
        var story = 2;
        var scene = 1;
        if (PlayerPrefs.HasKey(PlayerPrefsUtils.STORY_PREFIX + story))
        {
            scene = PlayerPrefs.GetInt(PlayerPrefsUtils.STORY_PREFIX + story);
        }
        if (PlayerPrefs.HasKey(PlayerPrefsUtils.CLUE))
        {
            clueCount = PlayerPrefs.GetInt(PlayerPrefsUtils.CLUE);
        }

        LoadNext(story, scene,0, true);
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
        // if (lastScene != scene) {
            clue.SetText(clueCount, 8);
            // lastScene = scene;
        // }
        PlayerPrefs.SetInt(PlayerPrefsUtils.STORY_PREFIX + story, scene);
        AnalyticsEvent.LevelComplete(story * 1000 + scene);
        AnalyticsEvent.Custom(PlayerPrefsUtils.STORY_PREFIX + story, new Dictionary<string, object> {{"scene", scene}});
    }

    private void ShowButtons(int room, int scene, int frameIndex, List<ButtonDto> frameButtons) {
        foreach (ButtonDto button in frameButtons)
        {
            if ("End".Equals(button.type))
            {
                _factory.Create(new NextFrameBtnParam(() =>
                    {
                        _dialogManager.OpenAboutDialog();
                    },
                    LocalisationSystem.GetLocalisedValue("$story.btn.end", room)
                ));
            }

            string buttonCaption = LocalisationSystem.GetLocalisedValue(button.caption, room);
            if ("Frame".Equals(button.type)) {
                _factory.Create(new NextFrameBtnParam(() => LoadNext(room, scene, frameIndex + 1, true),buttonCaption));
            }

            int index = clueCount - 4 < 0 ? 0 : clueCount - 4;
            string nextFrame = "ClueFrame".Equals(button.type) ? button.clueFrame[index] : button.type;
            
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
                    clueCount += button.clue;
                    PlayerPrefs.SetInt(PlayerPrefsUtils.CLUE, clueCount);
                }, buttonCaption));
            }
        }
    }
}