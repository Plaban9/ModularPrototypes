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
        [Range(0f, 1f)]
        [SerializeField] private float _normalizedBoundsSize;
        [SerializeField] private Bound _minMaxBounds;
        [SerializeField] private List<GameObject> _bounds;
        [SerializeField] private List<SpriteRenderer> _boundSpriteRenderers;

        private void Awake()
        {
            InitializeLists();
        }

        private void Update()
        {
            UpdateLists();
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
            UpdateList(_boundSpriteRenderers, _minMaxBounds, _normalizedBoundsSize);
        }

        private void UpdateList(List<SpriteRenderer> spriteRenderers, Bound bound, float normalizedValue)
        {
            var width = bound.min + (normalizedValue * (bound.max - bound.min));

            foreach (var renderer in spriteRenderers)
            {
                renderer.size = new Vector2(width, renderer.size.y);
            }
        }
    }
}
