using System.Collections.Generic;

using UnityEngine;

namespace ModularPrototypes.Platformer.Data
{

    [CreateAssetMenu(fileName = "PlatformConfig", menuName = "Data/Platform/Data/Platform Config")]
    public class PlatformConfig : ScriptableObject
    {
        #region Meta Data 
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
        [SerializeField] private Dictionary<PlatformTransformationSettings.Axis, PlatformData> _platformDataDictionary;
        #endregion

        private void Initialize()
        {
            _platformDataDictionary = new Dictionary<PlatformTransformationSettings.Axis, PlatformData>
            {
                { PlatformTransformationSettings.Axis.X, X },
                { PlatformTransformationSettings.Axis.Y, Y },
                { PlatformTransformationSettings.Axis.Z, Z }
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

        private PlatformData GetPlatformData(PlatformTransformationSettings.Axis axis)
        {
            if (_platformDataDictionary == null)
            {
                Initialize();
            }

            return _platformDataDictionary[axis];
        }
        #endregion

        #region SETTERS
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

            foreach (var axis in System.Enum.GetValues(typeof(PlatformTransformationSettings.Axis)))
            {
                var platformData = platformConfig.GetPlatformData((PlatformTransformationSettings.Axis)axis);
                GetPlatformData((PlatformTransformationSettings.Axis)axis).SetConfiguration(platformData);
            }
        }

        public void SetPlatformData(PlatformTransformationSettings.Axis axis, PlatformData data)
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
