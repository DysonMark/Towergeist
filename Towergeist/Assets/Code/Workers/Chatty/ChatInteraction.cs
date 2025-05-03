using UnityEngine;

namespace Actions.Chat
{
    [DisallowMultipleComponent]
    public class ChatInteraction : MonoBehaviour
    {
        #region Variables
        [Tooltip("Sprite to show when chatting.")]
        public Sprite chatSprite;
        [Tooltip("Height above the agent to spawn the sprite.")]
        public float height = 2f;
        [Tooltip("Seconds before the popup disappears.")]
        public float duration = 2f;
        #endregion

        public void ShowChat()
        {
            if (chatSprite == null) return;

            var go = new GameObject("ChatPopup");
            go.transform.SetParent(transform, false);
            go.transform.localPosition = Vector3.up * height;

            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = chatSprite;
            sr.sortingOrder = 100;

            Destroy(go, duration);
        }
    }
}
