using UnityEngine;
using Zenject;

public class GameMapService : IInitializable{
    
    [Inject] private GameSettingsInstaller.GameSetting setting;

    
    public int Wight => 9;
    public int Hight => 12;

    private MapBusyWeight[,] map = new MapBusyWeight[9,12]; 
    // int[,] map = new int[,] {
    //     {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
    //     {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
    //     {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
    //     {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
    //     {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
    //     {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
    //     {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
    //     {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
    //     {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
    // };

    TextMesh[,] debugTextArray = new TextMesh[16, 16];


    private void InitDebug() {
        for (int i = 0; i < Wight; i++) {
            for (int j = 0; j < Hight; j++) {
                debugTextArray[i, j] = CreateWorldText(map[i, j].ToString(), null,
                    new Vector3(i, j, 0) + new Vector3(0.1f, 0.1f, 0), 10, Color.white, TextAnchor.MiddleCenter);
            }
        }
    }
    
    public void Reload() {
        for (int i = 0; i < Wight; i++) {
            for (int j = 0; j < Hight; j++) {
                 map[i, j] = MapBusyWeight.FREE;
            }
        }
    }

    public void SetToMap(PointController[] list, MapBusyWeight weight) {
        foreach (PointController pointController in list) {
            Vector3Int p = pointController.GetVector();
            map[p.x, p.y] = weight;
            //Debug.Log("set to map " +" x="+ p.x +" y="+ p.y+ "; " + weight);
        }

        printDebug();
    }

    public void Free(PointController[] list) {
        foreach (PointController pointController in list) {
            Vector3Int p = pointController.GetVector();
            if (map[p.x, p.y] == MapBusyWeight.BRIDGE_ON_RIVER) {
                map[p.x, p.y] = MapBusyWeight.RIVER;
            } else {
                map[p.x, p.y] = MapBusyWeight.FREE;
            }
        }

        printDebug();
    }
    
    public bool Available(PointController[] list) {
        foreach (PointController pointController in list) {
            Vector3Int p = pointController.GetVector();
            if (p.x < 0 || p.x >= Wight || p.y < 0 || p.y >= Hight) {
                return false;
            }
        }
        return true;
    }

    public Vector3Int GetFreePoint() {
        while (true) {
            int x = Random.Range(0 + 2, Wight - 2);
            int y = Random.Range(0 + 2, Hight - 2);
            if (map[x, y] == MapBusyWeight.FREE) {
                return new Vector3Int(x, y, 0);
            }
        }
    }

    public PositionState Check(PointController[] list) {
        int outSidePoint = 0;
        bool isOnRiver = false;
        foreach (PointController pointController in list) {
            Vector3Int p = pointController.GetVector();
            if (p.x < 0 || p.x >= Wight || p.y < 0 || p.y >= Hight) {
                outSidePoint++;
                continue;
            }

            if (map[p.x, p.y] == MapBusyWeight.RIVER) {
                isOnRiver = true;
            } 
            if (map[p.x, p.y] == MapBusyWeight.BUSY) {
                return PositionState.BUSY;
            }
        }

        if (outSidePoint == 0) {
            return isOnRiver ? PositionState.RIVER : PositionState.CORRECT;
        }
        return outSidePoint < list.Length ? PositionState.OUTSIDE_PARTLY : PositionState.OUTSIDE_FULL;
    }

    public bool CheckPoint(Vector3Int p) {
        if (p.x < 0 || p.x >= Wight || p.y < 0 || p.y >= Hight) {
            return false;
        }

        return (map[p.x, p.y] != MapBusyWeight.FREE);
    }

    private void printDebug() {
        if (!setting.isDebug) {
            return;
        }

        for (int i = 0; i < Wight; i++) {
            for (int j = 0; j < Hight; j++) {
                // CreateWorldText(map[i, j].ToString(), null,
                // new Vector3(i, j, 0) + new Vector3(0.4f, 0.4f, 0), 15, Color.white, TextAnchor.MiddleCenter);
                debugTextArray[i, j].text = map[i, j].ToString();
            }
        }

        // TextMesh[,] debugTextArray = new TextMesh[width, height];
        // gridArray = new int[width, height];
        //
        // debugTextArray[eventArgs.x, eventArgs.y].text = gridArray[eventArgs.x, eventArgs.y].ToString();
        // debugTextArray[x, y] = CreateWorldText(gridArray[x, y].ToString(), null,
        //     GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f, 30, Color.white, TextAnchor.MiddleCenter);
    }


    // Create Text in the World
    public static TextMesh CreateWorldText(string text, Transform parent = null,
        Vector3 localPosition = default(Vector3), int fontSize = 40, Color? color = null,
        TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left,
        int sortingOrder = 5000) {
        if (color == null) color = Color.white;
        return CreateWorldText(parent, text, localPosition, fontSize, (Color) color, textAnchor, textAlignment,
            sortingOrder);
    }

    // Create Text in the World
    public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize,
        Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder) {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;
    }

    public void Initialize() {
        if (setting.isDebug) {
            InitDebug();
        }
    }
}