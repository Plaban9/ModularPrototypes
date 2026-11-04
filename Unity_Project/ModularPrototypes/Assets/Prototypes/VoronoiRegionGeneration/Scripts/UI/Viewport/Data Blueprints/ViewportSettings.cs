using UnityEngine;

namespace ModularPrototypes.Voronoi.ProceduralGeneration.Region.UI.Viewport.Data
{
    [CreateAssetMenu(fileName = "ViewportSettings", menuName = "Data/Voronoi/Region/UI/Viewport/Viewport Settings")]
    public sealed class ViewportSettings : ScriptableObject
    {
        private const float MinimumAllowedZoom = 0.01f;

        [Header("Zoom")]
        [Min(MinimumAllowedZoom)]
        [SerializeField] private float _minimumZoom = 0.5f;

        [Min(MinimumAllowedZoom)]
        [SerializeField] private float _maximumZoom = 4f;

        [Min(MinimumAllowedZoom)]
        [SerializeField] private float _zoomStep = 0.1f;

        [Min(MinimumAllowedZoom)]
        [SerializeField] private float _initialZoom = 1f;

        [Header("Pan")]
        [SerializeField] private bool _constrainPan = true;
        [SerializeField] private Vector2 _minimumPan;
        [SerializeField] private Vector2 _maximumPan;
        [SerializeField] private Vector2 _initialPan;

        private float _currentZoom;
        private Vector2 _currentPan;

        public float MinimumZoom
        {
            get => Mathf.Max(MinimumAllowedZoom, _minimumZoom);
        }

        public float MaximumZoom
        {
            get => Mathf.Max(MinimumZoom, _maximumZoom);
        }

        public float ZoomStep
        {
            get => Mathf.Max(MinimumAllowedZoom, _zoomStep);
        }

        public float InitialZoom
        {
            get => Mathf.Clamp(_initialZoom, MinimumZoom, MaximumZoom);
        }

        public bool ConstrainPan => _constrainPan;

        public Vector2 MinimumPan => _minimumPan;

        public Vector2 MaximumPan
        {
            get => new Vector2(Mathf.Max(_minimumPan.x, _maximumPan.x), Mathf.Max(_minimumPan.y, _maximumPan.y));
        }

        public Vector2 InitialPan
        {
            get => ConstrainPan ? ClampPan(_initialPan) : _initialPan;
        }

        public float CurrentZoom
        {
            get => _currentZoom;
            private set => _currentZoom = Mathf.Clamp(value, MinimumZoom, MaximumZoom);
        }

        public Vector2 CurrentPan
        {
            get => _currentPan;
            private set => _currentPan = ConstrainPan ? ClampPan(value) : value;
        }

        private void OnValidate()
        {
            _minimumZoom = Mathf.Max(MinimumAllowedZoom, _minimumZoom);

            _maximumZoom = Mathf.Max(_minimumZoom, _maximumZoom);

            _zoomStep = Mathf.Clamp(_zoomStep, MinimumAllowedZoom, _maximumZoom - _minimumZoom > 0f ? _maximumZoom - _minimumZoom : MinimumAllowedZoom);

            _initialZoom = Mathf.Clamp(_initialZoom, _minimumZoom, _maximumZoom);

            _maximumPan = new Vector2(Mathf.Max(_minimumPan.x, _maximumPan.x), Mathf.Max(_minimumPan.y, _maximumPan.y));

            _initialPan = ClampPan(_initialPan);
        }

        public ViewportSettings CreateRuntimeCopy()
        {
            var runtimeSettings = Instantiate(this);
            runtimeSettings.ResetRuntimeState();

            return runtimeSettings;
        }

        public void ResetRuntimeState()
        {
            CurrentZoom = InitialZoom;
            CurrentPan = InitialPan;
        }

        public void SetZoom(float zoom)
        {
            CurrentZoom = zoom;
        }

        public void IncreaseZoom()
        {
            SetZoom(CurrentZoom + ZoomStep);
        }

        public void DecreaseZoom()
        {
            SetZoom(CurrentZoom - ZoomStep);
        }

        public void SetPan(Vector2 pan)
        {
            CurrentPan = pan;
        }

        private Vector2 ClampPan(Vector2 pan)
        {
            var maximumPan = MaximumPan;

            return new Vector2(Mathf.Clamp(pan.x, MinimumPan.x, maximumPan.x), Mathf.Clamp(pan.y, MinimumPan.y, maximumPan.y));
        }
    }
}