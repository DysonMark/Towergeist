using System;
using System.Linq;
using System.Collections.Generic;
using JW.Grid.GOAP.Actions;
using UnityEngine;
using JW.Grid.GOAP.Goals;
using Stat;
using Actions.CompletionAnnouncement;
using Movement;

namespace Actions.Chat
{
    /// <summary>
    /// Friendly agents go to the chat area only if others are already there ++ Sprite chat pops up.
    /// </summary>
    public class ActionChat : ActionBase, ICompletableAction
    {
        #region Variables

        private GeneralAgentStats stats;
        [SerializeField] private AreaMover areaMover;

        public bool IsDone { get; private set; }
        public event Action OnCompleted;

        public override List<Type> GetSupportedGoals() => new() { typeof(GoalChat) };
        public override float GetCost() => 1f;

        private bool hasArrived = false;
        #endregion

        public override void OnActivated()
        {
            stats ??= GetComponent<GeneralAgentStats>();
            areaMover ??= GetComponent<AreaMover>();
            if (areaMover == null)
            {
                Complete(); return;
            }

            if (!stats.IsFriendly)
            {
                Debug.Log($"{name}: Unfriendly Agents don't chitchat.");
                Complete(); return;
            }

            bool someoneThere = GameObject.FindGameObjectsWithTag("Agent")
                .Any(g => g != gameObject && Vector3.Distance(g.transform.position, areaMover.chattingArea.position) < 1f);

            IsDone = false;
            hasArrived = false;
            Debug.Log($"{name}: Heading to chat area.");
            areaMover.OnArrived += Arrived;
            areaMover.MoveTo(AreaMover.Destination.ChattingArea);
        }

        private void Arrived()
        {
            areaMover.OnArrived -= Arrived;
            Debug.Log($"{name}: Chatting…");

            stats.IsBored = false;
            hasArrived = true;

            GetComponent<SpritePopup>()?.ShowChat();
            foreach (var other in GameObject.FindGameObjectsWithTag("Agent")
                         .Where(g =>
                             g != gameObject &&
                             Vector3.Distance(g.transform.position, areaMover.chattingArea.position) < 1f))
            {
                other.GetComponent<SpritePopup>()?.ShowChat();
            }

            Complete();
        }

        private void Complete()
        {
            IsDone = true;
            OnCompleted?.Invoke();
        }

        public override void OnTick(float dt)
        {
            if (hasArrived)
                stats.DecreaseBoredom(dt);
        }

        public override void OnDeactivated() { }
    }
}
