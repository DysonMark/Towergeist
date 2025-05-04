using System;
using UnityEngine;
using JW.Grid.GOAP.Goals;
using Stat;
using JW.Grid.Sensors;
using Movement;

namespace Agents.Goals
{
    public class GoalChill : GoalBase
    {
        #region Variables
        [SerializeField] private int spotFriendPriority = 100;
        [SerializeField] private int alonePriority = 20;
        [SerializeField] private int unfriendlyPenalty = 50;
        [SerializeField] private int tiredThreshold = 50;

        private GeneralAgentStats _stats;
        private BaseSensor _sensor;
        private AreaMover _mover;
        private ChatRoomSensor _roomSensor;
        #endregion

        #region Public Methods
        public override int CalculatePriority()
        {
            _stats ??= GetComponent<GeneralAgentStats>();
            _sensor ??= GetComponent<BaseSensor>();
            _mover ??= GetComponent<AreaMover>();
            _roomSensor ??= FindObjectOfType<ChatRoomSensor>();

            if (!_stats.IsFriendly || !_stats.IsBored || _stats.Tiredness > tiredThreshold)
                return -1;

            if (_sensor.IsTriggered
             && _sensor.TriggeredObject.TryGetComponent<GeneralAgentStats>(out var other)
             && other.IsFriendly && other.IsBored)
            {
                return spotFriendPriority;
            }

            int unfriendlyHere = _roomSensor.UnfriendlyCount;
            if (unfriendlyHere > 0)
                return Mathf.Max(0, alonePriority - unfriendlyPenalty);

            bool roomEmpty = (_roomSensor.FriendlyCount == 0 && unfriendlyHere == 0);
            return roomEmpty ? alonePriority : -1;
        }

        public override bool CanRun() => CalculatePriority() > 0;
        #endregion
    }
}