using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace ModularPrototypes.BulletHell.UI.StateMachine.States
{
    public class UIState_RadialBurst : UIState
    {
        [Header("UI Elements")]
        [SerializeField] private Slider _bulletAmountSlider;
        [SerializeField] private Slider _offsetSlider;
        [SerializeField] private Slider _startAngleSlider;
        [SerializeField] private Slider _endAngleSlider;
        [SerializeField] private Slider _shootIntervalSlider;
        [SerializeField] private Slider _bulletSpeedSlider;
        [SerializeField] private Slider _bulletLifeSlider;
        [SerializeField] private Toggle _trailToggle;

        [SerializeField] private TMP_InputField _bulletAmountInputField;
        [SerializeField] private TMP_InputField _offsetInputField;
        [SerializeField] private TMP_InputField _startAngleInputField;
        [SerializeField] private TMP_InputField _endAngleInputField;
        [SerializeField] private TMP_InputField _shootIntervalInputField;
        [SerializeField] private TMP_InputField _bulletSpeedInputField;
        [SerializeField] private TMP_InputField _bulletLifeInputField;

        [SerializeField] private Button _resetToDefaultButton;

        public override event UIObserver OnUIStateChanged;

        public override void Initialize()
        {
            stateName = string.IsNullOrEmpty(stateName) ? "Radial Burst" : stateName;
            bulletPattern = BulletPattern.RADIAL_BURST;
            ApplyDefaultBulletHellPatternData();
            ApplyToUI();

            D("Initialized");
        }

        protected override void ApplyToUI()
        {
            _bulletAmountSlider.value = bulletHellPatternData.BulletAmount;
            _bulletAmountInputField.text = bulletHellPatternData.BulletAmount.ToString();

            _offsetSlider.value = bulletHellPatternData.OffsetAngle;
            _offsetInputField.text = bulletHellPatternData.OffsetAngle.ToString();

            _startAngleSlider.value = bulletHellPatternData.StartAngle;
            _startAngleInputField.text = bulletHellPatternData.StartAngle.ToString();

            _endAngleSlider.value = bulletHellPatternData.EndAngle;
            _endAngleInputField.text = bulletHellPatternData.EndAngle.ToString();

            _shootIntervalSlider.value = bulletHellPatternData.ShootInterval;
            _shootIntervalInputField.text = bulletHellPatternData.ShootInterval.ToString("F2");

            _bulletSpeedSlider.value = bulletHellPatternData.BulletSpeed;
            _bulletSpeedInputField.text = bulletHellPatternData.BulletSpeed.ToString("F2");

            _bulletLifeSlider.value = bulletHellPatternData.BulletLifeInSeconds;
            _bulletLifeInputField.text = bulletHellPatternData.BulletLifeInSeconds.ToString("F2");

            _trailToggle.isOn = bulletHellPatternData.EnableTrail;
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

        protected override void OnUIInteracted()
        {
            ApplyToUI();

            if (OnUIStateChanged != null)
            {
                D("Invoking OnUIStateChanged event for Pattern: " + bulletPattern);
                OnUIStateChanged(bulletPattern, bulletHellPatternData);
            }
        }

        protected override void SubscribeToUIElements()
        {
            _bulletAmountSlider.onValueChanged.AddListener((value) =>
            {
                bulletHellPatternData.BulletAmount = (int)value;
                OnUIInteracted();
            });

            _offsetSlider.onValueChanged.AddListener((value) =>
            {
                bulletHellPatternData.OffsetAngle = (int)value;
                OnUIInteracted();
            });

            _startAngleSlider.onValueChanged.AddListener((value) =>
            {
                bulletHellPatternData.StartAngle = (int)value;
                OnUIInteracted();
            });

            _endAngleSlider.onValueChanged.AddListener((value) =>
            {
                bulletHellPatternData.EndAngle = (int)value;
                OnUIInteracted();
            });

            _shootIntervalSlider.onValueChanged.AddListener((value) =>
            {
                bulletHellPatternData.ShootInterval = value;
                OnUIInteracted();
            });

            _bulletSpeedSlider.onValueChanged.AddListener((value) =>
            {
                bulletHellPatternData.BulletSpeed = value;
                OnUIInteracted();
            });

            _bulletLifeSlider.onValueChanged.AddListener((value) =>
            {
                bulletHellPatternData.BulletLifeInSeconds = value;
                OnUIInteracted();
            });

            _trailToggle.onValueChanged.AddListener((value) =>
            {
                bulletHellPatternData.EnableTrail = value;
                OnUIInteracted();
            });
            
            _resetToDefaultButton.onClick.AddListener(() =>
            {
                ApplyDefaultBulletHellPatternData();
                ApplyToUI();
            });
        }

        protected override void UnsubscribeFromUIElements()
        {
            _bulletAmountSlider.onValueChanged.RemoveAllListeners();
            _offsetSlider.onValueChanged.RemoveAllListeners();
            _startAngleSlider.onValueChanged.RemoveAllListeners();
            _endAngleSlider.onValueChanged.RemoveAllListeners();
            _shootIntervalSlider.onValueChanged.RemoveAllListeners();
            _bulletSpeedSlider.onValueChanged.RemoveAllListeners();
            _bulletLifeSlider.onValueChanged.RemoveAllListeners();
            _trailToggle.onValueChanged.RemoveAllListeners();
            _resetToDefaultButton.onClick.RemoveAllListeners();
        }

        public override void OnFrame()
        {
            D("OnFrame");
        }
    }
}
