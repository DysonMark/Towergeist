using System;
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
    [SerializeField] private float priorityMultiplier = 10f;
    [SerializeField] private ResourceManager _resourceManager;
    [SerializeField] private float lowThreshold = 5f;
    public ResourceManager.ResourceType CurrentTargetResource { get; private set; }
    
    public bool restrictToSpecificResource = false;
    public ResourceManager.ResourceType specificResource;
    public override void OnGoalActivated()
    {   
        currentPriority = collectPriority;
        //isGoalActivated = true;
    }
    public override int CalculatePriority()
    {
        float amount = _resourceManager.GetResourceAmount(CurrentTargetResource);
        ResourceManager.ResourceType target = restrictToSpecificResource ? specificResource : CurrentTargetResource;
        if (amount >= lowThreshold)
        {
            return 0;
        }
        return Mathf.FloorToInt((lowThreshold - amount) * priorityMultiplier);
    }

    public override void OnGoalTick()
    {
        float stone = _resourceManager.GetResourceAmount(ResourceManager.ResourceType.Stone);
        float wood = _resourceManager.GetResourceAmount(ResourceManager.ResourceType.Wood);
        float cement = _resourceManager.GetResourceAmount(ResourceManager.ResourceType.Cement);

       // float lowest = Mathf.Min(wood, stone, cement);

       if (restrictToSpecificResource)
       {
           CurrentTargetResource = specificResource;
       }
        if (stone <= wood & stone <= cement)
        {
            CurrentTargetResource = ResourceManager.ResourceType.Stone;
        }
        else if (wood <= stone && wood <= cement)
        {
            CurrentTargetResource = ResourceManager.ResourceType.Wood;
        }
        else
        {
            CurrentTargetResource = ResourceManager.ResourceType.Cement;
        }
        /*
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
        currentPriority = Mathf.Clamp(currentPriority, 0f, 100f); */
    }
    
    public override bool CanRun()
    {
        GoalCanRun = true;
        return GoalCanRun;
    }

}
