using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rubik.BattleEngine
{
    [System.Serializable]
    public class TeamBattleShortData
    {
        public string TeamID;
        public CardBattleShordData[] Cards;
        public HeroBattleShortData Hero;
    }
}
