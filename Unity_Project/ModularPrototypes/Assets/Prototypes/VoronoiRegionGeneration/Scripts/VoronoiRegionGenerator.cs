using UnityEngine;
using UnityEngine.UI;

namespace ModularPrototypes.Voronoi.ProceduralGeneration.Region
{
    public class VoronoiRegionGenerator : MonoBehaviour
    {
        [SerializeField] private Vector2Int _mapDimension;
        [SerializeField] private int _cellSize = 10;

        private Vector2Int _gridLayout;
        private Vector2Int[,] _seedPositions;

        [SerializeField] private Gradient _colourGradient;
        [SerializeField] private Color[,] _cellColours;

        #region Visual
        [SerializeField] private RawImage _seedVisual;
        [SerializeField] private RawImage _gridVisual;
        [SerializeField] private RawImage _mapVisual;
        #endregion

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
                    _seedPositions[x, y] = new Vector2Int(x * _cellSize + Random.Range(1, _cellSize), y * _cellSize + Random.Range(1, _cellSize));

                    _cellColours[x, y] = _colourGradient.Evaluate(Random.Range(0f, 1f));
                }
            }
        }

        private void GenerateMap()
        {
            GenerateVoronoiRegion();
        }
        #endregion

        #region Visual Logic
        private void PaintGeneration()
        {
            PaintGrid();
            PaintSeeds();
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
                    var colour = isGridPixel ? new Color(.5f, .5f, .5f, .25f) : new Color(0f, 0f, 0f, .5f);

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

                    texture2D.SetPixel(seedPosition.x, seedPosition.y, Color.black);
                }
            }

            SaveToMap(texture2D, _seedVisual);
        }

        private void PaintVoronoiMap()
        {
            GenerateVoronoiRegion();
        }

        [ContextMenu("ToggleMapVisual()")]
        private void ToggleMapVisual()
        {
            _mapVisual.enabled = !_mapVisual.enabled;
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

        private void GenerateVoronoiRegion()
        {
            var texture2D = new Texture2D(_mapDimension.x, _mapDimension.y);
            texture2D.filterMode = FilterMode.Point;


            // Iteration of 2D to determine the region closeness towards the seed of each pixel
            for (int i = 0; i < _mapDimension.x; i++)
            {
                for (int j = 0; j < _mapDimension.y; j++)
                {
                    // Locate Current Grid for the Pixel
                    var currentGrid = new Vector2Int(i / _cellSize, j / _cellSize);

                    var nearestDistance = Mathf.Infinity;
                    var nearestCellPosition = new Vector2Int();

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
                            var distance = Vector2Int.Distance(new Vector2Int(i, j), _seedPositions[posX, posY]);

                            if (distance < nearestDistance)
                            {
                                nearestDistance = distance;
                                nearestCellPosition = new Vector2Int(posX, posY);
                            }
                        }
                    }

                    // Assign Colour of the nearest Seed
                    texture2D.SetPixel(i, j, _cellColours[nearestCellPosition.x, nearestCellPosition.y]);
                }
            }

            SaveToMap(texture2D, _mapVisual);
        }

        #region Utilities
        private void SaveToMap(Texture2D texture2D, RawImage rawImage)
        {
            texture2D.Apply();
            rawImage.texture = texture2D;
        }
        #endregion

        #region UI
        [ContextMenu("OnHover()")]
        private void OnHover()
        {
            OnHover(x: 40);
        }

        private void OnHover(int x = 30, int y = 30)
        {
            var currentGrid = new Vector2Int(x / _cellSize, y / _cellSize);
            var seedPosition = _seedPositions[currentGrid.x, currentGrid.y];
            var regionColor = _cellColours[currentGrid.x, currentGrid.y]; // TODO: Fix this to locate after Voronoi Region Generation

            var pixelData = new VoronoiRegionPixelData(new Vector2Int(x, y), currentGrid, seedPosition, regionColor);

            Debug.Log(pixelData);
        }
        #endregion
    }
}
