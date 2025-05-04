using Dyson.Towergeist;
using UnityEngine;
using JW.Grid.GOAP.Goals;
using Resources;
using Stat;
using Tower;

namespace Agents.Goals
{
    /// <summary>
    /// A goal to for the AI to gather resources
    /// Originally made by Dana, modified extensively by JW (JW version used in project)
    /// </summary>
    public class GoalWork : GoalBase
    {
        public float BasePriority = 50f;
        public float currentPriority = 30f;
        public float MaxPriority = 50f;
        public int tiredThreshold = 50;
        private GeneralAgentStats stats;
        public ResourceManager resourceManager;

        public override int CalculatePriority()
        {
            if (stats == null) stats = GetComponent<GeneralAgentStats>();

            currentPriority = BasePriority;

            // more boredom less work for friendly 
            if (stats.IsFriendly && stats.BoredomLevel > 0.2f)
                return 0; // let Chat win

            // more tired less work
            if (stats.Tiredness >= tiredThreshold) return -1;
            currentPriority = currentPriority - stats.Tiredness;

            // more resource of its type = lower priority
            currentPriority = currentPriority - resourceManager.GetResourceAmount(GetComponent<WhichAgent>().GetAgentType());

            currentPriority = Mathf.Clamp(currentPriority, 0, MaxPriority);
            return (int)currentPriority;
        }
        
        public override void OnGoalActivated()
        {   
            currentPriority = MaxPriority;
        }

        public override bool CanRun()
        {
            if (stats == null) stats = GetComponent<GeneralAgentStats>();

            // can run if:
            // not tired
            bool isTooTired = stats.Tiredness >= tiredThreshold;
            
            // is friendly && not bored
            bool isInMood = true;
            if (stats.IsFriendly)
            {
                if (stats.IsBored) isInMood = false;
            }
            
            // tower not done
            bool isTowerDone = false;
            TowerManager towerManager = FindObjectOfType<TowerManager>().GetComponent<TowerManager>();
            isTowerDone = towerManager.IsDone;
            
            return !isTooTired && isInMood && !isTowerDone;
        }
    }
}
