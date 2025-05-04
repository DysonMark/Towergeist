using UnityEngine;
using JW.Grid.GOAP.Goals;
using Stat;

namespace Agents.Goals
{
    public class GoalRest : GoalBase
    {
        #region Variables
        [SerializeField] private int restingBasePriority = 80;
        [SerializeField] private int tiredBoost = 50;
        [SerializeField] private float minTiredness = 0f;
        [SerializeField] private float highTiredThresh = 50f;

        private GeneralAgentStats _stats;
        #endregion

        public override int CalculatePriority()
        {
            _stats ??= GetComponent<GeneralAgentStats>();

            if (_stats.IsBored)
                return -1;

            if (_stats.Tiredness <= minTiredness)
                return -1;

            int p = restingBasePriority;

            if (_stats.Tiredness > highTiredThresh)
                p += tiredBoost;

            return p;
        }

        public override bool CanRun()
        {
            _stats ??= GetComponent<GeneralAgentStats>();
            return !_stats.IsBored
                && _stats.Tiredness > minTiredness;
        }
    }
}