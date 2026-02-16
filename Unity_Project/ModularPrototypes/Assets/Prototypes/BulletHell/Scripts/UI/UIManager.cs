using ModularPrototypes.BulletHell.Data;

using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace ModularPrototypes.BulletHell.UI
{
    public class UIManager : MonoBehaviour
    {
        #region UI Elements        
        [SerializeField] private Image _bulletHellPanelImage;
        [SerializeField] private BulletPattern _currentPattern = BulletPattern.RADIAL_BURST;
        [SerializeField] private TMPro.TextMeshProUGUI _bulletHellPatternNameText;
        [SerializeField] private List<BulletHellData> _bulletHellDataList;
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
        [SerializeField] private List<BulletHellConfig> _bulletHellPatternDefaultConfigList;
        [SerializeField] private Dictionary<BulletPattern, BulletHellPatternData> _bulletHellPatternInstanceConfigDictionary;
        #endregion

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
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

            for (int i = 0; i < _bulletHellPatternDefaultConfigList.Count; i++)
            {
                _bulletHellPatternInstanceConfigDictionary ??= new Dictionary<BulletPattern, BulletHellPatternData>();

                if (!_bulletHellPatternInstanceConfigDictionary.ContainsKey(_bulletHellPatternDefaultConfigList[i].GetBulletPattern()))
                {
                    var instanceConfig = ScriptableObject.CreateInstance<BulletHellConfig>();
                    instanceConfig.SetBulletPatternData(_bulletHellPatternDefaultConfigList[i].GetBulletHellPatternData());

                    _bulletHellPatternInstanceConfigDictionary.Add(_bulletHellPatternDefaultConfigList[i].GetBulletPattern(), instanceConfig.GetBulletHellPatternData());
                }
            }

            _currentPattern = BulletPattern.RADIAL_BURST;
            _bulletHellPatternButtonsDictionary[_currentPattern].interactable = false;
            _bulletHellPatternNameText.text = _bulletHellDataDictionary[_currentPattern].GetName();
            _bulletHellPatternPanelsDictionary[_currentPattern].SetActive(true);
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
        }

        void Update()
        {
            var lerpedColor = Color.Lerp(_bulletHellPanelImage.color, _bulletHellDataDictionary[_currentPattern].GetBackgroundColor(), 0.95f * Time.deltaTime);
            _bulletHellPanelImage.color = lerpedColor;
        }
    }
}