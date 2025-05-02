using System;
using System.Collections;
using System.Collections.Generic;
using AStarPathFinding.PathFinder;
using UnityEngine;
using JW.Grid.GOAP.Actions;
using Resources;
using JW.Grid.GOAP.Goals;

namespace Dyson.Towergeist
{
    public class ActionCollectResources : ActionBase
    {
        [Header("Collect Action")] private List<Type> supportedGoals = new List<Type>() { typeof(GoalCollect) };
        [SerializeField] private ResourceManager _resourcesStone;
        [SerializeField] private ResourceManager _resourcesWood;
        [SerializeField] private ResourceManager _resourcesCement;
        [SerializeField] private Transform stoneEndPoint;
        [SerializeField] private Transform woodEndPoint;
        [SerializeField] private Transform cementEndPoint;
        [SerializeField] private Transform towerPoint;
        [SerializeField] private CapsuleMover _capsuleMover;
        [SerializeField] private GoalCollect _goalCollect;
        [SerializeField] private WhichAgent _stoneAgent;
        [SerializeField] private WhichAgent _woodAgent;
        [SerializeField] private WhichAgent _cementAgent;
        public override List<Type> GetSupportedGoals()
        {
            return supportedGoals;
        }
        public override void OnActivated()
        {
            var target = _goalCollect.CurrentTargetResource;
            
            Debug.Log("target: " + target);

            Transform targetPoint = null;

            switch (target)
            {
                case ResourceManager.ResourceType.Wood:
                    if (_woodAgent.isWoodAgent)
                    {
                        targetPoint = woodEndPoint;
                    }
                    break;
                case ResourceManager.ResourceType.Stone:
                    if (_stoneAgent.isStoneAgent)
                    {
                        targetPoint = stoneEndPoint;
                    }
                    break;
                case ResourceManager.ResourceType.Cement:
                    if (_cementAgent.isCementAgent)
                    {
                        targetPoint = cementEndPoint;
                    }
                    break;
            }

            if (targetPoint != null)
            {
                moveSystem.endPoint = targetPoint;
            }
       /*     if (_resourcesStone.GetResourceAmount(ResourceManager.ResourceType.Stone) <= 0)
            {
                moveSystem.endPoint = stoneEndPoint;
            }
            if (_resourcesCement.GetResourceAmount(ResourceManager.ResourceType.Cement) <= 0)
            {
                moveSystem.endPoint = cementEndPoint;
            }
            if (_resourcesWood.GetResourceAmount(ResourceManager.ResourceType.Wood) <= 0)
            {
                moveSystem.endPoint = woodEndPoint;
            } */
            Debug.Log("Activation of collecting resources");
        }

        public override void OnTick(float dt)
        {
            if (_resourcesStone.GetResourceAmount(ResourceManager.ResourceType.Stone) >= 15)
            {
                _resourcesStone.StoneAmount = 15;
                moveSystem.endPoint = towerPoint;
            }
            if (_resourcesWood.GetResourceAmount(ResourceManager.ResourceType.Wood) >= 35)
            {
                _resourcesWood.WoodAmount = 35;
                moveSystemTwo.endPoint = towerPoint;
            }
            if (_resourcesCement.GetResourceAmount(ResourceManager.ResourceType.Cement) >= 20)
            {
                _resourcesCement.CementAmount = 20;
                moveSystemThree.endPoint = towerPoint;
            }
           // OnActivated();
           //Debug.Log("Activation of collecting resources");
        }
    }
}
