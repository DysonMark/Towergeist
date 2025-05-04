using System;
using System.Collections.Generic;
using AStarPathFinding.PathFinder;
using Dyson.Towergeist;
using JW.Grid.GOAP.Goals;
using Movement;
using Resources;
using UnityEngine;

namespace JW.Grid.GOAP.Actions
{
    //[RequireComponent(typeof(AI))]
    [RequireComponent(typeof(AreaMover))]
    [RequireComponent(typeof(ResourceManager))]
    [RequireComponent(typeof(WhichAgent))]
    public class ActionBase : MonoBehaviour
    {
        [Header("Base Action")]
        public int Cost;

        protected AreaMover moveSystem;
        protected ResourceManager resourceManager;
        protected WhichAgent agentType;
        // Add the AI script here for things like states and movement stuff
        private void Awake()
        {
            //Agent = GetComponent<AI>();
            moveSystem = GetComponent<AreaMover>();
            resourceManager = GetComponent<ResourceManager>();
            agentType = GetComponent<WhichAgent>();
        }

        public virtual List<Type> GetSupportedGoals()
        {
            return null;
        }

        public virtual float GetCost()
        {
            return Cost;
        }

        public virtual void OnActivated()
        {
        }

        public virtual void OnDeactivated()
        {
        }

        public virtual void OnTick(float dt)
        {
        }
    }
}