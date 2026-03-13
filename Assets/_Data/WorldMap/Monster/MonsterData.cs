using System.Collections;
using System.Collections.Generic;
using GoShared;
using NTPackage.Functions;
using Rubik.CardPlayer;
using Rubik.DataType;
using UnityEngine;

namespace Rubik.Myrk.Monster
{
    [System.Serializable]
    public class MonsterData
    {
        public string _id;
        public CardPlayerIndex Index;
        public int Lv;
        public int Star;
        public float Scale;

        public MonsterData(CardPlayerIndex index, int lv, int star, float scale){
            this._id = NTFunction.GenerateId();
            this.Index = index;
            this.Lv = lv;
            this.Star = star;
            this.Scale = scale;
        }
    }

    [System.Serializable]
    public class MonsterOnMapData{
        public string _id;
        public CardPlayerIndex Index;
        public int Lv;
        public Coordinates coordinates;
        public AttackType AttackType = AttackType.Grinding;

        public MonsterOnMapData(){
            this._id = NTFunction.GenerateId();
        }
    }
}
