using System;
using System.Collections.Generic;
using JW.Grid.GOAP.Goals;
using UnityEngine;

namespace JW.Grid.GOAP.Actions
{
    //[RequireComponent(typeof(AI))]
    public class ActionBase : MonoBehaviour
    {
        [Header("Base Action")]
        public int Cost;
        //protected AI Agent;
        // Add the AI script here for things like states and movement stuff
        private void Awake()
        {
            //Agent = GetComponent<AI>();
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