using System;

using UnityEngine;

using ModularPrototypes.Voronoi.ProceduralGeneration.Region.Data;
using Random = UnityEngine.Random;

namespace ModularPrototypes.Voronoi.ProceduralGeneration.Region.Generation
{
    public sealed class VoronoiMapGenerator
    {
        public VoronoiMapData Generate(VoronoiMapGenerationSettings settings, int randomSeed)
        {
            if (settings == null)
            {
                throw new System.ArgumentNullException(nameof(settings));
            }

            var mapDimension = settings.MapDimension;
            var cellSize = settings.CellSize;
            var gridLayout = new Vector2Int(mapDimension.x / cellSize, mapDimension.y / cellSize);

            var random = new System.Random(randomSeed);

            var seedPositions = GenerateSeeds(gridLayout, cellSize, settings.ColourGradient, out var regionColours);

            var regionMap = GenerateRegions(mapDimension, cellSize, gridLayout, seedPositions);

            return new VoronoiMapData(mapDimension, cellSize, seedPositions, regionMap, regionColours);
        }

        private static Vector2Int[,] GenerateSeeds(Vector2Int gridLayout, int cellSize, Gradient colourGradient, out Color[,] regionColours)
        {
            var seedPositions = new Vector2Int[gridLayout.x, gridLayout.y];

            regionColours = new Color[gridLayout.x, gridLayout.y];

            for (int x = 0; x < gridLayout.x; x++)
            {
                for (int y = 0; y < gridLayout.y; y++)
                {
                    seedPositions[x, y] = new Vector2Int((x * cellSize) + Random.Range(0, cellSize), (y * cellSize) + Random.Range(0, cellSize));

                    regionColours[x, y] = colourGradient != null ? colourGradient.Evaluate(Random.Range(0f, 1f)) : Color.white;
                }
            }

            return seedPositions;
        }

        private static Vector2Int[,] GenerateRegions(Vector2Int mapDimension, int cellSize, Vector2Int gridLayout, Vector2Int[,] seedPositions)
        {
            var regionMap = new Vector2Int[mapDimension.x, mapDimension.y];

            for (int x = 0; x < mapDimension.x; x++)
            {
                for (int y = 0; y < mapDimension.y; y++)
                {
                    var currentGrid = new Vector2Int(x / cellSize, y / cellSize);

                    var nearestDistance = float.MaxValue;
                    var nearestRegion = Vector2Int.zero;

                    for (int offsetX = -1; offsetX <= 1; offsetX++)
                    {
                        for (int offsetY = -1; offsetY <= 1; offsetY++)
                        {
                            var regionX = currentGrid.x + offsetX;
                            var regionY = currentGrid.y + offsetY;

                            if (regionX < 0 || regionY < 0 || regionX >= gridLayout.x || regionY >= gridLayout.y)
                            {
                                continue;
                            }

                            var seedPosition = seedPositions[regionX, regionY];

                            var pixelPosition = new Vector2Int(x, y);
                            var distance = (pixelPosition - seedPosition).sqrMagnitude;

                            if (distance < nearestDistance)
                            {
                                nearestDistance = distance;
                                nearestRegion = new Vector2Int(regionX, regionY);
                            }
                        }
                    }

                    regionMap[x, y] = nearestRegion;
                }
            }

            return regionMap;
        }
    }
}