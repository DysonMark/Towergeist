using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JW.Grid.GOAP.Goals;
public class GoalCollect : GoalBase
{
    [SerializeField] private float collectPriority = 1f;
    private float currentPriority = 1f;
    [SerializeField] private float priorityBuildRate = 1f;
    [SerializeField] private float priorityDecayRate = .5f;
    public override void OnGoalActivated()
    {   
        currentPriority = collectPriority;
        //isGoalActivated = true;
    }
    public override int CalculatePriority()
    {
        return Mathf.FloorToInt(currentPriority);
    }

    public override void OnGoalTick()
    {
        
    }
    
    public override bool CanRun()
    {
        GoalCanRun = true;
        return GoalCanRun;
    }

}
