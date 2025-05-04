using UnityEngine;
using Stat;

namespace Actions.Chat
{
    [DisallowMultipleComponent]
    public class YellInteraction : MonoBehaviour
    {
        #region Variables
        public SpriteRenderer yellRenderer;
        public Sprite yellSprite;
        public float duration = 2f;

        private float _timer;
        #endregion

        void Update()
        {
            if (yellRenderer.enabled)
            {
                _timer += Time.deltaTime;
                if (_timer >= duration)
                    yellRenderer.enabled = false;
            }
        }

        public void ShowYell()
        {
            foreach (var agent in FindObjectsOfType<GeneralAgentStats>())
                if (agent.IsFriendly)
                    agent.IsBeingYelledAt = true;

            _timer = 0f;
            yellRenderer.sprite = yellSprite;
            yellRenderer.enabled = true;
        }
    }
}