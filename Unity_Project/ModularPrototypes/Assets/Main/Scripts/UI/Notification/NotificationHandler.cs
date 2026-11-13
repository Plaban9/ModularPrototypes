using System.Collections;

using DG.Tweening;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace ModularPrototypes.UI.Notification
{
    public class NotificationHandler : MonoBehaviour
    {
        [SerializeField] private Image _backgroundImage;

        [SerializeField] private TextMeshProUGUI _headerText;
        [SerializeField] private TextMeshProUGUI _contentText;
        [SerializeField] private LayoutElement _layoutElement;

        [SerializeField] private RectTransform _rectTransform;

        [SerializeField] private int _characterWrapLimit;

        [Header("Notification Position")]
        [SerializeField] private RectTransform _notificationParent;

        [Header("Animation Settings")]
        [SerializeField] private float _entryDistance = 150f;
        [SerializeField] private float _exitDistance = 150f;
        [SerializeField] private float _animationDuration = 0.3f;

        private Tween _movementTween;
        private Coroutine _animationCoroutine;

        private Vector3 _fixedTargetWorldPosition;
        private Vector3 _originalScale;

        private void Awake()
        {
            if (_rectTransform == null)
            {
                _rectTransform = GetComponent<RectTransform>();
            }

            _originalScale = _rectTransform.localScale;
            _fixedTargetWorldPosition = GetTargetWorldPosition();

            _rectTransform.position = GetEntryPosition();
            gameObject.SetActive(false);
        }

        private Vector3 GetTargetWorldPosition()
        {
            if (_notificationParent == null)
            {
                return _rectTransform.position;
            }

            return _notificationParent.position;
        }

        private Vector3 GetEntryPosition()
        {
            return _fixedTargetWorldPosition + Vector3.up * _entryDistance;
        }

        private Vector3 GetExitPosition()
        {
            return _fixedTargetWorldPosition + Vector3.up * _exitDistance;
        }

        private void HandleLayoutSize()
        {
            int headerLength = _headerText.text.Length;
            int contentLength = _contentText.text.Length;

            _layoutElement.enabled = headerLength > _characterWrapLimit || contentLength > _characterWrapLimit;
        }

        public void SetNotificationAttributes(string header, Color headerColor, string content, Color contentColor, Color backgroundColor)
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
        }

        public void ShowNotification(float displayDuration)
        {
            StopAnimation(resetPosition: true);

            gameObject.SetActive(true);

            _animationCoroutine = StartCoroutine(PlayNotificationAnimation(displayDuration));
        }

        private IEnumerator PlayNotificationAnimation(float displayDuration)
        {
            Canvas.ForceUpdateCanvases();

            // Allow the Unity UI layout system to finish updating.
            yield return null;

            // Capture the spawn point once for this notification animation.
            _fixedTargetWorldPosition = GetTargetWorldPosition();

            Vector3 entryPosition = GetEntryPosition();

            _rectTransform.position = entryPosition;
            _rectTransform.localScale = _originalScale * 0.95f;

            Sequence notificationSequence = DOTween.Sequence();

            notificationSequence
                .Append(_rectTransform
                .DOMove(_fixedTargetWorldPosition, _animationDuration)
                .SetEase(Ease.OutBack))
                .Join(_rectTransform
                .DOScale(_originalScale, _animationDuration)
                .SetEase(Ease.OutBack))
                .AppendInterval(Mathf.Max(0f, displayDuration))
                .Append(_rectTransform
                .DOMove(GetExitPosition(), _animationDuration)
                .SetEase(Ease.InBack))
                .OnComplete(() =>
                {
                    _rectTransform.position = GetExitPosition();
                    _rectTransform.localScale = _originalScale;

                    _movementTween = null;
                    _animationCoroutine = null;

                    gameObject.SetActive(false);
                });

            _movementTween = notificationSequence;
        }

        public void HideNotification()
        {
            // Do not reset the current position when hiding.
            StopAnimation(resetPosition: false);

            if (!gameObject.activeSelf)
            {
                return;
            }

            Vector3 exitPosition = GetExitPosition();

            _movementTween = _rectTransform
                .DOMove(exitPosition, _animationDuration)
                .SetEase(Ease.InBack)
                .OnComplete(() =>
                {
                    _rectTransform.position = exitPosition;
                    _rectTransform.localScale = _originalScale;

                    _movementTween = null;
                    gameObject.SetActive(false);
                });
        }

        private void StopAnimation(bool resetPosition)
        {
            if (_animationCoroutine != null)
            {
                StopCoroutine(_animationCoroutine);
                _animationCoroutine = null;
            }

            if (_movementTween != null && _movementTween.IsActive())
            {
                _movementTween.Kill();
                _movementTween = null;
            }

            if (!resetPosition)
            {
                return;
            }

            _fixedTargetWorldPosition = GetTargetWorldPosition();

            _rectTransform.position = GetEntryPosition();
            _rectTransform.localScale = _originalScale;
        }
    }
}