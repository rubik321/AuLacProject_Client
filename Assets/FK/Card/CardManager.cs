using System;
using System.Collections;
using System.Collections.Generic;
using NTPackage_old;
using NTPackage_old.EventDispatcher;
using NTPackage_old.Functions;
using SimpleJSON;
using Sirenix.OdinInspector;
using UnityEngine;
using NTFunctions_old;
using GOA.Config;
using GOA.UserData;

namespace Rubik.KingFish.Card
{
    public class CardManager : LoadBehaviour
    {

        public NTDictionary<Card> CardDic;

        public string[] CardTeam{
            get {
                return UserData.Instance.data.CardTeam_FK;
            }
        }

        public static CardManager instance;
        protected override void Awake()
        {
            base.Awake();
            if (CardManager.instance != null)
            {
                Debug.LogWarning("Only 1 instance allow");
                return;
            }
            CardManager.instance = this;
        }

        public IEnumerator Init()
        {
            if (UserData.Instance.data.UserId.Length == 0)
            {
                Debug.LogWarning("Can't get userID");
            }
            else
            {
                this.CardDic = new NTDictionary<Card>();
                yield return this.GetCards();
                yield return this.CheckCardTeams();
            }
        }

        [Button]
        public void UpdateCardTeams(List<string> teams)
        {
            //Debug.Log("Reload game ");
            StartCoroutine(this.UpdateCardTeam(teams));

        }

        private IEnumerator GetCards()
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserData.Instance.data.UserId;
            yield return APIManager.Instance.PostDataUrl(jdata.ToString(), SeverConfigs.BASE_API_URL + SeverConfigs.API_Card_GetCards, (data) =>
            {
                jdata = JSONNode.Parse(data.downloadHandler.text);
                foreach (JSONNode item in jdata["Data"]["Cards"])
                {
                    Card card = JsonUtility.FromJson<Card>(item.ToString());
                    this.CardDic.Add(card._id, card);
                }
                // QuestManager.instance.UpdateAchievement(jdata["Data"]["Achievement"]);
            });
        }
        //public List<Card> lsCardRandoms = new List<Card>();
        public void GetCardRandom(int count, Action<List<CardSummon>> callback = null)
        {

            
          //  StartCoroutine(CoAddRandomCards(count, callback));

        }
       

        private IEnumerator AddRandomCard()
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserData.Instance.data.UserId;
            yield return APIManager.Instance.PostDataUrl(jdata.ToString(), SeverConfigs.BASE_API_URL + SeverConfigs.API_Card_AddRandomCard, (data) =>
            {
                jdata = JSONNode.Parse(data.downloadHandler.text);
                Card card = JsonUtility.FromJson<Card>(jdata["Data"].ToString());
                this.CardDic.Add(card._id, card);
              
            });
        }
        [Button]
        public void RanDomCard()
        {
            StartCoroutine(AddRandomCard());
        }

        [Button]
        public void TestSummon(int time)
        {
            StartCoroutine(Summon(time));
        }
        public void StartSummon(int time, Action<List<CardSummon>> callback )
        {
            StartCoroutine(Summon(time, callback));
        }
        public IEnumerator Summon(int time, Action<List<CardSummon>> callback = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserData.Instance.data.UserId;
            jdata["time"] = time;
            yield return APIManager.Instance.PostDataUrl(jdata.ToString(), SeverConfigs.BASE_API_URL + SeverConfigs.API_Card_Summon, (data) =>
            {
                jdata = JSONNode.Parse(data.downloadHandler.text);
                List<CardSummon> cardSummons = new List<CardSummon>();
                foreach (JSONNode item in jdata["Data"]["Cards"])
                {
                    try
                    {
                        CardSummon cardSummon = JsonUtility.FromJson<CardSummon>(item.ToString());
                        cardSummons.Add(cardSummon);
                    }
                    catch (System.Exception e)
                    {
                        NTLog.LogError(e.ToString(), gameObject);
                    }
                }
                foreach (CardSummon item in cardSummons)
                {
                    AddCardSummon(item);
                }
                UserData.Instance.data.SummonToken_FK -= time;
                // if(jdata["Data"]["DailyQuest"] != null){
                //     QuestManager.instance.UpdateQuest(jdata["Data"]["DailyQuest"]);
                // }
                // QuestManager.instance.UpdateAchievement(jdata["Data"]["Achievement"]);
                callback?.Invoke(cardSummons);
            });
        }

        public void AddCardSummon(CardSummon cardSummon)
        {
            Card card = this.GetCardByID(cardSummon._id);
            if (card == null)
            {
                card = new Card();
                card._id = cardSummon._id;
                card.UserID = UserData.Instance.data.UserId;
                card.Index = cardSummon.Index;
                card.AscendLv = 1;
                card.Lv = 1;
                this.CardDic.Add(card._id, card);
            }
            else
            {
                if (card.AscendLv == 0)
                {
                    card.AscendLv = 1;
                }
                else
                {
                    card.Shard += cardSummon.Shard;
                }
            }
        }

        private IEnumerator CheckCardTeams()
        {
            bool atLestOne = false;
            foreach (string item in this.CardTeam)
            {
                if(item.Length > 0){
                    atLestOne = true;
                }
            }
            if(!atLestOne){
                List<string> teams = new List<string>();
                foreach (Card item in this.CardDic.ToList())
                {
                    if(teams.Count >= 2){
                        break;
                    }else{
                        teams.Add(item._id);
                    }
                }
                yield return this.UpdateCardTeam(teams);
            }
            
        }

        public IEnumerator UpdateCardTeam(List<string> teams)
        {
            for (int i = 0; i < teams.Count; i++)
            {
                if(teams[i] == null) teams[i] = "";
            }
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserData.Instance.data.UserId;
            jdata["teams"] = teams;
            yield return APIManager.Instance.PostDataUrl(jdata.ToString(), SeverConfigs.BASE_API_URL + SeverConfigs.API_Card_UpdateCardTeam, (data) =>
            {
                jdata = JSONNode.Parse(data.downloadHandler.text);
               UserData.Instance.data.CardTeam_FK = jdata["Data"]["CardTeam_FK"];
                
                // EventListenerManager.instance.PostEvent(EventCode.FishKingdom_CardTeamUpdate);
                // try
                // {
                //     GameController.Instance.StartReLoadGame();
                // }
                // catch (System.Exception)
                // {}
                
            });
        }

        public IEnumerator UpgradeLv(string cardID, int lv = 1, Action done = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["cardID"] = cardID;
            jdata["lv"] = lv;
            yield return APIManager.Instance.PostDataUrl(jdata.ToString(), SeverConfigs.BASE_API_URL + SeverConfigs.API_Card_UpgradeLv, (data) =>
            {
                JSONNode jdata = JSONNode.Parse(data.downloadHandler.text);
                this.CardDic.Get(cardID).Lv = jdata["Data"]["CardLv"];
                UserData.Instance.data.Coin = jdata["Data"]["Coin"];
                done?.Invoke();
            });
        }

        public IEnumerator UpgradeEnhanceLv(string cardID, Action done)
        {
            JSONNode jdata = new JSONObject();
            jdata["cardID"] = cardID;
            yield return APIManager.Instance.PostDataUrl(jdata.ToString(), SeverConfigs.BASE_API_URL + SeverConfigs.API_Card_UpgradeEnhanceLv, (data) =>
            {
                JSONNode jdata = JSONNode.Parse(data.downloadHandler.text);
                this.CardDic.Get(cardID).LvEnhance = jdata["Data"]["CardLvEnhance"];
                UserData.Instance.data.PromotionGem_FK = jdata["Data"]["PromotionGem_FK"];
                done?.Invoke();
            });
        }

        public IEnumerator UpgradeGearLv(string cardID, int index = 0, int lv = 1, Action done = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["cardID"] = cardID;
            jdata["index"] = index;
            jdata["lv"] = lv;
            yield return APIManager.Instance.PostDataUrl(jdata.ToString(), SeverConfigs.BASE_API_URL + SeverConfigs.API_Card_UpgradeGearLv, (data) =>
            {
                JSONNode jdata = JSONNode.Parse(data.downloadHandler.text);
                List<Gear> gears = new List<Gear>();
                foreach (JSONNode item in jdata["Data"]["Gears"])
                {
                    Gear gear = JsonUtility.FromJson<Gear>(item.ToString());
                    gears.Add(gear);
                }
                // this.CardDic.Get(cardID).Gears = gears.ToArray();
                UserData.Instance.data.Iron_FK = jdata["Data"]["Iron_FK"];
                done?.Invoke();
            });
        }

        public IEnumerator ResetGearLv(string cardID, Action done = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["cardID"] = cardID;
            yield return APIManager.Instance.PostDataUrl(jdata.ToString(), SeverConfigs.BASE_API_URL + SeverConfigs.API_Card_ResetGearLv, (data) =>
            {
                JSONNode jdata = JSONNode.Parse(data.downloadHandler.text);
                List<Gear> gears = new List<Gear>();
                foreach (JSONNode item in jdata["Data"]["Gears"])
                {
                    Gear gear = JsonUtility.FromJson<Gear>(item.ToString());
                    gears.Add(gear);
                }
                // this.CardDic.Get(cardID).Gears = gears.ToArray();
                UserData.Instance.data.Iron_FK += jdata["Data"]["Inc_Iron_FK"];
                done?.Invoke();
            });
        }

        public IEnumerator QuickUpgradeLv(string cardID, Action done = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["cardID"] = cardID;
            yield return APIManager.Instance.PostDataUrl(jdata.ToString(), SeverConfigs.BASE_API_URL + SeverConfigs.API_Card_QuickUpgradeGearLv, (data) =>
            {
                JSONNode jdata = JSONNode.Parse(data.downloadHandler.text);
                List<Gear> gears = new List<Gear>();
                foreach (JSONNode item in jdata["Data"]["Gears"])
                {
                    Gear gear = JsonUtility.FromJson<Gear>(item.ToString());
                    gears.Add(gear);
                }
                // this.CardDic.Get(cardID).Gears = gears.ToArray();
                UserData.Instance.data.Iron_FK = jdata["Data"]["Iron_FK"];
                done?.Invoke();
            });
        }

        public void Ascend(string cardID, Action done = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["cardID"] = cardID;
            StartCoroutine(APIManager.Instance.PostDataUrl(jdata.ToString(), SeverConfigs.BASE_API_URL + SeverConfigs.API_Card_Ascend, (data) =>
            {
                JSONNode jdata = JSONNode.Parse(data.downloadHandler.text);
                Card card = CardManager.instance.GetCardByID(cardID);
                card.AscendLv = jdata["Data"]["AscendLv"];
                card.Shard = jdata["Data"]["Shard"];
                done?.Invoke();
            }));
        }

        public Card GetCardByID(string cardID)
        {
            return this.CardDic.Get(cardID);
        }

        public static bool CanLevelUp(int lv, int lvGain)
        {
            if (CaculateCoinUpgrade(lv, lvGain) > UserData.Instance.data.Coin) return false;
            else return true;
        }

        public static int CaculateCoinUpgrade(int lv, int lvGain)
        {
            return (lvGain + 1) * (lvGain + lv + lv) / 2;
        }

        public static bool CanEnhanceLvUp(int lvEnhance)
        {
            if (CaculatePriceEnhanceLv(lvEnhance) > UserData.Instance.data.PromotionGem_FK) return false;
            return true;
        }
        public static int CaculatePriceEnhanceLv(int lvEnhance)
        {
            return lvEnhance * 100 + 50;
        }

        public static bool GearCanLevelUp(int lv, int lvGain)
        {
            if (GearCaculateIronUpgrade(lv, lvGain) > UserData.Instance.data.Iron_FK) return false;
            else return true;
        }

        public static int GearCaculateIronUpgrade(int lv, int lvGain)
        {
            return ((int)(lv / 1000) + 1) * ((int)(lv / 1000) + 1) * lvGain;
        }

        public static bool GearCanEnhanceLvUp(int lv)
        {
            if (UserData.Instance.data.Iron_FK < GearCaculatePriceEnhanceLv(lv)) return false;
            return true;
        }

        public static int GearCaculatePriceEnhanceLv(int lv)
        {
            return ((int)lv / 1000) * ((int)lv / 1000) * 1000;
        }

        public static int ShardRequireAscend(int lvAscend)
        {
            return (int)Math.Pow(5,(int)((lvAscend-1)/5)) * 8;;
        }
    }
}
