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
            var aspectRatio = (float)currentMapDimension.x / currentMapDimension.y;

            if (aspectRatio < 1f)
            {
                zoom = Mathf.Clamp(aspectRatio, _runtimeSettings.MinimumZoom, _runtimeSettings.MaximumZoom);
            }
            //else if (aspectRatio > 1.8f)
            //{
            //    zoom = Mathf.Clamp(1/(aspectRatio - 1), _runtimeSettings.MinimumZoom, _runtimeSettings.MaximumZoom);
            //}

            _runtimeSettings.SetZoom(zoom);

            return Vector3.one * zoom;
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