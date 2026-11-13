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

        //[ContextMenu("Test_DefaultNotification()")]
        //public void ShowDefaultNotification()
        //{
        //    ModularPrototypes.UI.Notification.NotificationSystem.Show("Header", NotificationConstants.NotificationColors.DefaultHeaderColor, "Notification Content.", NotificationConstants.NotificationColors.DefaultContentColor, NotificationConstants.NotificationColors.DefaultBackgroundColor, 3f);
        //}

        //[ContextMenu("Test_ErrorNotification()")]
        //public void ShowErrorNotification()
        //{
        //    ModularPrototypes.UI.Notification.NotificationSystem.Show("Header", NotificationConstants.NotificationColors.ErrorHeaderColor, "Notification Content.", NotificationConstants.NotificationColors.ErrorContentColor, NotificationConstants.NotificationColors.ErrorBackgroundColor, 3f);
        //}

        //[ContextMenu("Test_WarningNotification()")]
        //public void ShowWarningNotification()
        //{
        //    ModularPrototypes.UI.Notification.NotificationSystem.Show("Header", NotificationConstants.NotificationColors.WarningHeaderColor, "Notification Content.", NotificationConstants.NotificationColors.WarningContentColor, NotificationConstants.NotificationColors.WarningBackgroundColor, 3f);
        //}

        //[ContextMenu("Test_SuccessNotification()")]
        //public void ShowSuccessNotification()
        //{
        //    ModularPrototypes.UI.Notification.NotificationSystem.Show("Header", NotificationConstants.NotificationColors.SuccessHeaderColor, "Notification Content.", NotificationConstants.NotificationColors.SuccessContentColor, NotificationConstants.NotificationColors.SuccessBackgroundColor, 3f);
        //}
    }
}