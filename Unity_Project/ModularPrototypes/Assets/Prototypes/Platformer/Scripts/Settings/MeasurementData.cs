using System;

using UnityEngine;

namespace ModularPrototypes.Platformer.Measurements
{
    [CreateAssetMenu(fileName = "MeasurementData", menuName = "Data/Platformer/Data/Measurement Data")]
    public class MeasurementData : ScriptableObject
    {
        [Header("Central Axis")]
        [Range(0f, 1f)][SerializeField] private float _normalizedAxisSize;
        [SerializeField] private Bound _minMaxAxis;

        [Header("Bounds Axis")]
        [Range(0f, 1f)][SerializeField] private float _normalizedBoundsSize;
        [SerializeField] private Bound _minMaxBoundsLength;
        [Range(0.1f, 20f)][SerializeField] private float _boundsGap;

        [Header("Extras")]
        [SerializeField] private bool _showReferenceBounds;
        [SerializeField] private bool _hideMesh;

        #region GETTERS
        public float GetNormalizedAxisSize()
        {
            return _normalizedAxisSize;
        }

        public Bound GetMinMaxAxis()
        {
            return _minMaxAxis;
        }

        public float GetNormalizedBoundsSize()
        {
            return _normalizedBoundsSize;
        }

        public Bound GetMinMaxBoundsLength()
        {
            return _minMaxBoundsLength;
        }

        public float GetBoundsGap()
        {
            return _boundsGap;
        }

        public bool GetShowReferenceBounds()
        {
            return _showReferenceBounds;
        }

        public bool GetHideMesh()
        {
            return _hideMesh;
        }
        #endregion

        #region SETTERS
        public void SetNormalizedAxisSize(float value)
        {
            _normalizedAxisSize = value;
        }

        public void SetMinMaxAxis(Bound value)
        {
            _minMaxAxis = value;
        }

        public void SetNormalizedBoundsSize(float value)
        {
            _normalizedBoundsSize = value;
        }

        public void SetMinMaxBoundsLength(Bound value)
        {
            _minMaxBoundsLength = value;
        }

        public void SetBoundsGap(float value)
        {
            _boundsGap = value;
        }

        public void SetShowReferenceBounds(bool value)
        {
            _showReferenceBounds = value;
        }

        public void SetHideMesh(bool value)
        {
            _hideMesh = value;
        }

        internal void SetConfiguration(MeasurementData measurementDeafaultData)
        {
            _normalizedAxisSize = measurementDeafaultData.GetNormalizedAxisSize();
            _minMaxAxis = measurementDeafaultData.GetMinMaxAxis();
             
            _normalizedBoundsSize = measurementDeafaultData.GetNormalizedBoundsSize();
            _minMaxBoundsLength = measurementDeafaultData.GetMinMaxBoundsLength();
            _boundsGap = measurementDeafaultData.GetBoundsGap();

            _showReferenceBounds = measurementDeafaultData.GetShowReferenceBounds();
            _hideMesh = measurementDeafaultData.GetHideMesh();
        }
        #endregion
    }
}
