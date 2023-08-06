using System;
using Main;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Object = System.Object;

namespace MenuSystemWithZenject.Elements {
    public class LevelNameController : MonoBehaviour {
        [SerializeField] private Text text;

        public void LoadLevel(Object index) {
            text.text = "Уровень ";
        }
    }
}