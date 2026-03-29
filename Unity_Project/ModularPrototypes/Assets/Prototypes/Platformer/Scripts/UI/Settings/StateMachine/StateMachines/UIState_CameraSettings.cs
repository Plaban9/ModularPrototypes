using UnityEngine;
using UnityEngine.UI;

namespace ModularPrototypes.Platformer.Settings.UI
{
    public class UIState_CameraSettings : UIState
    {
        #region UI Elements
        [SerializeField] Slider _showOrthographicSlider;
        [SerializeField] Button _resetToDefaultButton;
        #endregion

        public override event UIObserver OnUIStateChanged;

        public override void Initialize()
        {
            D("OnInitialize");
        }

        public override void OnEnter()
        {
            D("OnEnter");
            SubscribeToUIElements();
        }

        public override void OnExit()
        {
            D("OnExit");
            UnsubscribeFromUIElements();
        }

        public override void OnFrame()
        {
            D("OnFrame");
        }

        protected override void ApplyDefaultSettingsData()
        {
            
        }

        protected override void ApplyToUI()
        {
           
        }

        protected override void OnUIInteracted()
        {
            D("OnUIInteracted");
        }

        protected override void SubscribeToUIElements()
        {
            D("SubscribeToUIElements");
        }

        protected override void UnsubscribeFromUIElements()
        {
            D("UnsubscribeFromUIElements");
        }
    }
}
