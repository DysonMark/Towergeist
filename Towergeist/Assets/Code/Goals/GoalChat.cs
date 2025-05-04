using Dyson.Towergeist;
using Stat;
using UnityEngine;

namespace JW.Grid.GOAP.Goals
{
    public class GoalChat : GoalBase
    {
        [SerializeField] private int basePriority = 50;
        private GeneralAgentStats stats;
        private bool isBeingYelledAt;

        public override void OnGoalActivated()
        {
        }

        public override void OnGoalDeactivated()
        {
        }

        public override void OnGoalTick()
        {
        }

        public override int CalculatePriority()
        {
            if (stats == null) stats = GetComponent<GeneralAgentStats>();
            if (!stats.IsFriendly || stats.IsBeingYelledAt) return -1;

            if (stats.BoredomLevel > 0f)
                return 100;

            return -1;
        }

        public override bool CanRun()
        {
            if (stats == null) stats = GetComponent<GeneralAgentStats>();

            return stats.IsBored && stats.IsFriendly;
            //return CalculatePriority() > 0;
        }
    }
}