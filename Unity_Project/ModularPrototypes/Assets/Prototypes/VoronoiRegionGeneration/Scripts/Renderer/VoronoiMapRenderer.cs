using UnityEngine;
using UnityEngine.UI;

using ModularPrototypes.Voronoi.ProceduralGeneration.Region.Data;

namespace ModularPrototypes.Voronoi.ProceduralGeneration.Region.Rendering
{
    public sealed class VoronoiMapRenderer : MonoBehaviour
    {
        [Header("Visuals")]
        [SerializeField] private RawImage _mapVisual;
        [SerializeField] private RawImage _gridVisual;
        [SerializeField] private RawImage _seedVisual;
        [SerializeField] private RawImage _borderVisual;

        [Header("Colours")]
        [SerializeField]
        private Color _gridColour = new Color(0f, 0f, 0f, 0.5f);

        [SerializeField]
        private Color _gridBorderColour = new Color(0.5f, 0.5f, 0.5f, 0.25f);

        [SerializeField] private Color _seedColour = Color.gray4;
        [SerializeField] private Color _borderColour = Color.black;

        public void Render(VoronoiMapData mapData, VoronoiMapGenerationSettings settings)
        {
            RenderMap(mapData);
            RenderGrid(mapData);
            RenderSeeds(mapData);
            RenderBorders(mapData);

            ApplyVisibility(settings);
        }

        private void RenderMap(VoronoiMapData mapData)
        {
            var texture = CreateTexture(mapData.MapDimension);

            for (int x = 0; x < mapData.MapDimension.x; x++)
            {
                for (int y = 0; y < mapData.MapDimension.y; y++)
                {
                    texture.SetPixel(x, y, mapData.GetRegionColour(new Vector2Int(x, y)));
                }
            }

            SaveTexture(texture, _mapVisual);
        }

        private static bool IsInternalGridLine(int coordinate, int dimension, int cellSize)
        {
            if (coordinate < 0 || coordinate > dimension - 1)
            {
                return false;
            }

            var remainder = coordinate % cellSize;

            return remainder == 0 || remainder == cellSize - 1;
        }

        private void RenderGrid(VoronoiMapData mapData)
        {
            var texture = CreateTexture(mapData.MapDimension);

            for (int x = 0; x < mapData.MapDimension.x; x++)
            {
                for (int y = 0; y < mapData.MapDimension.y; y++)
                {
                    var isVerticalLine = IsInternalGridLine(x, mapData.MapDimension.x, mapData.CellSize);

                    var isHorizontalLine = IsInternalGridLine(y, mapData.MapDimension.y, mapData.CellSize);

                    var isGridLine = isVerticalLine || isHorizontalLine;

                    texture.SetPixel(x, y, isGridLine ? _gridBorderColour : _gridColour);
                }
            }

            SaveTexture(texture, _gridVisual);
        }

        private void RenderSeeds(VoronoiMapData mapData)
        {
            var texture = CreateTexture(mapData.MapDimension);

            for (int x = 0; x < mapData.GridLayout.x; x++)
            {
                for (int y = 0; y < mapData.GridLayout.y; y++)
                {
                    var seedPosition = mapData.SeedPositions[x, y];

                    texture.SetPixel(seedPosition.x, seedPosition.y, _seedColour);
                }
            }

            SaveTexture(texture, _seedVisual);
        }

        private void RenderBorders(VoronoiMapData mapData)
        {
            var texture = CreateTexture(mapData.MapDimension);

            for (int x = 0; x < mapData.MapDimension.x; x++)
            {
                for (int y = 0; y < mapData.MapDimension.y; y++)
                {
                    var currentRegion = mapData.RegionMap[x, y];

                    var isBorder = IsDifferentRegion(mapData, x - 1, y, currentRegion) || IsDifferentRegion(mapData, x + 1, y, currentRegion) || IsDifferentRegion(mapData, x, y - 1, currentRegion) || IsDifferentRegion(mapData, x, y + 1, currentRegion);

                    if (isBorder)
                    {
                        texture.SetPixel(x, y, _borderColour);
                    }
                }
            }

            SaveTexture(texture, _borderVisual);
        }

        private static bool IsDifferentRegion(VoronoiMapData mapData, int x, int y, Vector2Int currentRegion)
        {
            if (x < 0 || y < 0 || x >= mapData.MapDimension.x || y >= mapData.MapDimension.y)
            {
                return false;
            }

            return mapData.RegionMap[x, y] != currentRegion;
        }

        private static Texture2D CreateTexture(Vector2Int dimension)
        {
            var texture = new Texture2D(dimension.x, dimension.y, TextureFormat.RGBA32, false);

            texture.filterMode = FilterMode.Point;
            texture.wrapMode = TextureWrapMode.Clamp;

            var pixels = new Color[dimension.x * dimension.y];
            texture.SetPixels(pixels);

            return texture;
        }

        public void ApplyVisibility(VoronoiMapGenerationSettings settings)
        {
            if (_mapVisual != null)
            {
                _mapVisual.enabled = settings.ShowGeneratedMap;
            }

            if (_gridVisual != null)
            {
                _gridVisual.enabled = settings.ShowGrid;
            }

            if (_seedVisual != null)
            {
                _seedVisual.enabled = settings.ShowSeeds;
            }

            if (_borderVisual != null)
            {
                _borderVisual.enabled = settings.ShowRegionBorders;
            }
        }

        private static void SaveTexture(Texture2D texture, RawImage rawImage)
        {
            if (rawImage == null)
            {
                Destroy(texture);
                return;
            }

            texture.Apply();
            rawImage.texture = texture;
        }
    }
}