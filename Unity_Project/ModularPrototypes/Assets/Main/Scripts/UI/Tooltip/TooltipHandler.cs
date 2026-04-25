using DG.Tweening;
using TMPro;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace ModularPrototypes.UI.Tooltip
{
    [ExecuteInEditMode()]
    public class TooltipHandler : MonoBehaviour
    {
        #region Debug
        [Header("Debug Settings")]
        [SerializeField] private bool _updateInEditor;
        #endregion

        [SerializeField] private Image _backgroundImage;

        [SerializeField] private TextMeshProUGUI _headerText;
        [SerializeField] private TextMeshProUGUI _contentText;
        [SerializeField] private LayoutElement _layoutElement;

        [SerializeField] private RectTransform _rectTransform;

        [SerializeField] private int _characterWrapLimit;

        [Header("Fade Settings")]
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _fadeDuration = 0.15f;
        [SerializeField] private Ease _fadeEase = Ease.Linear;

        private Tween _fadeTween;

        private void Awake()
        {
            if (_rectTransform == null)
            {
                _rectTransform = GetComponent<RectTransform>();
            }

            if (_canvasGroup == null)
            {
                _canvasGroup = GetComponent<CanvasGroup>();
            }

            if (_canvasGroup == null)
            {
                _canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }

            // Ensure canvasGroup alpha matches active state in editor/runtime
            if (!Application.isPlaying)
            {
                // keep visibility for editor preview
                _canvasGroup.alpha = gameObject.activeSelf ? 1f : 0f;
            }
            else
            {
                // runtime: start hidden
                _canvasGroup.alpha = 0f;
                gameObject.SetActive(false);
            }
        }

        void Update()
        {
            // Only handle layout size in the editor to avoid unnecessary performance overhead during runtime
            if (Application.isEditor)
            {
                HandleLayoutSize();
            }

            //if (_updateInEditor)
            {
                SetTooltipPosition();
            }
        }

        private void SetTooltipPosition()
        {
            var position = Mouse.current.position.value;

            // Earlier Logic: Based on normalization of mouse position to screen dimensions, but this can cause issues showing tooltip on content and on mous position.
            // var pivotX = position.x / Screen.width;
            // var pivotY = position.y / Screen.height;

            // New Logic: Determine pivot based on proximity to screen edges to ensure tooltip stays fully visible
            var pivotX = position.x < Screen.width / 2f ? -0.05f : 1.05f; // Left half of screen -> pivot left, Right half -> pivot right
            var pivotY = position.y < Screen.height / 2f ? -0.1f : 1.1f; // Bottom half of screen -> pivot bottom, Top half -> pivot top

            _rectTransform.pivot = new Vector2(pivotX, pivotY);

            transform.position = position;
        }

        private void HandleLayoutSize()
        {
            int headerLength = _headerText.text.Length;
            int contentLength = _contentText.text.Length;

            _layoutElement.enabled = (headerLength > _characterWrapLimit) || (contentLength > _characterWrapLimit);
        }

        public void SetTooltipAttributes(string header, Color headerColor, string content, Color contentColor, Color backgroundColor)
        {
            if (string.IsNullOrEmpty(header))
            {
                _headerText.gameObject.SetActive(false);
            }
            else
            {
                _headerText.gameObject.SetActive(true);
                _headerText.text = header;
            }

            if (string.IsNullOrEmpty(content))
            {
                _contentText.gameObject.SetActive(false);
            }
            else
            {
                _contentText.gameObject.SetActive(true);
                _contentText.text = content;
            }

            _headerText.color = headerColor;
            _contentText.color = contentColor;
            _backgroundImage.color = backgroundColor;

            HandleLayoutSize();
            SetTooltipPosition();
        }

        public void ShowTooltip()
        {
            // Stop any running fade tween
            if (_fadeTween != null && _fadeTween.IsActive())
            {
                _fadeTween.Kill();
                _fadeTween = null;
            }

            // Ensure visible and start fade-in
            gameObject.SetActive(true);
            _canvasGroup.alpha = 0f;
            _fadeTween = _canvasGroup.DOFade(1f, _fadeDuration).SetEase(_fadeEase)
                .OnComplete(() => _fadeTween = null);
        }

        public void HideTooltip()
        {
            // Stop any running fade tween
            if (_fadeTween != null && _fadeTween.IsActive())
            {
                _fadeTween.Kill();
                _fadeTween = null;
            }

            // If already inactive just ensure alpha is zero
            if (!gameObject.activeSelf)
            {
                _canvasGroup.alpha = 0f;
                return;
            }

            // Fade out then deactivate
            _fadeTween = _canvasGroup.DOFade(0f, _fadeDuration /2f).SetEase(_fadeEase)
                .OnComplete(() =>
                {
                    _fadeTween = null;
                    gameObject.SetActive(false);
                });
        }
    }
}