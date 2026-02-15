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
        [SerializeField] BulletHellPatternData _bulletHellPatternData;
        //[SerializeField] private int _bulletAmount;
        //[SerializeField] private int _offsetAngle;
        //[SerializeField] private int _startAngle;
        //[SerializeField] private int _endAngle;
        //[SerializeField] private float _shootInterval;
        //[SerializeField] private bool _extraBullet;
        //[SerializeField] private float _bulletSpeed;
        //[SerializeField] private float _bulletLifeInSeconds;
        //[SerializeField] private bool _enableTrail = true;
        #endregion
    }
}
