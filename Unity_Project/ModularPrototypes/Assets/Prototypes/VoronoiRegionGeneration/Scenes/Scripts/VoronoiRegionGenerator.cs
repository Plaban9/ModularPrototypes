using UnityEngine;
using UnityEngine.UI;

namespace ModularPrototypes.Voronoi.ProceduralGeneration.Region
{
    public class VoronoiRegionGenerator : MonoBehaviour
    {
        [SerializeField] private Vector2Int _mapDimension;
        [SerializeField] private RawImage _mapVisual;


        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            _mapVisual = GetComponent<RawImage>();
            var rectTransform = _mapVisual.GetComponent<RectTransform>();

            _mapDimension = Vector2Int.RoundToInt(rectTransform.sizeDelta);

            GenerateVoronoiMap();
        }

        [ContextMenu("GenerateVoronoiMap()")]
        private void GenerateVoronoiMap()
        {
            GenerateMap();
        }

        private void GenerateMap()
        {
            var texture2d = new Texture2D(_mapDimension.x, _mapDimension.y);

            for (int i = 0; i < _mapDimension.x; i++)
            {
                for (int j = 0; j < _mapDimension.y; j++)
                {
                    var color = Random.Range(0f, 1f);
                    //var color = Random.Range(0, 2);

                    texture2d.SetPixel(i, j, new Color(color, color, color));
                }
            }

            texture2d.Apply();
            _mapVisual.texture = texture2d;
        }
    }
}
