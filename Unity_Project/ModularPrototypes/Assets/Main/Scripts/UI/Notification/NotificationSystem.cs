using UnityEngine;

namespace ModularPrototypes.UI.Notification
{
    public class NotificationSystem : MonoBehaviour
    {
        private static NotificationSystem _instance;

        [SerializeField] private NotificationHandler _notificationHandler;

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

        public static void Show(string header = "", Color? headerColor = null, string content = "", Color? contentColor = null, Color? backgroundColor = null, float displayDuration = 3f)
        {
            if (_instance == null)
            {
                return;
            }

            _instance._notificationHandler.SetNotificationAttributes(header, headerColor ?? Color.white, content, contentColor ?? Color.white, backgroundColor ?? Color.gray);
            _instance._notificationHandler.ShowNotification(displayDuration);
        }

        public static void Hide()
        {
            if (_instance == null)
            {
                return;
            }

            _instance._notificationHandler.HideNotification();
        }
    }
}