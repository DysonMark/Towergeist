using System;
using System.Collections;
using System.Collections.Generic;
using Agents.Goals;
using AStarPathFinding.PathFinder;
using UnityEngine;
using JW.Grid.GOAP.Actions;
using Resources;
using JW.Grid.GOAP.Goals;
using Movement;


namespace Dyson.Towergeist
{
    /// <summary>
    /// GOAP Action for resource collection behavior.
    /// Coordinates agent movement between gathering and working areas based on resource thresholds.
    /// </summary>
    public class ActionCollectResources : ActionBase
    {
        #region Serialized Fields
        [Header("Resource Configuration")]
        [SerializeField] private float resourceTargetAmount = 10f;
        #endregion

        #region Private Fields
        private readonly List<Type> supportedGoals = new List<Type> { typeof(GoalWork) };
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the current resource target amount for this action.
        /// </summary>
        public float ResourceTargetAmount => resourceTargetAmount;
        #endregion

        #region GOAP Action Interface
        /// <summary>
        /// Returns the list of goals this action can fulfill.
        /// </summary>
        public override List<Type> GetSupportedGoals()
        {
            return supportedGoals;
        }

        /// <summary>
        /// Initiates the resource collection process by moving to the gathering area.
        /// </summary>
        public override void OnActivated()
        {
            MoveToGatheringArea();
        }

        /// <summary>
        /// Updates the action state each frame, managing transitions between gathering and working.
        /// </summary>
        /// <param name="dt">Delta time since last update</param>
        public override void OnTick(float dt)
        {
            if (IsAtResourceSite())
            {
                CheckResourceThresholdAndTransition();
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Moves the agent to the designated resource gathering area.
        /// </summary>
        private void MoveToGatheringArea()
        {
            moveSystem.MoveTo(AreaMover.Destination.GatheringArea);
        }

        /// <summary>
        /// Checks if the agent is currently at a resource collection site.
        /// </summary>
        private bool IsAtResourceSite()
        {
            return resourceManager.IsOnResourceSite;
        }

        /// <summary>
        /// Evaluates current resource levels and transitions to working area if threshold is met.
        /// </summary>
        private void CheckResourceThresholdAndTransition()
        {
            float currentResources = resourceManager.GetResourceAmount(agentType.GetAgentType());
            
            if (HasSufficientResources(currentResources) && !moveSystem.IsMoving)
            {
                TransitionToWorkingArea();
            }
        }

        /// <summary>
        /// Determines if the agent has collected enough resources to proceed to work.
        /// </summary>
        private bool HasSufficientResources(float currentAmount)
        {
            return currentAmount > resourceTargetAmount;
        }

        /// <summary>
        /// Initiates movement to the working area for resource processing.
        /// </summary>
        private void TransitionToWorkingArea()
        {
            moveSystem.MoveTo(AreaMover.Destination.WorkingArea);
        }
        #endregion
    }
}

