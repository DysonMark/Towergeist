using System;
using UnityEngine;

namespace Resources
{
    /// <summary>
    /// This component allows for ResourceManagers to harvest a resource
    /// </summary>
    public class ResourceSite : MonoBehaviour
    {
        public ResourceManager.ResourceType resourceType;
        public float resourceAmountPerSecond;
    }
}