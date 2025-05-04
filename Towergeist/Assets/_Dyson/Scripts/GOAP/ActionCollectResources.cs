using System;
using System.Collections;
using System.Collections.Generic;
using Agents.Goals;
using AStarPathFinding.PathFinder;
using UnityEngine;
using JW.Grid.GOAP.Actions;
using Resources;
using JW.Grid.GOAP.Goals;
using Movement;

namespace Dyson.Towergeist
{
    public class ActionCollectResources : ActionBase
    {
        [Header("Collect Action")] 
        private List<Type> supportedGoals = new List<Type>() { typeof(GoalWork) };
        
        [Header("Resources")]
        [SerializeField] private float _resourceTargetAmount;
        public override List<Type> GetSupportedGoals()
        {
            return supportedGoals;
        }
        public override void OnActivated()
        {
            // Move to resource site
            moveSystem.MoveTo(AreaMover.Destination.GatheringArea);
            // If we have enough resources, drop them off at the tower
            
            Debug.Log("Activation of collecting resources");
        }

        public override void OnTick(float dt)
        {
            if (resourceManager.IsOnResourceSite)
            {
                if (resourceManager.GetResourceAmount(agentType.GetAgentType()) > _resourceTargetAmount)
                {
                    if (!moveSystem.IsMoving)
                    {
                        moveSystem.MoveTo(AreaMover.Destination.WorkingArea);
                    }
                }
            }
        }
    }
}
