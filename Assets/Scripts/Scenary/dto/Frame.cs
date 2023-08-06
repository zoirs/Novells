using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

[Serializable]
public class Frame {
    public string text;
    // public string backgroundImg;
    [JsonConverter(typeof(StringEnumConverter))]
    public SceneType sceneType;
    public HeroDto hero;
    public List<ButtonDto> buttons;
}