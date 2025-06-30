using JW.Grid.GOAP.Goals;
using Resources;
using UnityEngine;



namespace Dyson.Towergeist
{
    /// <summary>
    /// GOAP Goal for resource collection behavior.
    /// Dynamically prioritizes collection of the most scarce resource type.
    /// </summary>
    public class GoalCollect : GoalBase
    {
        #region Serialized Fields

        [Header("Priority Configuration")] [SerializeField]
        private float collectPriority = 1f;

        [SerializeField] private float priorityBuildRate = 1f;
        [SerializeField] private float priorityDecayRate = 0.5f;
        [SerializeField] private float priorityMultiplier = 10f;
        [SerializeField] private float lowThreshold = 5f;

        [Header("Resource Management")] [SerializeField]
        private ResourceManager resourceManager;

        [Header("Resource Targeting")] [SerializeField]
        private bool restrictToSpecificResource = false;

        [SerializeField] private ResourceManager.ResourceType specificResource;

        #endregion

        #region Private Fields

        private float currentPriority = 1f;

        #endregion

        #region Public Properties

        public ResourceManager.ResourceType CurrentTargetResource { get; private set; }

        #endregion

        #region Goal Lifecycle

        public override void OnGoalActivated()
        {
            currentPriority = collectPriority;
        }

        public override void OnGoalTick()
        {
            DetermineTargetResource();
        }

        public override bool CanRun()
        {
            GoalCanRun = resourceManager != null;
            return GoalCanRun;
        }

        #endregion

        #region Priority Calculation

        public override int CalculatePriority()
        {
            ResourceManager.ResourceType targetResource = GetTargetResourceType();
            float currentAmount = resourceManager.GetResourceAmount(targetResource);

            if (currentAmount >= lowThreshold)
            {
                return 0;
            }

            float scarcityFactor = lowThreshold - currentAmount;
            return Mathf.FloorToInt(scarcityFactor * priorityMultiplier);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Determines which resource type should be targeted for collection.
        /// Prioritizes the resource with the lowest current amount.
        /// </summary>
        private void DetermineTargetResource()
        {
            if (restrictToSpecificResource)
            {
                CurrentTargetResource = specificResource;
                return;
            }

            var resourceAmounts = GetAllResourceAmounts();
            CurrentTargetResource = FindLowestResource(resourceAmounts);
        }

        /// <summary>
        /// Gets the current amounts of all resource types.
        /// </summary>
        private ResourceAmounts GetAllResourceAmounts()
        {
            return new ResourceAmounts
            {
                Stone = resourceManager.GetResourceAmount(ResourceManager.ResourceType.Stone),
                Wood = resourceManager.GetResourceAmount(ResourceManager.ResourceType.Wood),
                Cement = resourceManager.GetResourceAmount(ResourceManager.ResourceType.Cement)
            };
        }

        /// <summary>
        /// Finds the resource type with the lowest current amount.
        /// </summary>
        private ResourceManager.ResourceType FindLowestResource(ResourceAmounts amounts)
        {
            if (amounts.Stone <= amounts.Wood && amounts.Stone <= amounts.Cement)
            {
                return ResourceManager.ResourceType.Stone;
            }

            if (amounts.Wood <= amounts.Stone && amounts.Wood <= amounts.Cement)
            {
                return ResourceManager.ResourceType.Wood;
            }

            return ResourceManager.ResourceType.Cement;
        }

        /// <summary>
        /// Gets the appropriate target resource type based on current settings.
        /// </summary>
        private ResourceManager.ResourceType GetTargetResourceType()
        {
            return restrictToSpecificResource ? specificResource : CurrentTargetResource;
        }

        #endregion

        #region Helper Structures

        /// <summary>
        /// Container for resource amounts to improve code readability.
        /// </summary>
        private struct ResourceAmounts
        {
            public float Stone;
            public float Wood;
            public float Cement;
        }

        #endregion
    }
}
