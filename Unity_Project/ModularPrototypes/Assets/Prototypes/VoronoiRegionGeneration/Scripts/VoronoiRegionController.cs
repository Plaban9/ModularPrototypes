using ModularPrototypes.Voronoi.ProceduralGeneration.Region.Data;
using ModularPrototypes.Voronoi.ProceduralGeneration.Region.Generation;
using ModularPrototypes.Voronoi.ProceduralGeneration.Region.Rendering;
using ModularPrototypes.Voronoi.ProceduralGeneration.Region.UI;
using ModularPrototypes.Voronoi.ProceduralGeneration.Region.UI.Viewport;

using UnityEditor.ShaderKeywordFilter;

using UnityEngine;

namespace ModularPrototypes.Voronoi.ProceduralGeneration.Region
{
    public sealed class VoronoiRegionController : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private UIManager _uiManager;

        [Header("Rendering")]
        [SerializeField] private VoronoiMapRenderer _mapRenderer;
        [SerializeField] private VoronoiMapAspectRatioController _aspectRatioController;

        [Header("Interaction")]
        [SerializeField] private VoronoiRegionInteraction _interaction;

        [Header("Generation")]
        [SerializeField] private int _randomSeed = 12345;


        private VoronoiMapGenerator _mapGenerator;
        private VoronoiMapData _mapData;

        public VoronoiMapData MapData => _mapData;

        private void Awake()
        {
            _mapGenerator = new VoronoiMapGenerator();
        }

        private void OnEnable()
        {
            if (_uiManager == null)
            {
                return;
            }

            _uiManager.GenerateRequested += OnGenerateRequested;
            _uiManager.ResetRequested += OnResetRequested;
            _uiManager.VisualSettingsChanged += OnVisualSettingsChanged;
        }

        private void OnDisable()
        {
            if (_uiManager == null)
            {
                return;
            }

            _uiManager.GenerateRequested -= OnGenerateRequested;
            _uiManager.ResetRequested -= OnResetRequested;
            _uiManager.VisualSettingsChanged -= OnVisualSettingsChanged;
        }

        private void Start()
        {
            if (_uiManager?.CurrentSettings != null)
            {
                GenerateMap(_uiManager.CurrentSettings);
            }
        }

        private void OnGenerateRequested(VoronoiMapGenerationSettings settings)
        {
            GenerateMap(settings);
        }

        private void OnResetRequested(VoronoiMapGenerationSettings settings)
        {
            _mapRenderer?.ApplyVisibility(settings);
            _interaction?.ResetInteraction();
        }

        private void OnVisualSettingsChanged(VoronoiMapGenerationSettings settings)
        {
            if (_mapRenderer == null)
            {
                return;
            }

            _mapRenderer.ApplyVisibility(settings);
            _interaction?.SetGridVisible(settings.ShowGrid);
        }

        private void GenerateMap(VoronoiMapGenerationSettings settings)
        {
            _mapData = _mapGenerator.Generate(settings, _randomSeed);

            _mapRenderer?.Render(_mapData, settings);

            _interaction?.SetMapData(_mapData);
            _interaction?.SetGridVisible(settings.ShowGrid);

            _aspectRatioController?.ApplyMapAspectRatio(settings.MapDimension);
        }
    }
}