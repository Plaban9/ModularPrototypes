using TMPro;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace ModularPrototypes.UI.Tooltip
{
    [ExecuteInEditMode()]
    public class TooltipHandler : MonoBehaviour
    {
        [SerializeField] private Image _backgroundImage;

        [SerializeField] private TextMeshProUGUI _headerText;
        [SerializeField] private TextMeshProUGUI _contentText;
        [SerializeField] private LayoutElement _layoutElement;

        [SerializeField] private RectTransform _rectTransform;

        [SerializeField] private int _characterWrapLimit;

        private void Awake()
        {
            if (_rectTransform == null)
            {
                _rectTransform = GetComponent<RectTransform>();
            }
        }

        void Update()
        {
            // Only handle layout size in the editor to avoid unnecessary performance overhead during runtime
            if (Application.isEditor)
            {
                HandleLayoutSize();
            }

            SetTooltipPosition();
        }

        private void SetTooltipPosition()
        {
            var position = Mouse.current.position.value;            

            var pivotX = position.x / Screen.width;
            var pivotY = position.y / Screen.height;

            _rectTransform.pivot = new Vector2(pivotX, pivotY);

            transform.position = position;
        }

        private void HandleLayoutSize()
        {
            int headerLength = _headerText.text.Length;
            int contentLength = _contentText.text.Length;

            _layoutElement.enabled = (headerLength > _characterWrapLimit) || (contentLength > _characterWrapLimit);
        }

        public void SetText(string header, string content, Color color)
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

            _backgroundImage.color = color;

            HandleLayoutSize();
            SetTooltipPosition();
        }
    }
}
