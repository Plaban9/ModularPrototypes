using ModularPrototypes.Platformer.Settings.UI;

using UnityEngine;

namespace ModularPrototypes.Platformer.Settings
{
    [CreateAssetMenu(fileName = "SettingsData", menuName = "Data/Platformer/Data/Settings")]
    public class SettingsData : ScriptableObject
    {
        [SerializeField] private string settingsName;
        [SerializeField] private PlatformSettings settingsType;
        [SerializeField] private Color backgroundColor;

        #region GETTERS
        public string GetSettingsName()
        {
            return settingsName;
        }

        public PlatformSettings GetSettingsType()
        {
            return settingsType;
        }

        public Color GetBackgroundColor()
        {
            return backgroundColor;
        }
        #endregion
    }
}
