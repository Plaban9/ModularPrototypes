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

        private float _currentAngle = 0f;
        private float _oldInvokeInterval;

        internal void Initialize(BulletPattern bulletPattern, BulletHellPatternData bulletHellPatternData)
        {
            _bulletPattern = bulletPattern;

            ApplyBulletHellPatternSettings(bulletHellPatternData);
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
                    // This case is for testing purposes, to see all patterns in action at the same time.
                    // In a real game, you would likely want to have more control over when each pattern is active, rather than having them all fire at the same time.
                    // Set the parameters in BulletSpawner Gameobject in the inspector to see the patterns in action.
                    //Spiral();
                    //DoubleSpiral();
                    //RadialBurst();
                    break;
            }
        }

        private void RadialBurst()
        {
            var totalBullets = _bulletHellPatternData.BulletAmount;
            var arcAngle = _bulletHellPatternData.EndAngle - _bulletHellPatternData.StartAngle;
            var angleStep = (float)arcAngle / (totalBullets == 1 ? 1 : totalBullets - 1);
            var currentAngle = (totalBullets == 1 ? _bulletHellPatternData.StartAngle + (angleStep / 2) : _bulletHellPatternData.StartAngle) + _bulletHellPatternData.OffsetAngle;

            for (int i = 0; i < totalBullets; i++)
            {
                DoBulletOperations(currentAngle, _radialBurstMaterial);

                currentAngle += angleStep;
            }
        }

        private void Spiral()
        {
            DoBulletOperations(_currentAngle, _spiralMaterial);

            _currentAngle += _bulletHellPatternData.DeltaAngle;
        }

        private void DoubleSpiral()
        {
            for (int i = 0; i < 2; i++)
            {
                float bulletDirectionX = transform.position.x + Mathf.Sin(((_currentAngle + 180f * i) * Mathf.PI) / 180f);
                float bulletDirectionY = transform.position.y + Mathf.Cos(((_currentAngle + 180f * i) * Mathf.PI) / 180f);

                DoBulletOperations(new Vector3(bulletDirectionX, bulletDirectionY), _doubleSpiralMaterial);
            }

            _currentAngle += _bulletHellPatternData.DeltaAngle;

            if (_currentAngle >= 360f)
            {
                _currentAngle = 0f;
            }
        }

        private void DoBulletOperations(Vector3 direction, Material bulletMaterial)
        {
            var bulletMoveVector = new Vector3(direction.x, direction.y, 0f);
            var filteredPosition = new Vector3(transform.position.x, transform.position.y, 0f);
            var bulletDirection = (bulletMoveVector - filteredPosition).normalized;

            var bullet = BulletPool.BulletPoolInstance.GetBullet();

            if (bullet == null)
            {
                return;
            }

            bullet.transform.SetPositionAndRotation(transform.position, transform.rotation);
            bullet.GetComponent<Bullet>().SetProperties(bulletDirection, bulletMaterial, _bulletHellPatternData.BulletSpeed, _bulletHellPatternData.BulletLifeInSeconds, _bulletHellPatternData.EnableTrail);

            bullet.SetActive(true);
        }

        private void DoBulletOperations(float angle, Material bulletMaterial)
        {
            var bulletDirectionX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180f);
            var bulletDirectionY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180f);

            var bulletMoveVector = new Vector3(bulletDirectionX, bulletDirectionY, 0f);
            var filteredPosition = new Vector3(transform.position.x, transform.position.y, 0f);
            var bulletDirection = (bulletMoveVector - filteredPosition).normalized;

            var bullet = BulletPool.BulletPoolInstance.GetBullet();

            if (bullet == null)
            {
                return;
            }

            bullet.transform.SetPositionAndRotation(transform.position, transform.rotation);
            bullet.GetComponent<Bullet>().SetProperties(bulletDirection, bulletMaterial, _bulletHellPatternData.BulletSpeed, _bulletHellPatternData.BulletLifeInSeconds, _bulletHellPatternData.EnableTrail);

            bullet.SetActive(true);
        }

        public void ApplyBulletHellPatternSettings(BulletHellPatternData data)
        {
            _bulletHellPatternData.SetConfiguration(data);
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
