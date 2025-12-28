using UnityEngine;

namespace ModularPrototypes.BulletHell
{
    public enum BulletPattern
    {
        RADIAL_BURST = 0b_0000_0001,
        SPIRAL = 0b_0000_0010,
        DOUBLE_SPIRAL = 0b_0000_0100,

        ALL = RADIAL_BURST | SPIRAL | DOUBLE_SPIRAL
    }
}
