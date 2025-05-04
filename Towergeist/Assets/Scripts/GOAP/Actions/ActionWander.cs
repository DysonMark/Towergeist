using System;
using System.Collections.Generic;
using JW.Grid.GOAP.Goals;
using UnityEngine;
using Random = UnityEngine.Random;

namespace JW.Grid.GOAP.Actions
{
    public class ActionWander : ActionBase
    {
        [Header("Wander Action")]
        [SerializeField] private int wanderRadius = 5;
        private List<Type> supportedGoals = new List<Type>() { typeof(GoalWander) };

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, wanderRadius);
        }

        public override List<Type> GetSupportedGoals()
        {
            return supportedGoals;
        }

        public override void OnActivated()
        {
            // Start picking a random location
            /*PickLocation:
            Vector2 wanderPositionVector2 = Random.insideUnitCircle * wanderRadius;
            Vector3 wanderPosition = new  Vector3(wanderPositionVector2.x, transform.position.y, wanderPositionVector2.y);
            var newWanderNode = moveSystem.gridScript.GetNode(wanderPosition);
            
            if (newWanderNode != null)
            {
                moveSystem.endPoint.transform.position = wanderPosition;
            }
            else
            {
                goto PickLocation;
            }*/
        }

        public override void OnTick(float dt)
        {
            /*if (Vector3.Distance(transform.position, moveSystem.endPoint.transform.position) < 0.1f) // If we are close to our goal
            {
                OnActivated(); // Then run the wander location picking again
            }*/
        }
    }
}