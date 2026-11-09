using UnityEngine;

namespace ModularPrototypes.Voronoi.ProceduralGeneration.Region
{
    public struct VoronoiRegionPixelData
    {
        public Vector2Int PixelLocation
        {
            get;
        }
        public Vector2Int GridLocation
        {
            get;
        }
        public Vector2Int SeedLocation
        {
            get;
        }
        public Color RegionColor
        {
            get;
        }

        public VoronoiRegionPixelData(Vector2Int pixelLocation, Vector2Int gridLocation, Vector2Int seedLocation, Color regionColor)
        {
            this.PixelLocation = pixelLocation;
            this.GridLocation = gridLocation;
            this.SeedLocation = seedLocation;
            this.RegionColor = regionColor;
        }

        public override string ToString()
        {
            return $"Pixel Location: {PixelLocation}, Grid Location: {GridLocation}, Seed Location: {SeedLocation}, Region Color: {RegionColor}";
        }
    }
}
