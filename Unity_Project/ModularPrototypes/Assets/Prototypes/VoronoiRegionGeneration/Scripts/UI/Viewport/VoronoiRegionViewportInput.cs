using ModularPrototypes.Voronoi.ProceduralGeneration.Region.Data;

using System;

using UnityEngine;
using UnityEngine.InputSystem;
using ModularPrototypes.Voronoi.ProceduralGeneration.Region.UI;
using ModularPrototypes.Voronoi.ProceduralGeneration.Region.Data;

namespace ModularPrototypes.Voronoi.ProceduralGeneration.Region.UI.Viewport
{
    public sealed class VoronoiRegionViewportInput : MonoBehaviour
    {
        [SerializeField] private VoronoiRegionViewportController _controller;
        [SerializeField] private RectTransform _panSpace;
        [SerializeField] private RectTransform _dragIcon;
        [SerializeField] private UIManager _uiManager;

        private bool _isPanning;
        private bool _cursorWasVisible;
        private Vector2 _lastPointerPosition;

        public event Action<bool> PanStateChanged;

        private void Update()
        {
            if (_controller == null || Mouse.current == null)
            {
                return;
            }

            var mouse = Mouse.current;
            var screenPosition = mouse.position.ReadValue();

            HandleZoom(mouse);
            HandlePan(mouse, screenPosition);
        }

        private void HandleZoom(Mouse mouse)
        {
            var scrollDelta = mouse.scroll.ReadValue().y;

            if (Mathf.Approximately(scrollDelta, 0f))
            {
                return;
            }

            _controller.Zoom(scrollDelta * _controller.ZoomStep);
        }

        private void HandlePan(Mouse mouse, Vector2 screenPosition)
        {
            if (mouse.rightButton.wasPressedThisFrame)
            {
                BeginPan(screenPosition);
            }

            if (_isPanning && mouse.rightButton.isPressed)
            {
                UpdatePan(screenPosition);
            }

            if (mouse.rightButton.wasReleasedThisFrame)
            {
                EndPan();
            }
        }

        private void BeginPan(Vector2 screenPosition)
        {
            if (!TryGetPanSpacePosition(screenPosition, out _lastPointerPosition))
            {
                return;
            }

            _isPanning = true;
            _cursorWasVisible = Cursor.visible;

            Cursor.visible = false;

            PanStateChanged?.Invoke(true);

            SetDragIconVisible(true);
            SetDragIconPosition(screenPosition);
        }

        private void UpdatePan(Vector2 screenPosition)
        {
            if (!TryGetPanSpacePosition(screenPosition, out var currentPointerPosition))
            {
                return;
            }

            var delta = currentPointerPosition - _lastPointerPosition;

            _lastPointerPosition = currentPointerPosition;

            _controller.Pan(delta);
            SetDragIconPosition(screenPosition);
        }

        private void EndPan()
        {
            if (!_isPanning)
            {
                return;
            }

            _isPanning = false;

            Cursor.visible = _cursorWasVisible;

            PanStateChanged?.Invoke(false);

            SetDragIconVisible(false);
        }

        private bool TryGetPanSpacePosition(Vector2 screenPosition, out Vector2 localPosition)
        {
            localPosition = default;

            if (_panSpace == null)
            {
                return false;
            }

            var canvas = _panSpace.GetComponentInParent<Canvas>();

            var eventCamera = canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay ? canvas.worldCamera : null;

            return RectTransformUtility.ScreenPointToLocalPointInRectangle(_panSpace, screenPosition, eventCamera, out localPosition);
        }

        private void SetDragIconPosition(Vector2 screenPosition)
        {
            if (_dragIcon == null)
            {
                return;
            }

            var iconParent = _dragIcon.parent as RectTransform;

            if (iconParent == null)
            {
                return;
            }

            var canvas = iconParent.GetComponentInParent<Canvas>();

            var eventCamera = canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay ? canvas.worldCamera : null;

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(iconParent, screenPosition, eventCamera, out var localPosition))
            {
                _dragIcon.anchoredPosition = localPosition;
            }
        }

        private void SetDragIconVisible(bool isVisible)
        {
            if (_dragIcon != null)
            {
                _dragIcon.gameObject.SetActive(isVisible);
            }
        }

        private void OnResetRequested(VoronoiMapGenerationSettings settings)
        {
            EndPan();
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

            EndPan();
        }
    }
}