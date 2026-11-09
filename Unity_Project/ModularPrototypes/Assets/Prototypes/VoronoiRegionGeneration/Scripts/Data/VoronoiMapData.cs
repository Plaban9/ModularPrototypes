using UnityEngine;

namespace ModularPrototypes.Voronoi.ProceduralGeneration.Region.Data
{
    public sealed class VoronoiMapData
    {
        public Vector2Int MapDimension
        {
            get;
        }
        public int CellSize
        {
            get;
        }
        public Vector2Int GridLayout
        {
            get;
        }
        public Vector2Int[,] SeedPositions
        {
            get;
        }
        public Vector2Int[,] RegionMap
        {
            get;
        }
        public Color[,] RegionColours
        {
            get;
        }

        public VoronoiMapData(Vector2Int mapDimension, int cellSize, Vector2Int[,] seedPositions, Vector2Int[,] regionMap, Color[,] regionColours)
        {
            MapDimension = mapDimension;
            CellSize = cellSize;
            GridLayout = new Vector2Int(mapDimension.x / cellSize, mapDimension.y / cellSize);

            SeedPositions = seedPositions;
            RegionMap = regionMap;
            RegionColours = regionColours;
        }

        public Vector2Int GetRegionAt(Vector2Int pixelPosition)
        {
            return RegionMap[pixelPosition.x, pixelPosition.y];
        }

        public Vector2Int GetSeedPosition(Vector2Int pixelPosition)
        {
            var region = GetRegionAt(pixelPosition);

            return SeedPositions[region.x, region.y];
        }

        public Color GetRegionColour(Vector2Int pixelPosition)
        {
            var region = GetRegionAt(pixelPosition);

            return RegionColours[region.x, region.y];
        }
    }
}