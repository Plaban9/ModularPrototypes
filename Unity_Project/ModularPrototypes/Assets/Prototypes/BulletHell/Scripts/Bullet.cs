using UnityEngine;

namespace ModularPrototypes.BulletHell
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private Vector3 _moveDirection;
        [SerializeField] private float _moveSpeed;

        private void OnEnable()
        {
            Invoke(nameof(Destroy), 3f);
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
            transform.Translate(_moveDirection * _moveSpeed * Time.deltaTime);
        }

        public void SetProperties(Vector3 direction, Material material, float speed = 5f)
        {
            _moveDirection = direction;
            _moveSpeed = speed;

            if (material != null)
            {
                GetComponent<Renderer>().material = material;
            }
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
