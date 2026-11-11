using ModularPrototypes.UI.Tooltip;
using ModularPrototypes.Voronoi.ProceduralGeneration.Region.Data;
using ModularPrototypes.Voronoi.ProceduralGeneration.Region.UI.Viewport;

using UnityEngine;
using UnityEngine.UI;

namespace ModularPrototypes.Voronoi.ProceduralGeneration.Region.UI
{
    public sealed class VoronoiRegionInteraction : MonoBehaviour
    {
        [Header("Input")]
        [SerializeField] private RawImageCoordinateEmitter _mapCoordinateInput;
        [SerializeField] private VoronoiRegionViewportInput _mapViewportInput;

        [Header("Map Visual")]
        [SerializeField] private RawImage _mapVisual;

        [Header("Highlight Visuals")]
        [SerializeField] private RawImage _cellHighlightVisual;
        [SerializeField] private RawImage _pixelHighlightVisual;

        [Header("Cell Highlight")]
        [SerializeField] private int _cellHighlightThickness = 1;

        private Vector2Int _lastPixelPosition = new Vector2Int(-1, -1);
        private VoronoiMapData _mapData;
        private Texture2D _cellHighlightTexture;

        private bool _isGridVisible;
        private bool _isPointerOver;

        private bool _isPanning;

        private void Awake()
        {
            SubscribeToInput();

            if (_mapViewportInput != null)
            {
                _mapViewportInput.PanStateChanged += OnPanStateChanged;
            }

            if (_cellHighlightVisual != null)
            {
                _cellHighlightVisual.raycastTarget = false;
                _cellHighlightVisual.enabled = false;
            }

            if (_pixelHighlightVisual != null)
            {
                _pixelHighlightVisual.raycastTarget = false;
                _pixelHighlightVisual.enabled = false;
            }
        }

        private void OnDestroy()
        {
            UnsubscribeFromInput();

            if (_mapViewportInput != null)
            {
                _mapViewportInput.PanStateChanged -= OnPanStateChanged;
            }

            if (_cellHighlightTexture != null)
            {
                Destroy(_cellHighlightTexture);
            }
        }

        public void SetMapData(VoronoiMapData mapData)
        {
            _mapData = mapData;
            _lastPixelPosition = new Vector2Int(-1, -1);

            if (_mapData == null)
            {
                HideHighlights();
                return;
            }

            InitializeCellHighlight(_mapData.CellSize);
            HideHighlights();
        }

        public void SetGridVisible(bool isVisible)
        {
            _isGridVisible = isVisible;

            if (_cellHighlightVisual == null)
            {
                return;
            }

            _cellHighlightVisual.enabled = _isGridVisible && _isPointerOver;
        }

        public void ResetInteraction()
        {
            _isPointerOver = false;

            HideHighlights();
            TooltipSystem.Hide();
        }

        private void SubscribeToInput()
        {
            if (_mapCoordinateInput == null)
            {
                return;
            }

            _mapCoordinateInput.PointerEntered += OnPointerEntered;
            _mapCoordinateInput.PointerMoved += OnPointerMoved;
            _mapCoordinateInput.PointerExited += OnPointerExited;
        }

        private void UnsubscribeFromInput()
        {
            if (_mapCoordinateInput == null)
            {
                return;
            }

            _mapCoordinateInput.PointerEntered -= OnPointerEntered;
            _mapCoordinateInput.PointerMoved -= OnPointerMoved;
            _mapCoordinateInput.PointerExited -= OnPointerExited;
        }

        private void OnPointerEntered()
        {
            D($"Pointer entered. Grid visible: {_isGridVisible}");

            _isPointerOver = true;

            if (_mapData == null)
            {
                return;
            }

            if (_cellHighlightVisual != null)
            {
                _cellHighlightVisual.enabled = _isGridVisible;
            }

            if (_pixelHighlightVisual != null)
            {
                _pixelHighlightVisual.enabled = true;
            }
        }

        private void OnPointerMoved(Vector2Int relativePixelPosition, Vector2Int inputSize)
        {
            if (_isPanning)
            {
                return;
            }

            if (_mapData == null || inputSize.x <= 0 || inputSize.y <= 0)
            {
                return;
            }

            var pixelPosition = ConvertToMapPixelPosition(relativePixelPosition, inputSize);

            if (pixelPosition == _lastPixelPosition)
            {
                return;
            }

            _lastPixelPosition = pixelPosition;

            var gridPosition = new Vector2Int(pixelPosition.x / _mapData.CellSize, pixelPosition.y / _mapData.CellSize);

            var region = _mapData.GetRegionAt(pixelPosition);

            var seedPosition = _mapData.SeedPositions[region.x, region.y];

            var regionColour = _mapData.GetRegionColour(pixelPosition);

            var cellSeedColour = _mapData.RegionColours[gridPosition.x, gridPosition.y];

            if (_isGridVisible)
            {
                HandleCellHighlight(gridPosition, GetComplementaryColour(cellSeedColour));
            }
            else if (_cellHighlightVisual != null)
            {
                _cellHighlightVisual.enabled = false;
            }

            HandlePixelHighlight(pixelPosition, GetComplementaryColour(regionColour));

            ShowTooltip(pixelPosition, gridPosition, seedPosition, regionColour);
        }

        private void OnPointerExited()
        {
            _isPointerOver = false;
            _lastPixelPosition = new Vector2Int(-1, -1);

            HideHighlights();
            TooltipSystem.Hide();
        }

        private void OnPanStateChanged(bool isPanning)
        {
            _isPanning = isPanning;

            if (!isPanning)
            {
                return;
            }

            HideHighlights();
            TooltipSystem.Hide();
        }

        private Vector2Int ConvertToMapPixelPosition(Vector2Int relativePixelPosition, Vector2Int inputSize)
        {
            var normalizedPosition = new Vector2((float)relativePixelPosition.x / inputSize.x, (float)relativePixelPosition.y / inputSize.y);

            return new Vector2Int(Mathf.Clamp(Mathf.FloorToInt(normalizedPosition.x * _mapData.MapDimension.x), 0, _mapData.MapDimension.x - 1), Mathf.Clamp(Mathf.FloorToInt(normalizedPosition.y * _mapData.MapDimension.y), 0, _mapData.MapDimension.y - 1));
        }

        private void HandleCellHighlight(Vector2Int gridPosition, Color highlightColour)
        {
            if (_cellHighlightVisual == null || _mapVisual == null)
            {
                return;
            }

            var mapRectTransform = _mapVisual.rectTransform;
            var highlightRectTransform = _cellHighlightVisual.rectTransform;

            var cellWidth = mapRectTransform.rect.width / _mapData.GridLayout.x;

            var cellHeight = mapRectTransform.rect.height / _mapData.GridLayout.y;

            var localCellCenter = new Vector3(mapRectTransform.rect.xMin + ((gridPosition.x + 0.5f) * cellWidth), mapRectTransform.rect.yMin + ((gridPosition.y + 0.5f) * cellHeight), 0f);

            var worldCellCenter = mapRectTransform.TransformPoint(localCellCenter);

            SetHighlightTransform(highlightRectTransform, worldCellCenter, new Vector2(cellWidth, cellHeight));

            _cellHighlightVisual.color = highlightColour;
            _cellHighlightVisual.color = new Color(highlightColour.r, highlightColour.g, highlightColour.b, 0.8f);
            _cellHighlightVisual.enabled = _isGridVisible && _isPointerOver;
        }

        private void HandlePixelHighlight(Vector2Int pixelPosition, Color highlightColour)
        {
            if (_pixelHighlightVisual == null || _mapVisual == null)
            {
                return;
            }

            var mapRectTransform = _mapVisual.rectTransform;
            var highlightRectTransform = _pixelHighlightVisual.rectTransform;

            var pixelWidth = mapRectTransform.rect.width / _mapData.MapDimension.x;

            var pixelHeight = mapRectTransform.rect.height / _mapData.MapDimension.y;

            var localPixelCenter = new Vector3(mapRectTransform.rect.xMin + ((pixelPosition.x + 0.5f) * pixelWidth), mapRectTransform.rect.yMin + ((pixelPosition.y + 0.5f) * pixelHeight), 0f);

            var worldPixelCenter = mapRectTransform.TransformPoint(localPixelCenter);

            SetHighlightTransform(highlightRectTransform, worldPixelCenter, new Vector2(pixelWidth, pixelHeight));

            _pixelHighlightVisual.color = highlightColour;
            _pixelHighlightVisual.enabled = _isPointerOver;
        }

        private static void SetHighlightTransform(RectTransform highlightRectTransform, Vector3 worldPosition, Vector2 size)
        {
            highlightRectTransform.pivot = new Vector2(0.5f, 0.5f);

            highlightRectTransform.sizeDelta = size;
            highlightRectTransform.position = worldPosition;
        }

        private void InitializeCellHighlight(int cellSize)
        {
            if (_cellHighlightVisual == null || cellSize <= 0)
            {
                return;
            }

            if (_cellHighlightTexture != null && (_cellHighlightTexture.width != cellSize || _cellHighlightTexture.height != cellSize))
            {
                Destroy(_cellHighlightTexture);
                _cellHighlightTexture = null;
            }

            if (_cellHighlightTexture == null)
            {
                _cellHighlightTexture = new Texture2D(cellSize, cellSize, TextureFormat.RGBA32, false);
            }

            _cellHighlightTexture.filterMode = FilterMode.Point;

            _cellHighlightTexture.wrapMode = TextureWrapMode.Clamp;

            var pixels = new Color[cellSize * cellSize];

            var thickness = Mathf.Clamp(_cellHighlightThickness, 1, Mathf.Max(1, cellSize / 2));

            for (int x = 0; x < cellSize; x++)
            {
                for (int y = 0; y < cellSize; y++)
                {
                    var isEdge = x < thickness || x >= cellSize - thickness || y < thickness || y >= cellSize - thickness;

                    pixels[(y * cellSize) + x] = isEdge ? Color.white : Color.clear;
                }
            }

            _cellHighlightTexture.SetPixels(pixels);
            _cellHighlightTexture.Apply();

            _cellHighlightVisual.texture = _cellHighlightTexture;

            _cellHighlightVisual.raycastTarget = false;
        }

        private void ShowTooltip(Vector2Int pixelPosition, Vector2Int gridPosition, Vector2Int seedPosition, Color regionColour)
        {
            var colourHex = ColorUtility.ToHtmlStringRGBA(regionColour);

            var colourBlock =
                $"<mark=#{colourHex}><color=#00000000>MI</color></mark>\n" +
                 "--------------------";

            var header = $"Region - {colourBlock}";

            var content =
                $"GRID LOCATION: ROW - {gridPosition.x}, COL - {gridPosition.y}\n" +
                $"SEED LOCATION: {seedPosition}\n" +
                $"PIXEL LOCATION: {pixelPosition}";

            var backgroundColor = new Color(0f, 0f, 0f, 0.8f);

            TooltipSystem.Show(
                header: header,
                headerColor: Color.white,
                content: content,
                contentColor: Color.white,
                backgroundColor: backgroundColor);
        }

        private void HideHighlights()
        {
            if (_cellHighlightVisual != null)
            {
                _cellHighlightVisual.enabled = false;
            }

            if (_pixelHighlightVisual != null)
            {
                _pixelHighlightVisual.enabled = false;
            }
        }

        private static Color GetComplementaryColour(Color colour)
        {
            return new Color(1f - colour.r, 1f - colour.g, 1f - colour.b, colour.a);
        }

        private void D(string message, bool isError = false)
        {
            DebugUtils.DebugInfo.Print($"<<VoronoiRegionInteraction>> {message}", isError ? DebugUtils.DebugConstants.ERROR : DebugUtils.DebugConstants.INFO);
        }
    }
}