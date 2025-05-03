using JW.Grid.GOAP.Goals;
using Stat;

namespace Agents.Goalss
{
    public class GoalRest : GoalBase
    {
        #region Variables
        public int tiredThreshold = 50;
        private GeneralAgentStats stats;
        #endregion

        public override int CalculatePriority()
        {
            if (stats == null)
                stats = GetComponent<GeneralAgentStats>();

            if (stats.Tiredness < tiredThreshold)
                return -1;

            return (int)stats.Tiredness;
        }

        public override bool CanRun()
        {
            if (stats == null) stats = GetComponent<GeneralAgentStats>();

            return stats.Tiredness >= tiredThreshold;
        }
    }
}