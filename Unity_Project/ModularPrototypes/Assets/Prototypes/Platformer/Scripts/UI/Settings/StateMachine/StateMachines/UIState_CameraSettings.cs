using ModularPrototypes.Platformer.Measurements;

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

        #region Camera Settings
        [SerializeField] private CameraData _cameraDefaultData;
        [SerializeField] private CameraData _cameraData;
        [SerializeField] private Camera _camera;
        #endregion

        public override event UIObserver OnUIStateChanged;

        public override void Initialize()
        {
            D("OnInitialize");
            stateName = string.IsNullOrEmpty(stateName) ? "Camera Settings" : stateName;
            _cameraData = Instantiate(_cameraDefaultData);
            _camera.orthographic = _cameraData.GetIsOrthographic();
            ApplyToUI();
            D("Initialized");
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
            _cameraData.ApplyConfiguratioon(_cameraDefaultData);
            ApplyToUI();
        }

        protected override void ApplyToUI()
        {
            _showOrthographicSlider.value = _cameraData.GetIsOrthographic() ? 1f : 0f;
        }

        protected override void OnUIInteracted()
        {
            D("OnUIInteracted");
            _camera.orthographic = _cameraData.GetIsOrthographic();
        }

        protected override void SubscribeToUIElements()
        {
            D("SubscribeToUIElements");

            _showOrthographicSlider.onValueChanged.AddListener((value) =>
            {
                _cameraData.SetIsOrthographic(_showOrthographicSlider.value > 0.5f);
                OnUIInteracted();
            });

            _resetToDefaultButton.onClick.AddListener(() =>
            {
                ApplyDefaultSettingsData();
                OnUIInteracted();
            });
        }

        protected override void UnsubscribeFromUIElements()
        {
            D("UnsubscribeFromUIElements");
            _showOrthographicSlider.onValueChanged.RemoveAllListeners();
            _resetToDefaultButton.onClick.RemoveAllListeners();
        }
    }
}
