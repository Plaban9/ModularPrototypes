using System;

using UnityEngine;
using UnityEngine.EventSystems;

namespace ModularPrototypes.Voronoi.ProceduralGeneration.Region.UI
{
    public class RawImageCoordinateEmitter : MonoBehaviour, IPointerMoveHandler, IPointerExitHandler
    {
        [SerializeField] private RectTransform _rawImageRectTransform;
        [SerializeField] private Vector2Int _coordinateDimensions;

        public event Action<Vector2Int, Vector2Int> PointerMoved;
        public event Action PointerExited;

        private void Awake()
        {
            if (_rawImageRectTransform == null)
            {
                _rawImageRectTransform = GetComponent<RectTransform>();
            }

            _coordinateDimensions = new Vector2Int((int)_rawImageRectTransform.rect.width, (int)_rawImageRectTransform.rect.height);
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            if (TryGetCoordinate(eventData.position, out var coordinate))
            {
                PointerMoved?.Invoke(coordinate, _coordinateDimensions);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            PointerExited?.Invoke();
        }

        private bool TryGetCoordinate(Vector2 screenPosition, out Vector2Int coordinate)
        {
            coordinate = default;

            var rawImage = GetComponent<RectTransform>();
            var canvas = GetComponentInParent<Canvas>();

            var eventCamera = canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay ? canvas.worldCamera : null;

            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rawImage, screenPosition, eventCamera, out var localPosition))
            {
                return false;
            }

            var rect = rawImage.rect;

            if (!rect.Contains(localPosition))
            {
                return false;
            }

            var normalizedPosition = new Vector2(Mathf.InverseLerp(rect.xMin, rect.xMax, localPosition.x), Mathf.InverseLerp(rect.yMin, rect.yMax, localPosition.y));

            coordinate = new Vector2Int(Mathf.Clamp(Mathf.FloorToInt(normalizedPosition.x * _coordinateDimensions.x), 0, _coordinateDimensions.x - 1), Mathf.Clamp(Mathf.FloorToInt(normalizedPosition.y * _coordinateDimensions.y), 0, _coordinateDimensions.y - 1));

            return true;
        }

        private static void D(string message)
        {
            DebugUtils.DebugInfo.Print("<<RawImageCoordinateEmitter>> " + message);
        }
    }
}
