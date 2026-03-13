using UnityEngine;
using System.Collections.Generic;

namespace Rubik.Myrk.Outpost
{
    using ItemPlayer;
    using Rubik.BattleEngine;

    [System.Serializable]
    public class UserOutpost
    {
        public long Amount;
        public List<string> Occupied = new List<string>();
    }

    [System.Serializable]
    public class OutpostData
    {
        public int Index;
        public int Star;
        public float Scale;
        public long Min;
        public long Max;
        public List<ItemData> Reward = new List<ItemData>();
        public List<ItemData> DailyReward = new List<ItemData>();
    }

    [System.Serializable]
    public class OutpostBattleResult
    {
        public string _id;
        public BattleShortData Result = new BattleShortData();
        public ItemData[] Reward;
        public bool IsWin;
    }
}