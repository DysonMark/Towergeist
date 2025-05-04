using System;
using System.Linq;
using JW.Grid.GOAP.Actions;
using UnityEngine;
using Agents.Goalss;
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

        public override List<Type> GetSupportedGoals() => new() { typeof(GoalRest) };
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
            Debug.Log($"{name}: Heading to chat area to find a victim.");
            areaMover.OnArrived += OnArrivedHandler;
            areaMover.MoveTo(AreaMover.Destination.ChattingArea);
        }

        private void OnArrivedHandler()
        {
            areaMover.OnArrived -= OnArrivedHandler;

            var victim = GameObject.FindGameObjectsWithTag("Agent")
                .FirstOrDefault(g =>
                    g != gameObject &&
                    Vector3.Distance(g.transform.position, areaMover.chattingArea.position) < 1f &&
                    g.GetComponent<GeneralAgentStats>()?.IsFriendly == true
                );

            if (victim != null)
            {
                Debug.Log($"{name}: Yelling at {victim.name}!");
                victim.GetComponent<GeneralAgentStats>().IsBeingYelledAt = true;

                GetComponent<SpritePopup>()?.ShowYell();
                //victim.GetComponent<SpritePopup>()?.ShowYell();
            }
            else
            {
                Debug.Log($"{name}: No lazy agents here.");
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