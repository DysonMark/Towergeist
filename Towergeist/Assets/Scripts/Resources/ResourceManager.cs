using System;
using TMPro;
using UnityEngine;

namespace Resources
{
    /// <summary>
    /// A component for holding 3 resources and allowing for interaction with them
    /// </summary>
    public class ResourceManager : MonoBehaviour
    {
        [Header("Resources")] 
        [SerializeField] private float WoodAmount;
        [SerializeField] private float StoneAmount;
        [SerializeField] private float CementAmount;
        
        [Header("UI")]
        [SerializeField] private TMP_Text woodAmountText;
        [SerializeField] private TMP_Text stoneAmountText;
        [SerializeField] private TMP_Text cementAmountText;

        public enum ResourceType
        {
            Wood,
            Stone,
            Cement
        }

        private void Start()
        {
            // Add 0 to all resources to update their text fields
            AddResource(ResourceType.Wood, 0f);
            AddResource(ResourceType.Stone, 0f);
            AddResource(ResourceType.Cement, 0f);
        }

        /// <summary>
        /// Returns the amount of the resource type
        /// </summary>
        /// <param name="resource">Wood, Stone, Cement</param>
        /// <returns>-1 if error</returns>
        public float GetResourceAmount(ResourceType resource)
        {
            if (resource == ResourceType.Wood) return WoodAmount;
            else if (resource == ResourceType.Stone) return StoneAmount;
            else if (resource == ResourceType.Cement) return CementAmount;
            else return -1f;
        }

        /// <summary>
        /// Adds the given amount to the given resource
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="amount"></param>
        public void AddResource(ResourceType resource, float amount)
        {
            if (resource == ResourceType.Wood)
            {
                WoodAmount += amount; 
                woodAmountText.text = $"Wood: {WoodAmount}";
            }
            else if (resource == ResourceType.Stone)
            {
                StoneAmount += amount; 
                stoneAmountText.text = $"Stone: {StoneAmount}";
            }
            else if (resource == ResourceType.Cement)
            {
                CementAmount += amount; 
                cementAmountText.text = $"Cement: {CementAmount}";
            }
        }

        #region Debug
        public void AddWood(float amount)
        {
            AddResource(ResourceType.Wood, amount);
        }
        public void AddStone(float amount)
        {
            AddResource(ResourceType.Stone, amount);
        }
        public void AddCement(float amount)
        {
            AddResource(ResourceType.Cement, amount);
        }
        #endregion
    }
}