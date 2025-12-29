using UnityEngine;

namespace ModularPrototypes.BulletHell
{
    public class BulletSpawner : MonoBehaviour
    {
        [SerializeField] BulletPattern _bulletPattern = BulletPattern.RADIAL_BURST;

        [SerializeField] Material _radialBurstMaterial;
        [SerializeField] Material _spiralMaterial;
        [SerializeField] Material _doubleSpiralMaterial;

        [SerializeField] private int _bulletsAmount = 10;

        private float _currentAngle = 0f;
        [SerializeField] private float _deltaAngle = 10f;

        [Range(0f, 360f)]
        [SerializeField] private float _spawnerAngle = 0f;
        [Range(0f, 360f)]
        [SerializeField] private float _startAngle = 0f;
        [Range(0f, 360f)]
        [SerializeField] private float _endAngle = 0f;

        [SerializeField] private float _invokeInterval = 2f;

        [SerializeField] private float _bulletSpeed;
        [SerializeField] private float _bulletLifeInSeconds = 3f;
        [SerializeField] private bool _enableTrail = true;

        private float _oldInvokeInterval;

        private Vector3 _bulletMoveDirection;

        private void Start()
        {
            _currentAngle = _startAngle;
            _oldInvokeInterval = _invokeInterval;
            InvokeRepeating(nameof(Fire), 0f, _oldInvokeInterval);
        }

        private void Update()
        {
            transform.rotation = Quaternion.Euler(0f, 0f, _spawnerAngle);

            if (_oldInvokeInterval != _invokeInterval)
            {
                _oldInvokeInterval = _invokeInterval;
                CancelInvoke(nameof(Fire));
                InvokeRepeating(nameof(Fire), 0f, _oldInvokeInterval);
            }
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
            var angleStep = (_endAngle - _startAngle) / _bulletsAmount;
            float angle = _startAngle;

            for (int i = 0; i < _bulletsAmount; i++)
            {
                var bulletDirectionX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180f);
                var bulletDirectionY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180f);

                var bulletMoveVector = new Vector3(bulletDirectionX, bulletDirectionY, 0f);
                var bulletDirection = (bulletMoveVector - transform.position).normalized;

                var bullet = BulletPool.BulletPoolInstance.GetBullet();
                bullet.transform.SetPositionAndRotation(transform.position, transform.rotation);
                bullet.GetComponent<Bullet>().SetProperties(bulletDirection, _radialBurstMaterial, _bulletSpeed, _bulletLifeInSeconds, _enableTrail);

                bullet.SetActive(true);

                angle += angleStep;
            }
        }

        private void Spiral()
        {
            float bulletDirectionX = transform.position.x + Mathf.Sin((_currentAngle * Mathf.PI) / 180f);
            float bulletDirectionY = transform.position.y + Mathf.Cos((_currentAngle * Mathf.PI) / 180f);

            var bulletMoveVector = new Vector3(bulletDirectionX, bulletDirectionY, 0f);
            var bulletDirection = (bulletMoveVector - transform.position).normalized;

            var bullet = BulletPool.BulletPoolInstance.GetBullet();
            bullet.transform.SetPositionAndRotation(transform.position, transform.rotation);
            bullet.GetComponent<Bullet>().SetProperties(bulletDirection, _spiralMaterial, _bulletSpeed, _bulletLifeInSeconds, _enableTrail);

            bullet.SetActive(true);

            _currentAngle += _deltaAngle;
        }

        private void DoubleSpiral()
        {
            for (int i = 0; i <= 1; i++)
            {
                float bulletDirectionX = transform.position.x + Mathf.Sin(((_currentAngle + 180f * i) * Mathf.PI) / 180f);
                float bulletDirectionY = transform.position.y + Mathf.Cos(((_currentAngle + 180f * i) * Mathf.PI) / 180f);

                var bulletMoveVector = new Vector3(bulletDirectionX, bulletDirectionY, 0f);
                var bulletDirection = (bulletMoveVector - transform.position).normalized;

                var bullet = BulletPool.BulletPoolInstance.GetBullet();
                bullet.transform.SetPositionAndRotation(transform.position, transform.rotation);
                bullet.GetComponent<Bullet>().SetProperties(bulletDirection, _doubleSpiralMaterial, _bulletSpeed, _bulletLifeInSeconds, _enableTrail);

                bullet.SetActive(true);
            }

            _currentAngle += _deltaAngle;

            if (_currentAngle >= 360f)
            {
                _currentAngle = 0f;
            }
        }
    }
}
