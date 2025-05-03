using Dyson.Towergeist;
using Stat;
using UnityEngine;

namespace JW.Grid.GOAP.Goals
{
    public class GoalChat : GoalBase
    {
        [SerializeField] private int basePriority = 20;
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
            if (!stats.IsFriendly) return -1;
            if (isBeingYelledAt) return -1;
            if (stats.IsBored || stats.Tiredness <= 50)
                return basePriority;
            return -1;
        }

        public override bool CanRun()
        {
            if (stats == null) stats = GetComponent<GeneralAgentStats>();
               
            return CalculatePriority() > 0;
        }
    }
}