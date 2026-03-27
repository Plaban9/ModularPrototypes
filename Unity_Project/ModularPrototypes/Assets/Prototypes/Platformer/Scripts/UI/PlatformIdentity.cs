using UnityEngine;

namespace ModularPrototypes.Platformer.UI
{
    public class PlatformIdentity : MonoBehaviour
    {
        [SerializeField] private PlatformTransformationSettings.TransformDomain transformDomain;
        public PlatformTransformationSettings.TransformDomain GetTransformDomain() => transformDomain;
    }
}
