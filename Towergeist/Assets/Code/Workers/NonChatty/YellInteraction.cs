using UnityEngine;

namespace Actions.Chat
{
    [DisallowMultipleComponent]
    public class YellInteraction : MonoBehaviour
    {
        #region Variables
        [Tooltip("Sprite to show when yelling.")]
        public Sprite yellSprite;
        [Tooltip("Height above the agent to spawn the sprite.")]
        public float height = 2f;
        [Tooltip("Seconds before the pop-up disappears.")]
        public float duration = 2f;
        #endregion

        public void ShowYell()
        {
            if (yellSprite == null) return;

            var go = new GameObject("YellPopup");
            go.transform.SetParent(transform, false);
            go.transform.localPosition = Vector3.up * height;

            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = yellSprite;
            sr.sortingOrder = 100;

            Destroy(go, duration);
        }
    }
}