using System.Collections.Generic;

using UnityEngine;

namespace ModularPrototypes.Platformer.Data
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "PlatformConfig", menuName = "Data/Platform/Data/Platform Config")]
    public class PlatformConfig : ScriptableObject
    {
        #region Top Level Data 
        [SerializeField] private string _domainName;
        [Range(0.01f, 30f)][SerializeField] private float _period;
        [SerializeField] private PlatformTransformationSettings.TransformDomain _transformDomain;
        [SerializeField] private PlatformTransformationSettings.TransformDimension _transformDimension;
        [SerializeField] private Color _backgroundColor;
        #endregion

        #region Configuration Data
        [Header("---------- Platform Data ----------")]
        [SerializeField] private PlatformData X;
        [SerializeField] private PlatformData Y;
        [SerializeField] private PlatformData Z;
        [SerializeField] private Dictionary<PlatformTransformationSettings.TransformAxis, PlatformData> _platformDataDictionary;
        #endregion

        private void Initialize()
        {
            _platformDataDictionary = new Dictionary<PlatformTransformationSettings.TransformAxis, PlatformData>
            {
                { PlatformTransformationSettings.TransformAxis.X, X },
                { PlatformTransformationSettings.TransformAxis.Y, Y },
                { PlatformTransformationSettings.TransformAxis.Z, Z }
            };
        }

        #region GETTERS
        public string GetName()
        {
            if (string.IsNullOrEmpty(_domainName))
            {
                return _transformDomain.ToString();
            }

            return _domainName;
        }

        public float GetPeriod() => _period;
        public PlatformTransformationSettings.TransformDomain GetTransformDomain() => _transformDomain;
        public PlatformTransformationSettings.TransformDimension GetTransformDimension() => _transformDimension;
        public Color GetBackgroundColor() => _backgroundColor;

        public PlatformData GetPlatformData(PlatformTransformationSettings.TransformAxis axis)
        {
            if (_platformDataDictionary == null)
            {
                Initialize();
            }

            return _platformDataDictionary[axis];
        }
        #endregion

        #region SETTERS
        public void SetPeriod(float period)
        {
            _period = period;
        }

        public void SetConfiguration(PlatformConfig platformConfig, bool applyCosmetics = false)
        {
            if (_platformDataDictionary == null)
            {
                Initialize();
            }

            _domainName = platformConfig._domainName;
            _period = platformConfig._period;
            _transformDomain = platformConfig._transformDomain;
            _transformDimension = platformConfig._transformDimension;
            
            if (applyCosmetics)
            {
                _backgroundColor = platformConfig._backgroundColor;
            }            

            foreach (var axis in System.Enum.GetValues(typeof(PlatformTransformationSettings.TransformAxis)))
            {
                var platformData = platformConfig.GetPlatformData((PlatformTransformationSettings.TransformAxis)axis);
                GetPlatformData((PlatformTransformationSettings.TransformAxis)axis).SetConfiguration(platformData);
            }
        }

        public void SetPlatformData(PlatformTransformationSettings.TransformAxis axis, PlatformData data)
        {
            if (_platformDataDictionary == null)
            {
                Initialize();
            }

            _platformDataDictionary[axis] = data;
        }
        #endregion
    }
}
