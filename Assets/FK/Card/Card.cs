using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rubik.KingFish.Card
{
    [System.Serializable]
    public class Gear
    {
        public int Lv = 1;
    }

    [System.Serializable]
    public class Card
    {
        public string _id;
        public string UserID;
        public int Index;
        public int Lv;
        public int LvEnhance;

        // public Gear[] Gears;

        public int Shard;
        public int AscendLv;
        public int rarity;

        public Card(){
            // List<Gear> gears = new List<Gear>();
            // for (int i = 0; i < 4; i++)
            // {
            //     gears.Add(new Gear());
            // }
            // this.Gears = gears.ToArray();
        }
    }
}
