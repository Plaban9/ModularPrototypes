using NUnit.Framework;

using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace ModularPrototypes.Platformer.Settings.UI
{
    public class UIManager : MonoBehaviour
    {
        #region UI Elements
        [SerializeField] private Image _platformerPanelImage;
        [SerializeField] private PlatformSettings _startSetting;
        [SerializeField] private PlatformSettings _currentSetting;
        [SerializeField] private TMPro.TextMeshProUGUI _settingsNameText;

        [SerializeField] private UIStateMachine _uiStateMachine;
        private Dictionary<PlatformSettings, Button> _settingsButtonsDictionary;
        private Dictionary<PlatformSettings, GameObject> _settingsPanelsDictionary;

        [Header("Buttons")]
        [SerializeField] private List<Button> _settingsButtonList;
        [SerializeField] private Button _panelInteractionButton;
        [SerializeField] private List<GameObject> _settingsPanelsList;
        [SerializeField] private Animator _panelAnimation;
        #endregion

        void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            _currentSetting = _startSetting;
            InitializeLists();
            InitializeButtonSubscriptions();
        }

        private void InitializeLists()
        {
            _settingsButtonsDictionary = new Dictionary<PlatformSettings, Button>();
            _settingsPanelsDictionary = new Dictionary<PlatformSettings, GameObject>();

            for (int i = 0; i < _settingsButtonList.Count; i++)
            {
                if (_settingsButtonList[i].TryGetComponent(out SettingsIdentity buttonIdentity))
                {
                    if (!_settingsButtonsDictionary.ContainsKey(buttonIdentity.GetIdentity()))
                    {
                        _settingsButtonsDictionary.Add(buttonIdentity.GetIdentity(), _settingsButtonList[i]);
                    }
                }
            }

            for (int i = 0; i < _settingsPanelsList.Count; i++)
            {
                if (_settingsPanelsList[i].TryGetComponent(out SettingsIdentity buttonIdentity))
                {
                    if (!_settingsPanelsDictionary.ContainsKey(buttonIdentity.GetIdentity()))
                    {
                        _settingsPanelsDictionary.Add(buttonIdentity.GetIdentity(), _settingsPanelsList[i]);
                    }
                }
            }

            _uiStateMachine.Initialize(_currentSetting);

            _settingsButtonsDictionary[_currentSetting].interactable = false;
            _settingsNameText.text = _uiStateMachine.CurrentState.GetStateName();
            _settingsPanelsDictionary[_currentSetting].SetActive(true);
        }

        private void InitializeButtonSubscriptions()
        {
            for (int i = 0; i < _settingsButtonList.Count; i++)
            {
                if (_settingsButtonList[i].TryGetComponent(out SettingsIdentity buttonIdentity))
                {
                    var domain = buttonIdentity.GetIdentity();
                    if (_settingsButtonsDictionary.ContainsKey(domain))
                    {
                        _settingsButtonsDictionary[domain].onClick.AddListener(() => OnSettingsButtonClicked(domain));
                    }
                }
            }

            _panelInteractionButton.onClick.AddListener(() => _panelAnimation.SetTrigger("Animate"));
        }

        private void OnSettingsButtonClicked(PlatformSettings platformSetting)
        {
            _settingsButtonsDictionary[_currentSetting].interactable = true;
            _settingsButtonsDictionary[platformSetting].interactable = false;

            _settingsNameText.text = _uiStateMachine.GetState(platformSetting).GetStateName();

            _settingsPanelsDictionary[_currentSetting].SetActive(false);
            _settingsPanelsDictionary[platformSetting].SetActive(true);

            _currentSetting = platformSetting;
            _uiStateMachine.TransitionTo(platformSetting);
        }

        void Update()
        {
            var lerpedColor = Color.Lerp(_platformerPanelImage.color, _uiStateMachine.CurrentState.GetSettingsData().GetBackgroundColor(), 0.95f * Time.deltaTime);
            _platformerPanelImage.color = lerpedColor;
        }

        private void D(string message, bool isError = false)
        {
            DebugUtils.DebugInfo.Print($"<<UIManager>> {message}", isError ? DebugUtils.DebugConstants.ERROR : DebugUtils.DebugConstants.INFO);
        }
    }
}
