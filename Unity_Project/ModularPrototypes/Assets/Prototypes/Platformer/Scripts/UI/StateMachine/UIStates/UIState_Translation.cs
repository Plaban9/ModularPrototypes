using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace ModularPrototypes.Platformer.UI.StateMachine.States
{
    public class UIState_Translation : UIState
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
            stateName = string.IsNullOrEmpty(stateName) ? "Translation" : stateName;
            transformDomain = PlatformTransformationSettings.TransformDomain.TRANSLATION;

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
            _axisDropdown.value = (int)platformConfig.GetTransformDimension(); // TODO: Change Later
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

            element.FunctionDropdown.value = (int)platformData.Function;
            element.AmplitudeSlider.value = platformData.Amplitude;
            element.AmplitudeInputField.text = platformData.Amplitude.ToString("F2");
            element.ModuloToggle.isOn = platformData.Modulo;
            element.NegateToggle.isOn = platformData.Negate;
        }

        protected override void SubscribeToUIElements()
        {
            _axisDropdown.onValueChanged.AddListener((value) =>
            {
                var selectedAxis = (PlatformTransformationSettings.TransformDimension)value;
                D("Selected Axis: " + selectedAxis);

                if (selectedAxis == PlatformTransformationSettings.TransformDimension.XY
                || selectedAxis == PlatformTransformationSettings.TransformDimension.YZ
                || selectedAxis == PlatformTransformationSettings.TransformDimension.XZ)
                {
                    _axisDropdown.captionText.text = selectedAxis.ToString();
                }
                else if (selectedAxis == PlatformTransformationSettings.TransformDimension.ALL_DIMENSIONS)
                {
                    _axisDropdown.captionText.text = "XYZ";
                }

                platformConfig.SetTransformDimension(selectedAxis);

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

            var element = _axisToUIElements[axis];

            element.FunctionDropdown.onValueChanged.AddListener((value) =>
            {
                var selectedFunction = (PlatformTransformationSettings.TransformFunction)value;
                D($"Selected Function for {axis}: {selectedFunction}");
                platformConfig.GetPlatformData(axis).Function = selectedFunction;

                OnUIInteracted();
            });

            element.AmplitudeSlider.onValueChanged.AddListener((value) =>
            {
                platformConfig.GetPlatformData(axis).Amplitude = value;
                D($"Set Amplitude for {axis}: {value}");
                OnUIInteracted();
            });

            element.ModuloToggle.onValueChanged.AddListener((value) =>
            {
                platformConfig.GetPlatformData(axis).Modulo = value;
                D($"Set Modulo for {axis}: {value}");
                OnUIInteracted();
            });

            element.NegateToggle.onValueChanged.AddListener((value) =>
            {
                platformConfig.GetPlatformData(axis).Negate = value;
                D($"Set Negate for {axis}: {value}");
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
            element.ModuloToggle.onValueChanged.RemoveAllListeners();
            element.NegateToggle.onValueChanged.RemoveAllListeners();
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
