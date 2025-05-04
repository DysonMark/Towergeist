using UnityEngine;
using Stat;

namespace JW.Grid.GOAP.Goals
{
    public class GoalChat : GoalBase
    {
        #region Variables
        [SerializeField] private int basePriority = 150;

        private GeneralAgentStats _stats;
        #endregion

        public override int CalculatePriority()
        {
            _stats ??= GetComponent<GeneralAgentStats>();

            if (!_stats.IsFriendly || _stats.IsBeingYelledAt)
                return -1;

            return basePriority;
        }

        public override bool CanRun() => CalculatePriority() > 0;
    }
}