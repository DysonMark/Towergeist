using System.Collections;
using UnityEngine;

namespace Actions.Chat
{
    [DisallowMultipleComponent]
    public class SpritePopup : MonoBehaviour
    {
        #region Variables
        public Sprite chatSprite;
        public Sprite yellSprite;
        public float height = 2f;
        public float duration = 2f;
        [SerializeField] private SpriteRenderer _renderer;
        private Coroutine _hideCoroutine;
        #endregion

        #region Unity Methods
        void Awake()
        {
            _renderer.sortingOrder = 100;
            _renderer.enabled = false;
        }
        #endregion

        #region Public Methods
        public void ShowChat() => Show(chatSprite);
        public void ShowYell() => Show(yellSprite);
        #endregion

        #region Private Methods
        private void Show(Sprite sprite)
        {
            if (sprite == null) return;
            _renderer.sprite = sprite;
            _renderer.enabled = true;
            if (_hideCoroutine != null) StopCoroutine(_hideCoroutine);
            _hideCoroutine = StartCoroutine(HideAfter());
        }

        private IEnumerator HideAfter()
        {
            yield return new WaitForSeconds(duration);
            _renderer.enabled = false;
        }
        #endregion
    }
}