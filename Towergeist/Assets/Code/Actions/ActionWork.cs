using System;
using UnityEngine;
using Stat;
using JW.Grid.GOAP.Actions;
using Agents.Goals;
using Movement;
using Actions.CompletionAnnouncement;
using System.Collections.Generic;

namespace Actions.Work
{
    public class ActionWork : ActionBase, ICompletableAction
    {
        #region Variables
        public int tiredThreshold = 50;
        [Tooltip("Tiredness per second.")]
        public float workRate = 10f;

        [SerializeField] private AreaMover areaMover;
        private GeneralAgentStats stats;
        private float _tiredAcc;
        private bool _started;

        
        public event Action OnCompleted;
        public bool IsDone { get; private set; }

        public override List<Type> GetSupportedGoals() => new() { typeof(GoalWork) };
        public override float GetCost() => 1f;
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

            stats.IsBeingYelledAt = false;

            IsDone = false;
            _tiredAcc = 0f;
            _started = false;
            areaMover.OnArrived += OnArrived;
            areaMover.MoveTo(AreaMover.Destination.WorkingArea);
        }

        private void OnArrived()
        {
            areaMover.OnArrived -= OnArrived;
            _started = true;
        }

        public override void OnTick(float dt)
        {
            if (IsDone || !_started) return;

            _tiredAcc += workRate * dt;
            int toAdd = Mathf.FloorToInt(_tiredAcc);
            if (toAdd > 0)
            {
                stats.Tiredness = Mathf.Min(100f, stats.Tiredness + toAdd);
                _tiredAcc -= toAdd;
            }

            if (stats.Tiredness >= tiredThreshold)
            {
                Complete();
            }
        }

        private void Complete()
        {
            IsDone = true;
            OnCompleted?.Invoke();
        }

        public override void OnDeactivated()
        {
            stats.BoredomLevel = 100f;
        }
    }
}