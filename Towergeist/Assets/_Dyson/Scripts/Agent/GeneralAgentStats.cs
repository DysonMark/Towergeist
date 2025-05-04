using UnityEngine;

namespace Stat
{
    public class GeneralAgentStats : MonoBehaviour
    {
        [Header("Tiredness")]
        [Tooltip("0 = fully rested & 100 = completely exhausted")]
        [Range(0, 100)]
        public float Tiredness = 0f;


        [Tooltip("True: The worker cango to sleep")]
        public bool CanSleep = false;

        #region Boredom
        [Tooltip("True: Agent feels bored and may chat if theyre friendly.")]
        [Range(0, 100)] public float BoredomLevel = 0f;
        public bool IsBored => BoredomLevel >= 50f;

        public float MaxBoredom = 100f;
        public void ResetBoredom()
        {
            if (IsFriendly)
            {
                BoredomLevel = MaxBoredom;
                return;
            }
            BoredomLevel = 0f;
        }
        #endregion

        #region Gossip
        public float GossipMeter;
        public float GossipThreshold = 50f;
        public float MaxGossip = 100f;
        public bool HasGossip => GossipMeter >= GossipThreshold;

        public void AddGossip(float amount) => GossipMeter = Mathf.Min(MaxGossip, GossipMeter + amount);

        public void ClearGossip() => GossipMeter = 0f;

        #endregion

        [Header("Chat riority")]
        public int ChatPriorityModifier;

        [Header("Rested Counter")]
        [Tooltip("The rest value of the agent accumulated to subtract from tiredness meter when interrutped")]
        public int Rested = 0;

        [Header("Social State")]
        [Tooltip("To plan new goal/ go back to priamry goal and reset tiredness to 0 and boredom to 50")]
        public bool IsBeingYelledAt = false;

        public bool IsTired => Tiredness >= 50;
        public bool IsExhausted => Tiredness >= 100;

        [Header("Agent Attributes")]
        [Tooltip("True: This Agent can fly")]
        public bool canFly = false;
        [Tooltip("True: This agent is probably a giant.")]
        public bool isHeavy = false;
        [Tooltip("True: This Agent is friendly; otherwise it is not friendly")]
        public bool IsFriendly = true;
    }
}