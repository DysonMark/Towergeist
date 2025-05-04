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

        [Header("Boredom")]
        [Tooltip("True: Agent feels bored and may chat if theyre friendly.")]
        public bool IsBored = false;
        [Range(0, 1)] public float BoredomLevel = 0f;
        public float BoredomThreshold = 0.7f;

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

        public void IncreaseBoredom(float rate)
        {
            BoredomLevel = Mathf.Clamp01(BoredomLevel + rate * Time.deltaTime);
            IsBored = BoredomLevel >= BoredomThreshold;
        }
        public void DecreaseBoredom(float deltaTime)
        {
            BoredomLevel = Mathf.Clamp01(BoredomLevel - deltaTime * 0.2f);
            IsBored = BoredomLevel >= BoredomThreshold;
        }
    }
}