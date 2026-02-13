using ModularPrototypes.Scene;

using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace ModularPrototypes.Manager
{
    public class UIManager : MonoBehaviour
    {
        // Match the sequence in Dropdown list
        [SerializeField]
        private static readonly List<string> _availalbleScenes = new()
        {
            SceneModules.BulletHell_Prototype,
            SceneModules.Platformer_Prototype,
            SceneModules.PlatformWithProjectiles_Prototype,
        };

        [SerializeField] private Button _sceneSwitchButton;
        [SerializeField] private SceneLoader _sceneLoader;
        [SerializeField] private int _currentSceneIndex = 0;

        [SerializeField] Color _newSceneColor = new Color(0.2f, 0.8f, 0.2f);
        [SerializeField] Color _defaultSceneColor = new Color(1f, 1f, 1f);

        [SerializeField] private Button _panelCollapseButton;
        [SerializeField] private Animator _panelAnimator;


        private void Awake()
        {
            _sceneLoader = FindFirstObjectByType<SceneLoader>();
        }

        private void Start()
        {
            OnSceneSwitchButtonPressed();
            _sceneLoader.SetCurrentScene(_availalbleScenes[_currentSceneIndex]);
        }

        public void OnSceneSwitchButtonPressed()
        {
            if (_sceneSwitchButton == null)
            {
                return;
            }

            if (_sceneLoader == null)
            {
                return;
            }

            D("Switching Scene to: " + _availalbleScenes[_currentSceneIndex] + ", Current Scene: " + _sceneLoader.GetCurrentScene());
            _sceneLoader.LoadScene(_availalbleScenes[_currentSceneIndex]);
            _sceneSwitchButton.image.color = _defaultSceneColor;
        }

        public void OnDropDownSelected(int index)
        {
            _currentSceneIndex = index;

            if (_sceneSwitchButton == null)
            {
                return;
            }

            if (_sceneLoader == null)
            {
                return;
            }

            if (_sceneLoader.GetCurrentScene().Equals(_availalbleScenes[index]))
            {
                _sceneSwitchButton.image.color = _defaultSceneColor;
            }
            else
            {
                _sceneSwitchButton.image.color = _newSceneColor;
            }
        }

        private static void D(string message)
        {
            DebugUtils.DebugInfo.Print("<<UIManager>> " + message);
        }

        private void OnEnable()
        {
            _sceneSwitchButton.onClick.AddListener(OnSceneSwitchButtonPressed);
            _panelCollapseButton.onClick.AddListener(() => _panelAnimator.SetTrigger("PanelInOut"));
        }

        private void OnDisable()
        {
            _sceneSwitchButton.onClick.RemoveListener(OnSceneSwitchButtonPressed);
            _panelCollapseButton.onClick.RemoveListener(() => _panelAnimator.SetTrigger("PanelInOut"));

        }
    }
}
