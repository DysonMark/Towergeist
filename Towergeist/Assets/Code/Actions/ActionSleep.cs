using System;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Actions.CompletionAnnouncement;
using JW.Grid.GOAP.Actions;
using Movement;
using Agents.Goalss;   

namespace Actions.Sleep
{
    /// <summary>Teleports to the rest area, then resets tiredness.</summary>
    public class ActionSleep : ActionBase, ICompletableAction
    {
        [SerializeField] private AreaMover areaMover;        
        private GeneralAgentStats stats;

        public bool IsDone { get; private set; }
        public event Action OnCompleted;

        public override List<Type> GetSupportedGoals() => new() { typeof(GoalRest) };
        public override float GetCost() => 1f;

        public override void OnActivated()
        {
            stats ??= GetComponent<GeneralAgentStats>();

            areaMover ??= GetComponent<AreaMover>();
            if (areaMover == null)
            {
                Debug.LogError($"{name}: Missing AreaMover!");
                Complete();
                return;
            }

            IsDone = false;
            Debug.Log($"{name}: Heading to resting area.");
            areaMover.OnArrived += Arrived;
            areaMover.MoveTo(AreaMover.Destination.RestingArea);
        }

        private void Arrived()
        {
            areaMover.OnArrived -= Arrived;
           // Debug.Log($"{name}: Sleeping…");
            stats.Tiredness = 0;
           // Debug.Log($"{name}: Fully rested.");
            Complete();
        }

        private void Complete()
        {
            IsDone = true;
            OnCompleted?.Invoke();
        }

        public override void OnTick(float dt) { }
        public override void OnDeactivated() { }
    }
}
