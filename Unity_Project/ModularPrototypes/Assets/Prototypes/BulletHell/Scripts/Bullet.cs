using UnityEngine;

namespace ModularPrototypes.BulletHell
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private Vector3 _moveDirection;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private ParticleSystem _trail;
        private float _lifetimeInSeconds = 3f;

        private void OnEnable()
        {
            Invoke(nameof(Destroy), _lifetimeInSeconds);
        }

        private void Start()
        {
            _moveSpeed = 5f;
        }

        private void Update()
        {
            Move();
        }

        private void Move()
        {
            transform.Translate(_moveSpeed * Time.deltaTime * _moveDirection, Space.World);
        }

        public void SetProperties(Vector3 direction, Material material, float speed = 5f, float lifetimeInSeconds = 3f, bool enableTrail = true)
        {
            _moveDirection = direction;
            _moveSpeed = speed;
            _lifetimeInSeconds = lifetimeInSeconds;

            if (material != null)
            {
                GetComponent<Renderer>().material = material;
            }

            //var main = _trail.main;
            //main.startLifetime = _lifetimeInSeconds;
            _trail.gameObject.SetActive(enableTrail);
        }

        private void Destroy()
        {
            gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            CancelInvoke();
        }
    }
}
