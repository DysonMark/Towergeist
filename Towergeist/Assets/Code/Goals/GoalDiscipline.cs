using UnityEngine;
using Stat;
using JW.Grid.GOAP.Goals;
using JW.Grid.Sensors;

namespace Goals
{
    [RequireComponent(typeof(BaseSensor))]
    [RequireComponent(typeof(GeneralAgentStats))]
    public class GoalDiscipline : GoalBase
    {
        private BaseSensor _sensor;
        private GeneralAgentStats _stats;
        private float lastYellTime;
        public float yellCooldown = 5f;

        public override int CalculatePriority()
        {
            _sensor ??= GetComponent<BaseSensor>();
            _stats ??= GetComponent<GeneralAgentStats>();

            if (_stats == null || _stats.IsFriendly || _sensor == null || !_sensor.IsTriggered)
                return -1;

            if (Time.time - lastYellTime < yellCooldown)
                return -1;

            var target = _sensor.TriggeredObject;
            if (target == null) return -1;

            var targetStats = target.GetComponent<GeneralAgentStats>();
            if (targetStats == null) return -1;

            bool isDistracted = targetStats.IsFriendly && targetStats.BoredomLevel > 0.5f;

            if (isDistracted)
            {
                lastYellTime = Time.time;
                return 150;
            }

            return -1;
        }

        public override bool CanRun()
        {
            return CalculatePriority() > 0;
        }
    }
}