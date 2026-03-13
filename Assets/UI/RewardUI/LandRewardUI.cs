using System;
using System.Collections;
using System.Collections.Generic;
using GOA.Item;
using GOA.WorldMap;
using NTFunctions_old;
using NTPackage_old.Functions;
using UnityEngine;

namespace GOA.Reward
{
    public class LandRewardUI : MonoBehaviour
    {
        public ItemRewardUI ItemRewardUISample;
        public Transform Holder;

        public void OnUI(List<RewardData> rewardDatas)
        {
            NTFunction.ClearChild(this.Holder);
            foreach (RewardData item in rewardDatas)
            {
                if(item.Coin > 0){
                    this.SpawnItemRewardUI(ItemCode.Coin, item.Coin);
                }
                if(item.Gin > 0){
                    this.SpawnItemRewardUI(ItemCode.Gin, item.Gin);
                }
                foreach (var ele in item.RewardItems)
                {
                    try
                    {
                        this.SpawnItemRewardUI((ItemCode)Enum.Parse(typeof(ItemCode), ele.Type), ele.Amount);
                    }
                    catch (System.Exception e){
                        NTLog.LogWarning(e.ToString(), gameObject);
                    }
                }
            }
            gameObject.SetActive(true);
        }

        public void SpawnItemRewardUI(ItemCode code, int amount){
            ItemRewardUI itemRewardUI = Instantiate(this.ItemRewardUISample);
            itemRewardUI.transform.SetParent(this.Holder);
            itemRewardUI.SetData(code, amount);
            itemRewardUI.transform.localScale = Vector3.one;
            itemRewardUI.gameObject.SetActive(true);
        }

        public void OffUI()
        {
            gameObject.SetActive(false);
        }
    }
}