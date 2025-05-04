using UnityEngine;

namespace Actions.Chat
{
    [DisallowMultipleComponent]
    public class ChatInteraction : MonoBehaviour
    {
        #region Variables
        public SpriteRenderer chatRenderer;
        public Sprite chatSprite;
        public float duration = 2f;

        private float _timer;
        #endregion

        void Update()
        {
            if (chatRenderer.enabled)
            {
                _timer += Time.deltaTime;
                if (_timer >= duration)
                {
                    chatRenderer.enabled = false;
                }
            }
        }

        public void ShowChat()
        {
            _timer = 0f;
            chatRenderer.sprite = chatSprite;
            chatRenderer.enabled = true;
        }
    }
}