using UnityEngine;

namespace ModularPrototypes.Platformer.Settings.UI
{
    [System.Serializable]
    public abstract class UIState : MonoBehaviour
    {
        [SerializeField] private string _stateName;
        [SerializeField] private SettingsData _settingsData;

        #region  Events and Callbacks
        public delegate void UIObserver(SettingsData data);
        public abstract event UIObserver OnUIStateChanged;
        #endregion

        #region Getters
        public string GetStateName() => _stateName;
        public SettingsData GetSettingsData() => _settingsData;
        #endregion

        #region Abstract Methods
        public abstract void Initialize();
        public abstract void OnEnter();
        public abstract void OnFrame();
        public abstract void OnExit();

        protected abstract void SubscribeToUIElements();
        protected abstract void UnsubscribeFromUIElements();
        protected abstract void OnUIInteracted();
        protected abstract void ApplyToUI();
        #endregion

        #region Debug
        internal void D(string message, bool isError = false)
        {
            DebugUtils.DebugInfo.Print($"<<UIState - {_stateName}>> {message}", isError ? DebugUtils.DebugConstants.ERROR : DebugUtils.DebugConstants.INFO);
        }
        #endregion
    }
}
