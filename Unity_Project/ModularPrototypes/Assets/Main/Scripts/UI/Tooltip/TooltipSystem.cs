using UnityEngine;

namespace ModularPrototypes.UI.Tooltip
{
    public class TooltipSystem : MonoBehaviour
    {
        private static TooltipSystem _instance;

        [SerializeField] private TooltipHandler _tooltipHandler;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public static void Show(string header = "", Color? headerColor = null, string content = "", Color? contentColor = null, Color? backgroundColor = null)
        {
            _instance._tooltipHandler.SetTooltipAttributes(header, headerColor ?? Color.white, content, contentColor ?? Color.white, backgroundColor ?? Color.gray);
            _instance._tooltipHandler.ShowTooltip();
        }

        public static void Hide()
        {
            _instance._tooltipHandler.HideTooltip();
        }
    }
}