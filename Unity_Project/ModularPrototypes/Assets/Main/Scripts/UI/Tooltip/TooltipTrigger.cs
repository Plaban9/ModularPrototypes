using DG.Tweening;

using UnityEngine;
using UnityEngine.EventSystems;

namespace ModularPrototypes.UI.Tooltip
{
    public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Tooltip Text Fields")]
        [SerializeField] private string _header;
        [SerializeField][Multiline()] private string _content;

        [Header("Tooltip Colors")]
        [SerializeField] private Color _headerColor = new(0.5401388f, 0.9497362f, 0.9622642f, 1f); //Color.white
        [SerializeField] private Color _contentColor = Color.white;
        [SerializeField] private Color _backgroundColor = new(0.1886792f, 0.1886792f, 0.1886792f, 0.95f); //Color.gray3

        [Header("Animation Attributes")]
        [SerializeField] private float _showDelay = 0.5f;

        private static Sequence _tooltipDelaySequence;

        public void OnPointerEnter(PointerEventData eventData)
        {
            // Ensure any previous pending sequence is cancelled before starting a new one
            if (_tooltipDelaySequence != null && _tooltipDelaySequence.IsActive())
            {
                _tooltipDelaySequence.Kill();
                _tooltipDelaySequence = null;
            }

            // Create a DOTween sequence that waits for the configured delay then shows the tooltip.
            _tooltipDelaySequence = DOTween.Sequence();
            _tooltipDelaySequence.AppendInterval(_showDelay).OnComplete(() =>
            {
                TooltipSystem.Show(_header, _headerColor, _content, _contentColor, _backgroundColor);
                _tooltipDelaySequence = null;
            });
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            // Cancel the pending show delay if pointer exits before tooltip is shown
            if (_tooltipDelaySequence != null && _tooltipDelaySequence.IsActive())
            {
                _tooltipDelaySequence.Kill();
                _tooltipDelaySequence = null;
            }

            // Hide tooltip immediately if already shown
            TooltipSystem.Hide();
        }
    }
}
