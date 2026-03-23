using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace ModularPrototypes.BulletHell.UI.StateMachine.States
{
    public class UIState_Spiral : UIState
    {
        [Header("UI Elements")]
        [SerializeField] private Slider _deltaAngleSlider;
        [SerializeField] private Slider _shootIntervalSlider;
        [SerializeField] private Slider _bulletSpeedSlider;
        [SerializeField] private Slider _bulletLifeSlider;
        [SerializeField] private Toggle _trailToggle;

        [SerializeField] private TMP_InputField _deltaInputField;
        [SerializeField] private TMP_InputField _shootIntervalInputField;
        [SerializeField] private TMP_InputField _bulletSpeedInputField;
        [SerializeField] private TMP_InputField _bulletLifeInputField;

        [SerializeField] private Button _resetToDefaultButton;

        public override event UIObserver OnUIStateChanged;

        public override void Initialize()
        {
            stateName = string.IsNullOrEmpty(stateName) ? "Spiral" : stateName;
            bulletPattern = BulletPattern.SPIRAL;

            ApplyDefaultBulletHellPatternData();
            ApplyToUI();

            D("Initialized");
        }

        protected override void ApplyToUI()
        {
            _deltaAngleSlider.value = bulletHellPatternData.DeltaAngle;
            _deltaInputField.text = bulletHellPatternData.DeltaAngle.ToString();

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

        public override void OnFrame()
        {
            D("OnFrame");
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
            _deltaAngleSlider.onValueChanged.AddListener((value) =>
            {
                bulletHellPatternData.DeltaAngle = (int)value;
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
                OnUIInteracted();
            });
        }

        protected override void UnsubscribeFromUIElements()
        {
            _deltaAngleSlider.onValueChanged.RemoveAllListeners();
            _shootIntervalSlider.onValueChanged.RemoveAllListeners();
            _bulletSpeedSlider.onValueChanged.RemoveAllListeners();
            _bulletLifeSlider.onValueChanged.RemoveAllListeners();
            _trailToggle.onValueChanged.RemoveAllListeners();
            _resetToDefaultButton.onClick.RemoveAllListeners();
        }
    }
}
