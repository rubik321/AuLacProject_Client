using System;
using System.Collections;
using System.Collections.Generic;
using NTPackage.Functions;
using Rubik.CardPlayer;
using Rubik.Config;
using Rubik.Manager;
using SimpleJSON;
using UnityEngine;

namespace Rubik.Myrk.Monster
{
    using System.Linq;
    using GoShared;
    using Rubik.BattleEngine;
    using Rubik.DataCenter;
    using Rubik.DataType;
    using Rubik.ItemPlayer;
    using Rubik.Myrk.BattleTeam;
    using Rubik.Server;
    using Rubik.Myrk.GeoPoint;
    using Spine.Unity;
    using UnityEngine.UIElements;
    using UserDataPlayer;
    public class MonsterConfig
    {
        public static readonly List<int> MonsterMainSlot = new List<int>() { 4 };
        public static readonly List<int> MonsterSupportSlot = new List<int>() { 0, 1, 2, 3, 5, 6, 7, 8 };

        public static float Visibility = 400;


        public const string API_AttackMonster = "/api/2D_GPS/monster_world_map/attack_monster";
        public const string API_ResultAttackMonster = "/api/2D_GPS/monster_world_map/result_attack_monster";
        public const string API_DoubleReward = "/api/2D_GPS/monster_world_map/double_reward";
    }

    public class MonsterManager : NTBehaviour
    {
        public BattleMonsterWorldMapData BattleMonsterWorldMapData;

        public List<MonsterOnMapData> ShardOnMapData;
        public List<MonsterOnMapData> GrindingOnMapData;
        public Transform HolderMonster;
        public List<MonsterOnMapModel> MonsterOnMapModel;
        public MonsterOnMapModel MonsterOnMapModelPrefab;

        public BattleStatus BattleStatus;

        public static MonsterManager Instance;
        protected override void Awake()
        {
            base.Awake();
            if (MonsterManager.Instance != null)
            {
                NTLog.LogError("Only 1 Instance allow");
                return;
            }
            MonsterManager.Instance = this;
        }

        #region Function
        public void LoadData()
        {
            this.ShardOnMapData = new List<MonsterOnMapData>();
            this.GrindingOnMapData = new List<MonsterOnMapData>();
            this.BattleMonsterWorldMapData = JsonUtility.FromJson<BattleMonsterWorldMapData>(DataCenterManager.Instance.GetData(DataName.BattleMonsterWorldMapData));
        }

        public void UpdateBattleStatus(BattleStatus battleStatus)
        {
            if (battleStatus == null || battleStatus.LastTime < 1000) return;
            this.BattleStatus = battleStatus;

            // Shard
            if (this.ShardOnMapData.Count < this.BattleMonsterWorldMapData.MonsterLimit - this.BattleStatus.MonsterLimit)
            {
                for (int i = this.ShardOnMapData.Count; i < this.BattleMonsterWorldMapData.MonsterLimit - this.BattleStatus.MonsterLimit; i++)
                {
                    if (this.ShardOnMapData.Count >= this.BattleMonsterWorldMapData.MonsterOnMapLimit) break;
                    MonsterOnMapData monsterOnMapData = new MonsterOnMapData();
                    monsterOnMapData._id = NTFunction.GenerateId();
                    monsterOnMapData.Index = this.BattleMonsterWorldMapData.MonsterOnMap[UnityEngine.Random.Range(0, this.BattleMonsterWorldMapData.MonsterOnMap.Length)];
                    monsterOnMapData.Lv = UserDataManager.Instance.GetLevel();
                    monsterOnMapData.AttackType = AttackType.Shard;
                    Coordinates coordinates = GeoPointManager.Instance.GetRandomRange(GeoPointManager.Instance.GetCurrentLocation(), MonsterConfig.Visibility);
                    monsterOnMapData.coordinates = coordinates;
                    this.ShardOnMapData.Add(monsterOnMapData);
                }
            }

            // Grinding
            if (this.GrindingOnMapData.Count < this.BattleMonsterWorldMapData.CreepOnMapLimit)
            {
                for (int i = this.GrindingOnMapData.Count; i < this.BattleMonsterWorldMapData.CreepOnMapLimit; i++)
                {
                    MonsterOnMapData monsterOnMapData = new MonsterOnMapData();
                    monsterOnMapData._id = NTFunction.GenerateId();
                    monsterOnMapData.Index = this.BattleMonsterWorldMapData.CreepOnMap[UnityEngine.Random.Range(0, this.BattleMonsterWorldMapData.CreepOnMap.Length)];
                    monsterOnMapData.Lv = UserDataManager.Instance.GetLevel();
                    monsterOnMapData.AttackType = AttackType.Grinding;
                    Coordinates coordinates = GeoPointManager.Instance.GetRandomRange(GeoPointManager.Instance.GetCurrentLocation(), MonsterConfig.Visibility);
                    monsterOnMapData.coordinates = coordinates;
                    this.GrindingOnMapData.Add(monsterOnMapData);
                }
            }

            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.HolderMonster);
            this.MonsterOnMapModel = new List<MonsterOnMapModel>();
            for (int i = 0; i < this.ShardOnMapData.Count; i++)
            {
                MonsterOnMapModel monsterOnMapModel = ObjectPoolingManager.Instance.PullObjectFromPooling<MonsterOnMapModel>(ObjectPoolingConfig.MonsterOnMapModel);
                if (monsterOnMapModel == null)
                {
                    monsterOnMapModel = Instantiate(this.MonsterOnMapModelPrefab);
                }
                monsterOnMapModel.name = ObjectPoolingConfig.MonsterOnMapModel;
                monsterOnMapModel.transform.SetParent(this.HolderMonster);
                monsterOnMapModel.SetData(this.ShardOnMapData[i]);
                this.MonsterOnMapModel.Add(monsterOnMapModel);
            }

            for (int i = 0; i < this.GrindingOnMapData.Count; i++)
            {
                MonsterOnMapModel monsterOnMapModel = ObjectPoolingManager.Instance.PullObjectFromPooling<MonsterOnMapModel>(ObjectPoolingConfig.MonsterOnMapModel);
                if (monsterOnMapModel == null)
                {
                    monsterOnMapModel = Instantiate(this.MonsterOnMapModelPrefab);
                }
                monsterOnMapModel.name = ObjectPoolingConfig.MonsterOnMapModel;
                monsterOnMapModel.transform.SetParent(this.HolderMonster);
                monsterOnMapModel.SetData(this.GrindingOnMapData[i]);
                this.MonsterOnMapModel.Add(monsterOnMapModel);
            }
        }

        public void UpdateData()
        {
            foreach (MonsterOnMapModel monsterOnMapModel in this.MonsterOnMapModel)
            {
                monsterOnMapModel.UpdateData();
            }
        }

        public void Logout()
        {
            this.BattleStatus = new BattleStatus();
            this.MonsterOnMapModel.Clear();
            this.ShardOnMapData.Clear();
            this.GrindingOnMapData.Clear();
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.HolderMonster);
        }

        public void ResultAttackMonster(Action done = null)
        {
            StartCoroutine(IEResultAttackMonster(() =>
            {
                done?.Invoke();
                this.UpdateData();
            }));
        }

        public void AttackMonster(MonsterAttackData teamData, Action done = null)
        {
            StartCoroutine(IEAttackMonster(teamData, false, done));
        }

        public void AdvAttackMonster(MonsterAttackData teamData, Action done = null)
        {
            StartCoroutine(IEAttackMonster(teamData, true, done));
        }

        #endregion

        #region API
        public IEnumerator IEAttackMonster(MonsterAttackData teamData, bool isAdv = false, Action done = null)
        {
            BattleEngineController.Instance.ResultAttackMonsterResponse = null;
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.Instance.GetUserID();
            jdata["team"] = JSONNode.Parse(JsonUtility.ToJson(BattleTeamManager.Instance.GetBattleTeamDataSelected().ToShortTeam()));
            jdata["attackData"] = JSONNode.Parse(JsonUtility.ToJson(teamData));
            jdata["isAdv"] = isAdv;
            yield return APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + MonsterConfig.API_AttackMonster, (data) =>
            {
                BattleEngineController.Instance.LastMonsterAttackData = teamData;
                MonsterOnMapData monsterOnMapData = this.ShardOnMapData.Find(x => x._id == teamData.TeamID);
                if (monsterOnMapData != null)
                {
                    this.ShardOnMapData.Remove(monsterOnMapData);
                }
                APIResponseData aPIResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (aPIResponseData.Status == 0) return;
                if (aPIResponseData.BattleResult.IsWin)
                {
                    string monsterTeam = aPIResponseData.BattleResult.Result.TeamB.TeamID;
                    MonsterOnMapData found = this.GrindingOnMapData.Find(x => x._id == monsterTeam);
                    if (found != null)
                    {
                        this.GrindingOnMapData.Remove(found);
                        this.UpdateBattleStatus(aPIResponseData.BattleMonsterStatus);
                    }
                    else
                    {
                        found = this.ShardOnMapData.Find(x => x._id == monsterTeam);
                        if (found != null)
                        {
                            this.ShardOnMapData.Remove(found);
                            this.UpdateBattleStatus(aPIResponseData.BattleMonsterStatus);
                        }
                    }
                }
                done?.Invoke();
            });
        }

        public IEnumerator IEResultAttackMonster(Action done = null)
        {
            if (BattleEngineController.Instance.ResultAttackMonsterResponse == null || BattleEngineController.Instance.ResultAttackMonsterResponse._id == null || BattleEngineController.Instance.ResultAttackMonsterResponse._id != BattleEngineController.Instance.BattleResult._id)
            {
                JSONNode jdata = new JSONObject();
                jdata["userID"] = UserDataManager.Instance.GetUserID();
                ResultAttackMonsterData resultAttackMonsterData = new ResultAttackMonsterData();
                resultAttackMonsterData._id = BattleEngineController.Instance.BattleResult._id;
                resultAttackMonsterData.Victory = BattleEngineController.Instance.BattleResult.IsWin;
                resultAttackMonsterData.AttackType = BattleEngineController.Instance.LastMonsterAttackData.AttackType;
                resultAttackMonsterData.MonsterIndex = this.GetMainMonsterReward(BattleEngineController.Instance.LastMonsterAttackData.Monsters);
                jdata["resultAttackMonster"] = JSONNode.Parse(JsonUtility.ToJson(resultAttackMonsterData));

                yield return APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + MonsterConfig.API_ResultAttackMonster, (data) =>
                {
                    APIResponseData aPIResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                    if (aPIResponseData.Status == 0) return;
                    BattleEngineController.Instance.ResultAttackMonsterResponse = aPIResponseData.ResultAttackMonsterResponse;
                    done?.Invoke();
                });
            }
            else yield break;

        }

        public IEnumerator IEDoubleReward(Action done = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.Instance.GetUserID();
            jdata["data"] = JSONNode.Parse(JsonUtility.ToJson(BattleEngineController.Instance.ResultAttackMonsterResponse));
            yield return APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + MonsterConfig.API_DoubleReward, (data) =>
            {
                APIResponseData aPIResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (aPIResponseData.Status == 0) return;
                done?.Invoke();
            });
        }
        #endregion

        #region Getter

        public List<MonsterData> GetMonsterData(MonsterOnMapData monsterOnMapData)
        {
            // 9 slot
            List<MonsterData> monsterData = new List<MonsterData>(){
                null, null, null,
                null, null, null,
                null, null, null,
            };

            if (monsterOnMapData.AttackType == AttackType.Shard)
            {
                // Get Main Monster

                CardPlayerIndex mainMonsterIndex = monsterOnMapData.Index;
                monsterData[MonsterConfig.MonsterMainSlot[0]] = new MonsterData(mainMonsterIndex, monsterOnMapData.Lv, this.GetStarMonsterByLevel(monsterOnMapData.AttackType), this.GetScaleMonsterByLevel(monsterOnMapData.AttackType));


                // Get Support Monster
                System.Random randomSupport = new System.Random((int)(monsterOnMapData.coordinates.longitude * 1000));
                int amountMonsterSupport = this.GetAmountMonsterSupport();
                List<int> supportSlot = new List<int>(MonsterConfig.MonsterSupportSlot);

                List<CardPlayerIndex> evolveMonster = new List<CardPlayerIndex>();
                foreach (CardPlayerIndex index in CardPlayerManager.Instance.GetPathEvolve(mainMonsterIndex))
                {
                    if (index == mainMonsterIndex) continue;
                    evolveMonster.Add(index);
                }
                evolveMonster.Add(mainMonsterIndex);

                for (int i = 0; i < amountMonsterSupport; i++)
                {
                    CardPlayerIndex supportMonsterIndex = evolveMonster[randomSupport.Next(0, evolveMonster.Count)];
                    int slot = supportSlot[randomSupport.Next(0, supportSlot.Count)];
                    monsterData[slot] = new MonsterData(supportMonsterIndex, monsterOnMapData.Lv, this.GetStarMonsterByLevel(monsterOnMapData.AttackType), this.GetScaleMonsterByLevel(monsterOnMapData.AttackType));
                    supportSlot.RemoveAt(supportSlot.IndexOf(slot));
                }
            }
            else
            {
                CardPlayerIndex mainMonsterIndex = monsterOnMapData.Index;
                monsterData[MonsterConfig.MonsterMainSlot[0]] = new MonsterData(mainMonsterIndex, monsterOnMapData.Lv, this.GetStarMonsterByLevel(monsterOnMapData.AttackType), this.GetScaleMonsterByLevel(monsterOnMapData.AttackType));


                // Get Support Monster
                System.Random randomSupport = new System.Random((int)(monsterOnMapData.coordinates.longitude * 1000));
                int amountMonsterSupport = this.GetAmountMonsterSupport();
                List<int> supportSlot = new List<int>(MonsterConfig.MonsterSupportSlot);
                for (int i = 0; i < amountMonsterSupport; i++)
                {
                    // CardPlayerIndex supportMonsterIndex = MonsterConfig.CreepOnMap[randomSupport.Next(0, MonsterConfig.CreepOnMap.Count)];
                    CardPlayerIndex supportMonsterIndex = this.BattleMonsterWorldMapData.CreepOnMap[UnityEngine.Random.Range(0, this.BattleMonsterWorldMapData.CreepOnMap.Length)]; ;
                    int slot = supportSlot[randomSupport.Next(0, supportSlot.Count)];
                    monsterData[slot] = new MonsterData(supportMonsterIndex, monsterOnMapData.Lv, this.GetStarMonsterByLevel(monsterOnMapData.AttackType), this.GetScaleMonsterByLevel(monsterOnMapData.AttackType));
                    supportSlot.RemoveAt(supportSlot.IndexOf(slot));
                }
            }
            return monsterData;
        }

        public MonsterOnMapData GetMonsterOnMapData(AttackType attackType = AttackType.Grinding)
        {
            MonsterOnMapData monsterOnMapData = new MonsterOnMapData();
            if (attackType == AttackType.Shard)
            {
                monsterOnMapData.Index = this.BattleMonsterWorldMapData.MonsterOnMap[UnityEngine.Random.Range(0, this.BattleMonsterWorldMapData.MonsterOnMap.Length)];
            }
            else
            {
                monsterOnMapData.Index = this.BattleMonsterWorldMapData.CreepOnMap[UnityEngine.Random.Range(0, this.BattleMonsterWorldMapData.CreepOnMap.Length)];
            }
            monsterOnMapData.Lv = UserDataManager.Instance.GetLevel();
            monsterOnMapData.AttackType = attackType;
            monsterOnMapData.coordinates = GeoPointManager.Instance.GetRandomRange(GeoPointManager.Instance.GetCurrentLocation(), MonsterConfig.Visibility);
            return monsterOnMapData;
        }

        // Max 4 monster support
        public int GetAmountMonsterSupport()
        {
            return this.GetAmountMonsterWorldMap();
        }

        public CardPlayerIndex GetMainMonsterReward(MonsterData[] monsters)
        {
            if (monsters == null || monsters.Length == 0) return CardPlayerIndex.Card_0;
            if (monsters[4] == null || monsters[4]._id == null || monsters[4]._id.Length == 0) return CardPlayerIndex.Card_0;
            CardPlayerData cardPlayerData = CardPlayerManager.Instance.GetCardPlayerDataByIndex(monsters[4].Index);
            return cardPlayerData.Index;
        }

        public RewardItem_Rate GetMonsterReward(MonsterAttackData monsterAttackData)
        {
            RewardItem_Rate rewardItem_Rate = new RewardItem_Rate();
            if (monsterAttackData.AttackType == AttackType.Shard)
            {
                // Get Shard Reward
                List<ItemRate> itemRates = new List<ItemRate>();
                CardPlayerIndex mainMonster = this.GetMainMonsterReward(monsterAttackData.Monsters);
                ItemType mainMonsterReward = CardPlayerManager.Instance.GetCardPlayerDataByIndex(mainMonster).ShardCardType;
                foreach (AmountRate amountRate in this.BattleMonsterWorldMapData.ShardReward)
                {
                    ItemRate itemRate = new ItemRate();
                    itemRate.Type = mainMonsterReward;
                    itemRate.Amount = amountRate.Amount;
                    itemRate.Rate = amountRate.Rate;
                    itemRates.Add(itemRate);
                }
                itemRates.Sort((a, b) => b.Rate.CompareTo(a.Rate));
                foreach (ItemRate itemRate in itemRates)
                {
                    rewardItem_Rate.Rates.Add(itemRate);
                }

                // Get Item Reward
                foreach (ItemData itemData in this.BattleMonsterWorldMapData.ShardItemReward)
                {
                    rewardItem_Rate.Items.Add(itemData);
                }
            }
            else
            {
                // Get Shard Reward
                List<ItemRate> itemRates = new List<ItemRate>();
                CardPlayerIndex mainMonster = this.GetMainMonsterReward(monsterAttackData.Monsters);
                ItemType mainMonsterReward = CardPlayerManager.Instance.GetCardPlayerDataByIndex(mainMonster).ShardCardType;
                foreach (AmountRate amountRate in this.BattleMonsterWorldMapData.GrindingShardReward)
                {
                    ItemRate itemRate = new ItemRate();
                    itemRate.Type = mainMonsterReward;
                    itemRate.Amount = amountRate.Amount;
                    itemRate.Rate = amountRate.Rate;
                    itemRates.Add(itemRate);
                }
                itemRates.Sort((a, b) => b.Rate.CompareTo(a.Rate));
                foreach (ItemRate itemRate in itemRates)
                {
                    rewardItem_Rate.Rates.Add(itemRate);
                }
                foreach (ItemData itemData in this.BattleMonsterWorldMapData.GrindingItemReward)
                {
                    rewardItem_Rate.Items.Add(itemData);
                }
                foreach (ItemRate itemRate in this.BattleMonsterWorldMapData.GrindingReward)
                {
                    rewardItem_Rate.Rates.Add(itemRate);
                }
            }
            return rewardItem_Rate;
        }

        public float GetScaleMonsterByLevel(AttackType attackType)
        {
            ScaleMonsterByLevel[] scaleMonsterByLevels = this.BattleMonsterWorldMapData.ScaleShardByLevel;
            if (attackType == AttackType.Grinding) scaleMonsterByLevels = this.BattleMonsterWorldMapData.ScaleGrindingByLevel;
            foreach (ScaleMonsterByLevel scaleMonsterByLevel in scaleMonsterByLevels)
            {
                if (UserDataManager.Instance.GetLevel() <= scaleMonsterByLevel.LowerLevel)
                {
                    return scaleMonsterByLevel.Scale;
                }
            }
            return 1;
        }

        public int GetStarMonsterByLevel(AttackType attackType)
        {
            ScaleMonsterByLevel[] scaleMonsterByLevels = this.BattleMonsterWorldMapData.ScaleShardByLevel;
            if (attackType == AttackType.Grinding) scaleMonsterByLevels = this.BattleMonsterWorldMapData.ScaleGrindingByLevel;
            foreach (ScaleMonsterByLevel scaleMonsterByLevel in scaleMonsterByLevels)
            {
                if (UserDataManager.Instance.GetLevel() <= scaleMonsterByLevel.LowerLevel)
                {
                    return scaleMonsterByLevel.Star;
                }
            }
            return 0;
        }

        public int GetAmountMonsterWorldMap()
        {
            foreach (MonsterWorldMapAmount monsterWorldMapAmount in this.BattleMonsterWorldMapData.MonsterWorldMapAmount)
            {
                if (UserDataManager.Instance.GetLevel() <= monsterWorldMapAmount.LowerLevel)
                {
                    return monsterWorldMapAmount.Amount;
                }
            }
            return 4;
        }
        public MonsterOnMapModel GetMonsterOnMap(int index)
        {
            return MonsterOnMapModel[index];
        }
        public bool IsSkipBattle()
        {
            return PlayerPrefs.GetInt("SkipBattle", 0) == 1;
        }

        public ItemData GetCostEnergy()
        {
            return this.BattleMonsterWorldMapData.ItemConsume;
        }

        public int GetPower(CardPlayerIndex index, int level, int star = 0)
        {
            CardPlayerData cardPlayerData = CardPlayerManager.Instance.GetCardPlayerDataByIndex(index);
            if (cardPlayerData == null) return 0;
            CardUpStar data_up_star = CardPlayerManager.Instance.GetCardUpStarDataByStar(star);
            if (data_up_star == null) return 0;
            CardLevelData cardLevelData = CardPlayerManager.Instance.GetCardLevelDataByLevel(level);
            if (cardLevelData == null) return 0;
            long atk = (long)(cardPlayerData.ATK * (1 + cardLevelData.UpStat));
            long def = (long)(cardPlayerData.DEF * (1 + cardLevelData.UpStat));
            long hp = (long)(cardPlayerData.HP * (1 + cardLevelData.UpStat));
            long spd = (long)(cardPlayerData.SPD * (1 + cardLevelData.UpStat));
            return (int)(atk * 5 + def * 5 + hp + spd * 10);
        }

        public BattleShortTeam GetBattleShortTeam(MonsterAttackData monsterAttackData){
            BattleShortTeam battleShortTeam = new BattleShortTeam();
            battleShortTeam.Cards = new CardShortTeam[monsterAttackData.Monsters.Length];
            battleShortTeam.Gears = null;
            battleShortTeam.Hero = null;
            List<CardShortTeam> cardShortTeams = new List<CardShortTeam>();
            for (int i = 0; i < monsterAttackData.Monsters.Length; i++)
            {
                if(monsterAttackData.Monsters[i] == null || monsterAttackData.Monsters[i]._id == null || monsterAttackData.Monsters[i]._id == "") continue;
                CardShortTeam cardShortTeam = new CardShortTeam();
                cardShortTeam.Index = monsterAttackData.Monsters[i].Index;
                cardShortTeam.Level = monsterAttackData.Monsters[i].Lv;
                cardShortTeam.Star = monsterAttackData.Monsters[i].Star;
                cardShortTeams.Add(cardShortTeam);
            }
            battleShortTeam.Cards = cardShortTeams.ToArray();
            return battleShortTeam;
        }

        //public (int remain, int cap) GetAmountAdvAttackLimit()
        //{
        //    return (this.BattleMonsterWorldMapData.AdvLimit - this.BattleStatus.AttackAdvLimit, this.BattleMonsterWorldMapData.AdvLimit);
        //}

        #endregion

        #region Setter
        public void SetSkipBattle()
        {
            PlayerPrefs.SetInt("SkipBattle", this.IsSkipBattle() ? 0 : 1);
        }
        #endregion
    }
}

