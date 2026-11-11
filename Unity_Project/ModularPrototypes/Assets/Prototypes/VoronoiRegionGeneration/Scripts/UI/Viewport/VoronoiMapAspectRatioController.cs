using UnityEngine;

namespace ModularPrototypes.Voronoi.ProceduralGeneration.Region.UI.Viewport
{
    public sealed class VoronoiMapAspectRatioController : MonoBehaviour
    {
        [SerializeField] private RectTransform _mapTransform;

        private Vector3 _initialScale;

        private void Awake()
        {
            if (_mapTransform == null)
            {
                _mapTransform = GetComponent<RectTransform>();
            }

            _initialScale = _mapTransform.localScale;
            D("Awake: " + _initialScale.ToString());
        }

        public void ApplyMapAspectRatio(Vector2Int mapDimension)
        {
            if (_mapTransform == null || mapDimension.x <= 0 || mapDimension.y <= 0)
            {
                return;
            }

            var baseSize = _mapTransform.rect.size;

            if (baseSize.x <= 0f || baseSize.y <= 0f)
            {
                return;
            }

            var mapAspectRatio = (float)mapDimension.x / mapDimension.y;

            var objectAspectRatio = baseSize.x / baseSize.y;

            var aspectScale = Vector2.one;

            if (mapAspectRatio > objectAspectRatio)
            {
                aspectScale.x = mapAspectRatio / objectAspectRatio;
            }
            else
            {
                aspectScale.y = objectAspectRatio / mapAspectRatio;
            }

            D($"ApplyMapAspectRatio: mapDimension={mapDimension}, baseSize={baseSize}, mapAspectRatio={mapAspectRatio}, objectAspectRatio={objectAspectRatio}, aspectScale={aspectScale}");

            _mapTransform.localScale = new Vector3(_initialScale.x * aspectScale.x, _initialScale.y * aspectScale.y, _initialScale.z);
        }

        private static void D(string message)
        {
            DebugUtils.DebugInfo.Print("<<VoronoiMapAspectRatioController>> " + message);
        }
    }
}