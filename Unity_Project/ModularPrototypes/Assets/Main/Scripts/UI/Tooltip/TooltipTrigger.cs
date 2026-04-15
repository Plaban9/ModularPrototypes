using DG.Tweening;

using UnityEngine;
using UnityEngine.EventSystems;

namespace ModularPrototypes.UI.Tooltip
{
    public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private string _header;
        [SerializeField][Multiline()] private string _content;
        [SerializeField] private Color _backgroundColor = Color.gray3;

        //private static Sequence _delaySequence;

        public void OnPointerEnter(PointerEventData eventData)
        {
            TooltipSystem.Show(_backgroundColor, _header, _content);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            TooltipSystem.Hide();
        }
    }
}
