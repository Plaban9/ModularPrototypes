using System.Collections.Generic;

using UnityEngine;

namespace ModularPrototypes.BulletHell
{
    public class BulletPool : MonoBehaviour
    {
        private static BulletPool _bulletPoolInstance;

        [SerializeField] private GameObject _pooledBullet;
        [SerializeField] private bool _deficientPool = true;
        [SerializeField] private List<GameObject> _bullets;

        public static BulletPool BulletPoolInstance
        {
            get => _bulletPoolInstance;
            private set => _bulletPoolInstance = value;
        }

        private void Awake()
        {
            BulletPoolInstance = this;
        }

        private void Start()
        {
            _bullets = new List<GameObject>();
        }

        public GameObject GetBullet()
        {
            if (_bullets.Count > 0)
            {
                for (int i = 0; i < _bullets.Count; i++)
                {
                    if (!_bullets[i].activeInHierarchy)
                    {
                        return _bullets[i];
                    }
                }
            }

            if (_deficientPool)
            {
                //D("Pool is deficient. Instantiating new bullet.");
                var bullet = Instantiate(_pooledBullet);
                bullet.SetActive(false);
                _bullets.Add(bullet);

                return bullet;
            }

            return null;
        }

        private void D(string message)
        {
            DebugUtils.DebugInfo.Print($"<<BulletPool>> {message}");
        }
    }
}
