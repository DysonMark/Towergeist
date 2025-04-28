using System;
using UnityEngine;

namespace Dyson.Towergeist
{
    public class StoneAgent : AgentState
    {
        private AgentState _state;
        private void Start()
        {
            _state.speed = 20;
        }

        public override void BaseToFloor()
        {
           //TODO: Roam in the base, when stone is found, take it and put it at the entrance
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