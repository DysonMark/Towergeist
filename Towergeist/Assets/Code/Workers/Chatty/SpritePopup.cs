using UnityEngine;

namespace Actions.Chat
{
    [DisallowMultipleComponent]
    public class SpritePopup : MonoBehaviour
    {
        #region Variables
        [Header("Popup Sprites")]
        [Tooltip("Sprite to show when chatting.")]
        public Sprite chatSprite;
        [Tooltip("Sprite to show when yelling.")]
        public Sprite yellSprite;

        [Header("Display Settings")]
        [Tooltip("Height above the agent for the spriite to spawn")]
        public float height = 2f;
        [Tooltip("Seconds before the pop-up disappears.")]
        public float duration = 2f;
        #endregion

        public void Show(Sprite sprite)
        {
            if (sprite == null) return;
            var go = new GameObject("SpritePopup");
            go.transform.SetParent(transform, false);
            go.transform.localPosition = Vector3.up * height;

            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = sprite;
            sr.sortingOrder = 100;

            Destroy(go, duration);
        }

        public void ShowChat() => Show(chatSprite);

        public void ShowYell() => Show(yellSprite);
    }
}

