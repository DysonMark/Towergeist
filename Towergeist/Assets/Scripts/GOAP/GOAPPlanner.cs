    using System;
using System.Collections.Generic;
using System.Linq;
using JW.Grid.GOAP.Actions;
using JW.Grid.GOAP.Goals;
using UnityEngine;

namespace JW.Grid.GOAP
{
    public class GOAPPlanner : MonoBehaviour
    {
#if ASTAR_DEBUG
        public List<GoalBase> GoalsHistory = new List<GoalBase>();
#endif

        public GoalBase currentGoal;
        public ActionBase currentAction;

        public ActionBase[] actions;
        public GoalBase[] goals;

        private void Awake()
        {
            goals = GetComponents<GoalBase>();
            actions = GetComponents<ActionBase>();
        }

        private void FixedUpdate()
        {

            // Get the best goal and its actions to achieve it
            GoalBase bestGoal = null;
            ActionBase bestAction = null;

            // Updates all this AI's goals he has access to
            foreach (GoalBase goal in goals)
            {
                goal.OnGoalTick();

                // Skip goals that can't run
                if (!goal.CanRun())
                {
                    continue;
                }

                // If this goal is worse than the current best, then skip it
                if (!(bestGoal == null || goal.CalculatePriority() > bestGoal.CalculatePriority()))
                {
                    continue;
                }

                if (bestGoal == null)
                {
                    bestGoal = goal;
                } // TODO: Check this

                // Go through all our actions
                ActionBase candidateAction = null;
                foreach (ActionBase action in actions)
                {
                    if (!action.GetSupportedGoals().Contains(goal.GetType()))
                    {
                        continue;
                    }

                    // Check if the candidate action is better than the current one
                    if (candidateAction == null || action.GetCost() < candidateAction.GetCost())
                    {
                        candidateAction = action;
                    }
                }

                // Update the best goal and action
                if (candidateAction != null)
                {
                    bestGoal = goal;
                    bestAction = candidateAction;
                }
            }

            // If we don't have a goal and we found one
            if (currentGoal == null)
            {
                // Set our goal and action
                currentGoal = bestGoal;
                currentAction = bestAction;

#if ASTAR_DEBUG
                    GoalsHistory.Add(currentGoal);
#endif

                // Activate them if they are not null
                if (currentGoal != null)
                {
                    currentGoal.OnGoalActivated();
                }
                if (currentAction != null)
                {
                    currentAction.OnActivated();
                }
            }
            else if (currentGoal == bestGoal) // If the goal did not change
            {
                if (currentAction != bestAction) // If the action changed
                {
                    currentAction.OnDeactivated(); // Deactivate

                    currentAction = bestAction; // Set the new action

                    if (currentAction != null) currentAction.OnActivated(); // And activate if there is an action
                }
            }
            else if (currentGoal != bestGoal) // If the goal did change
            {
                // Deactivate our current goal and action
                currentGoal.OnGoalDeactivated();
                currentAction.OnDeactivated();

                // Set the new ones
                currentGoal = bestGoal;
                currentAction = bestAction;

#if ASTAR_DEBUG
                    GoalsHistory.Add(currentGoal);
#endif

                // Activate them if we have them
                if (currentGoal != null)
                {
                    currentGoal.OnGoalActivated();
                }
                if (currentAction != null)
                {
                    currentAction.OnActivated();
                }
            }


            // Update the current action if we have one
            if (currentAction != null)
            {
                currentAction.OnTick(Time.fixedDeltaTime);
            }
        }
    }
}