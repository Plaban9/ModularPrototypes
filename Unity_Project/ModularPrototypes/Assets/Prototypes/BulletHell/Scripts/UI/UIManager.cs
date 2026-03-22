using ModularPrototypes.BulletHell.Data;
using ModularPrototypes.BulletHell.UI.StateMachine;

using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

using UnityEngine;
using UnityEngine.UI;

namespace ModularPrototypes.BulletHell.UI
{
    public class UIManager : MonoBehaviour
    {
        #region UI Elements        
        [SerializeField] private Image _bulletHellPanelImage;
        [SerializeField] private BulletPattern _startPattern = BulletPattern.RADIAL_BURST;
        [SerializeField] private BulletPattern _currentPattern = BulletPattern.RADIAL_BURST;
        [SerializeField] private TMPro.TextMeshProUGUI _bulletHellPatternNameText;
        [SerializeField] private List<BulletHellData> _bulletHellDataList;
        [SerializeField] private UIStateMachine _uiStateMachine;
        private Dictionary<BulletPattern, BulletHellData> _bulletHellDataDictionary;
        private Dictionary<BulletPattern, Button> _bulletHellPatternButtonsDictionary;
        private Dictionary<BulletPattern, GameObject> _bulletHellPatternPanelsDictionary;

        [Header("Buttons")]
        [SerializeField] private List<Button> _bulletHellButtonList;
        [SerializeField] private Button _panelInteractionButton;

        [Header("Panels")]
        [SerializeField] private List<GameObject> _bulletHellPanelsList;

        [SerializeField] private Animator _panelAnimation;
        #endregion

        [Header("Bullet Hell System")]
        #region Bullet Hell System
        [SerializeField] private BulletSpawner _bulletSpawner;
        #endregion

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            _currentPattern = _startPattern;
            InitializeLists();
            InitializeButtonSubscriptions();
        }

        private void InitializeLists()
        {
            for (int i = 0; i < _bulletHellDataList.Count; i++)
            {
                _bulletHellDataDictionary ??= new Dictionary<BulletPattern, BulletHellData>();

                if (!_bulletHellDataDictionary.ContainsKey(_bulletHellDataList[i].GetBulletPattern()))
                {
                    _bulletHellDataDictionary.Add(_bulletHellDataList[i].GetBulletPattern(), _bulletHellDataList[i]);
                }
            }

            for (int i = 0; i < _bulletHellButtonList.Count; i++)
            {
                _bulletHellPatternButtonsDictionary ??= new Dictionary<BulletPattern, Button>();

                if (_bulletHellButtonList[i].TryGetComponent(out BulletPatternIdentity button))
                {
                    _bulletHellPatternButtonsDictionary.Add(button.GetBulletPattern(), _bulletHellButtonList[i]);
                }
            }

            for (int i = 0; i < _bulletHellPanelsList.Count; i++)
            {
                _bulletHellPatternPanelsDictionary ??= new Dictionary<BulletPattern, GameObject>();

                if (_bulletHellPanelsList[i].TryGetComponent(out BulletPatternIdentity panel))
                {
                    _bulletHellPatternPanelsDictionary.Add(panel.GetBulletPattern(), _bulletHellPanelsList[i].gameObject);
                }
            }

            //_currentPattern = BulletPattern.RADIAL_BURST;
            _bulletHellPatternButtonsDictionary[_currentPattern].interactable = false;
            _bulletHellPatternNameText.text = _bulletHellDataDictionary[_currentPattern].GetName();
            _bulletHellPatternPanelsDictionary[_currentPattern].SetActive(true);

            _uiStateMachine.Initialize(_currentPattern);
        }

        private void InitializeButtonSubscriptions()
        {
            for (int i = 0; i < _bulletHellButtonList.Count; i++)
            {
                if (_bulletHellButtonList[i].TryGetComponent(out BulletPatternIdentity button))
                {
                    BulletPattern capturedPattern = button.GetBulletPattern();
                    _bulletHellButtonList[i].onClick.AddListener(() => OnBulletHellPatternButtonClicked(capturedPattern));
                }
            }

            _panelInteractionButton.onClick.AddListener(() => _panelAnimation.SetTrigger("Animate_Out"));
        }

        public void OnBulletHellPatternButtonClicked(BulletPattern bulletPattern)
        {
            _bulletHellPatternButtonsDictionary[_currentPattern].interactable = true;
            _bulletHellPatternButtonsDictionary[bulletPattern].interactable = false;

            BulletHellData newData = _bulletHellDataDictionary[bulletPattern];

            _bulletHellPatternNameText.text = newData.GetName();

            _bulletHellPatternPanelsDictionary[_currentPattern].SetActive(false);
            _bulletHellPatternPanelsDictionary[bulletPattern].SetActive(true);

            _currentPattern = bulletPattern;

            _bulletSpawner.SetBulletPattern(_currentPattern);

            _uiStateMachine.TransitionTo(_currentPattern);
            SubscribeToUIEvents(_currentPattern);
            _bulletSpawner.ApplyBulletHellPatternSettings(_uiStateMachine.CurrentState.GetBulletHellPatternData());
        }

        void Update()
        {
            var lerpedColor = Color.Lerp(_bulletHellPanelImage.color, _bulletHellDataDictionary[_currentPattern].GetBackgroundColor(), 0.95f * Time.deltaTime);
            _bulletHellPanelImage.color = lerpedColor;
        }

        private void OnUIStateMachineStateChanged(BulletPattern bulletPattern, BulletHellPatternData bulletHellPatternData)
        {
            if (bulletPattern != _currentPattern)
                return;

            _bulletSpawner.ApplyBulletHellPatternSettings(bulletHellPatternData);
        }

        private void SubscribeToUIEvents(BulletPattern bulletPattern)
        {
            if (_uiStateMachine != null)
            {
                _uiStateMachine.OnUIStateChanged += OnUIStateMachineStateChanged;
            }
        }

        private void UnsubscribeFromUIEvents(BulletPattern bulletPattern)
        {
            if (_uiStateMachine != null)
            {
                _uiStateMachine.OnUIStateChanged -= OnUIStateMachineStateChanged;
            }
        }

        private void OnEnable()
        {
            SubscribeToUIEvents(_currentPattern);
        }

        private void OnDisable()
        {
            UnsubscribeFromUIEvents(_currentPattern);
        }

        private void D(string message, bool isError = false)
        {
            DebugUtils.DebugInfo.Print($"<<UIManager>> {message}", isError ? DebugUtils.DebugConstants.ERROR : DebugUtils.DebugConstants.INFO);
        }
    }
}