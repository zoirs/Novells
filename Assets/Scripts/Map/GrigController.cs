using UnityEngine;
using Zenject;

public class GrigController : MonoBehaviour {
    [Inject] private GameMapService _mapService;
    private void Start() {
        Show();
    }

    void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.2f) {
        GameObject line = new GameObject();
        line.transform.SetParent(gameObject.transform);
        line.transform.position = start;
        line.AddComponent<LineRenderer>();
        LineRenderer lr = line.GetComponent<LineRenderer>();
        lr.material = Resources.Load("Materials/Grid", typeof(Material)) as Material;
        lr.startColor = color;
        lr.endColor = color;
        lr.SetWidth(0.05f, 0.05f);
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }

    public void Show() {
        float delta = 0.5f;

        for (int i = 0; i <= _mapService.Hight ; i++) {
            DrawLine(new Vector3(   - delta, i - delta, 4.9f), new Vector3(_mapService.Wight - delta,  i- delta, 4.9f), Color.green);
        }
        
        for (int i = 0; i <= _mapService.Wight ; i++) {
            DrawLine(new Vector3(i - delta,  - delta, 4.9f), new Vector3( i- delta, _mapService.Hight - delta, 4.9f), Color.green);
        }
    }

    public void Hide() { }

}
