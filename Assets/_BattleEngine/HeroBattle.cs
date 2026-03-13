using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rubik.BattleEngine
{
    [System.Serializable]
    public class HeroBattleShortData
    {
        public string _id;
        public int Index = 0;
        public string TeamID;
        public int ATK = 0;
        public int AP;
        public int[] GearIndexs;
    }
}