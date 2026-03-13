using System;
using System.Collections.Generic;
using NTPackage_old.Functions;
using Rubik.KingFish.Card;
using SimpleJSON;
using Sirenix.OdinInspector;
using UnityEngine;
using NTFunctions_old;
using GOA.UserData;
using GOA.Config;
using GOA.Item;

namespace Rubik.KingFish.Chest
{
    public class ChestManager : LoadBehaviour
    {
        public static ChestManager instance;
        protected override void Awake()
        {
            base.Awake();
            if (ChestManager.instance != null)
            {
                NTLog.LogWarning("Only 1 instance allow", gameObject);
                return;
            }
            ChestManager.instance = this;
        }
        public List<ChestReward> rewards;
        [Button]
        public void OpenChest(ChestType chestType, int amount, Action<List<ChestReward>> callback = null, Action callbackError = null)
        {
            if (amount < 1) return;
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserData.Instance.data.UserId;
            jdata["chestType"] = (int)chestType;
            jdata["amount"] = amount;
            StartCoroutine(APIManager.Instance.PostDataUrl(jdata.ToString(), SeverConfigs.BASE_API_URL + SeverConfigs.API_Chest_OpenChest, (data) =>
                {
                    JSONNode jdata = JSONNode.Parse(data.downloadHandler.text);
                    rewards = new List<ChestReward>();
                    foreach (JSONNode item in jdata["Data"]["Reward"])
                    {
                        try
                        {
                            ChestReward chestReward = JsonUtility.FromJson<ChestReward>(item.ToString());
                            if (chestReward.CurrencyAmount > 0)
                            {
                                UserData.Instance.data.AddCurrency(chestReward.ItemType, chestReward.CurrencyAmount);
                            }
                            else
                            {
                                CardManager.instance.AddCardSummon(chestReward.CardSummon);
                            }
                            rewards.Add(chestReward);
                        }
                        catch (System.Exception e)
                        {
                            NTLog.LogError(e.ToString(), gameObject);
                        }
                    }
                    switch (chestType)
                    {
                        case ChestType.BronzeChest:
                            UserData.Instance.data.AddCurrency(ItemCode.BronzeChest_FK, -amount);
                            break;
                        case ChestType.GoldChest:
                            UserData.Instance.data.AddCurrency(ItemCode.GoldChest_FK, -amount);
                            break;
                        case ChestType.DiamondChest:
                            UserData.Instance.data.AddCurrency(ItemCode.DiamondChest_FK, -amount);
                            break;
                        case ChestType.PlatinumChest:
                            UserData.Instance.data.AddCurrency(ItemCode.PlatinumChest_FK, -amount);
                            break;
                        default:
                            UserData.Instance.data.AddCurrency(ItemCode.WoodChest_FK, -amount);
                            break;
                    }
                    UserData.Instance.data.SetCurrency(ItemCode.ScoreChest_FK, jdata["Data"]["ScoreChest_FK"]);
                    // if (jdata["Data"]["DailyQuest"] != null)
                    // {
                    //     QuestManager.instance.UpdateQuest(jdata["Data"]["DailyQuest"]);
                    // }
                    // if (jdata["Data"]["Achievement"] != null)
                    // {
                    //     QuestManager.instance.UpdateAchievement(jdata["Data"]["Achievement"]);
                    // }
                    callback?.Invoke(rewards);
                }
            ));
        }
    }
}