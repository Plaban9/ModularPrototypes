using UnityEngine;

namespace ModularPrototypes.BulletHell.Data
{
    [CreateAssetMenu(fileName = "BulletHellData", menuName = "Data/BulletHell/BulletHellData")]
    public class BulletHellData : ScriptableObject
    {
        [SerializeField] private string patternName;
        [SerializeField] private BulletPattern bulletPattern;
        [SerializeField] private Color patternBackgroundColor;

        public string GetName() => patternName;
        public BulletPattern GetBulletPattern() => bulletPattern;
        public Color GetBackgroundColor() => patternBackgroundColor;

    }
}
