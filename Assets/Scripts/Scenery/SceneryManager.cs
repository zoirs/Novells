using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using Zenject;

public class SceneryManager :IInitializable{
    private SceneType _sceneType;
    private List<SceneData> scenario = new List<SceneData>();

    private int position;
    private Dictionary<int, Dictionary<string, SceneDto>> stories = new Dictionary<int, Dictionary<string, SceneDto>>();
    
    private static readonly string STORY_PREFIX = "Story_";

    public void Initialize() {

        // string resourcsPath = Application.dataPath + "/Resources";

        string[] fileNames = {"Common", "Story_1", "Story_2"};
        foreach (string fileName in fileNames) {
            string folder = fileName;
            if (folder.StartsWith(STORY_PREFIX)) {
                short storyNumber = Int16.Parse(folder.Substring(STORY_PREFIX.Length));
                TextAsset[] all = Resources.LoadAll<TextAsset>(folder + "/Scenary/");
                Debug.Log("== " + folder + "/Scenary");
                Debug.Log("== " + all.Length);
                Dictionary<string, SceneDto> story = new Dictionary<string, SceneDto>();
                foreach (TextAsset textAsset in all) {
                    SceneDto scene = JsonConvert.DeserializeObject<SceneDto>(textAsset.text);
                    story.Add(textAsset.name, scene);
                }
                stories.Add(storyNumber, story);
            }
        }
        
       
        
        // TextAsset levelasset = Resources.Load<TextAsset>("Story/1_room/room_1_scene_1");
        // Debug.Log(levelasset.text);
        // SceneDto fromJson = JsonConvert.DeserializeObject<SceneDto>(levelasset.text);
        SceneDto fromJson = stories[1]["room_1_scene_1"];
        foreach (Frame frame in fromJson.frames) {
            Debug.Log("sceneType " + frame.sceneType);
            if (frame.sceneType == SceneType.LEFT) {
                scenario.Add(SceneData.SpeakLeft(frame.text, frame.hero.name));
            }
            if (frame.sceneType == SceneType.RIGHT) {
                scenario.Add(SceneData.SpeakRight(frame.text, frame.hero.name));
            }
            if (frame.sceneType == SceneType.CENTER) {
                scenario.Add(SceneData.Description(frame.text, fromJson.place));
            }
        }
        Debug.Log("Frames count " + scenario.Count);
    }

    public SceneData next() {
        SceneData sceneData = scenario[position];
        position++;
        return sceneData;
    }
    
    // public Tuple<string, Frame> next( NextFrameBtnParam param) {
    //     SceneDto sceneDto = _scenes[param.getFileName()];
    //     return  new Tuple<string, Frame>(sceneDto.place, sceneDto.frames[param.Frame]);
    // }

    public Tuple<string, Frame> next(int story, int scene, int frameIndex) {
        string room = "room_" + story + "_scene_" + scene;
        Debug.Log(room);
        SceneDto sceneDto = stories[story][room];
        return new Tuple<string, Frame>(sceneDto.place, sceneDto.frames[frameIndex]);
    }
}