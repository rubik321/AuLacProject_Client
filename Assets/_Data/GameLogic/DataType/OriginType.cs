using System.Collections.Generic;

namespace Rubik.DataType
{
    [System.Serializable]
    public enum OriginType
    {
        Metal,
        Wood,
        Water,
        Fire,
        Earth,
    }

    public class OriginConfig
    {
        public static List<OriginType> OriginList = new List<OriginType>() { OriginType.Metal, OriginType.Wood, OriginType.Water, OriginType.Fire, OriginType.Earth };
    }
}
