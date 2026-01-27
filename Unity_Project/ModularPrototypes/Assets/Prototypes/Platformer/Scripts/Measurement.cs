using System.Collections.Generic;

using UnityEngine;

namespace ModularPrototypes.Platformer.Measurements
{
    [System.Serializable]
    public struct Bound
    {
        public float min;
        public float max;
    }

    public class Measurement : MonoBehaviour
    {
        [Header("------------------------")]
        [Header("AXIS")]
        [Header("------------------------")]
        [Range(0f, 1f)]
        [SerializeField] private float _normalizedAxisSize;
        [SerializeField] private Bound _minMaxAxis;
        [SerializeField] private GameObject _centralAxis;
        [SerializeField] private List<SpriteRenderer> _axisSpriteRenderers;

        [Header("------------------------")]
        [Header("BOUNDS")]
        [Header("------------------------")]
        [SerializeField] private bool _showReferenceBounds = true;
        [SerializeField] private GameObject _referenceBounds;
        [Range(0f, 1f)]
        [SerializeField] private float _normalizedBoundsSize;
        [SerializeField] private Bound _minMaxBoundsLength;
        [Range(0.1f, 18f)]
        [SerializeField] private float _boundsGap = 3.5f;
        [SerializeField] private List<GameObject> _bounds;
        [SerializeField] private List<SpriteRenderer> _boundSpriteRenderers;

        private void Awake()
        {
            InitializeLists();
        }

        private void Update()
        {
            UpdateLists();
            ToggleReferenceBounds();
        }

        private void ToggleReferenceBounds()
        {
            if (_referenceBounds != null && _referenceBounds.activeInHierarchy != _showReferenceBounds)
            {
                _referenceBounds.SetActive(_showReferenceBounds);
            }
        }

        private void InitializeLists()
        {
            InitializeRenderers(ref _axisSpriteRenderers, _centralAxis);
            InitializeRenderers(ref _boundSpriteRenderers, _bounds);
        }

        private void InitializeRenderers(ref List<SpriteRenderer> listToInitialize, GameObject parentGameobject)
        {
            listToInitialize = new List<SpriteRenderer>();

            foreach (var renderer in parentGameobject.GetComponentsInChildren<SpriteRenderer>())
            {
                listToInitialize.Add(renderer);
            }
        }

        private void InitializeRenderers(ref List<SpriteRenderer> listToInitialize, List<GameObject> parentGameobjects)
        {
            listToInitialize = new List<SpriteRenderer>();

            foreach (var gameobject in parentGameobjects)
            {
                foreach (var renderer in gameobject.GetComponentsInChildren<SpriteRenderer>())
                {
                    listToInitialize.Add(renderer);
                }
            }
        }

        private void UpdateLists()
        {
            UpdateList(_axisSpriteRenderers, _minMaxAxis, _normalizedAxisSize);
            UpdateList(_boundSpriteRenderers, _minMaxBoundsLength, _normalizedBoundsSize);
            UpdateListTransforms(_boundSpriteRenderers, _boundsGap);
        }

        private void UpdateList(List<SpriteRenderer> spriteRenderers, Bound bound, float normalizedValue)
        {
            var width = bound.min + (normalizedValue * (bound.max - bound.min));

            foreach (var renderer in spriteRenderers)
            {
                renderer.size = new Vector2(width, renderer.size.y);
            }
        }

        private void UpdateListTransforms(List<SpriteRenderer> spriteRenderers, float boundsGap)
        {
            foreach (var renderer in spriteRenderers)
            {
                var rendererTransform = renderer.transform;
                renderer.transform.SetPositionAndRotation(GetPosition(rendererTransform.position, boundsGap), rendererTransform.rotation);
            }
        }

        private Vector3 GetPosition(Vector3 position, float magnitude)
        {
            return new Vector3(GetDirectionSign(position.x) * magnitude, GetDirectionSign(position.y) * magnitude, GetDirectionSign(position.z) * magnitude);
        }

        private int GetDirectionSign(float axisPosition)
        {
            return axisPosition == 0f ? 0 : (axisPosition > 0f ? 1 : -1);
        }
    }
}
