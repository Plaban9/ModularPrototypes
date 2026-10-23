using UnityEngine;

namespace ModularPrototypes.Voronoi.ProceduralGeneration.Region
{
    public struct VoronoiRegionPixelData
    {

        public Vector2Int pixelLocation;
        public Vector2Int gridLocation;
        public Vector2Int seedLocation;
        public Color regionColor;

        public VoronoiRegionPixelData(Vector2Int pixelLocation, Vector2Int gridLocation, Vector2Int seedLocation, Color regionColor)
        {
            this.pixelLocation = pixelLocation;
            this.gridLocation = gridLocation;
            this.seedLocation = seedLocation;
            this.regionColor = regionColor;
        }

        public override string ToString()
        {
            return $"Pixel Location: {pixelLocation}, Grid Location: {gridLocation}, Seed Location: {seedLocation}, Region Color: {regionColor}";
        }
    }
}
