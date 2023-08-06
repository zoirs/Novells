using UnityEngine;
using UnityEngine.UI;

public class ScrollController : MonoBehaviour {
    private ScrollRect _scroll;

    private void Start() {
        _scroll = GetComponent<ScrollRect>();
    }

    private void Update() {
        if (!_scroll.enabled) {
            if (Input.GetMouseButtonUp(0)) {
                _scroll.enabled = true;
            }
        }
    }
}