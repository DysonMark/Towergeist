using System;
using System.Linq;
using System.Collections.Generic;
using JW.Grid.GOAP.Actions;
using UnityEngine;
using Agents.Goalss;
using Stat;
using Actions.CompletionAnnouncement;
using Movement;

namespace Actions.Chat
{
    /// <summary>
    /// Friendly agents go to chat area only if others are already there.
    /// </summary>
    public class ActionChat : ActionBase, ICompletableAction
    {
        [Tooltip("How much tiredness chatting recovers.")]
        public int chatRestAmount = 20;

        private GeneralAgentStats stats;

        [SerializeField] private AreaMover areaMover;

        public bool IsDone { get; private set; }
        public event Action OnCompleted;

        public override List<Type> GetSupportedGoals() => new() { typeof(GoalRest) };
        public override float GetCost() => 1f;

        public override void OnActivated()
        {
            stats ??= GetComponent<GeneralAgentStats>();
            if (!stats.IsFriendly)
            {
                Debug.Log($"{name} ::Unfriendly don't chitchat.");
                Complete();
                return;
            }

            areaMover ??= GetComponent<AreaMover>();
            if (areaMover == null)
            {
                Complete();
                return;
            }

            bool someoneThere = GameObject.FindGameObjectsWithTag("Agent").Any(g => g != gameObject && Vector3.Distance(g.transform.position, areaMover.chattingArea.position) < 1f);
                
            if (!someoneThere)
            {
                Debug.Log($"{name}: No one to chat with—cancelinv.");
                Complete();
                return;
            }

            IsDone = false;
            Debug.Log($"{name}: Heading to chat area.");
            areaMover.OnArrived += Arrived;
            areaMover.MoveTo(AreaMover.Destination.ChattingArea);
        }

        private void Arrived()
        {
            areaMover.OnArrived -= Arrived;
            Debug.Log($"{name}: Chatting…");
            stats.Tiredness = Mathf.Max(0, stats.Tiredness - chatRestAmount);
            stats.IsBored = false;
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