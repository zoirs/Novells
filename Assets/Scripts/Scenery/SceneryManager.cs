using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using Zenject;

public class SceneryManager :IInitializable{
    private SceneType _sceneType;
    private List<SceneData> scenario = new List<SceneData>();

    private int position;
    private Dictionary<string, SceneDto> _scenes = new Dictionary<string, SceneDto>();

    public void Initialize() {

        TextAsset[] all = Resources.LoadAll<TextAsset>("Story/1_room/");
        
        foreach (TextAsset textAsset in all) {
            SceneDto scene = JsonConvert.DeserializeObject<SceneDto>(textAsset.text);
            _scenes.Add(textAsset.name, scene);
        }
        
        TextAsset levelasset = Resources.Load<TextAsset>("Story/1_room/room_1_scene_1");
        Debug.Log(levelasset.text);
        SceneDto fromJson = JsonConvert.DeserializeObject<SceneDto>(levelasset.text);
        
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
    
    public Tuple<string, Frame> next( NextFrameBtnParam param) {
        SceneDto sceneDto = _scenes[param.getFileName()];
        return  new Tuple<string, Frame>(sceneDto.place, sceneDto.frames[param.Frame]);
    }
}