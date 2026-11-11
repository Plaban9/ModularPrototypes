using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace ModularPrototypes.Voronoi.ProceduralGeneration.Region.UI.Viewport
{
    [RequireComponent(typeof(CanvasGroup))]
    public sealed class VoronoiRegionScaleVisualizer : MonoBehaviour
    {
        [SerializeField] private VoronoiRegionViewportController _controller;
        [SerializeField] private Slider _scaleSlider;
        [SerializeField] private TMP_Text _zoomText;

        [Header("Display")]
        [SerializeField] private float _visibleDuration = 1f;
        [SerializeField] private float _fadeDuration = 0.25f;

        private CanvasGroup _canvasGroup;
        private Coroutine _hideCoroutine;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0f;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;

            if (_scaleSlider != null)
            {
                _scaleSlider.minValue = 0f;
                _scaleSlider.maxValue = 1f;
                _scaleSlider.interactable = false;
            }
        }

        private void OnEnable()
        {
            if (_controller != null)
            {
                _controller.ZoomChanged += OnZoomChanged;
            }
        }

        private void OnDisable()
        {
            if (_controller != null)
            {
                _controller.ZoomChanged -= OnZoomChanged;
            }

            if (_hideCoroutine != null)
            {
                StopCoroutine(_hideCoroutine);
                _hideCoroutine = null;
            }
        }

        private void OnZoomChanged(float currentZoom, float normalizedZoom)
        {
            if (_scaleSlider != null)
            {
                _scaleSlider.SetValueWithoutNotify(normalizedZoom);
            }

            if (_zoomText != null)
            {
                _zoomText.text = $"{currentZoom:0.00}x";
            }

            ShowVisualizer();
        }

        private void ShowVisualizer()
        {
            if (_hideCoroutine != null)
            {
                StopCoroutine(_hideCoroutine);
            }

            _hideCoroutine = StartCoroutine(FadeAfterDelay());
        }

        private System.Collections.IEnumerator FadeAfterDelay()
        {
            _canvasGroup.alpha = 1f;

            yield return new WaitForSeconds(_visibleDuration);

            var elapsedTime = 0f;
            var initialAlpha = _canvasGroup.alpha;

            while (elapsedTime < _fadeDuration)
            {
                elapsedTime += Time.deltaTime;

                var progress = Mathf.Clamp01(elapsedTime / _fadeDuration);

                _canvasGroup.alpha = Mathf.Lerp(initialAlpha, 0f, progress);

                yield return null;
            }

            _canvasGroup.alpha = 0f;
            _hideCoroutine = null;
        }
    }
}