using System;
using System.Linq;
using JW.Grid.GOAP.Actions;
using UnityEngine;
using Goals;
using Stat;
using Actions.CompletionAnnouncement;
using Movement;
using System.Collections.Generic;
using Actions.Chat;

namespace Actions.Yell
{
    public class ActionYell : ActionBase, ICompletableAction
    {
        #region Variables
        [SerializeField] private AreaMover areaMover;

        public bool IsDone { get; private set; }
        public event Action OnCompleted;

        public override List<Type> GetSupportedGoals() => new() { typeof(GoalDiscipline) };
        public override float GetCost() => 1f;
        private GeneralAgentStats _stats;
        #endregion

        public override void OnActivated()
        {
            _stats ??= GetComponent<GeneralAgentStats>();
            areaMover ??= GetComponent<AreaMover>();

            if (areaMover == null)
            {
                Complete(); return;
            }
            if (_stats.IsFriendly)
            {
                Debug.Log($"{name}: Friendly agents do not yell.");
                Complete(); return;
            }

            IsDone = false;
            Debug.Log($"{name}: Patrolling for bored slackers.");
            areaMover.OnArrived += OnArrivedHandler;
            areaMover.MoveTo(AreaMover.Destination.ChattingArea);
        }

        private void OnArrivedHandler()
        {
            areaMover.OnArrived -= OnArrivedHandler;

            var victims = GameObject.FindGameObjectsWithTag("Agent")
                .Where(g =>
                    g != gameObject &&
                    Vector3.Distance(g.transform.position, areaMover.chattingArea.position) < 1f)
                .Select(g => g.GetComponent<GeneralAgentStats>())
                .Where(stats => stats != null && stats.IsFriendly && stats.BoredomLevel > 0.5f)
                .ToList();

            if (victims.Any())
            {
                foreach (var victim in victims)
                {
                    Debug.Log($"{name}: Yelling at {victim.name}!");
                    victim.IsBeingYelledAt = true;
                    victim.BoredomLevel = 0f;
                    victim.IsBored = false;
                    victim.Tiredness = 0;

                    victim.GetComponent<SpritePopup>()?.ShowYell();
                }

                GetComponent<SpritePopup>()?.ShowYell();
            }
            else
            {
                Debug.Log($"{name}: No slackers to yell at.");
            }

            Complete();
        }

        private void Complete()
        {
            IsDone = true;
            OnCompleted?.Invoke();
        }

        public override void OnTick(float dt) { }

        public override void OnDeactivated()
        {
            _stats.IsBeingYelledAt = false;
        }
    }
}
