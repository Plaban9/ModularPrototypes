using UnityEngine;

using ModularPrototypes.Voronoi.ProceduralGeneration.Region.Data;
using ModularPrototypes.Voronoi.ProceduralGeneration.Region.Generation;
using ModularPrototypes.Voronoi.ProceduralGeneration.Region.Rendering;
using ModularPrototypes.Voronoi.ProceduralGeneration.Region.UI;

namespace ModularPrototypes.Voronoi.ProceduralGeneration.Region
{
    public sealed class VoronoiRegionController : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private UIManager _uiManager;

        [Header("Rendering")]
        [SerializeField] private VoronoiMapRenderer _mapRenderer;

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
        }

        private void OnDisable()
        {
            if (_uiManager == null)
            {
                return;
            }

            _uiManager.GenerateRequested -= OnGenerateRequested;
            _uiManager.ResetRequested -= OnResetRequested;
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

        private void OnResetRequested()
        {
            if (_uiManager?.CurrentSettings != null)
            {
                GenerateMap(_uiManager.CurrentSettings);
            }
        }

        private void GenerateMap(VoronoiMapGenerationSettings settings)
        {
            _mapData = _mapGenerator.Generate(settings, _randomSeed);

            if (_mapRenderer != null)
            {
                _mapRenderer.Render(_mapData, settings);
            }
        }
    }
}