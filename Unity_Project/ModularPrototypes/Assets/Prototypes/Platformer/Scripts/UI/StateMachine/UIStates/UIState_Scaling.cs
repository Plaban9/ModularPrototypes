using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace ModularPrototypes.Platformer.UI.StateMachine.States
{
    public class UIState_Scaling : UIState
    {
        [Header("Platformer UI Elements")]
        [SerializeField] private TMP_Dropdown _axisDropdown;
        [SerializeField] private Slider _periodSlider;
        [SerializeField] private TMP_InputField _periodInputField;

        [Header("Per Axis UI Elements")]
        [SerializeField] private UIAxisElements _xAxisElements;
        [SerializeField] private UIAxisElements _yAxisElements;
        [SerializeField] private UIAxisElements _zAxisElements;
        [SerializeField] private Dictionary<PlatformTransformationSettings.TransformAxis, UIAxisElements> _axisToUIElements;


        [Header("Other UI Elements")]
        [SerializeField] private Button _resetToDefaultButton;

        public override event UIObserver OnUIStateChanged;

        public override void Initialize()
        {
            stateName = string.IsNullOrEmpty(stateName) ? "Scaling" : stateName;
            transformDomain = PlatformTransformationSettings.TransformDomain.SCALING;

            platformConfig = Instantiate(defaultPlatformConfig);

            _axisToUIElements = new Dictionary<PlatformTransformationSettings.TransformAxis, UIAxisElements>()
            {
                { PlatformTransformationSettings.TransformAxis.X, _xAxisElements },
                { PlatformTransformationSettings.TransformAxis.Y, _yAxisElements },
                { PlatformTransformationSettings.TransformAxis.Z, _zAxisElements }
            };

            ApplyDefaultPlatformerData();
            ApplyToUI();

            D("Initialized");
        }

        protected override void ApplyToUI()
        {
            //_axisDropdown.value = (int)platformConfig.SelectedAxis; // TODO: Fix This
            _periodSlider.value = platformConfig.GetPeriod();
            _periodInputField.text = platformConfig.GetPeriod().ToString("F2");

            ApplyToUIElementsForAxis(PlatformTransformationSettings.TransformAxis.X);
            ApplyToUIElementsForAxis(PlatformTransformationSettings.TransformAxis.Y);
            ApplyToUIElementsForAxis(PlatformTransformationSettings.TransformAxis.Z);
        }

        private void ApplyToUIElementsForAxis(PlatformTransformationSettings.TransformAxis axis)
        {
            var element = _axisToUIElements[axis];
            var platformData = platformConfig.GetPlatformData(axis);

            //element.FunctionDropdown.value = (int)platformData.Function; // TODO: Fix This
            element.AmplitudeSlider.value = platformData.Amplitude;
            element.AmplitudeInputField.text = platformData.Amplitude.ToString("F2");
        }

        protected override void SubscribeToUIElements()
        {
            //TODO: Fix Later
            _axisDropdown.onValueChanged.AddListener((value) =>
            {
                var selectedAxis = (PlatformTransformationSettings.TransformAxis)value;
                D("Selected Axis: " + selectedAxis);
                OnUIInteracted();
            });

            _periodSlider.onValueChanged.AddListener((value) =>
            {
                platformConfig.SetPeriod(value);
                OnUIInteracted();
            });

            _resetToDefaultButton.onClick.AddListener(() =>
            {
                D("Reset to Default Button Clicked");
                ApplyDefaultPlatformerData();
                ApplyToUI();
                OnUIInteracted();
            });

            SubscribeToUIElementsForAxis(PlatformTransformationSettings.TransformAxis.X);
            SubscribeToUIElementsForAxis(PlatformTransformationSettings.TransformAxis.Y);
            SubscribeToUIElementsForAxis(PlatformTransformationSettings.TransformAxis.Z);
        }

        private void SubscribeToUIElementsForAxis(PlatformTransformationSettings.TransformAxis axis)
        {
            D($"Subscribing from UI elements for {axis}");
            //TODO: Fix Later
            var element = _axisToUIElements[axis];

            element.FunctionDropdown.onValueChanged.AddListener((value) =>
            {
                var selectedFunction = (PlatformTransformationSettings.TransformFunction)value;
                D($"Selected Function for {axis}: {selectedFunction}");
                OnUIInteracted();
            });

            element.AmplitudeSlider.onValueChanged.AddListener((value) =>
            {
                platformConfig.GetPlatformData(axis).Amplitude = value;
                D($"Set Amplitude for {axis}: {value}");
                OnUIInteracted();
            });
        }

        protected override void UnsubscribeFromUIElements()
        {
            _axisDropdown.onValueChanged.RemoveAllListeners();
            _periodSlider.onValueChanged.RemoveAllListeners();

            UnsubscribeFromUIElementsForAxis(PlatformTransformationSettings.TransformAxis.X);
            UnsubscribeFromUIElementsForAxis(PlatformTransformationSettings.TransformAxis.Y);
            UnsubscribeFromUIElementsForAxis(PlatformTransformationSettings.TransformAxis.Z);
        }

        private void UnsubscribeFromUIElementsForAxis(PlatformTransformationSettings.TransformAxis axis)
        {
            D($"Unsubscribing from UI elements for {axis}");

            var element = _axisToUIElements[axis];

            element.FunctionDropdown.onValueChanged.RemoveAllListeners();
            element.AmplitudeSlider.onValueChanged.RemoveAllListeners();
        }

        protected override void OnUIInteracted()
        {
            ApplyToUI();

            if (OnUIStateChanged != null)
            {
                D("Invoking OnUIStateChanged event for Pattern: " + platformConfig);
                OnUIStateChanged(transformDomain, platformConfig);
            }
        }

        public override void OnEnter()
        {
            D("OnEnter");
            SubscribeToUIElements();
        }

        public override void OnFrame()
        {
            D("OnFrame");
        }

        public override void OnExit()
        {
            D("OnExit");
            UnsubscribeFromUIElements();
        }
    }
}
