using UnityEngine;
using JW.Grid.GOAP.Goals;
using Stat;

namespace Agents.Goals
{
    public class GoalWork : GoalBase
    {
        #region Variables
        public int tiredThreshold = 50;
        private GeneralAgentStats stats;
        #endregion

        public override int CalculatePriority()
        {
            if (stats == null) stats = GetComponent<GeneralAgentStats>();

            if (stats.IsBeingYelledAt)
            {
                return 200;
            }

            if (stats.IsFriendly && stats.IsBored)
                return -1;

            if (stats.Tiredness >= tiredThreshold)
                return -1;

            return 100 - (int)stats.Tiredness;
        }

        public override bool CanRun()
        {
            if (stats == null) stats = GetComponent<GeneralAgentStats>();

            if (stats.IsFriendly && stats.IsBored)
                return false;

            return stats.Tiredness < tiredThreshold;
        }
    }
}