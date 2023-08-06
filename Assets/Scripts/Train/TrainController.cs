using System;
using System.Collections.Generic;
using Main;
using PathCreation;
using Train;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(FollowerController))]
public class TrainController : MonoBehaviour {

    [Inject] private GameController _gameController;
    [SerializeField] private TrainType _trainType;
    
    private List<PathCreator> paths;
    private int currentPath = -1;
    
    
    public float speed = 4.5f;
    private float distanceTraveled;
    private int index;
    private float delta = 1f;
    private FollowerController _followerController;
    private TrainController next;
    // private PathCreator _pathController1;

    private void Start() {
        _followerController = GetComponent<FollowerController>();
    }

    public Vector3Int GetVector() {
        Vector3 p = transform.position;
        return new Vector3Int((int) Math.Round(p.x, 0),(int) Math.Round(p.y, 0),(int) Math.Round( p.z, 0));
    }

    public class Factory : PlaceholderFactory<TrainCreateParam, TrainController> { }

    public TrainType TrainType => _trainType;

    private void Update() {
        if (paths == null || paths.Count == 0 || currentPath < 0) {
            return;
        }

        distanceTraveled += speed * Time.deltaTime;

        EndOfPathInstruction stop = EndOfPathInstruction.Stop;
        transform.position = paths[currentPath].path.GetPointAtDistance(distanceTraveled - index * delta , stop);
        transform.rotation = paths[currentPath].path.GetRotationAtDistance(distanceTraveled - index * delta, stop);

        if (_gameController.State == GameStates.GoTrain
            && distanceTraveled > (paths[currentPath].path.length - 0.1f)) {
            distanceTraveled = 0;
            currentPath++;
            if (currentPath >= paths.Count) {
                currentPath = -1;
                paths.Clear();
            //StopTrain();
            // Debug.Log("LevelComplete");
             _gameController.State = GameStates.LevelComplete;
            }

        }
    }

    private void StopTrain() {
        this.paths.Clear();
        if (this.next != null) {
            this.next.StopTrain();
        }
    }

    public void Run(List<PathCreator> paths, int index, TrainController next) {
        // _followerController.Add(pathController, index);
        this.index = index;
        this.paths = paths;
        this.currentPath = 0;
        // _pathController1 = pathController1;
        this.next = next;
        distanceTraveled = 1.25f;
    }
}