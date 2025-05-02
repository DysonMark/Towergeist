using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JW.Grid.GOAP.Actions;
using Resources;

namespace Dyson.Towergeist
{
    public class ActionCollectResources : ActionBase
    {
        [Header("Collect Action")] private List<Type> supportedGoals = new List<Type>() { typeof(GoalCollect) };
        [SerializeField] private ResourceManager _resources;
        [SerializeField] private Transform stoneEndPoint;
        [SerializeField] private Transform testPoint;
        public override List<Type> GetSupportedGoals()
        {
            return supportedGoals;
        }
        public override void OnActivated()
        {
            if (_resources.GetResourceAmount(ResourceManager.ResourceType.Stone) <= 0)
            {
                moveSystem.endPoint = stoneEndPoint;
            }
            Debug.Log("Activation of collecting resources");
        }

        public override void OnTick(float dt)
        {
            if (_resources.GetResourceAmount(ResourceManager.ResourceType.Stone) >= 15)
            {
                _resources.StoneAmount = 15;
                moveSystem.endPoint = testPoint;
            }
           // OnActivated();
           //Debug.Log("Activation of collecting resources");
        }
    }
}
