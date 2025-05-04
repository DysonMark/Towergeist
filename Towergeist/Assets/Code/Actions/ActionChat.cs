using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using JW.Grid.GOAP.Actions;
using Stat;
using Movement;
using Actions.Chat;
using Actions.CompletionAnnouncement;
using JW.Grid.GOAP.Goals;

namespace Actions.Chat
{
    /// <summary>
    /// Friendly agents go to the chat area only if others are already there ++ Sprite chat pops up.
    /// </summary>
    public class ActionChat : ActionBase, ICompletableAction
    {
        #region Variables
        public float chatDuration = 5f;
        private GeneralAgentStats _stats;
        private ChatRoomSensor _chatSensor;
        private AreaMover _mover;
        private ChatInteraction _chatView;
        private float _timer;
        private bool _hasArrived;
        #endregion

        #region Public Functions
        public bool IsDone { get; private set; }
        public event Action OnCompleted;

        public override List<Type> GetSupportedGoals() => new() { typeof(GoalChat) };
        public override float GetCost() => 1f;

        public override void OnActivated()
        {
            _stats ??= GetComponent<GeneralAgentStats>();
            _chatSensor ??= FindObjectOfType<ChatRoomSensor>();
            _mover ??= GetComponent<AreaMover>();
            _chatView ??= GetComponent<ChatInteraction>();

            if (!_stats.IsFriendly || !_stats.IsBored || _mover == null || _chatView == null)
            {
                Complete();
                return;
            }

            _hasArrived = false;
            _mover.OnArrived += OnArrived;
            _mover.MoveTo(AreaMover.Destination.ChattingArea);
        }

        public override void OnTick(float deltaTime)
        {
            if (!_hasArrived) return;

            _timer += deltaTime;
            _stats.BoredomLevel = Mathf.Max(0f, _stats.BoredomLevel - _chatSensor.boredomDrainRate * deltaTime);
            _chatView.ShowChat();

            if (_timer >= chatDuration || !_stats.IsBored)
                Complete();
        }

        public override void OnDeactivated()
        {
            _mover.OnArrived -= OnArrived;
        }
        #endregion

        #region Private Functions
        private void OnArrived()
        {
            _mover.OnArrived -= OnArrived;
            _hasArrived = true;
            _timer = 0f;

            foreach (var other in _chatSensor._inside.Where(s => s.IsFriendly))
            {
                other.GetComponent<ChatInteraction>()?.ShowChat();
            }
        }

        private void Complete()
        {
            _mover.OnArrived -= OnArrived;
            IsDone = true;
            OnCompleted?.Invoke();
        }
        #endregion
    }
}