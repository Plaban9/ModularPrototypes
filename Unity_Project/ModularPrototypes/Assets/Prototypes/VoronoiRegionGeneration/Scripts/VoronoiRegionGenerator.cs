using ModularPrototypes.DebugUtils;
using ModularPrototypes.Voronoi.ProceduralGeneration.Region.UI;

using UnityEngine;
using UnityEngine.UI;

namespace ModularPrototypes.Voronoi.ProceduralGeneration.Region
{
    public class VoronoiRegionGenerator : MonoBehaviour
    {
        [Header("Map Properties")]
        [SerializeField] private Vector2Int _mapDimension;
        [SerializeField] private int _cellSize = 10;

        private Vector2Int _gridLayout;
        private Vector2Int[,] _seedPositions;
        private Vector2Int[,] _regionMap;

        [Header("Map Visualization")]
        [SerializeField] private Gradient _colourGradient;
        [SerializeField] private Color[,] _cellColours;

        #region Visual
        [Header("UI")]
        [SerializeField] private RawImage _seedVisual;
        [SerializeField] private RawImage _gridVisual;
        [SerializeField] private RawImage _borderVisual;
        [SerializeField] private RawImage _mapVisual;
        #endregion

        #region UI
        [Header("UI Events")]
        [SerializeField] private RawImageCoordinateEmitter _mapCoordinateInput;
        #endregion

        #region Colour Constants
        [Header("Colour Constants")]
        [SerializeField] private Color _seedColour = Color.gray4;
        [SerializeField] private Color _gridColour = new Color(0f, 0f, 0f, .5f);
        [SerializeField] private Color _gridBorderColour = new Color(.5f, .5f, .5f, .25f);
        [SerializeField] private Color _borderColour = Color.black;
        #endregion

        private void OnEnable()
        {
            if (_mapCoordinateInput == null)
            {
                return;
            }

            _mapCoordinateInput.PointerMoved += OnMapPointerMoved;
            _mapCoordinateInput.PointerExited += OnMapPointerExited;
        }

        private void OnDisable()
        {
            if (_mapCoordinateInput == null)
            {
                return;
            }

            _mapCoordinateInput.PointerMoved -= OnMapPointerMoved;
            _mapCoordinateInput.PointerExited -= OnMapPointerExited;
        }

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            GenerateVoronoiMap();
        }

        [ContextMenu("GenerateVoronoiMap()")]
        private void GenerateVoronoiMap()
        {
            GenerateGrid();
            GenerateSeeds();
            GenerateMap();

            PaintGeneration();
        }

        #region Generation Logic
        private void GenerateGrid()
        {
            // Calculate Grid Count in X and Y
            _gridLayout = new Vector2Int(_mapDimension.x / _cellSize, _mapDimension.y / _cellSize);
        }

        private void GenerateSeeds()
        {
            // Initialize the 2D array to store respective data (Seed's Position and Colour)
            _seedPositions = new Vector2Int[_gridLayout.x, _gridLayout.y];
            _cellColours = new Color[_gridLayout.x, _gridLayout.y];

            for (int x = 0; x < _gridLayout.x; x++)
            {
                for (int y = 0; y < _gridLayout.y; y++)
                {
                    _seedPositions[x, y] = new Vector2Int((x * _cellSize) + Random.Range(1, _cellSize), (y * _cellSize) + Random.Range(1, _cellSize));

                    _cellColours[x, y] = _colourGradient.Evaluate(Random.Range(0f, 1f));
                }
            }
        }

        private void GenerateMap()
        {
            GenerateVoronoiRegion();
            GenerateBorder();
        }

        private void GenerateVoronoiRegion()
        {
            _regionMap = new Vector2Int[_mapDimension.x, _mapDimension.y];

            // Iteration of 2D to determine the region closeness towards the seed of each pixel
            for (int i = 0; i < _mapDimension.x; i++)
            {
                for (int j = 0; j < _mapDimension.y; j++)
                {
                    // Locate Current Grid for the Pixel
                    var currentGrid = new Vector2Int(i / _cellSize, j / _cellSize);

                    var nearestDistance = Mathf.Infinity;
                    var nearestCellPosition = Vector2Int.zero;

                    // Checking Surrounding Grids
                    for (int x = -1; x < 2; x++)
                    {
                        for (int y = -1; y < 2; y++)
                        {
                            int posX = x + currentGrid.x;
                            int posY = y + currentGrid.y;

                            // Edge Cases
                            if (posX < 0 || posY < 0 || posX >= _gridLayout.x || posY >= _gridLayout.y)
                            {
                                continue;
                            }

                            // Checking shortest distance from that pixel to seed of the respective Grid
                            var seedPosition = _seedPositions[posX, posY];
                            var pixelPosition = new Vector2Int(i, j);
                            var distance = (pixelPosition - seedPosition).sqrMagnitude;

                            if (distance < nearestDistance)
                            {
                                nearestDistance = distance;
                                nearestCellPosition = new Vector2Int(posX, posY);
                            }
                        }
                    }

                    _regionMap[i, j] = nearestCellPosition;
                }
            }
        }

        private void GenerateBorder()
        {
            var texture2D = new Texture2D(_mapDimension.x, _mapDimension.y);
            texture2D.filterMode = FilterMode.Point;

            var pixels = new Color[_mapDimension.x * _mapDimension.y];

            texture2D.SetPixels(pixels);

            for (int x = 0; x < _mapDimension.x; x++)
            {
                for (int y = 0; y < _mapDimension.y; y++)
                {
                    var currentRegion = _regionMap[x, y];

                    var isBorder =
                        IsDifferentRegion(x - 1, y, currentRegion) ||
                        IsDifferentRegion(x + 1, y, currentRegion) ||
                        IsDifferentRegion(x, y - 1, currentRegion) ||
                        IsDifferentRegion(x, y + 1, currentRegion);

                    if (isBorder)
                    {
                        texture2D.SetPixel(x, y, _borderColour);
                    }
                }
            }

            SaveToMap(texture2D, _borderVisual);
        }

        private bool IsDifferentRegion(int x, int y, Vector2Int currentRegion)
        {
            // Do not draw a border around the outside of the map.
            if (x < 0 || y < 0 || x >= _mapDimension.x || y >= _mapDimension.y)
            {
                return false;
            }

            return _regionMap[x, y] != currentRegion;
        }
        #endregion

        #region Visual Logic
        private void PaintGeneration()
        {
            PaintGrid();
            PaintSeeds();
            PaintVoronoiMap();
        }

        private void PaintGrid()
        {
            var texture2D = new Texture2D(_mapDimension.x, _mapDimension.y);
            texture2D.filterMode = FilterMode.Point;

            for (int i = 0; i < _mapDimension.x; i++)
            {
                for (int j = 0; j < _mapDimension.y; j++)
                {
                    var isGridPixel = (i > 0 && i % _cellSize == 0) || (j > 0 && j % _cellSize == 0);
                    var colour = isGridPixel ? _gridBorderColour : _gridColour;

                    texture2D.SetPixel(i, j, colour);
                }
            }

            SaveToMap(texture2D, _gridVisual);
        }

        private void PaintSeeds()
        {
            var texture2D = new Texture2D(_mapDimension.x, _mapDimension.y);
            texture2D.filterMode = FilterMode.Point;

            var pixels = new Color[texture2D.width * texture2D.height];
            texture2D.SetPixels(pixels);

            for (int i = 0; i < _gridLayout.x; i++)
            {
                for (int j = 0; j < _gridLayout.y; j++)
                {
                    var seedPosition = _seedPositions[i, j];

                    texture2D.SetPixel(seedPosition.x, seedPosition.y, _seedColour);
                }
            }

            SaveToMap(texture2D, _seedVisual);
        }

        private void PaintVoronoiMap()
        {
            var texture2D = new Texture2D(_mapDimension.x, _mapDimension.y);
            texture2D.filterMode = FilterMode.Point;

            for (int x = 0; x < _mapDimension.x; x++)
            {
                for (int y = 0; y < _mapDimension.y; y++)
                {
                    var region = _regionMap[x, y];
                    texture2D.SetPixel(x, y, _cellColours[region.x, region.y]);
                }
            }

            SaveToMap(texture2D, _mapVisual);
        }

        [ContextMenu("ToggleMapVisual()")]
        private void ToggleMapVisual()
        {
            _mapVisual.enabled = !_mapVisual.enabled;
        }

        [ContextMenu("ToggleBorderVisual()")]
        private void ToggleBorderVisual()
        {
            _borderVisual.enabled = !_borderVisual.enabled;
        }

        [ContextMenu("ToggleGridVisual()")]
        private void ToggleGridVisual()
        {
            _gridVisual.enabled = !_gridVisual.enabled;
        }

        [ContextMenu("ToggleSeedVisual()")]
        private void ToggleSeedVisual()
        {
            _seedVisual.enabled = !_seedVisual.enabled;
        }
        #endregion

        #region UI
        [ContextMenu("OnHover()")]
        private void OnHover()
        {

        }

        private void OnHover(int x, int y)
        {
        }
        #endregion

        #region UI Event Handlers
        private void OnMapPointerMoved(Vector2Int pixelPosition, Vector2Int mapSize)
        {
            if (_regionMap == null)
            {
                return;
            }

            Vector2 normalizedPixelPosition = new Vector2((float)pixelPosition.x / mapSize.x, (float)pixelPosition.y / mapSize.y);
            Vector2Int cellPosition = new Vector2Int((int)(normalizedPixelPosition.x * _mapDimension.x), (int)(normalizedPixelPosition.y * _mapDimension.y));

            var region = _regionMap[cellPosition.x, cellPosition.y];
            var seedPosition = _seedPositions[region.x, region.y];
            var regionColor = _cellColours[region.x, region.y];

            var pixelData = new VoronoiRegionPixelData(cellPosition, region, seedPosition, regionColor);

            D(pixelData.ToString());
        }

        private void OnMapPointerExited()
        {
            D("Pointer exited the map area.");
        }
        #endregion

        #region Utilities
        private void SaveToMap(Texture2D texture2D, RawImage rawImage)
        {
            texture2D.Apply();
            rawImage.texture = texture2D;
        }

        private static void D(string message)
        {
            DebugUtils.DebugInfo.Print("<<VoronoiRegionGenerator>> " + message);
        }
        #endregion
    }
}
