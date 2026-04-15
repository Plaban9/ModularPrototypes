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

        public static void Show(Color color, string header = "", string content = "")
        {
            _instance._tooltipHandler.SetText(header, content, color);
            _instance._tooltipHandler.gameObject.SetActive(true);
        }

        public static void Hide()
        {
            _instance._tooltipHandler.gameObject.SetActive(false);
        }
    }
}
