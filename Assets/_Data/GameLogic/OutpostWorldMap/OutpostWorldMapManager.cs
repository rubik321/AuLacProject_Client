using System;
using System.Collections;
using System.Collections.Generic;
using NTPackage.Functions;
using Rubik.CardPlayer;
using Rubik.Config;
using Rubik.Manager;
using SimpleJSON;
using UnityEngine;

namespace Rubik.Myrk.Outpost
{
    using NTPackage.UI;
    using Rubik.DataCenter;
    using Rubik.Myrk.BattleTeam;
    using Rubik.Server;
    using Rubik.Myrk.Monster;
    using Sirenix.OdinInspector;
    using Spine.Unity;
    using UserDataPlayer;
    using Rubik.ServerGame;
    using Rubik.DataType;
    using Rubik.MsgDelivery;
    using Rubik.UserProfile;
    using NTPackage;
    using NTPackage.EventDispatcher;
    using System.Linq;
    using Rubik.ItemPlayer;
    using Rubik.Quest;

    public class OutpostWorldMapConfig
    {
        public const string API_Attack = "/api/2D_GPS/outpost/attack";
    }

    public class OutpostWorldMapManager : NTBehaviour
    {
        #region Player Data
        public UserOutpost UserOutpost; 
        public NTDictionary<string, bool> OutpostCache = new NTDictionary<string, bool>();
        #endregion

        #region Game Data
        public List<OutpostData> OutpostDatas;
        #endregion

        #region Resource
        #endregion

        public static OutpostWorldMapManager Instance;
        protected override void Awake()
        {
            base.Awake();
            if (OutpostWorldMapManager.Instance != null)
            {
                NTLog.LogError("Only 1 Instance allow");
                return;
            }
            OutpostWorldMapManager.Instance = this;
        }

        #region Test
        #endregion

        #region Function
        public void LoadData()
        {
            JSONNode jdata = JSONNode.Parse(DataCenterManager.Instance.GetData(DataName.OutpostData));
            this.OutpostDatas = new List<OutpostData>();
            foreach (JSONNode item in jdata)
            {
                OutpostData outpostData = JsonUtility.FromJson<OutpostData>(item.ToString());
                this.OutpostDatas.Add(outpostData);
            }
        }

        public void Logout()
        {
            this.UserOutpost = new UserOutpost();
            this.OutpostCache.Clear();
        }

        public void UpdateUserOutpost(UserOutpost userOutpost)
        {
            if (userOutpost == null || userOutpost.Amount == 0) return;
            this.OutpostCache.Clear();
            foreach (string key in userOutpost.Occupied)
            {
                this.OutpostCache.Add(key, true);
            }
            EventListenerManager.instance.PostEvent(EventCode.OutpostWorldMapManager_UpdateData);
            this.UserOutpost = userOutpost;
        }

        #endregion

        #region API
        public IEnumerator IEAttack(long tileX, long tileY, MonsterAttackData attackData, Action<BattleResult> done = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.Instance.GetUserID();
            jdata["tileX"] = tileX;
            jdata["tileY"] = tileY;
            jdata["team"] = JSONNode.Parse(JsonUtility.ToJson(BattleTeamManager.Instance.GetBattleTeamDataSelected().ToShortTeam()));
            jdata["attackData"] = JSONNode.Parse(JsonUtility.ToJson(attackData));

            yield return APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + OutpostWorldMapConfig.API_Attack, (data) =>
            {
                APIResponseData aPIResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if(aPIResponseData.Status == 0) return;
                done?.Invoke(aPIResponseData.BattleResult);
            });
        }

        #endregion

        #region Getter

        public long GetAmountOutpost()
        {
            return this.UserOutpost.Amount;
        }

        public bool IsOccupied(long tileX, long tileY)
        {
            return this.OutpostCache.Get(this.GetKeyOccupied(tileX, tileY));
        }

        public string GetKeyOccupied(long tileX, long tileY)
        {
            return tileX + "-" + tileY;
        }

        public OriginType GetOriginByTileXTileY(long tileX, long tileY)
        {
            return OriginConfig.OriginList[((int)tileX + (int)(tileY % OriginConfig.OriginList.Count)) % OriginConfig.OriginList.Count];
        }

        public MonsterAttackData GetMonsterAttackData(long tileX, long tileY)
        {
            List<MonsterData> monsterData = new List<MonsterData>(){
                null, null, null,
                null, null, null,
                null, null, null,
            };

            OriginType originType = this.GetOriginByTileXTileY(tileX, tileY);

            // Get Main Monster
            OutpostData outpostData = this.OutpostDatas[0];
            foreach (OutpostData index in this.OutpostDatas)
            {
                if (this.GetAmountOutpost() <= index.Max)
                {
                    outpostData = index;
                    break;
                }
            }

            CardPlayerIndex[] indexes = CardPlayerManager.Instance.GetCardTierOriginData(outpostData.Star, originType);

            // Get Support Monster
            System.Random randomSupport = new System.Random((int)(tileX + tileY));
            List<int> supportSlot = new List<int>(MonsterConfig.MonsterSupportSlot);

            for (int i = 0; i < 5; i++)
            {
                CardPlayerIndex supportMonsterIndex = indexes[randomSupport.Next(0, indexes.Length)];
                int slot = supportSlot[randomSupport.Next(0, supportSlot.Count)];
                monsterData[slot] = new MonsterData(supportMonsterIndex, this.GetOutpostLevel(), outpostData.Star, outpostData.Scale);
                supportSlot.RemoveAt(supportSlot.IndexOf(slot));
            }

            MonsterAttackData monsterAttackData = new MonsterAttackData();
            monsterAttackData.Monsters = monsterData.ToArray();
            monsterAttackData.TeamID = "Outpost:" + this.GetKeyOccupied(tileX, tileY);
            monsterAttackData.AttackType = AttackType.Outpost;
            monsterAttackData.ScaleMonster = outpostData.Scale;
            return monsterAttackData;
        }

        public List<ItemData> GetOutpostReward(){
            OutpostData outpostData = this.OutpostDatas[0];
            foreach (OutpostData index in this.OutpostDatas)
            {
                if ((this.GetAmountOutpost()+1) <= index.Max)
                {
                    outpostData = index;
                    break;
                }
            }
            return outpostData.Reward;
        }

        public List<ItemData> GetOutpostDailyReward(){
            OutpostData outpostData = this.OutpostDatas[0];
            foreach (OutpostData index in this.OutpostDatas)
            {
                if ((this.GetAmountOutpost()) <= index.Max)
                {
                    outpostData = index;
                    break;
                }
            }
            if(this.GetAmountOutpost() == 0){
                List<ItemData> itemDatas = new List<ItemData>();
                foreach (ItemData itemData in outpostData.DailyReward){
                    itemDatas.Add(new ItemData(itemData.Type, 0));
                }
                return itemDatas;
            }
            return outpostData.DailyReward;
        }

        public List<ItemData> GetOutpostDailyRewardNext(){
            OutpostData outpostData = this.OutpostDatas[0];
            foreach (OutpostData index in this.OutpostDatas)
            {
                if ((this.GetAmountOutpost()+1) <= index.Max)
                {
                    outpostData = index;
                    break;
                }
            }
            return outpostData.DailyReward;
        }

        public int GetOutpostLevel(){
            return (int)this.UserOutpost.Amount;
        }

        #endregion
    }
}

