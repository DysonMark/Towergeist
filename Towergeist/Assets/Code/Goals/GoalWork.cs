using UnityEngine;
using JW.Grid.GOAP.Goals;
using Stat;

namespace Agents.Goals
{
    public class GoalWork : GoalBase
    {
        public int tiredThreshold = 50;
        private GeneralAgentStats stats;

        public override int CalculatePriority()
        {
            if (stats == null) stats = GetComponent<GeneralAgentStats>();

            if (stats.Tiredness >= tiredThreshold) return -1;

            return 100 - (int)stats.Tiredness;
        }

        public override bool CanRun()
        {
            if (stats == null) stats = GetComponent<GeneralAgentStats>();

            return stats.Tiredness < tiredThreshold;
        }
    }
}
