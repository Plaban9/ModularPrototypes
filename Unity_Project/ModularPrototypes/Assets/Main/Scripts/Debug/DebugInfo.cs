using UnityEngine;

namespace ModularPrototypes.DebugUtils
{
    public class DebugInfo : MonoBehaviour
    {
        public static void Print(string message, DebugConstants debugConstants = DebugConstants.INFO)
        {
            switch (debugConstants)
            {
                case DebugConstants.INFO:
                    Debug.Log(message);
                    break;
                case DebugConstants.WARNING:
                    Debug.LogWarning(message);
                    break;
                case DebugConstants.ERROR:
                    Debug.LogError(message);
                    break;
            }
        }
    }
}
