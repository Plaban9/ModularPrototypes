using ModularPrototypes.Platformer.Data;
using ModularPrototypes.Platformer.UI.StateMachine;

using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;


namespace ModularPrototypes.Platformer.UI
{
    public class UIManager : MonoBehaviour
    {
        #region UI Elements        
        [SerializeField] private Image _platformerPanelImage;
        [SerializeField] private PlatformTransformationSettings.TransformDomain _startDomain = PlatformTransformationSettings.TransformDomain.TRANSLATION;
        [SerializeField] private PlatformTransformationSettings.TransformDomain _currentDomain = PlatformTransformationSettings.TransformDomain.TRANSLATION;
        [SerializeField] private TMPro.TextMeshProUGUI _platformerNameText;
        [SerializeField] private UIStateMachine _uiStateMachine;
        [SerializeField] private List<PlatformConfig> _platformerConfigList;
        private Dictionary<PlatformTransformationSettings.TransformDomain, Button> _platformerButtonsDictionary;
        private Dictionary<PlatformTransformationSettings.TransformDomain, GameObject> _platformerPanelsDictionary;
        //private Dictionary<PlatformTransformationSettings.TransformDomain, PlatformConfig> _platformerConfigDictionary;

        [Header("Buttons")]
        [SerializeField] private List<Button> _platformerButtonList;
        [SerializeField] private Button _panelInteractionButton;

        [Header("Panels")]
        [SerializeField] private List<GameObject> _platformerPanelsList;

        [SerializeField] private Animator _panelAnimation;
        #endregion

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            _currentDomain = _startDomain;

            InitializeLists();
            InitializeButtonSubscriptions();
        }

        private void InitializeLists()
        {
            //for (int i = 0; i < _platformerConfigList.Count; i++)
            //{
            //    _platformerConfigDictionary ??= new Dictionary<PlatformTransformationSettings.TransformDomain, PlatformConfig>();

            //    if (!_platformerConfigDictionary.ContainsKey(_platformerConfigList[i].GetTransformDomain()))
            //    {
            //        _platformerConfigDictionary.Add(_platformerConfigList[i].GetTransformDomain(), _platformerConfigList[i]);
            //    }
            //}

            for (int i = 0; i < _platformerButtonList.Count; i++)
            {
                if (_platformerButtonList[i].TryGetComponent(out PlatformIdentity platformIdentity))
                {
                    var domain = platformIdentity.GetTransformDomain();
                    _platformerButtonsDictionary ??= new Dictionary<PlatformTransformationSettings.TransformDomain, Button>();
                    if (!_platformerButtonsDictionary.ContainsKey(domain))
                    {
                        _platformerButtonsDictionary.Add(domain, _platformerButtonList[i]);
                    }
                }
            }

            for (int i = 0; i < _platformerPanelsList.Count; i++)
            {
                if (_platformerPanelsList[i].TryGetComponent(out PlatformIdentity platformIdentity))
                {
                    var domain = platformIdentity.GetTransformDomain();
                    _platformerPanelsDictionary ??= new Dictionary<PlatformTransformationSettings.TransformDomain, GameObject>();
                    if (!_platformerPanelsDictionary.ContainsKey(domain))
                    {
                        _platformerPanelsDictionary.Add(domain, _platformerPanelsList[i]);
                    }
                }

            }

            _uiStateMachine.Initialize(_currentDomain);

            _platformerButtonsDictionary[_currentDomain].interactable = false;
            _platformerNameText.text = _uiStateMachine.CurrentState.name;
            _platformerPanelsDictionary[_currentDomain].SetActive(true);            
        }

        private void InitializeButtonSubscriptions()
        {
            for (int i = 0; i < _platformerButtonList.Count; i++)
            {
                if (_platformerButtonList[i].TryGetComponent(out PlatformIdentity platformIdentity))
                {
                    var domain = platformIdentity.GetTransformDomain();
                    _platformerButtonList[i].onClick.AddListener(() => OnPlatformerButtonClicked(domain));
                }
            }

            _panelInteractionButton.onClick.AddListener(() => _panelAnimation.SetTrigger("Animate"));
        }

        public void OnPlatformerButtonClicked(PlatformTransformationSettings.TransformDomain domain)
        {
            _platformerButtonsDictionary[_currentDomain].interactable = true;
            _platformerButtonsDictionary[domain].interactable = false;

            var newState = _uiStateMachine.GetState(domain);

            _platformerNameText.text = newState.GetPlatformConfig().GetName();

            _platformerPanelsDictionary[_currentDomain].SetActive(false);   
            _platformerPanelsDictionary[domain].SetActive(true);

            _currentDomain = domain;
            _uiStateMachine.TransitionTo(domain);

            SubscribeToUIEvents(domain);
        }

        void Update()
        {
            var lerpedColor = Color.Lerp(_platformerPanelImage.color, _uiStateMachine.CurrentState.GetPlatformConfig().GetBackgroundColor(), 0.95f * Time.deltaTime);
            _platformerPanelImage.color = lerpedColor;
        }

        private void OnUIStateMachineStateChanged(PlatformTransformationSettings.TransformDomain domain, PlatformConfig platformConfig)
        {
            if (domain != _currentDomain)
               return;

            //_bulletSpawner.ApplyBulletHellPatternSettings(bulletHellPatternData);
        }

        private void SubscribeToUIEvents(PlatformTransformationSettings.TransformDomain domain)
        {
            if (_uiStateMachine != null)
            {
                _uiStateMachine.OnUIStateChanged += OnUIStateMachineStateChanged;
            }
        }

        private void UnsubscribeFromUIEvents(PlatformTransformationSettings.TransformDomain domain)
        {
            if (_uiStateMachine != null)
            {
                _uiStateMachine.OnUIStateChanged -= OnUIStateMachineStateChanged;
            }
        }

        private void OnEnable()
        {
            SubscribeToUIEvents(_currentDomain);
        }

        private void OnDisable()
        {
            UnsubscribeFromUIEvents(_currentDomain);
        }

        private void D(string message, bool isError = false)
        {
            DebugUtils.DebugInfo.Print($"<<UIManager>> {message}", isError ? DebugUtils.DebugConstants.ERROR : DebugUtils.DebugConstants.INFO);
        }
    }
}