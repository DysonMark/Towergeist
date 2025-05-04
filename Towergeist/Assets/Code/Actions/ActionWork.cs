using System;
using System.Collections.Generic;
using JW.Grid.GOAP.Actions;
using UnityEngine;
using Stat;
using Actions.CompletionAnnouncement;
using Agents.Goals;
using Movement;

namespace Actions.Work
{
    public class ActionWork : ActionBase, ICompletableAction
    {
        #region Variables
        public int tiredThreshold = 50;
        [Tooltip("Tiredness per second.")]
        public float workRate = 2f;

        [SerializeField] private AreaMover areaMover;

        private GeneralAgentStats stats;
        private float _tiredAcc;
        private bool _started;

        public bool IsDone { get; private set; }
        public event Action OnCompleted;

        public override List<Type> GetSupportedGoals() => new() { typeof(GoalWork) };
        public override float GetCost() => 1f;
        public float resourceThreshold = 10f;
        public AreaMover.Destination currentDestination;
        #endregion


        public override void OnActivated()
        {
            stats ??= GetComponent<GeneralAgentStats>();
            areaMover ??= GetComponent<AreaMover>();
            if (areaMover == null)
            {
                Complete();
                return;
            }

            IsDone = false;
            _tiredAcc = 0f;
            _started = false;

            // If we have enough resources, go straight to tower
            if (resourceManager.GetResourceAmount(agentType.GetAgentType()) >= resourceThreshold)
            {
                Debug.Log($"<color=green>{name}: Dropping off resources at tower</color>");
                areaMover.OnArrived += OnArrived;
                areaMover.MoveTo(AreaMover.Destination.WorkingArea);
                currentDestination = AreaMover.Destination.WorkingArea;
            }
            else
            {
                // otherwise go gather resources
                Debug.Log($"{name}: Moving to gathering area.");
                areaMover.OnArrived += OnArrived;
                areaMover.MoveTo(AreaMover.Destination.GatheringArea);
                currentDestination = AreaMover.Destination.GatheringArea;
            }
        }

        private void OnArrived()
        {
            areaMover.OnArrived -= OnArrived;
            
            if (currentDestination == AreaMover.Destination.WorkingArea) // If we are at the tower,
            {
                // Drop off the resources and go straight back to gathering
                Debug.Log($"{name}: Moving to gathering area.");
                areaMover.OnArrived += OnArrived;
                areaMover.MoveTo(AreaMover.Destination.GatheringArea);
                currentDestination = AreaMover.Destination.GatheringArea;
            }
            else if (currentDestination == AreaMover.Destination.GatheringArea)
            {
                _started = true;
            }
            //Debug.Log($"{name}: Arrived—starting work.");
        }

        public override void OnTick(float dt)
        {
            //if (IsDone || !_started) return;

            // Tiredness increases as we work
            if (_started)
            {
                _tiredAcc += workRate * dt;
                int toAdd = Mathf.FloorToInt(_tiredAcc);
                if (toAdd > 0)
                {
                    stats.Tiredness = Mathf.Min(100, stats.Tiredness + toAdd);
                    _tiredAcc -= toAdd;
                    //Debug.Log($"{name}: Working… Tiredness = {stats.Tiredness}");
                }

                if (stats.Tiredness >= tiredThreshold)
                {
                    Complete();
                    Debug.Log($"{name}: Too tired, not gonna get work done.");
                }
                
                if (resourceManager.GetResourceAmount(agentType.GetAgentType()) >= resourceThreshold)
                {
                    areaMover.MoveTo(AreaMover.Destination.WorkingArea);
                    currentDestination = AreaMover.Destination.WorkingArea;
                    areaMover.OnArrived += OnArrived;

                    _started = false;
                }
            }

            // Resource Handling
            // if we are on a resource site, wait until we have enough resources before moving to the tower

            
        }

        private void Complete()
        {
            IsDone = true;
            OnCompleted?.Invoke();
        }

        public override void OnDeactivated()
        {
            Debug.Log($"{name}: Work action deactivated.");
        }
    }
}