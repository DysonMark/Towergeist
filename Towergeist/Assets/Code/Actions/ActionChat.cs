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
    /// Friendly agents go to the chat area only if others are already there ++ Sprite chat pops up.
    /// </summary>
    public class ActionChat : ActionBase, ICompletableAction
    {
        #region Variables
        [Tooltip("How much tiredness chatting recovers.")]
        public int chatRestAmount = 20;

        private GeneralAgentStats stats;
        [SerializeField] private AreaMover areaMover;

        public bool IsDone { get; private set; }
        public event Action OnCompleted;

        public override List<Type> GetSupportedGoals() => new() { typeof(GoalRest) };
        public override float GetCost() => 1f;
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
                .Any(g =>
                    g != gameObject &&
                    Vector3.Distance(g.transform.position, areaMover.chattingArea.position) < 1f
                );

            if (!someoneThere)
            {
                Debug.Log($"{name}: No one to chat with; cancelling.");
                Complete(); return;
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

            GetComponent<Animator>()?.SetTrigger("Chat");
            stats.Tiredness = Mathf.Max(0, stats.Tiredness - chatRestAmount);
            stats.IsBored = false;

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

        public override void OnTick(float dt) { }
        public override void OnDeactivated() { }
    }
}