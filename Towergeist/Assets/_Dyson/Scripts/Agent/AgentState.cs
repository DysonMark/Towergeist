using JW.Grid.GOAP;
using UnityEngine;

namespace Dyson.Towergeist
{
    public abstract class AgentState : MonoBehaviour
    {
        public int speed;
        public abstract void Idle();
        public abstract void BaseToFloor();
        public abstract void FloorToDepot();
        public abstract void DepotToTower();
        public abstract void Chatting();
    }
}