using UnityEngine;

namespace ModularPrototypes.Platformer.Data
{
    [System.Serializable]
    public class PlatformData
    {
        [SerializeField] private PlatformTransformationSettings.TransformFunction _function;
        [Range(0.1f, 360f)][SerializeField] private float _amplitude;
        [SerializeField] private bool _modulo;
        [SerializeField] private bool _negate;

        public void SetConfiguration(PlatformData data)
        {
            _function = data._function;
            _amplitude = data._amplitude;
            _modulo = data._modulo;
            _negate = data._negate;
        }

        public PlatformTransformationSettings.TransformFunction Function
        {
            get => _function;
            set => _function = value;
        }
        public float Amplitude
        {
            get => _amplitude;
            set => _amplitude = value;
        }
        public bool Modulo
        {
            get => _modulo;
            set => _modulo = value;
        }
        public bool Negate
        {
            get => _negate;
            set => _negate = value;
        }
    }
}