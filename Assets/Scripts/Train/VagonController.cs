using System;
using UnityEngine;
using UnityEngine.AI;

public class VagonController : MonoBehaviour {
    
    private NavMeshAgent current;
    private NavMeshAgent head;
    
        private void Start() {
            current = GetComponent<NavMeshAgent>();
        }

        private void Update() {
            // if (Input.GetMouseButtonDown(0)) {
                // current.SetDestination(head.destination);
            // }
        }

        public NavMeshAgent Current => current;
}