using System;
using Main;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MenuSystemWithZenject.Elements {
    public class LevelNameController : MonoBehaviour {
        [SerializeField] private Text text;

        public void LoadLevel(LevelPath index) {
            text.text = "Уровень " + index.Package + " " + index.Number;
        }
    }
}