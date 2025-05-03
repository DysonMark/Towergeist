using System;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Actions.CompletionAnnouncement;
using JW.Grid.GOAP.Goals;
using JW.Grid.GOAP.Actions;
using Movement;  

namespace Actions.GoToChatRoom
{
    /// <summary>Moves a friendly AI into the chat room, then triggers the chat animation.</summary>
    public class ActionGoToChatRoom : ActionBase, ICompletableAction
    {
        #region Variables
        [SerializeField] private AreaMover areaMover;      
        public bool IsDone { get; private set; }
        public event Action OnCompleted;

        public override List<Type> GetSupportedGoals() => new() { typeof(GoalChat) };
        public override float GetCost() => 1f;
        #endregion

        public override void OnActivated()
        {
            areaMover ??= GetComponent<AreaMover>();
            if (areaMover == null)
            {
                Complete();
                return;
            }

            IsDone = false;
            Debug.Log($"{name}: Heading to chat room.");
            areaMover.OnArrived += Arrived;
            areaMover.MoveTo(AreaMover.Destination.ChattingArea);
        }

        private void Arrived()
        {
            areaMover.OnArrived -= Arrived;
            GetComponent<Animator>()?.SetTrigger("Chat");
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
            var stats = GetComponent<GeneralAgentStats>();
            if (stats != null)
            {
                stats.IsBored = false;
                if (stats.Tiredness <= 20) stats.CanSleep = true;
                else stats.Rested += 10;
            }
        }
    }
}