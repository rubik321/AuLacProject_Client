using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NTFunctions_old;
using GOA.WorldMap;
using NTPackage_old.Functions;
using GOA.Reward;
using TMPro;

namespace GOA.Portal{
    public class PopupRewardUI : PopupUI
    {
        public PoptalRewardUI PoptalRewardUI;
        public LandRewardUI LandRewardUI;
        public List<RewardData> RewardDatas;

        public TextMeshProUGUI TextNumbMons;

        public override void LoadComponents()
        {
            base.LoadComponents();
        }

        public void OnUI(List<RewardData> rewardDatas){
            return;
            this.RewardDatas = rewardDatas;
            if(rewardDatas.Count == 0) return;
            List<RewardData> groupReward = new List<RewardData>();
            for (int i = rewardDatas.Count -1; i >= 0; i--)
            {
                if(rewardDatas[i].Id.Equals(rewardDatas[0].Id)){
                    groupReward.Add(rewardDatas[i]);
                    rewardDatas.RemoveAt(i);
                }
            }
            if(groupReward[0].Type.Equals("Land")){
                this.show = KindPopup.oneStep;
                this.PoptalRewardUI.OffUI();
                this.LandRewardUI.OnUI(groupReward);
                this.hide = KindPopup.none;
            }else{
                this.show = KindPopup.twoStep;
                this.PoptalRewardUI.OnUI(groupReward);
                this.LandRewardUI.OffUI();
                this.hide = KindPopup.oneStep;
            }
            if(groupReward[0].MonsterSkilled > 0){
                this.TextNumbMons.text = groupReward[0].MonsterSkilled+"";
                this.TextNumbMons.gameObject.SetActive(true);
            }else this.TextNumbMons.gameObject.SetActive(false);
            this.Show();
        }

        public override void OffUI(){
            if(this.RewardDatas.Count == 0){
                this.Hide();
                WorldMapMaster.instance.TransReward.gameObject.SetActive(false);
                return;
            }
            this.OnUI(this.RewardDatas);
        }

    }
}