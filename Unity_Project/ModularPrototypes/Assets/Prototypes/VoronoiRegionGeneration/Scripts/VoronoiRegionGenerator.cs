using UnityEngine;
using UnityEngine.UI;

namespace ModularPrototypes.Voronoi.ProceduralGeneration.Region
{
    public class VoronoiRegionGenerator : MonoBehaviour
    {
        [SerializeField] private Vector2Int _mapDimension;
        [SerializeField] private int _cellSize = 10;

        private Vector2Int _gridDimension;
        private Vector2Int[,] _seedPositions;

        [SerializeField] private RawImage _mapVisual;
        private Texture2D _texture2D;


        [SerializeField] private Gradient _colourGradient;
        [SerializeField] private Color[,] _cellColours;

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            _mapVisual = GetComponent<RawImage>();

            GenerateVoronoiMap();
        }

        private void CalculateDimensions()
        {
            _gridDimension = new Vector2Int(_mapDimension.x / _cellSize, _mapDimension.y / _cellSize);
        }

        [ContextMenu("GenerateVoronoiMap()")]
        private void GenerateVoronoiMap()
        {
            CalculateDimensions();
            GenerateSeeds();
            GenerateMap();
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
            _texture2D = new Texture2D(_mapDimension.x, _mapDimension.y);
            _texture2D.filterMode = FilterMode.Point;

            PaintGeneratedSeedsWithGrid();

            SaveToMap();
        }

        private void PaintGeneratedSeeds()
        {
            PaintBackgroundWithGrid();
            PaintSeeds();
            GenerateVoronoiRegion();
        }

        private void PaintGeneratedSeedsWithGrid()
        {
            PaintBackgroundWithGrid(true);
            PaintSeeds();
            GenerateVoronoiRegion();
        }


        private void PaintBackgroundWithGrid(bool paintGrid = false)
        {
            for (int i = 0; i < _mapDimension.x; i++)
            {
                for (int j = 0; j < _mapDimension.y; j++)
                {
                    var isGridPixel = paintGrid && ((i > 0 && i % _cellSize == 0) || (j > 0 && j % _cellSize == 0));

                    var colour = isGridPixel ? Color.gray : Color.white;
                    //var colour = isGridPixel ? new Color(.25f, .25f, .25f, .25f) : new Color(1f, 1f, 1f, .25f);

                    _texture2D.SetPixel(i, j, colour);
                }
            }
        }

        [ContextMenu("PaintSeeds()")]
        private void VisualizeSeeds()
        {
            PaintSeeds(true);
        }

        private void PaintSeeds(bool applyChanges = false)
        {
            for (int i = 0; i < _gridDimension.x; i++)
            {
                for (int j = 0; j < _gridDimension.y; j++)
                {
                    var seedPosition = _seedPositions[i, j];

                    _texture2D.SetPixel(seedPosition.x, seedPosition.y, Color.black);
                }
            }

            if (applyChanges)
            {
                SaveToMap();
            }
        }

        private void GenerateVoronoiRegion()
        {
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

                    _texture2D.SetPixel(i, j, _cellColours[nearestCellPosition.x, nearestCellPosition.y]);
                }
            }
        }

        private void SaveToMap()
        {
            _texture2D.Apply();
            _mapVisual.texture = _texture2D;
        }
    }
}
