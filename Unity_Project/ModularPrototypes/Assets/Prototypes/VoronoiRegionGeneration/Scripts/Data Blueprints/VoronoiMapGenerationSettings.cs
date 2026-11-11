using UnityEngine;

namespace ModularPrototypes.Voronoi.ProceduralGeneration.Region.Data
{
    [CreateAssetMenu(fileName = "VoronoiMapGenerationSettings", menuName = "Data/Voronoi/Region/Voronoi Map Generation Settings")]
    public sealed class VoronoiMapGenerationSettings : ScriptableObject
    {
        //[Header("Bounds")]
        [SerializeField] private const int MinimumCellSize = 5;

        [SerializeField] private const int MinimumDimension = 25;
        [SerializeField] private const int MaximumDimension = 500;

        [Header("Map")]
        [SerializeField] private Vector2Int _mapDimension = new Vector2Int(1000, 1000);

        [Min(MinimumCellSize)]
        [SerializeField] private int _cellSize = 100;

        [Header("Region Colours")]
        [SerializeField] private Gradient _colourGradient;

        [Header("Visuals")]
        [SerializeField] private bool _showGrid = true;
        [SerializeField] private bool _showSeeds = true;
        [SerializeField] private bool _showRegionBorders = true;
        [SerializeField] private bool _showGeneratedMap = true;

        public Vector2Int MapDimension => _mapDimension;

        public int CellSize => Mathf.Clamp(_cellSize, MinimumCellSize, Mathf.Min(_mapDimension.x, _mapDimension.y));

        public Gradient ColourGradient => _colourGradient;

        public bool ShowGrid => _showGrid;
        public bool ShowSeeds => _showSeeds;
        public bool ShowRegionBorders => _showRegionBorders;
        public bool ShowGeneratedMap => _showGeneratedMap;

        private void OnValidate()
        {
            _mapDimension = new Vector2Int(Mathf.Clamp(_mapDimension.x, MinimumDimension, MaximumDimension), Mathf.Clamp(_mapDimension.y, MinimumDimension, MaximumDimension));

            _cellSize = Mathf.Clamp(_cellSize, MinimumCellSize, Mathf.Min(_mapDimension.x, _mapDimension.y));
        }

        public VoronoiMapGenerationSettings CreateRuntimeCopy()
        {
            return Instantiate(this);
        }

        public void SetMapDimension(Vector2Int mapDimension)
        {
            if (!IsValidMapDimension(mapDimension))
            {
                Debug.LogWarning($"Map dimension {mapDimension} is outside the valid range " + $"of {MinimumDimension} to {MaximumDimension}.");
                return;
            }

            _mapDimension = mapDimension;

            _cellSize = Mathf.Clamp(_cellSize, MinimumCellSize, Mathf.Min(_mapDimension.x, _mapDimension.y));
        }

        public void SetCellSize(int cellSize)
        {
            _cellSize = Mathf.Clamp(cellSize, MinimumCellSize, Mathf.Min(_mapDimension.x, _mapDimension.y));
        }

        public void SetColourGradient(Gradient colourGradient)
        {
            if (colourGradient == null)
            {
                return;
            }

            _colourGradient = colourGradient;
        }

        public void SetShowGrid(bool showGrid)
        {
            _showGrid = showGrid;
        }

        public void SetShowSeeds(bool showSeeds)
        {
            _showSeeds = showSeeds;
        }

        public void SetShowBorders(bool showBorders)
        {
            _showRegionBorders = showBorders;
        }

        public void SetShowGeneratedMap(bool showGeneratedMap)
        {
            _showGeneratedMap = showGeneratedMap;
        }

        private static bool IsValidMapDimension(Vector2Int mapDimension)
        {
            return mapDimension.x >= MinimumDimension && mapDimension.y >= MinimumDimension && mapDimension.x <= MaximumDimension && mapDimension.y <= MaximumDimension;
        }

        private static void D(string message)
        {
            DebugUtils.DebugInfo.Print("<<VoronoiMapGenerationSettings>> " + message);
        }
    }
}
