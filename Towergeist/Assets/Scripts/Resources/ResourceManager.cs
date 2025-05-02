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
        [SerializeField] public float StoneAmount;
        [SerializeField] private float CementAmount;
        [SerializeField] private ResourceSite resourceSite;
        public bool IsOnResourceSite = false;
        
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

        private void Update()
        {
            // Resource Site harvesting
            if (IsOnResourceSite)
            {
                AddResource(resourceSite.resourceType, resourceSite.resourceAmountPerSecond * Time.deltaTime);
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("ResourceSite"))
            {
                IsOnResourceSite = true;
                resourceSite = other.GetComponent<ResourceSite>();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("ResourceSite"))
            {
                IsOnResourceSite = false;
                resourceSite = null;
            }
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

        public void TransferResource(ResourceType resource, float amount, ref ResourceManager transferTo)
        {
            if (resource == ResourceType.Wood)
            {
                if (WoodAmount >= amount) // If we have more than requested amound
                {
                    transferTo.AddWood(amount);
                    AddResource(resource, -amount);
                }
                else // If we don't have enough
                {
                    transferTo.AddResource(resource, WoodAmount); // Give all the wood we have
                    AddResource(resource, -WoodAmount); // Remove all our wood we had
                }
            }
            else if (resource == ResourceType.Stone)
            {
                if (StoneAmount >= amount) // If we have more than requested amound
                {
                    // Give the resource and subtract it from our amount
                    transferTo.AddResource(resource, amount);
                    AddResource(resource, -amount);
                }
                else // If we don't have enough
                {
                    transferTo.AddResource(resource, StoneAmount); // Give all the wood we have
                    AddResource(resource, -StoneAmount); // Remove all our wood we had
                }
            }
            else if (resource == ResourceType.Cement)
            {
                if (CementAmount >= amount) // If we have more than requested amound
                {
                    // Give the resource and subtract it from our amount
                    transferTo.AddResource(resource, amount); 
                    AddResource(resource, -amount);
                }
                else // If we don't have enough
                {
                    // Give the resource and subtract it from our amount
                    transferTo.AddResource(resource, CementAmount); // Give all the wood we have
                    AddResource(resource, -CementAmount); // Remove all our wood we had
                }
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