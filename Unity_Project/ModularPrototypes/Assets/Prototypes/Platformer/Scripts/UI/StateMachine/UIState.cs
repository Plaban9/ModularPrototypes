using ModularPrototypes.Platformer;
using ModularPrototypes.Platformer.Data;

using UnityEngine;

namespace ModularPrototypes.Platformer.UI.StateMachine
{
    [System.Serializable]
    public abstract class UIState : MonoBehaviour
    {
        [SerializeField] protected string stateName;
        [SerializeField] protected PlatformTransformationSettings.TransformDomain transformDomain;
        [SerializeField] protected PlatformConfig defaultPlatformConfig;
        [Header("Instance Data Here (For viewing only)")]
        [SerializeField] protected PlatformConfig platformConfig;

        #region Events and Callbacks
        public delegate void UIObserver(PlatformTransformationSettings.TransformDomain transformDomain, PlatformConfig platformerData);
        public abstract event UIObserver OnUIStateChanged;
        #endregion

        public string GetStateName()
        {
            return stateName;
        }

        public PlatformTransformationSettings.TransformDomain GetTransformDomain()
        {
            return transformDomain;
        }

        public PlatformConfig GetPlatformConfig()
        {
            return platformConfig;
        }

        public abstract void Initialize();

        protected abstract void ApplyToUI();

        protected abstract void SubscribeToUIElements();
        protected abstract void UnsubscribeFromUIElements();
        protected abstract void OnUIInteracted();

        public abstract void OnEnter();
        public abstract void OnFrame();
        public abstract void OnExit();

        protected void ApplyDefaultPlatformerData()
        {
            platformConfig.SetConfiguration(defaultPlatformConfig);
        }

        internal void D(string message, bool isError = false)
        {
            DebugUtils.DebugInfo.Print($"<<UIState - {stateName}>> {message}", isError ? DebugUtils.DebugConstants.ERROR : DebugUtils.DebugConstants.INFO);
        }
    }
}
