using System;
using System.Collections;
using System.Collections.Generic;
using Resources;
using UnityEngine;

namespace Dyson.Towergeist
{
    public class WhichAgent : MonoBehaviour
    {
        [SerializeField] private bool isCementAgent;
        [SerializeField] private bool isStoneAgent;
        [SerializeField] private bool isWoodAgent;

        public ResourceManager.ResourceType GetAgentType()
        {
            if (isCementAgent) return ResourceManager.ResourceType.Cement;
            else if (isStoneAgent) return ResourceManager.ResourceType.Stone;
            else if (isWoodAgent) return ResourceManager.ResourceType.Wood;
            else return ResourceManager.ResourceType.Unknown;
        }
    }
}
