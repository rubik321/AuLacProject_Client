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

namespace Rubik._2DGPS.Card
{
    using UserData;
    using DataCenter;
    public class CardManager : LoadBehaviour
    {

        public NTDictionary<CardData> CardDataDic;
        public NTDictionary<Card> CardDic;

        public string[] CardTeam{
            get {
                return UserDataManager.instance.UserData.CardTeam;
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

        public IEnumerator LoadData(){
            this.LoadCardData();
            yield return null;
        }

        public void LoadCardData(){
            this.CardDataDic = new NTDictionary<CardData>();
            JSONNode jdata = JSONNode.Parse(DataCenterManager.instance.GetData(DataName.CardData));
            foreach (JSONNode item in jdata)
            {
                CardData cardData = JsonUtility.FromJson<CardData>(item.ToString());
                this.CardDataDic.Add(cardData.Index.ToString(), cardData);
            }
        }

        public IEnumerator Init()
        {
            if (UserDataManager.instance.GetUserID().Length == 0)
            {
                Debug.LogWarning("Can't get userID");
            }
            else
            {
                this.CardDic = new NTDictionary<Card>();
                yield return this.GetCards();
            }
        }

        public void UpdateCards(JSONNode jdata){
            if (this.CardDic == null || this.CardDic.Count() == 0) this.CardDic = new NTDictionary<Card>();
            foreach (JSONNode item in jdata)
            {
                Card card = JsonUtility.FromJson<Card>(item.ToString());
                if(this.CardDic.Get(card._id) == null) this.CardDic.Add(card._id,card);
                else this.CardDic.Get(card._id).UpdateData(card);
            }
        }
        

        private IEnumerator GetCards()
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.instance.GetUserID();
            yield return APIManager.Instance.PostDataUrl(jdata.ToString(), SeverConfigs.GPS2D_Server + SeverConfigs._2DGPS_API_GetCards, (data) =>
            {
                jdata = JSONNode.Parse(data.downloadHandler.text);
                APIManager.Instance.BaseAPIRespone(jdata);
            });
        }

    
        private IEnumerator AddRandomCard()
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.instance.GetUserID();
            yield return APIManager.Instance.PostDataUrl(jdata.ToString(), SeverConfigs.GPS2D_Server + SeverConfigs._2DGPS_API_Card_AddRandomCard, (data) =>
            {
                jdata = JSONNode.Parse(data.downloadHandler.text);
                APIManager.Instance.BaseAPIRespone(jdata);
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
        public void StartSummon(int time, Action<List<Card>> callback )
        {
            StartCoroutine(Summon(time, callback));
        }
        public IEnumerator Summon(int time, Action<List<Card>> callback = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.instance.GetUserID();
            jdata["time"] = time;
            yield return APIManager.Instance.PostDataUrl(jdata.ToString(), SeverConfigs.GPS2D_Server + SeverConfigs._2DGPS_API_SummonCard, (data) =>
            {
                jdata = JSONNode.Parse(data.downloadHandler.text);
                APIManager.Instance.BaseAPIRespone(jdata);
                List<Card> cards = new List<Card>();
                foreach (JSONNode item in jdata["Data"]["Summon_Card"])
                {
                    try
                    {
                        Card card = JsonUtility.FromJson<Card>(item.ToString());
                        cards.Add(card);
                    }
                    catch (System.Exception e)
                    {
                        NTLog.LogError(e.ToString(), gameObject);
                    }
                }
                callback?.Invoke(cards);
            });
        }
        public void UpdateListCardTeam(List<string> teams)
        {
            StartCoroutine(UpdateCardTeam(teams));
        }
        public IEnumerator UpdateCardTeam(List<string> teams)
        {
            for (int i = 0; i < teams.Count; i++)
            {
                if(teams[i] == null) teams[i] = "";
            }
            JSONNode jdata = new JSONObject();
            jdata["userID"] = Rubik.UserDataPlayer.UserDataManager.Instance.GetUserID();
            jdata["teams"] = teams;
            yield return APIManager.Instance.PostDataUrl(jdata.ToString(), SeverConfigs.GPS2D_Server + SeverConfigs._2DGPS_API_UpdateCardTeam, (data) =>
            {
                jdata = JSONNode.Parse(data.downloadHandler.text);
                APIManager.Instance.BaseAPIRespone(jdata);
                
            });
        }

        public Card GetCardByID(string cardID)
        {
            return this.CardDic.Get(cardID);
        }
        public CardData GetCardDataByIndex(CardIndex index)
        {
            return this.CardDataDic.Get(index.ToString());
        }
    }
}
