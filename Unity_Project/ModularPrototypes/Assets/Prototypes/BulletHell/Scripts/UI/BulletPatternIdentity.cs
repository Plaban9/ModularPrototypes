using UnityEngine;

namespace ModularPrototypes.BulletHell.UI
{
    public class BulletPatternIdentity : MonoBehaviour
    {
        [SerializeField] private BulletPattern _bulletPattern;
        public BulletPattern GetBulletPattern() => _bulletPattern;
    }
}
