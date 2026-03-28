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
            //throw new System.NotImplementedException();
        }

        public override void OnEnter()
        {
            throw new System.NotImplementedException();
        }

        public override void OnExit()
        {
            throw new System.NotImplementedException();
        }

        public override void OnFrame()
        {
            throw new System.NotImplementedException();
        }

        protected override void ApplyToUI()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnUIInteracted()
        {
            throw new System.NotImplementedException();
        }

        protected override void SubscribeToUIElements()
        {
            throw new System.NotImplementedException();
        }

        protected override void UnsubscribeFromUIElements()
        {
            throw new System.NotImplementedException();
        }
    }
}
