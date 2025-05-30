using System;
using AStarPathFinding.PathFinder;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Dyson.Towergeist
{
    public class StoneAgent : AgentState
    {
        [SerializeField] private AgentState _state;
        public CapsuleMover _capsuleMover;
        [SerializeField] private GameObject newEndPoint;
        [SerializeField] private GameObject backToBase;
        private bool isBaseFull;
        private bool isRoaming;
        private int stones;
        public override void Start()
        {
         //   _state = new StoneAgent();
            _state.speed = 20;
        }

        private void Update()
        {
            BaseToFloor();
            RaycastHit hit;
            
            if (Physics.SphereCast(transform.position + Vector3.up * 0.5f, 0.3f, Vector3.down, out hit, 3f))
            {
                if (hit.collider.CompareTag("Stone"))
                {
                    Debug.Log("Stone");
                    Destroy(hit.collider.gameObject);
                    stones++;
                  //  isRoaming = false;
                    //newEndPoint.transform.position = backToBase.transform.position;
                    //if (newEndPoint.transform.position == backToBase.transform.position)
                      //  isRoaming = true;
                    //Debug.Log("Collect: " + stones);
                }
            }
        }

        private void ReturnToBase()
        {
            newEndPoint.transform.position = backToBase.transform.position;
            var collected = stones;
            if (stones <= 0)
            {
                stones = 0;
            }
            Debug.Log("Collected: " + collected);
            if (collected >= 5)
            {
                Debug.Log("Base full of stones");
                isBaseFull = true;
            }
            //BaseToFloor();
        }
        public override void BaseToFloor()
        {
            //TODO: Roam in the base, when stone is found, take it and put it at the entrance
            
            if (_capsuleMover.hasArrivedToDestination)
            {
                isRoaming = true;
                Invoke("AgentRoaming", 3);
                Invoke("ReturnToBase", 10);
            }
        }

        private void AgentRoaming()
        {
            var randomX = Random.Range(-6, 3);
            var randomZ = Random.Range(-6, 3);
            newEndPoint.transform.position = new Vector3(randomX, 0, randomZ);
            
            //Flags
            _capsuleMover.hasArrivedToDestination = false;
            isRoaming = false;
        }
        public override void FloorToDepot()
        {
            //TODO: When stone is full, take it to the depot and store it
        }
        
        public override void Idle()
        {
            //TODO: Check if stone is full before transporting
        }

        public override void DepotToTower()
        {
            //TODO: Check if depot is full of stone, if it is transport to Tower
        }

        public override void Chatting()
        {
            
        }
    }
}