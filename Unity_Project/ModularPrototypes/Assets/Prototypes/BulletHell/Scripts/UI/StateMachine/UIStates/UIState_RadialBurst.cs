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
        [SerializeField] private Toggle _extraBulletToggle;
        [SerializeField] private Toggle _extraTrailToggle;

        [SerializeField] private Button _resetToDefaultButton;

        public override void Initialize()
        {
            stateName = string.IsNullOrEmpty(stateName) ? "Radial Burst" : stateName;
            bulletPattern = BulletPattern.RADIAL_BURST;
            ApplyDefaultBulletHellPatternData();
            ApplyToUI();

            D("Initialized");
        }

        public override void ApplyToUI()
        {
            _bulletAmountSlider.value = bulletHellPatternData.BulletAmount;
            _offsetSlider.value = bulletHellPatternData.OffsetAngle;
            _startAngleSlider.value = bulletHellPatternData.StartAngle;
            _endAngleSlider.value = bulletHellPatternData.EndAngle;
            _shootIntervalSlider.value = bulletHellPatternData.ShootInterval;
            _bulletSpeedSlider.value = bulletHellPatternData.BulletSpeed;
            _bulletLifeSlider.value = bulletHellPatternData.BulletLifeInSeconds;
            _extraBulletToggle.isOn = bulletHellPatternData.ExtraBullet;
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

        private void SubscribeToUIElements()
        {
            _bulletAmountSlider.onValueChanged.AddListener((value) => bulletHellPatternData.BulletAmount = (int)value);
            _offsetSlider.onValueChanged.AddListener((value) => bulletHellPatternData.OffsetAngle = (int)value);
            _startAngleSlider.onValueChanged.AddListener((value) => bulletHellPatternData.StartAngle = (int)value);
            _endAngleSlider.onValueChanged.AddListener((value) => bulletHellPatternData.EndAngle = (int)value);
            _shootIntervalSlider.onValueChanged.AddListener((value) => bulletHellPatternData.ShootInterval = value);
            _bulletSpeedSlider.onValueChanged.AddListener((value) => bulletHellPatternData.BulletSpeed = value);
            _bulletLifeSlider.onValueChanged.AddListener((value) => bulletHellPatternData.BulletLifeInSeconds = value);
            _extraBulletToggle.onValueChanged.AddListener((value) => bulletHellPatternData.ExtraBullet = value);
            _extraTrailToggle.onValueChanged.AddListener((value) => bulletHellPatternData.EnableTrail = value);
            
            _resetToDefaultButton.onClick.AddListener(() =>
            {
                ApplyDefaultBulletHellPatternData();
                ApplyToUI();
            });
        }

        private void UnsubscribeToUIElements()
        {
            _bulletAmountSlider.onValueChanged.RemoveAllListeners();
            _offsetSlider.onValueChanged.RemoveAllListeners();
            _startAngleSlider.onValueChanged.RemoveAllListeners();
            _endAngleSlider.onValueChanged.RemoveAllListeners();
            _shootIntervalSlider.onValueChanged.RemoveAllListeners();
            _bulletSpeedSlider.onValueChanged.RemoveAllListeners();
            _bulletLifeSlider.onValueChanged.RemoveAllListeners();
            _extraBulletToggle.onValueChanged.RemoveAllListeners();
            _extraTrailToggle.onValueChanged.RemoveAllListeners();
            _resetToDefaultButton.onClick.RemoveAllListeners();
        }

        public override void OnFrame()
        {
            D("OnFrame");
        }
    }
}
