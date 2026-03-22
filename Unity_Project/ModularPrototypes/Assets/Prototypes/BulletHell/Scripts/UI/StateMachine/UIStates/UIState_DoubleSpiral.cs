using UnityEngine;
using UnityEngine.UI;

namespace ModularPrototypes.BulletHell.UI.StateMachine.States
{
    public class UIState_DoubleSpiral : UIState
    {
        [Header("UI Elements")]
        [SerializeField] private Slider _deltaAngleSlider;
        [SerializeField] private Slider _shootIntervalSlider;
        [SerializeField] private Slider _bulletSpeedSlider;
        [SerializeField] private Slider _bulletLifeSlider;
        [SerializeField] private Toggle _extraTrailToggle;

        [SerializeField] private Button _resetToDefaultButton;

        public override void Initialize()
        {
            stateName = string.IsNullOrEmpty(stateName) ? "Double Spiral" : stateName;
            bulletPattern = BulletPattern.DOUBLE_SPIRAL;
            ApplyDefaultBulletHellPatternData();
            ApplyToUI();

            D("Initialized");
        }

        public override void ApplyToUI()
        {
            _deltaAngleSlider.value = bulletHellPatternData.DeltaAngle;
            _shootIntervalSlider.value = bulletHellPatternData.ShootInterval;
            _bulletSpeedSlider.value = bulletHellPatternData.BulletSpeed;
            _bulletLifeSlider.value = bulletHellPatternData.BulletLifeInSeconds;
            _extraTrailToggle.isOn = bulletHellPatternData.EnableTrail;
        }

        public override void OnEnter()
        {
            D("OnEnter");
            SubscribeToUIElements();
        }

        public override void OnExit()
        {
            D("OnExit");
            UnsubscribeToUIElements();
        }

        public override void OnFrame()
        {
            D("OnFrame");
        }

        private void SubscribeToUIElements()
        {
            _deltaAngleSlider.onValueChanged.AddListener((value) => bulletHellPatternData.DeltaAngle = (int)value);
            _shootIntervalSlider.onValueChanged.AddListener((value) => bulletHellPatternData.ShootInterval = value);
            _bulletSpeedSlider.onValueChanged.AddListener((value) => bulletHellPatternData.BulletSpeed = value);
            _bulletLifeSlider.onValueChanged.AddListener((value) => bulletHellPatternData.BulletLifeInSeconds = value);
            _extraTrailToggle.onValueChanged.AddListener((value) => bulletHellPatternData.EnableTrail = value);

            _resetToDefaultButton.onClick.AddListener(() =>
            {
                ApplyDefaultBulletHellPatternData();
                ApplyToUI();
            });
        }

        private void UnsubscribeToUIElements()
        {
            _deltaAngleSlider.onValueChanged.RemoveAllListeners();
            _shootIntervalSlider.onValueChanged.RemoveAllListeners();
            _bulletSpeedSlider.onValueChanged.RemoveAllListeners();
            _bulletLifeSlider.onValueChanged.RemoveAllListeners();
            _extraTrailToggle.onValueChanged.RemoveAllListeners();
            _resetToDefaultButton.onClick.RemoveAllListeners();
        }
    }
}
