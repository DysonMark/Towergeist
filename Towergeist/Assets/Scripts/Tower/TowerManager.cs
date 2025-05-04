using System;
using System.Collections.Generic;
using Resources;
using TMPro;
using UnityEngine;

namespace Tower
{
    [RequireComponent(typeof(ResourceManager))]
    public class TowerManager : MonoBehaviour
    {
        [Header("Tower Levels")]
        int towerLevel = 0;
        [SerializeField] private GameObject[] towerLevels;
        public bool IsDone => towerLevel == towerLevels.Length;

        [Header("Resources")] 
        private ResourceManager resourceManager;
        [SerializeField] private List<float> woodNeeded;
        [SerializeField] private List<float> stoneNeeded;
        [SerializeField] private List<float> cementNeeded;
        private List<ResourceManager> agentsInRange = new List<ResourceManager>();
        [SerializeField] private float resourceCollectionRange;

        [Header("Debug")] 
        public bool resourcesSufficient = false;
        public TMP_Text output;

        private void Start()
        {
            resourceManager = GetComponent<ResourceManager>();
        }

        private void OnTriggerEnter(Collider other)
        {
            ResourceManager agentResourceManager = other.GetComponent<ResourceManager>();
            if (agentResourceManager == null) return;
            print("Transfering resources");
            agentResourceManager.TransferResource(ResourceManager.ResourceType.Wood,   agentResourceManager.GetResourceAmount(ResourceManager.ResourceType.Wood),  ref resourceManager);
            agentResourceManager.TransferResource(ResourceManager.ResourceType.Stone,  agentResourceManager.GetResourceAmount(ResourceManager.ResourceType.Stone), ref resourceManager);
            agentResourceManager.TransferResource(ResourceManager.ResourceType.Cement, agentResourceManager.GetResourceAmount(ResourceManager.ResourceType.Cement),ref resourceManager);

            if (CheckResourcesAmounts())
            {
                resourceManager.AddWood(-woodNeeded[towerLevel]);
                resourceManager.AddStone(-stoneNeeded[towerLevel]);
                resourceManager.AddCement(-cementNeeded[towerLevel]);
                towerLevels[towerLevel].SetActive(true);
                towerLevel++;
            }
        }

        public void CheckResources()
        {
            if (towerLevel == towerLevels.Length)
            {
                output.text = $"<color=yellow>Max Tower Levels have been reached!</color>";
                resourcesSufficient = false;
                return;
            }

            if (woodNeeded[towerLevel] > resourceManager.GetResourceAmount(ResourceManager.ResourceType.Wood))
            {
                output.text = $"<color=red>Wood Needed!</color>";
                resourcesSufficient = false; 
                return;
            }

            if (stoneNeeded[towerLevel] > resourceManager.GetResourceAmount(ResourceManager.ResourceType.Stone))
            {
                output.text = $"<color=red>Stone Needed!</color>";
                resourcesSufficient = false;
                return;
            }

            if (cementNeeded[towerLevel] > resourceManager.GetResourceAmount(ResourceManager.ResourceType.Cement))
            {
                output.text = $"<color=red>Cement Needed!</color>";
                resourcesSufficient = false;
                return;
            }
            
            output.text = $"<color=green>Resources for {towerLevel} sufficient</color>";
            resourcesSufficient = true;
            return;
        }

        public bool CheckResourcesAmounts()
        {
            CheckResources();
            return resourcesSufficient;
        }
    }
}