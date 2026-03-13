using System.Collections;
using System.Collections.Generic;
using GOA.Portal;
using GOA.WorldMap;
using NTFunctions_old;
using UnityEngine;

namespace GOA.Reward
{
    public class PoptalRewardUI : MonoBehaviour
    {
        public Box_Dmg_Info_Reward RewardSample;
        public Transform Holder;

        public void OnUI(List<RewardData> rewardDatas)
        {
            NTFunction.ClearChild(this.Holder);
            foreach (RewardData item in rewardDatas)
            {
                Box_Dmg_Info_Reward reward = Instantiate(this.RewardSample);
                reward.InitData(item);
                reward.transform.SetParent(this.Holder);
                reward.transform.gameObject.SetActive(true);
                reward.transform.localScale = new Vector3(1, 1, 1);
            }
            gameObject.SetActive(true);
        }

        public void OffUI()
        {
            gameObject.SetActive(false);
        }
    }
}

