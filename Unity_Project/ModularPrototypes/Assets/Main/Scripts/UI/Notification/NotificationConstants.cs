using UnityEngine;

namespace ModularPrototypes.UI.Notification
{
    public class NotificationConstants
    {
        public readonly struct NotificationColors
        {
            public static readonly Color DefaultHeaderColor = Color.cyan;
            public static readonly Color DefaultContentColor = Color.white;
            public static readonly Color DefaultBackgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.8f);

            public static readonly Color ErrorHeaderColor = Color.red;
            public static readonly Color ErrorContentColor = Color.white;
            public static readonly Color ErrorBackgroundColor = new Color(0.5f, 0f, 0f, 0.75f);

            public static readonly Color WarningHeaderColor = Color.yellow;
            public static readonly Color WarningContentColor = Color.white;
            public static readonly Color WarningBackgroundColor = new Color(1f, 0.5f, 0f, 0.75f);

            public static readonly Color SuccessHeaderColor = Color.green;
            public static readonly Color SuccessContentColor = Color.white;
            public static readonly Color SuccessBackgroundColor = new Color(0f, 0.5f, 0f, 0.75f);            
        }
    }
}
