using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JW.Grid.GOAP.Goals;
using Resources;

public class GoalCollect : GoalBase
{
    [SerializeField] private float collectPriority = 1f;
    private float currentPriority = 1f;
    [SerializeField] private float priorityBuildRate = 1f;
    [SerializeField] private float priorityDecayRate = .5f;
    [SerializeField] private ResourceManager _resourceManager;
    [SerializeField] private float lowStoneThreshold = 5f;
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
        float stone = _resourceManager.GetResourceAmount(ResourceManager.ResourceType.Stone);
        
        if (stone <= lowStoneThreshold)
        {
            // Raise priority over time while low on stone
            currentPriority += priorityBuildRate * Time.fixedDeltaTime;
        }
        else
        {
            // Decay priority when stone is enough
            currentPriority -= priorityDecayRate * Time.fixedDeltaTime;
        }

        // Clamp to avoid negatives or overflow
        currentPriority = Mathf.Clamp(currentPriority, 0f, 100f);
    }
    
    public override bool CanRun()
    {
        GoalCanRun = true;
        return GoalCanRun;
    }

}
