using UnityEngine;

namespace ModularPrototypes.BulletHell.Data
{
    [CreateAssetMenu(fileName = "BulletHellConfig", menuName = "Data/Bullet Hell/Data/Bullet Hell Config")]
    public class BulletHellConfig : ScriptableObject
    {
        #region Meta Data 
        [SerializeField] private string patternName;
        [SerializeField] private BulletPattern bulletPattern;
        #endregion

        #region Configuration Data
        [SerializeField] private BulletHellPatternData _bulletHellPatternData;
        #endregion

        public string GetName() => patternName;
        public BulletPattern GetBulletPattern() => bulletPattern;

        public BulletHellPatternData GetBulletHellPatternData() => _bulletHellPatternData;

        public void SetBulletPatternData(BulletHellPatternData data)
        {
            _bulletHellPatternData.SetConfiguration(data);
        }

        //public void ApplyBulletPatternData(BulletHellPatternData data)
        //{
        //    _bulletHellPatternData.ApplyConfiguration(data);
        //}
    }
}
