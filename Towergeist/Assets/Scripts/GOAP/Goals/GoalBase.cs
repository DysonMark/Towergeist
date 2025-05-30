using AStarPathFinding.PathFinder;
using Movement;
using UnityEngine;

namespace JW.Grid.GOAP.Goals
{
    //[RequireComponent(typeof(AI))]
    public class GoalBase : MonoBehaviour, IGoal
    {
        [Header("Goal States")]
        [HideInInspector] public bool GoalCanRun;
        [HideInInspector] public bool GoalCompleted;
        [HideInInspector] public bool isGoalActivated;
        protected AreaMover moveSystem;

        public virtual void Awake()
        {
            //Agent = GetComponent<AI>();
        }

        public virtual int CalculatePriority()
        {
            return -1;
        }
        public virtual bool CanRun()
        {
            return false;
        }
        public virtual void OnGoalActivated()
        {
        }
        public virtual void OnGoalDeactivated()
        {
        }
        public virtual void OnGoalTick()
        {
        }

        public virtual void SetPriority(int priority)
        {
        }
    }
}