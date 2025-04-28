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

        public override void Idle()
        {
            throw new System.NotImplementedException();
        }

        public override void BaseToFloor()
        {
            throw new System.NotImplementedException();
        }

        public override void FloorToDepot()
        {
            throw new System.NotImplementedException();
        }

        public override void DepotToTower()
        {
            throw new System.NotImplementedException();
        }

        public override void Chatting()
        {
            throw new System.NotImplementedException();
        }
    }
}