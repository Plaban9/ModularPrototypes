using UnityEngine;

namespace ModularPrototypes.BulletHell.Data
{
    [System.Serializable]
    public class BulletHellPatternData
    {
        [Range(0, 100)][SerializeField] private int _bulletAmount;
        [Range(0, 360)][SerializeField] private int _deltaAngle;
        [Range(0, 360)][SerializeField] private int _offsetAngle;
        [Range(0, 360)][SerializeField] private int _startAngle;
        [Range(0, 360)][SerializeField] private int _endAngle;
        [Range(0.001f, 10f)][SerializeField] private float _shootInterval;
        [Range(0, 100f)][SerializeField] private float _bulletSpeed;
        [Range(0, 10f)][SerializeField] private float _bulletLifeInSeconds;
        [SerializeField] private bool _extraBullet;
        [SerializeField] private bool _enableTrail;

        public void SetConfiguration(BulletHellPatternData data)
        {
            _bulletAmount = data._bulletAmount;
            _deltaAngle = data._deltaAngle;
            _offsetAngle = data._offsetAngle;
            _startAngle = data._startAngle;
            _endAngle = data._endAngle;
            _shootInterval = data._shootInterval;
            _bulletSpeed = data._bulletSpeed;
            _bulletLifeInSeconds = data._bulletLifeInSeconds;
            _extraBullet = data._extraBullet;
            _enableTrail = data._enableTrail;
        }

        //public void ApplyConfiguration(BulletHellPatternData data)
        //{
        //    data._bulletAmount = _bulletAmount;
        //    data._deltaAngle = _deltaAngle;
        //    data._offsetAngle = _offsetAngle;
        //    data._startAngle = _startAngle;
        //    data._endAngle = _endAngle;
        //    data._shootInterval = _shootInterval;
        //    data._bulletSpeed = _bulletSpeed;
        //    data._bulletLifeInSeconds = _bulletLifeInSeconds;
        //    data._extraBullet = _extraBullet;
        //    data._enableTrail = _enableTrail;
        //}

        public int BulletAmount
        {
            get => _bulletAmount;
            set => _bulletAmount = value;
        }
        public int OffsetAngle
        {
            get => _offsetAngle;
            set => _offsetAngle = value;
        }
        public int StartAngle
        {
            get => _startAngle;
            set => _startAngle = value;
        }
        public int EndAngle
        {
            get => _endAngle;
            set => _endAngle = value;
        }
        public float ShootInterval
        {
            get => _shootInterval;
            set => _shootInterval = value;
        }
        public bool ExtraBullet
        {
            get => _extraBullet;
            set => _extraBullet = value;
        }
        public float BulletSpeed
        {
            get => _bulletSpeed;
            set => _bulletSpeed = value;
        }
        public float BulletLifeInSeconds
        {
            get => _bulletLifeInSeconds;
            set => _bulletLifeInSeconds = value;
        }
        public bool EnableTrail
        {
            get => _enableTrail;
            set => _enableTrail = value;
        }
        public int DeltaAngle
        {
            get => _deltaAngle;
            set => _deltaAngle = value;
        }
    }
}
