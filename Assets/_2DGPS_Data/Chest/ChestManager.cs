using System;
using System.Collections.Generic;
using NTPackage_old.Functions;
using Rubik.KingFish.Card;
using SimpleJSON;
using Sirenix.OdinInspector;
using UnityEngine;
using NTFunctions_old;
using GOA.Config;

namespace Rubik._2DGPS.Chest
{
    using UserData;

    public class ChestManager : LoadBehaviour
    {
        public static ChestManager instance;
        protected override void Awake()
        {
            base.Awake();
            if (ChestManager.instance != null)
            {
                NTLog.LogWarning("Only 1 instance allow");
                return;
            }
            ChestManager.instance = this;
        }
        public List<ChestReward> rewards;
        [Button]
        public void OpenChest(ChestType chestType, int amount, Action<List<ChestReward>> callback = null)
        {
            if (amount < 1) return;
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.instance.UserData._id;
            jdata["chestType"] = (int)chestType;
            jdata["amount"] = amount;
            StartCoroutine(APIManager.Instance.PostDataUrl(jdata.ToString(), SeverConfigs.GPS2D_Server + SeverConfigs._2DGPS_API_Chest_OpenChest, (data) =>
                {
                    JSONNode jdata = JSONNode.Parse(data.downloadHandler.text);
                    APIManager.Instance.BaseAPIRespone(jdata);
                    rewards = new List<ChestReward>();
                    foreach (JSONNode item in jdata["Data"]["ChestReward"])
                    {
                        try
                        {
                            ChestReward chestReward = JsonUtility.FromJson<ChestReward>(item.ToString());
                            rewards.Add(chestReward);
                        }
                        catch (System.Exception e)
                        {
                            NTLog.LogError(e.ToString(), gameObject);
                        }
                    }
                    callback?.Invoke(rewards);
                }
            ));
        }
    }
}