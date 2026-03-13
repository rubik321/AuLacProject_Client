using Rubik.ItemPlayer;
using UnityEngine;

namespace Rubik.Myrk.DailyReward
{
    using Rubik.ItemPlayer;
    [System.Serializable]
    public class DailyReward
    {
        public int Version = 0;
        public int Day = 0;
        public bool Claimed = false;
    }


    [System.Serializable]
    public class DailyRewardData
    {
        public int Day;

        public ItemData[] RewardItems = new ItemData[0];
    }
}
