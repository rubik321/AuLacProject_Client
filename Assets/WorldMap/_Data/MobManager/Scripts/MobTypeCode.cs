using System;
using GOA.WorldMap;

namespace GOA.WorldMap
{
    public class MobTypeCodeParser
    {
        public static MobTypeCode FromString(string name)
        {
            try
            {
                name = name.Trim();
                return (MobTypeCode)Enum.Parse(typeof(MobTypeCode), name.Replace(" ",""));
            }
            catch (System.Exception)
            {
                return MobTypeCode.unknowType;
            }
            
        }
    }

    public enum MobTypeCode
    {
        unknowType = 0,

        RegularMobs= 10001,
        GreaterMobs = 20001,
        MobChieftainOutpost = 30001,
        MobReaperPortal = 40001,
        Dungeon = 4
    }
}