using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace ModularPrototypes.Voronoi.ProceduralGeneration.Region
{
    public class VoronoiRegionGenerator : MonoBehaviour
    {
        [SerializeField] private Vector2Int _mapDimension;
        [SerializeField] private int _cellSize = 10;

        private Vector2Int _gridDimension;
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
            _gridDimension = new Vector2Int(_mapDimension.x / _cellSize, _mapDimension.y / _cellSize);
        }

        private void GenerateSeeds()
        {
            _seedPositions = new Vector2Int[_gridDimension.x, _gridDimension.y];
            _cellColours = new Color[_gridDimension.x, _gridDimension.y];

            for (int x = 0; x < _gridDimension.x; x++)
            {
                for (int y = 0; y < _gridDimension.y; y++)
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


            for (int i = 0; i < _gridDimension.x; i++)
            {
                for (int j = 0; j < _gridDimension.y; j++)
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

            for (int i = 0; i < _mapDimension.x; i++)
            {
                for (int j = 0; j < _mapDimension.y; j++)
                {
                    var currentGrid = new Vector2Int(i / _cellSize, j / _cellSize);

                    var nearestDistance = Mathf.Infinity;
                    var nearestCellPosition = new Vector2Int();

                    for (int x = -1; x < 2; x++)
                    {
                        for (int y = -1; y < 2; y++)
                        {
                            int posX = x + currentGrid.x;
                            int posY = y + currentGrid.y;

                            if (posX < 0 || posY < 0 || posX >= _gridDimension.x || posY >= _gridDimension.y)
                            {
                                continue;
                            }

                            var distance = Vector2Int.Distance(new Vector2Int(i, j), _seedPositions[posX, posY]);

                            if (distance < nearestDistance)
                            {
                                nearestDistance = distance;
                                nearestCellPosition = new Vector2Int(posX, posY);
                            }
                        }
                    }

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
    }
}
