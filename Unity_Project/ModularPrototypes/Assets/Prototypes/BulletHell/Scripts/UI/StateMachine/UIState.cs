using ModularPrototypes.BulletHell.Data;

using UnityEngine;

namespace ModularPrototypes.BulletHell.UI.StateMachine
{
    [System.Serializable]
    public abstract class UIState : MonoBehaviour
    {
        [SerializeField] protected string stateName;
        [SerializeField] protected BulletPattern bulletPattern;
        [SerializeField] protected BulletHellConfig defaultBulletHellPatternConfig;
        [Header("Instance Data Here (For viewing only)")]
        [SerializeField] protected BulletHellPatternData bulletHellPatternData;

        #region Events and Callbacks
        public delegate void UIObserver(BulletPattern bulletPattern, BulletHellPatternData bulletHellPatternData);
        public abstract event UIObserver OnUIStateChanged;
        #endregion

        public string GetStateName()
        {
            return stateName;
        }

        public BulletPattern GetBulletPattern()
        {
            return bulletPattern;
        }

        public BulletHellPatternData GetBulletHellPatternData()
        {
            return bulletHellPatternData;
        }

        public abstract void Initialize();

        protected abstract void ApplyToUI();

        protected abstract void SubscribeToUIElements();
        protected abstract void UnsubscribeFromUIElements();
        protected abstract void OnUIInteracted();

        public abstract void OnEnter();
        public abstract void OnFrame();
        public abstract void OnExit();

        protected void ApplyDefaultBulletHellPatternData()
        {
            bulletHellPatternData.SetConfiguration(defaultBulletHellPatternConfig.GetBulletHellPatternData());
        }

        internal void D(string message, bool isError = false)
        {
            DebugUtils.DebugInfo.Print($"<<UIState - {stateName}>> {message}", isError ? DebugUtils.DebugConstants.ERROR : DebugUtils.DebugConstants.INFO);
        }
    }
}
