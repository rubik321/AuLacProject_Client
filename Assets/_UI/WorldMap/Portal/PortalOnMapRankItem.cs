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
    using Rubik.UserDataPlayer;

    public class PortalOnMapRankItem : MonoBehaviour
    {
        public RankingPlayerRewardItem RankingPlayerRewardItem;

        public void SetData(UserDataShort userDataShort, UserRank userRank, RankBossReward rankBossReward, float rewardMultiplier, float fixRewardMultiplier)
        {
            List<ItemData> rewards = new List<ItemData>();
            foreach (ItemData itemData in rankBossReward.Reward)
            {
                ItemData itemDataReward = new ItemData();
                itemDataReward.Type = itemData.Type;
                itemDataReward.Amount = (int)(itemData.Amount * rewardMultiplier);
                rewards.Add(itemDataReward);
            }
            foreach (ItemData itemData in rankBossReward.FixReward)
            {
                ItemData itemDataReward = new ItemData();
                itemDataReward.Type = itemData.Type;
                itemDataReward.Amount = (int)(itemData.Amount * fixRewardMultiplier);
                rewards.Add(itemDataReward);
            }
            bool isGreen = false;
            if(userDataShort.UserID == UserDataManager.Instance.GetUserID()) isGreen = true;
            this.RankingPlayerRewardItem.SetData(userDataShort.DisplayName, userDataShort.Level, userDataShort.Avatar, userDataShort.AvatarBorder, userRank.Rank, userRank.Score, rewards, isGreen);
        }

        public void SetPlayerRank(int rank, long damage, RankBossReward rankBossReward, float rewardMultiplier, float fixRewardMultiplier)
        {
            this.RankingPlayerRewardItem.Clear();
            if (rank < 0 || damage < 1)
            {
                this.RankingPlayerRewardItem.TextRank.text = "--";
                this.RankingPlayerRewardItem.TextScore.text = "--";
                this.RankingPlayerRewardItem.BarBGs[this.RankingPlayerRewardItem.BarBGs.Count - 1].gameObject.SetActive(true);
            }
            else
            {
                this.RankingPlayerRewardItem.TextRank.text = rank.ToString();
                this.RankingPlayerRewardItem.TextScore.text = damage.ToString();
                switch (rank)
                {
                    case 0:
                        this.RankingPlayerRewardItem.BarBGs[0].gameObject.SetActive(true);
                        break;
                    case 1:
                        this.RankingPlayerRewardItem.BarBGs[1].gameObject.SetActive(true);
                        break;
                    case 2:
                        this.RankingPlayerRewardItem.BarBGs[2].gameObject.SetActive(true);
                        break;
                    default:
                        this.RankingPlayerRewardItem.TextRank.gameObject.SetActive(true);
                        this.RankingPlayerRewardItem.BarBGs[this.RankingPlayerRewardItem.BarBGs.Count - 1].gameObject.SetActive(true);
                        break;
                }

                if (rank > 99)
                {
                    this.RankingPlayerRewardItem.TextRank.text = (99) + "+";
                }
                else
                {
                    this.RankingPlayerRewardItem.TextRank.text = (rank + 1) + "";
                }

                List<ItemData> rewards = new List<ItemData>();
                foreach (ItemData itemData in rankBossReward.Reward)
                {
                    ItemData itemDataReward = new ItemData();
                    itemDataReward.Type = itemData.Type;
                    itemDataReward.Amount = (int)(itemData.Amount * rewardMultiplier);
                    rewards.Add(itemDataReward);
                }
                foreach (ItemData itemData in rankBossReward.FixReward)
                {
                    ItemData itemDataReward = new ItemData();
                    itemDataReward.Type = itemData.Type;
                    itemDataReward.Amount = (int)(itemData.Amount * fixRewardMultiplier);
                    rewards.Add(itemDataReward);
                }
                this.RankingPlayerRewardItem.itemDataUI.SetData(rewards.ToArray(), null, true, true);
            }
            this.RankingPlayerRewardItem.TextLevel.text = "Lv." + (UserDataManager.Instance.GetLevel() +1);
            this.RankingPlayerRewardItem.TextDisplayName.text = UserDataManager.Instance.GetDisplayName();
            this.RankingPlayerRewardItem.AvatarPlayerUI.SetData(UserProfileManager.Instance.GetAvatarUsedIndex(), UserProfileManager.Instance.GetAvatarBorderUsedIndex());
        }

        public void Clear()
        {
            this.RankingPlayerRewardItem.Clear();
        }
    }
}
