using ModularPrototypes.BulletHell.Data;

using UnityEngine;

namespace ModularPrototypes.BulletHell
{
    public class BulletSpawner : MonoBehaviour
    {
        [SerializeField] private BulletPattern _bulletPattern = BulletPattern.RADIAL_BURST;

        [SerializeField] private Material _radialBurstMaterial;
        [SerializeField] private Material _spiralMaterial;
        [SerializeField] private Material _doubleSpiralMaterial;

       
        [Header("|<--- Bullet Hell Pattern Data --->|")]
        [SerializeField] BulletHellPatternData _bulletHellPatternData;

        //[Range(0f, 360f)]
        //[SerializeField] private float _spawnerRotation = 0f;
        

        private float _currentAngle = 0f;
        //[SerializeField] private int _bulletsAmount = 10;
        //[SerializeField] private float _deltaAngle = 10f;
        //[Range(0f, 360f)]
        //[SerializeField] private float _offset = 0f;
        //[Range(0f, 360f)]
        //[SerializeField] private float _startAngle = 0f;
        //[Range(0f, 360f)]
        //[SerializeField] private float _endAngle = 0f;
        //[Range(0.001f, 10f)]
        //[SerializeField] private float _invokeInterval = 0.25f;
        //[SerializeField] private bool _addExtraBullet = true;

        //[SerializeField] private float _bulletSpeed;
        //[SerializeField] private float _bulletLifeInSeconds = 3f;
        //[SerializeField] private bool _enableTrail = true;

        private float _oldInvokeInterval;

        private Vector3 _bulletMoveDirection;

        private void Start()
        {
            _currentAngle = _bulletHellPatternData.StartAngle;

            ResetFire();
        }

        private void Update()
        {
            //transform.rotation = Quaternion.Euler(0f, 0f, _spawnerRotation);

            if (_oldInvokeInterval != _bulletHellPatternData.ShootInterval)
            {
                D("Changing Fire Rate, From: " + _oldInvokeInterval + ", To: " + _bulletHellPatternData.ShootInterval);
                ResetFire();
            }
        }

        private void ResetFire()
        {
            _oldInvokeInterval = _bulletHellPatternData.ShootInterval;
            CancelInvoke(nameof(Fire));
            InvokeRepeating(nameof(Fire), 0f, _oldInvokeInterval);
        }

        private void Fire()
        {
            switch (_bulletPattern)
            {
                case BulletPattern.RADIAL_BURST:
                    RadialBurst();
                    break;

                case BulletPattern.SPIRAL:
                    Spiral();
                    break;

                case BulletPattern.DOUBLE_SPIRAL:
                    DoubleSpiral();
                    break;

                case BulletPattern.ALL:
                    Spiral();
                    DoubleSpiral();
                    RadialBurst();
                    break;
            }
        }

        private void RadialBurst()
        {
            var angleStep = (_bulletHellPatternData.EndAngle - _bulletHellPatternData.StartAngle) / _bulletHellPatternData.BulletAmount;
            float angle = (_bulletHellPatternData.ExtraBullet ? _bulletHellPatternData.StartAngle : _bulletHellPatternData.StartAngle + angleStep / 2f) + _bulletHellPatternData.OffsetAngle;

            var totalBullets = _bulletHellPatternData.ExtraBullet ? _bulletHellPatternData.BulletAmount + 1 : _bulletHellPatternData.BulletAmount;

            for (int i = 0; i < totalBullets; i++)
            {
                var bulletDirectionX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180f);
                var bulletDirectionY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180f);

                var bulletMoveVector = new Vector3(bulletDirectionX, bulletDirectionY, 0f);
                var filteredPosition = new Vector3(transform.position.x, transform.position.y, 0f);
                var bulletDirection = (bulletMoveVector - filteredPosition).normalized;

                var bullet = BulletPool.BulletPoolInstance.GetBullet();

                if (bullet == null)
                {
                    continue;
                }

                bullet.transform.SetPositionAndRotation(transform.position, transform.rotation);
                bullet.GetComponent<Bullet>().SetProperties(bulletDirection, _radialBurstMaterial, _bulletHellPatternData.BulletSpeed, _bulletHellPatternData.BulletLifeInSeconds, _bulletHellPatternData.EnableTrail);

                bullet.SetActive(true);

                angle += angleStep;
            }
        }

        private void Spiral()
        {
            float bulletDirectionX = transform.position.x + Mathf.Sin((_currentAngle * Mathf.PI) / 180f);
            float bulletDirectionY = transform.position.y + Mathf.Cos((_currentAngle * Mathf.PI) / 180f);

            var bulletMoveVector = new Vector3(bulletDirectionX, bulletDirectionY, 0f);
            var filteredPosition = new Vector3(transform.position.x, transform.position.y, 0f);
            var bulletDirection = (bulletMoveVector - filteredPosition).normalized;

            var bullet = BulletPool.BulletPoolInstance.GetBullet();

            if (bullet == null)
            {
                return;
            }

            bullet.transform.SetPositionAndRotation(transform.position, transform.rotation);
            bullet.GetComponent<Bullet>().SetProperties(bulletDirection, _spiralMaterial, _bulletHellPatternData.BulletSpeed, _bulletHellPatternData.BulletLifeInSeconds, _bulletHellPatternData.EnableTrail);

            bullet.SetActive(true);

            _currentAngle += _bulletHellPatternData.DeltaAngle;
        }

        private void DoubleSpiral()
        {
            for (int i = 0; i < 2; i++)
            {
                float bulletDirectionX = transform.position.x + Mathf.Sin(((_currentAngle + 180f * i) * Mathf.PI) / 180f);
                float bulletDirectionY = transform.position.y + Mathf.Cos(((_currentAngle + 180f * i) * Mathf.PI) / 180f);

                var bulletMoveVector = new Vector3(bulletDirectionX, bulletDirectionY, 0f);
                var filteredPosition = new Vector3(transform.position.x, transform.position.y, 0f);
                var bulletDirection = (bulletMoveVector - filteredPosition).normalized;

                var bullet = BulletPool.BulletPoolInstance.GetBullet();

                if (bullet == null)
                {
                    return;
                }

                bullet.transform.SetPositionAndRotation(transform.position, transform.rotation);
                bullet.GetComponent<Bullet>().SetProperties(bulletDirection, _doubleSpiralMaterial, _bulletHellPatternData.BulletSpeed, _bulletHellPatternData.BulletLifeInSeconds, _bulletHellPatternData.EnableTrail);

                bullet.SetActive(true);
            }

            _currentAngle += _bulletHellPatternData.DeltaAngle;

            if (_currentAngle >= 360f)
            {
                _currentAngle = 0f;
            }
        }

        #region SETTERS
        public void SetBulletPattern(BulletPattern bulletPattern)
        {
            _bulletPattern = bulletPattern;
        }
        #endregion

        #region DEBUG
        private void D(string message)
        {
            DebugUtils.DebugInfo.Print($"<<BulletSpawner>> {message}");
        }
        #endregion
    }
}
