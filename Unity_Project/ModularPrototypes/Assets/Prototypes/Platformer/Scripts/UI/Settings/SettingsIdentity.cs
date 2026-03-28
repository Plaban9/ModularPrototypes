using UnityEngine;

namespace ModularPrototypes.Platformer.Settings.UI
{
    public class SettingsIdentity : MonoBehaviour
    {
        [SerializeField] private PlatformSettings settingIdentity;
        public PlatformSettings GetIdentity() => settingIdentity;
    }
}
