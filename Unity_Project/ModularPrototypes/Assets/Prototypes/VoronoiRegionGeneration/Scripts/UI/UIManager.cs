using System;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

using ModularPrototypes.Voronoi.ProceduralGeneration.Region.Data;

namespace ModularPrototypes.Voronoi.ProceduralGeneration.Region.UI
{
    public sealed class UIManager : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private VoronoiMapGenerationSettings _settingsTemplate;

        [Header("Map Dimensions")]
        [SerializeField] private TMP_InputField _mapWidthInput;
        [SerializeField] private TMP_InputField _mapHeightInput;
        [SerializeField] private TMP_InputField _cellSizeInput;

        [Header("Visual Toggles")]
        [SerializeField] private Toggle _showGridToggle;
        [SerializeField] private Toggle _showSeedsToggle;
        [SerializeField] private Toggle _showBordersToggle;
        [SerializeField] private Toggle _showGeneratedMapToggle;

        [Header("Actions")]
        [SerializeField] private Button _generateButton;
        [SerializeField] private Button _resetButton;

        [Header("Panel Animation")]
        [SerializeField] private Button _panelToggleButton;
        [SerializeField] private Animator _panelAnimator;
        [SerializeField] private string _panelToggleTrigger = "Animate";

        private VoronoiMapGenerationSettings _runtimeSettings;

        public event Action<VoronoiMapGenerationSettings> GenerateRequested;
        public event Action ResetRequested;

        public VoronoiMapGenerationSettings CurrentSettings => _runtimeSettings;

        private void Awake()
        {
            if (_settingsTemplate == null)
            {
                D("Settings template is missing.", isError: true);

                return;
            }

            _runtimeSettings = _settingsTemplate.CreateRuntimeCopy();

            ApplySettingsToUI();
        }

        private void OnEnable()
        {
            if (_mapWidthInput != null)
            {
                _mapWidthInput.onEndEdit.AddListener(OnMapWidthChanged);
            }

            if (_mapHeightInput != null)
            {
                _mapHeightInput.onEndEdit.AddListener(OnMapHeightChanged);
            }

            if (_cellSizeInput != null)
            {
                _cellSizeInput.onEndEdit.AddListener(OnCellSizeChanged);
            }

            if (_showGridToggle != null)
            {
                _showGridToggle.onValueChanged.AddListener(OnShowGridChanged);
            }

            if (_showSeedsToggle != null)
            {
                _showSeedsToggle.onValueChanged.AddListener(OnShowSeedsChanged);
            }

            if (_showBordersToggle != null)
            {
                _showBordersToggle.onValueChanged.AddListener(OnShowBordersChanged);
            }

            if (_showGeneratedMapToggle != null)
            {
                _showGeneratedMapToggle.onValueChanged.AddListener(OnShowGeneratedMapChanged);
            }

            if (_generateButton != null)
            {
                _generateButton.onClick.AddListener(OnGenerateButtonPressed);
            }

            if (_resetButton != null)
            {
                _resetButton.onClick.AddListener(OnResetButtonPressed);
            }

            if (_panelToggleButton != null)
            {
                _panelToggleButton.onClick.AddListener(OnPanelToggleButtonPressed);
            }
        }

        private void OnDisable()
        {
            if (_mapWidthInput != null)
            {
                _mapWidthInput.onEndEdit.RemoveListener(OnMapWidthChanged);
            }

            if (_mapHeightInput != null)
            {
                _mapHeightInput.onEndEdit.RemoveListener(OnMapHeightChanged);
            }

            if (_cellSizeInput != null)
            {
                _cellSizeInput.onEndEdit.RemoveListener(OnCellSizeChanged);
            }

            if (_showGridToggle != null)
            {
                _showGridToggle.onValueChanged.RemoveListener(OnShowGridChanged);
            }

            if (_showSeedsToggle != null)
            {
                _showSeedsToggle.onValueChanged.RemoveListener(OnShowSeedsChanged);
            }

            if (_showBordersToggle != null)
            {
                _showBordersToggle.onValueChanged.RemoveListener(OnShowBordersChanged);
            }

            if (_showGeneratedMapToggle != null)
            {
                _showGeneratedMapToggle.onValueChanged.RemoveListener(OnShowGeneratedMapChanged);
            }

            if (_generateButton != null)
            {
                _generateButton.onClick.RemoveListener(OnGenerateButtonPressed);
            }

            if (_resetButton != null)
            {
                _resetButton.onClick.RemoveListener(OnResetButtonPressed);
            }

            if (_panelToggleButton != null)
            {
                _panelToggleButton.onClick.RemoveListener(OnPanelToggleButtonPressed);
            }
        }

        private void OnDestroy()
        {
            if (_runtimeSettings != null)
            {
                Destroy(_runtimeSettings);
            }
        }

        private void OnMapWidthChanged(string value)
        {
            if (_runtimeSettings == null || !int.TryParse(value, out var width))
            {
                ApplySettingsToUI();
                return;
            }

            var dimension = _runtimeSettings.MapDimension;

            _runtimeSettings.SetMapDimension(new Vector2Int(width, dimension.y));

            ApplySettingsToUI();
        }

        private void OnMapHeightChanged(string value)
        {
            if (_runtimeSettings == null || !int.TryParse(value, out var height))
            {
                ApplySettingsToUI();
                return;
            }

            var dimension = _runtimeSettings.MapDimension;

            _runtimeSettings.SetMapDimension(new Vector2Int(dimension.x, height));

            ApplySettingsToUI();
        }

        private void OnCellSizeChanged(string value)
        {
            if (_runtimeSettings == null || !int.TryParse(value, out var cellSize))
            {
                ApplySettingsToUI();
                return;
            }

            _runtimeSettings.SetCellSize(cellSize);
            ApplySettingsToUI();
        }

        private void OnShowGridChanged(bool value)
        {
            if (_runtimeSettings != null)
            {
                _runtimeSettings.SetShowGrid(value);
            }
        }

        private void OnShowSeedsChanged(bool value)
        {
            if (_runtimeSettings != null)
            {
                _runtimeSettings.SetShowSeeds(value);
            }
        }

        private void OnShowBordersChanged(bool value)
        {
            if (_runtimeSettings != null)
            {
                _runtimeSettings.SetShowBorders(value);
            }
        }

        private void OnShowGeneratedMapChanged(bool value)
        {
            if (_runtimeSettings != null)
            {
                _runtimeSettings.SetShowGeneratedMap(value);
            }
        }

        private void OnGenerateButtonPressed()
        {
            if (_runtimeSettings == null)
            {
                return;
            }

            ApplyInputValues();
            GenerateRequested?.Invoke(_runtimeSettings);
        }

        private void OnResetButtonPressed()
        {
            if (_settingsTemplate == null)
            {
                return;
            }

            if (_runtimeSettings != null)
            {
                Destroy(_runtimeSettings);
            }

            _runtimeSettings = _settingsTemplate.CreateRuntimeCopy();

            ApplySettingsToUI();

            ResetRequested?.Invoke();
        }

        private void OnPanelToggleButtonPressed()
        {
            if (_panelAnimator == null)
            {
                return;
            }

            _panelAnimator.SetTrigger(_panelToggleTrigger);
        }

        private void ApplyInputValues()
        {
            if (_runtimeSettings == null)
            {
                return;
            }

            if (_mapWidthInput != null)
            {
                OnMapWidthChanged(_mapWidthInput.text);
            }

            if (_mapHeightInput != null)
            {
                OnMapHeightChanged(_mapHeightInput.text);
            }

            if (_cellSizeInput != null)
            {
                OnCellSizeChanged(_cellSizeInput.text);
            }

            if (_showGridToggle != null)
            {
                _runtimeSettings.SetShowGrid(_showGridToggle.isOn);
            }

            if (_showSeedsToggle != null)
            {
                _runtimeSettings.SetShowSeeds(_showSeedsToggle.isOn);
            }

            if (_showBordersToggle != null)
            {
                _runtimeSettings.SetShowBorders(_showBordersToggle.isOn);
            }

            if (_showGeneratedMapToggle != null)
            {
                _runtimeSettings.SetShowGeneratedMap(_showGeneratedMapToggle.isOn);
            }
        }

        private void ApplySettingsToUI()
        {
            if (_runtimeSettings == null)
            {
                return;
            }

            var dimension = _runtimeSettings.MapDimension;

            if (_mapWidthInput != null)
            {
                _mapWidthInput.SetTextWithoutNotify(dimension.x.ToString());
            }

            if (_mapHeightInput != null)
            {
                _mapHeightInput.SetTextWithoutNotify(dimension.y.ToString());
            }

            if (_cellSizeInput != null)
            {
                _cellSizeInput.SetTextWithoutNotify(_runtimeSettings.CellSize.ToString());
            }

            if (_showGridToggle != null)
            {
                _showGridToggle.SetIsOnWithoutNotify(_runtimeSettings.ShowGrid);
            }

            if (_showSeedsToggle != null)
            {
                _showSeedsToggle.SetIsOnWithoutNotify(_runtimeSettings.ShowSeeds);
            }

            if (_showBordersToggle != null)
            {
                _showBordersToggle.SetIsOnWithoutNotify(_runtimeSettings.ShowRegionBorders);
            }

            if (_showGeneratedMapToggle != null)
            {
                _showGeneratedMapToggle.SetIsOnWithoutNotify(_runtimeSettings.ShowGeneratedMap);
            }
        }

        private void D(string message, bool isError = false)
        {
            DebugUtils.DebugInfo.Print($"<<UIManager>> {message}", isError ? DebugUtils.DebugConstants.ERROR : DebugUtils.DebugConstants.INFO);
        }
    }
}