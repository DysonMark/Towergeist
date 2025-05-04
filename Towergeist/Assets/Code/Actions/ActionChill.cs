using System;
using System.Collections.Generic;
using UnityEngine;
using JW.Grid.GOAP.Actions;
using Stat;
using Movement;
using Actions.CompletionAnnouncement;
using JW.Grid.Sensors;
using Agents.Goals;

namespace Actions.Chill
{
    public class ActionChill : ActionBase, ICompletableAction
    {
        #region Variables
        [SerializeField] private AreaMover areaMover;

        private GeneralAgentStats _stats;
        private BaseSensor _sensor;

        public bool IsDone { get; private set; }
        public event Action OnCompleted;

        public override List<Type> GetSupportedGoals() => new() { typeof(GoalChill) };
        public override float GetCost() => 1f;
        #endregion

        public override void OnActivated()
        {
            _stats ??= GetComponent<GeneralAgentStats>();
            _sensor ??= GetComponent<BaseSensor>();
            areaMover ??= GetComponent<AreaMover>();

            if (!_stats.IsFriendly || !_stats.IsBored || areaMover == null)
            {
                Complete();
                return;
            }

            // we know sensor.IsTriggered implied friendly sight
            IsDone = false;
            areaMover.OnArrived += Arrived;
            areaMover.MoveTo(AreaMover.Destination.ChattingArea);
        }

        #region Private Functions
        private void Arrived()
        {
            areaMover.OnArrived -= Arrived;
            // once we reach chatroom, mark chill done so GOAP will pick GoalChat next
            Complete();
        }

        private void Complete()
        {
            IsDone = true;
            OnCompleted?.Invoke();
        }
        #endregion

        public override void OnTick(float dt) { }
        public override void OnDeactivated() { }
    }
}