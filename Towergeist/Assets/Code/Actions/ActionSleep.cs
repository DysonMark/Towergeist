using System;
using UnityEngine;
using Stat;
using JW.Grid.GOAP.Actions;
using Agents.Goals;
using Movement;
using Actions.CompletionAnnouncement;
using System.Collections.Generic;

namespace Actions.Sleep
{
    public class ActionSleep : ActionBase, ICompletableAction
    {
        #region Variables
        [SerializeField] private AreaMover areaMover;
        private GeneralAgentStats stats;

        public bool IsDone { get; private set; }
        public event Action OnCompleted;

        public override List<Type> GetSupportedGoals() => new() { typeof(GoalRest) };
        public override float GetCost() => 2f;
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
            areaMover.OnArrived += Arrived;
            areaMover.MoveTo(AreaMover.Destination.RestingArea);
        }

        private void Arrived()
        {
            areaMover.OnArrived -= Arrived;
            stats.Tiredness = 0f;

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