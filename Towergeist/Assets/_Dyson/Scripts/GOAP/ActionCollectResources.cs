using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JW.Grid.GOAP.Actions;

namespace Dyson.Towergeist
{
    public class ActionCollectResources : ActionBase
    {
        [Header("Collect Action")] private List<Type> supportedGoals = new List<Type>() { typeof(GoalCollect) };
        
        public override List<Type> GetSupportedGoals()
        {
            return supportedGoals;
        }
        public override void OnActivated()
        {
            Debug.Log("Activation of collecting resources");
        }

        public override void OnTick(float dt)
        {
           // OnActivated();
           //Debug.Log("Activation of collecting resources");
        }
    }
}
