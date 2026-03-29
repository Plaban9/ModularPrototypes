using ModularPrototypes.Platformer.Measurements;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace ModularPrototypes.Platformer.Settings.UI
{
    public class UIState_MeasurementSetting : UIState
    {
        #region UI Elements
        [Header("Measurement UI Elements")]
        [Header("Central Axis")]
        [SerializeField] private Slider _minWidth;
        [SerializeField] private TMP_InputField _minWidthInputField;

        [SerializeField] private Slider _maxWidth;
        [SerializeField] private TMP_InputField _maxWidthInputField;

        [SerializeField] private Slider _normalized;
        [SerializeField] private TMP_InputField _normalizedInputField;

        [Header("Bounds")]
        [SerializeField] private Slider _boundsMinWidth;
        [SerializeField] private TMP_InputField _boundsMinWidthInputField;

        [SerializeField] private Slider _boundsMaxWidth;
        [SerializeField] private TMP_InputField _boundsMaxWidthInputField;

        [SerializeField] private Slider _separationWidth;
        [SerializeField] private TMP_InputField _separationWidthInputField;

        [SerializeField] private Slider _boundsNormalized;
        [SerializeField] private TMP_InputField _boundsNormalizedInputField;

        [Header("Extras")]
        [SerializeField] private Toggle _showReferenceToggle;
        [SerializeField] private Toggle _hideMesh;
        [SerializeField] private Button _resetToDefaultButton;
        #endregion

        [Header("Measurement")]
        [SerializeField] private MeasurementData _measurementDeafaultData;
        [SerializeField] private MeasurementData _measurementData;
        [SerializeField] private Measurement _measurement;

        public override event UIObserver OnUIStateChanged;

        public override void Initialize()
        {
            D("OnInitialize");
            stateName = string.IsNullOrEmpty(stateName) ? "Measurement Settings" : stateName;
            _measurementData = Instantiate(_measurementDeafaultData);
            _measurement.Initialize(_measurementData);
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
            _measurementData.SetConfiguration(_measurementDeafaultData);
            _measurement.ApplyConfigurations(_measurementData);
            ApplyToUI();
        }

        protected override void ApplyToUI()
        {
            _minWidth.value = _measurementData.GetMinMaxAxis().min;
            _minWidthInputField.text = _measurementData.GetMinMaxAxis().min.ToString("F2");

            _maxWidth.value = _measurementData.GetMinMaxAxis().max;
            _maxWidthInputField.text = _measurementData.GetMinMaxAxis().max.ToString("F2");

            _normalized.value = _measurementData.GetNormalizedAxisSize();
            _normalizedInputField.text = _measurementData.GetNormalizedAxisSize().ToString("F2");

            _boundsMinWidth.value = _measurementData.GetMinMaxBoundsLength().min;
            _boundsMinWidthInputField.text = _measurementData.GetMinMaxBoundsLength().min.ToString("F2");

            _boundsMaxWidth.value = _measurementData.GetMinMaxBoundsLength().max;
            _boundsMaxWidthInputField.text = _measurementData.GetMinMaxBoundsLength().max.ToString("F2");

            _separationWidth.value = _measurementData.GetBoundsGap();
            _separationWidthInputField.text = _measurementData.GetBoundsGap().ToString("F2");

            _boundsNormalized.value = _measurementData.GetNormalizedBoundsSize();
            _boundsNormalizedInputField.text = _measurementData.GetNormalizedBoundsSize().ToString("F2");

            _showReferenceToggle.isOn = _measurementData.GetShowReferenceBounds();

            if (_hideMesh != null)
            {
                _hideMesh.isOn = _measurementData.GetHideMesh();
            }
        }

        protected override void OnUIInteracted()
        {
            ApplyToUI();

            _measurement.ApplyConfigurations(_measurementData);
        }

        protected override void SubscribeToUIElements()
        {
            _minWidth.onValueChanged.AddListener((value) =>
            {
                var bound = _measurementData.GetMinMaxAxis();
                bound.min = value;
                _measurementData.SetMinMaxAxis(bound);
                OnUIInteracted();
            });

            _maxWidth.onValueChanged.AddListener((value) =>
            {
                var bound = _measurementData.GetMinMaxAxis();
                bound.max = value;
                _measurementData.SetMinMaxAxis(bound);
                OnUIInteracted();
            });

            _normalized.onValueChanged.AddListener((value) =>
            {
                _measurementData.SetNormalizedAxisSize(value);
                OnUIInteracted();
            });

            _boundsMinWidth.onValueChanged.AddListener((value) =>
            {
                var bound = _measurementData.GetMinMaxBoundsLength();
                bound.min = value;
                _measurementData.SetMinMaxBoundsLength(bound);
                OnUIInteracted();
            });

            _boundsMaxWidth.onValueChanged.AddListener((value) =>
            {
                var bound = _measurementData.GetMinMaxBoundsLength();
                bound.max = value;
                _measurementData.SetMinMaxBoundsLength(bound);
                OnUIInteracted();
            });

            _separationWidth.onValueChanged.AddListener((value) =>
            {
                _measurementData.SetBoundsGap(value);
                OnUIInteracted();
            });

            _boundsNormalized.onValueChanged.AddListener((value) =>
            {
                _measurementData.SetNormalizedBoundsSize(value);
                OnUIInteracted();
            });

            _showReferenceToggle.onValueChanged.AddListener((value) =>
            {
                _measurementData.SetShowReferenceBounds(value);
                OnUIInteracted();
            });

            if (_hideMesh != null)
            {
                _hideMesh.onValueChanged.AddListener((value) =>
                {
                    _measurementData.SetHideMesh(value);
                    OnUIInteracted();
                });
            }

            _resetToDefaultButton.onClick.AddListener(() =>
            {
                ApplyDefaultSettingsData();
                OnUIInteracted();
            });
        }

        protected override void UnsubscribeFromUIElements()
        {
            _minWidth.onValueChanged.RemoveAllListeners();
            _maxWidth.onValueChanged.RemoveAllListeners();
            _normalized.onValueChanged.RemoveAllListeners();
            _boundsMinWidth.onValueChanged.RemoveAllListeners();
            _boundsMaxWidth.onValueChanged.RemoveAllListeners();
            _separationWidth.onValueChanged.RemoveAllListeners();
            _boundsNormalized.onValueChanged.RemoveAllListeners();
            _showReferenceToggle.onValueChanged.RemoveAllListeners();

            if (_hideMesh != null)
            {
                _hideMesh.onValueChanged.RemoveAllListeners();
            }

            _resetToDefaultButton.onClick.RemoveAllListeners();
        }
    }
}
