using ModularPrototypes.UI.Tooltip;
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
        private Color[,] _cellColours;

        [Header("Map Visualization")]
        [SerializeField] private Gradient _colourGradient;
        [SerializeField] private int _cellHighlightThickness = 2;


        #region Visual
        [Header("UI")]
        [SerializeField] private RawImage _seedVisual;
        [SerializeField] private RawImage _gridVisual;
        [SerializeField] private RawImage _borderVisual;
        [SerializeField] private RawImage _mapVisual;
        [SerializeField] private RawImage _cellHighlightVisual;
        [SerializeField] private RawImage _pixelHighlightVisual;
        private Texture2D _cellHighlightTexture;
        private Vector2Int _lastPixelPosition = new Vector2Int(-1, -1);
        #endregion

        #region UI
        [Header("UI Events")]
        [SerializeField] private RawImageCoordinateEmitter _mapCoordinateInput;
        #endregion

        #region Colour Constants
        [Header("Colour Constants")]
        [SerializeField] private Color _seedColour = Color.gray4;
        [SerializeField] private Color _gridColour = new Color(0f, 0f, 0f, 0.5f);
        [SerializeField] private Color _gridBorderColour = new Color(0.5f, 0.5f, 0.5f, 0.25f);
        [SerializeField] private Color _borderColour = Color.black;

        //[SerializeField] private Color _gridHighlightColour = Color.red;
        #endregion

        #region Unity Lifecycle
        private void OnEnable()
        {
            if (_mapCoordinateInput == null)
            {
                return;
            }

            _mapCoordinateInput.PointerEntered += OnMapPointerEntered;
            _mapCoordinateInput.PointerMoved += OnMapPointerMoved;
            _mapCoordinateInput.PointerExited += OnMapPointerExited;
        }

        private void OnDisable()
        {
            if (_mapCoordinateInput == null)
            {
                return;
            }

            _mapCoordinateInput.PointerEntered -= OnMapPointerEntered;
            _mapCoordinateInput.PointerMoved -= OnMapPointerMoved;
            _mapCoordinateInput.PointerExited -= OnMapPointerExited;
        }

        private void OnDestroy()
        {
            if (_cellHighlightTexture != null)
            {
                Destroy(_cellHighlightTexture);
            }
        }

        private void Start()
        {
            Initialize();
        }
        #endregion

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
            InitializeCellHighlight();
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

        private void InitializeCellHighlight()
        {
            if (_cellHighlightVisual == null || _cellSize <= 0)
            {
                return;
            }

            if (_cellHighlightTexture != null)
            {
                Destroy(_cellHighlightTexture);
            }

            _cellHighlightTexture = new Texture2D(_cellSize, _cellSize, TextureFormat.RGBA32, false);

            _cellHighlightTexture.filterMode = FilterMode.Point;
            _cellHighlightTexture.wrapMode = TextureWrapMode.Clamp;

            var pixels = new Color[_cellSize * _cellSize];
            var edgeThickness = Mathf.Clamp(_cellHighlightThickness, 1, Mathf.Max(1, _cellSize / 2));

            for (int x = 0; x < _cellSize; x++)
            {
                for (int y = 0; y < _cellSize; y++)
                {
                    var isEdge = x < edgeThickness || x >= _cellSize - edgeThickness || y < edgeThickness || y >= _cellSize - edgeThickness;

                    pixels[(y * _cellSize) + x] = isEdge ? Color.white : Color.clear;
                }
            }

            _cellHighlightTexture.SetPixels(pixels);
            _cellHighlightTexture.Apply();

            _cellHighlightVisual.texture = _cellHighlightTexture;
            _cellHighlightVisual.raycastTarget = false;
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

        private bool IsInternalGridLine(int coordinate, int dimension)
        {
            //// Works for edge borders exclusion
            //if (coordinate <= 0 || coordinate >= dimension - 1)
            //{
            //    return false;
            //}

            // Works for edge borders inclusion
            if (coordinate < 0 || coordinate > dimension - 1)
            {
                return false;
            }

            var remainder = coordinate % _cellSize;

            return remainder == 0 || remainder == _cellSize - 1;
        }

        private void PaintGrid()
        {
            var texture2D = new Texture2D(_mapDimension.x, _mapDimension.y);
            texture2D.filterMode = FilterMode.Point;

            for (int i = 0; i < _mapDimension.x; i++)
            {
                for (int j = 0; j < _mapDimension.y; j++)
                {
                    var isVerticalGridLine = IsInternalGridLine(i, _mapDimension.x);

                    var isHorizontalGridLine = IsInternalGridLine(j, _mapDimension.y);

                    var isGridPixel = isVerticalGridLine || isHorizontalGridLine;
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
        private void OnMapPointerEntered()
        {
            D("Pointer entered the map area.");

            if (_cellHighlightVisual != null)
            {
                _cellHighlightVisual.enabled = _gridVisual.enabled;
            }

            if (_pixelHighlightVisual != null)
            {
                _pixelHighlightVisual.enabled = true;
            }
        }

        private void OnMapPointerMoved(Vector2Int relativePixelPosition, Vector2Int mapSize)
        {
            if (_regionMap == null)
            {
                return;
            }

            Vector2 normalizedPixelPosition = new Vector2((float)relativePixelPosition.x / mapSize.x, (float)relativePixelPosition.y / mapSize.y);
            Vector2Int pixelPosition = new Vector2Int((int)(normalizedPixelPosition.x * _mapDimension.x), (int)(normalizedPixelPosition.y * _mapDimension.y));

            var region = _regionMap[pixelPosition.x, pixelPosition.y];
            var seedPosition = _seedPositions[region.x, region.y];
            var regionColour = _cellColours[region.x, region.y];

            var pixelData = new VoronoiRegionPixelData(pixelPosition, region, seedPosition, regionColour);

            var gridPosition = new Vector2Int(pixelPosition.x / _cellSize, pixelPosition.y / _cellSize);
            var seedRegionColour = _cellColours[gridPosition.x, gridPosition.y];

            var complementaryCellColour = GetComplementaryColour(seedRegionColour);
            var complementaryPixelColour = GetComplementaryColour(regionColour);

            HandleCellHighlighter(pixelPosition, complementaryCellColour);
            HandlePixelHighlighter(pixelPosition, complementaryPixelColour);
            ShowPixelDataTooltip(pixelData);
        }

        private void OnMapPointerExited()
        {
            D("Pointer exited the map area.");

            if (_cellHighlightVisual != null)
            {
                _cellHighlightVisual.enabled = false;
            }

            if (_pixelHighlightVisual != null)
            {
                _pixelHighlightVisual.enabled = false;
            }

            TooltipSystem.Hide();
        }

        private void HandleCellHighlighter(Vector2Int pixelPosition, Color highlightColour)
        {
            //Implement logic to highlight the cell under the pointer
            if (_cellHighlightVisual == null || _mapVisual == null)
            {
                return;
            }

            highlightColour.a = _cellHighlightVisual.color.a;
            _cellHighlightVisual.color = highlightColour;

            var mapRectTransform = _mapVisual.rectTransform;
            var highlightRectTransform = _cellHighlightVisual.rectTransform;

            var gridPosition = new Vector2Int(Mathf.Clamp(pixelPosition.x / _cellSize, 0, _gridLayout.x - 1), Mathf.Clamp(pixelPosition.y / _cellSize, 0, _gridLayout.y - 1));

            var cellWidth = mapRectTransform.rect.width / _gridLayout.x;
            var cellHeight = mapRectTransform.rect.height / _gridLayout.y;

            // Calculate the center of the grid cell in map-local coordinates.
            var localCellCenter = new Vector3(mapRectTransform.rect.xMin + ((gridPosition.x + 0.5f) * cellWidth), mapRectTransform.rect.yMin + ((gridPosition.y + 0.5f) * cellHeight), 0f);

            // Convert the map-local position to the UI canvas/world position.
            var worldCellCenter = mapRectTransform.TransformPoint(localCellCenter);

            highlightRectTransform.pivot = new Vector2(0.5f, 0.5f);
            highlightRectTransform.sizeDelta = new Vector2(cellWidth, cellHeight);
            highlightRectTransform.position = worldCellCenter;
        }

        private void HandlePixelHighlighter(Vector2Int pixelPosition, Color highlightColour)
        {
            //Implement logic to highlight the pixel under the pointer
            if (_pixelHighlightVisual == null || _mapVisual == null)
            {
                return;
            }

            highlightColour.a = _pixelHighlightVisual.color.a;
            _pixelHighlightVisual.color = highlightColour;

            var mapRectTransform = _mapVisual.rectTransform;
            var highlightRectTransform = _pixelHighlightVisual.rectTransform;

            var pixelWidth = mapRectTransform.rect.width / _mapDimension.x;
            var pixelHeight = mapRectTransform.rect.height / _mapDimension.y;

            // Calculate the center of the pixel cell in map-local coordinates.
            var localPixelCenter = new Vector3(mapRectTransform.rect.xMin + ((pixelPosition.x + 0.5f) * pixelWidth), mapRectTransform.rect.yMin + ((pixelPosition.y + 0.5f) * pixelHeight), 0f);

            // Convert the map-local position to the UI canvas/world position.
            var worldPixelCenter = mapRectTransform.TransformPoint(localPixelCenter);

            highlightRectTransform.pivot = new Vector2(0.5f, 0.5f);
            highlightRectTransform.sizeDelta = new Vector2(pixelWidth, pixelHeight);
            highlightRectTransform.position = worldPixelCenter;
        }

        private void ShowPixelDataTooltip(VoronoiRegionPixelData pixelData)
        {
            if (pixelData.PixelLocation == _lastPixelPosition)
            {
                return;
            }

            _lastPixelPosition = pixelData.PixelLocation;

            var colourHex = ColorUtility.ToHtmlStringRGBA(pixelData.RegionColor);

            var colourBlock =
                $"<mark=#{colourHex}><color=#00000000>MI</color></mark>\n" +
                 "--------------------";

            var header =
                $"Region - {colourBlock}";

            var content =
                $"GRID LOCATION: ROW - {pixelData.GridLocation.x}, COL - {pixelData.GridLocation.y}\n" +
                $"SEED LOCATION: {pixelData.SeedLocation}\n" +
                $"PIXEL LOCATION: {pixelData.PixelLocation}";

            var backgroundColor = new Color(0f, 0f, 0f, 0.8f);

            TooltipSystem.Show(
                header: header,
                headerColor: Color.white,
                content: content,
                contentColor: Color.white,
                backgroundColor: backgroundColor);
        }
        #endregion

        #region Utilities
        private void SaveToMap(Texture2D texture2D, RawImage rawImage)
        {
            texture2D.Apply();
            rawImage.texture = texture2D;
        }

        private static Color GetComplementaryColour(Color colour)
        {
            return new Color(1f - colour.r, 1f - colour.g, 1f - colour.b, colour.a);
        }

        private static void D(string message)
        {
            DebugUtils.DebugInfo.Print("<<VoronoiRegionGenerator>> " + message);
        }
        #endregion
    }
}
