using UnityEngine;

namespace ModularPrototypes.Platformer.Measurements
{

    [CreateAssetMenu(fileName = "CameraData", menuName = "Data/Platformer/Data/Camera Data")]
    public class CameraData : ScriptableObject
    {
        [Header("Camera Settings")]
        [SerializeField] private bool _isOrthographic;


        #region GETTERS
        public bool GetIsOrthographic()
        {
            return _isOrthographic;
        }
        #endregion

        #region SETTERS
        public void SetIsOrthographic(bool value)
        {
            _isOrthographic = value;
        }
        #endregion

        public void ApplyConfiguratioon(CameraData data)
        {
            _isOrthographic = data.GetIsOrthographic();
        }
    }
}