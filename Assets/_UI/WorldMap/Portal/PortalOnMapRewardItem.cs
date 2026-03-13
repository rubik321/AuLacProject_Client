using System.Collections.Generic;
using Rubik.ItemPlayer;
using Rubik.UserProfile;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rubik.Myrk.Portal
{
    using Rubik.ItemPlayer;
    using Rubik.Myrk.Ranking;

    public class PortalOnMapRewardItem : MonoBehaviour
    {
        public RankingRewardItem RankingRewardItem;

        public void SetData(RankBossReward rankBossReward, float rewardMultiplier, float fixRewardMultiplier){
            List<ItemData> rewards = new List<ItemData>();
            foreach(ItemData itemData in rankBossReward.Reward){
                ItemData itemDataReward = new ItemData();
                itemDataReward.Type = itemData.Type;
                itemDataReward.Amount = (int)(itemData.Amount * rewardMultiplier);
                rewards.Add(itemDataReward);
            }
            foreach(ItemData itemData in rankBossReward.FixReward){
                ItemData itemDataReward = new ItemData();
                itemDataReward.Type = itemData.Type;
                itemDataReward.Amount = (int)(itemData.Amount * fixRewardMultiplier);
                rewards.Add(itemDataReward);
            }
            this.RankingRewardItem.SetData(rankBossReward.RankMax, rankBossReward.RankMin, rewards);
        }

        public void Clear(){
            this.RankingRewardItem.Clear();
        }
    }
}
