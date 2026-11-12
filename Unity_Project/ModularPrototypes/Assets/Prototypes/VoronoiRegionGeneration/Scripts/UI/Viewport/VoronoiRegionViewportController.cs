using System;

using UnityEngine;

using ModularPrototypes.Voronoi.ProceduralGeneration.Region.UI.Viewport.Data;
using ModularPrototypes.Voronoi.ProceduralGeneration.Region.Data;

namespace ModularPrototypes.Voronoi.ProceduralGeneration.Region.UI.Viewport
{
    public sealed class VoronoiRegionViewportController : MonoBehaviour
    {
        [SerializeField] private RectTransform _mapTransform;
        [SerializeField] private ViewportSettings _settingsTemplate;
        [SerializeField] private UIManager _uiManager;

        private Vector2Int _mapDimension = Vector2Int.one;
        public Vector2Int MapDimension => _mapDimension;


        private ViewportSettings _runtimeSettings;

        public event Action<float, float> ZoomChanged;

        public float CurrentZoom => _runtimeSettings != null ? _runtimeSettings.CurrentZoom : 1f;

        public float ZoomStep => _runtimeSettings != null ? _runtimeSettings.ZoomStep : 0f;

        // For current setup it is the threshold where it requires zoom less than 1f to fit screen in landscape mode.
        private const float LandscapeStartAspectRatioThreshold = 1.8f; 

        private void Awake()
        {
            if (_mapTransform == null)
            {
                _mapTransform = GetComponent<RectTransform>();
            }

            if (_settingsTemplate == null)
            {
                D("<<VoronoiRegionViewportController>> Settings template is missing.");

                return;
            }

            _runtimeSettings = _settingsTemplate.CreateRuntimeCopy();

            ApplyTransform();
        }

        public void Zoom(float amount)
        {
            if (_runtimeSettings == null)
            {
                return;
            }

            _runtimeSettings.SetZoom(_runtimeSettings.CurrentZoom + amount);

            ApplyTransform(true);
            NotifyZoomChanged();
        }

        private void NotifyZoomChanged()
        {
            if (_runtimeSettings == null)
            {
                return;
            }

            var normalizedZoom = Mathf.InverseLerp(_runtimeSettings.MinimumZoom, _runtimeSettings.MaximumZoom, _runtimeSettings.CurrentZoom);

            ZoomChanged?.Invoke(_runtimeSettings.CurrentZoom, normalizedZoom);
        }

        public void Pan(Vector2 delta)
        {
            if (_runtimeSettings == null)
            {
                return;
            }

            _runtimeSettings.SetPan(_runtimeSettings.CurrentPan + delta);

            ApplyTransform();
        }

        public void ResetViewport()
        {
            if (_runtimeSettings == null)
            {
                return;
            }

            _runtimeSettings.ResetRuntimeState();

            ApplyTransform();
            NotifyZoomChanged();
        }

        private void ApplyTransform(bool isScrollOperation = false)
        {
            if (_mapTransform == null || _runtimeSettings == null)
            {
                return;
            }

            _mapTransform.localScale = isScrollOperation ? Vector3.one * _runtimeSettings.CurrentZoom : CalculateRelativeZoom();
            _mapTransform.anchoredPosition = _runtimeSettings.CurrentPan;
        }

        private Vector3 CalculateRelativeZoom()
        {
            var currentMapDimension = _mapDimension;
            var zoom = _runtimeSettings.CurrentZoom;
            var aspectRatio = (float)currentMapDimension.x / currentMapDimension.y; // Calculated aspect ratio since it is actually the zoom factor for portrait mode

            if (aspectRatio < 1f)
            {
                zoom = CalculateSteppedZoom(aspectRatio);
            }
            else if (aspectRatio > LandscapeStartAspectRatioThreshold)
            {
                zoom = CalculateSteppedZoom(CalculateLandscapeZoom(aspectRatio));
            }

            _runtimeSettings.SetZoom(zoom);

            return Vector3.one * zoom;
        }

        // Based on the threshold formula. It caluclates from the realtion where y = Threshold / x.
        private float CalculateLandscapeZoom(float aspectRatio)
        {
            return LandscapeStartAspectRatioThreshold / aspectRatio;
        }

        // This method calculates the stepped zoom for based on the aspect ratio. Adjusts it to the nearest step defined in the runtime settings.
        private float CalculateSteppedZoom(float calculatedZoom)
        {
            if (_runtimeSettings == null)
            {
                return 1f;
            }

            var minimumZoom = _runtimeSettings.MinimumZoom;
            var maximumZoom = _runtimeSettings.MaximumZoom;
            var zoomStep = _runtimeSettings.ZoomStep;

            if (zoomStep <= 0f)
            {
                return Mathf.Clamp(calculatedZoom, minimumZoom, maximumZoom);
            }

            var steps = Mathf.Floor((calculatedZoom - minimumZoom) / zoomStep);
            var steppedZoom = minimumZoom + (steps * zoomStep);

            return Mathf.Clamp(steppedZoom, minimumZoom, maximumZoom);
        }

        public void SetMapDimensions(Vector2Int mapDimension)
        {
            if (mapDimension.x <= 0 || mapDimension.y <= 0)
            {
                return;
            }

            _mapDimension = mapDimension;
        }

        private void OnDestroy()
        {
            if (_runtimeSettings != null)
            {
                Destroy(_runtimeSettings);
            }
        }

        private void OnEnable()
        {
            if (_uiManager != null)
            {
                _uiManager.ResetRequested += OnResetRequested;
            }
        }

        private void OnDisable()
        {
            if (_uiManager != null)
            {
                _uiManager.ResetRequested -= OnResetRequested;
            }
        }

        private void OnResetRequested(VoronoiMapGenerationSettings settings)
        {
            ResetViewport();
        }

        private static void D(string message)
        {
            DebugUtils.DebugInfo.Print("<<VoronoiRegionViewportController>> " + message);
        }
    }
}