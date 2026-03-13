using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using Sirenix.OdinInspector;
using UnityEngine;
using GOA.Config;
using Rubik.DataCenter;
using Rubik.Config;
using Rubik.Manager;
using Rubik.UserDataPlayer;
using NTPackage.Functions;
using NTPackage;

namespace Rubik.CardPlayer
{
    using System.Linq;
    using ItemPlayer;
    using NTPackage.UI;
    using Rubik.AddressablesLoader;
    using Rubik.CharacterPlayer;
    using Rubik.Common.AudioHelper;
    using Rubik.DataType;
    using Rubik.Myrk.BattleTeam;
    using Rubik.Myrk.Skill;
    using Rubik.Quest;
    using Spine.Unity;

    public class CardPlayerConfig
    {
        public const string API_Summon_Card = "/api/2D_GPS/card_player/summon_card";
        public const string API_EvolveCard = "/api/2D_GPS/card_player/evolve_card";
        public const string API_UpStarCard = "/api/2D_GPS/card_player/up_star_card";
        public const string API_LevelUp = "/api/2D_GPS/card_player/level_up";
        public const string API_CombineCardShard = "/api/2D_GPS/card_player/combine_card_shard";
        public const string API_MergeCardShard = "/api/2D_GPS/card_player/merge_card_shard";

        public static List<CardPlayerIndex> PortalBossCardPlayerIndex = new List<CardPlayerIndex>() { CardPlayerIndex.Portal_Boss_0, CardPlayerIndex.Portal_Boss_1, CardPlayerIndex.Portal_Boss_2, CardPlayerIndex.Portal_Boss_3, CardPlayerIndex.Portal_Boss_4 };
        public static List<CardPlayerIndex> ClanBossCardPlayerIndex = new List<CardPlayerIndex>() { CardPlayerIndex.ClanBoss_0, CardPlayerIndex.ClanBoss_1, CardPlayerIndex.ClanBoss_2, CardPlayerIndex.ClanBoss_3, CardPlayerIndex.ClanBoss_4 };

    }

    public class CardPlayerManager : NTBehaviour
    {
        #region Player Data
        [Header("Player Data")]
        [SerializeField] private NTDictionary<string, CardPlayer> CardPlayerDic;
        [SerializeField] private NTDictionary<string, SummonHistory> SummonHistory;
        #endregion

        #region Game Data
        [Header("Game Data")]
        [SerializeField] private NTDictionary<string, CardPlayerData> CardPlayerDataDic;
        [SerializeField] private CardEvolveData CardEvolveData;
        [SerializeField] private NTDictionary<string, CardUpStar> CardUpStarDataDic;
        [SerializeField] private NTDictionary<string, CardPlayerIndex[]> CardPlayerPathEvolveDic;
        [SerializeField] private NTDictionary<string, CardSkillPassive> CardSkillPassiveDataDic;
        [SerializeField] private NTDictionary<string, CardSkillActive> CardSkillActiveDataDic;
        [SerializeField] private NTDictionary<string, CardLevelData> CardLevelDataDic;
        [SerializeField] private SkillLevelByStar SkillLevelByStarData;
        [SerializeField] private List<CardSummonData> CardSummonDatas;
        [SerializeField] private List<CardTierOriginData> CardTierOriginDatas;
        #endregion

        #region Resource Data
        [Header("Resource Data")]

        [SerializeField] private ListSpriteAddressable CardPlayerAvatarSpriteAddressable;
        [SerializeField] private NTDictionary<string, Sprite> CardPlayerAvatarSpriteDic;

        [SerializeField] private ListTransformAddressable CardPlayerAvatarTransAddressable;
        [SerializeField] private NTDictionary<string, Transform> CardPlayerAvatarDic;

        [SerializeField] private ListTransformAddressable CardPlayerSkeletonOnMapTransAddressable;
        [SerializeField] private NTDictionary<string, Transform> CardPlayerSkeletonOnMapDic;

        [SerializeField] private ListTransformAddressable CardPlayerSkeletonGraphicAddressable;
        [SerializeField] private NTDictionary<string, RectTransform> CardPlayerSkeletonGraphicDic;

        [SerializeField] private ListScriptableObjectAddressable CardPlayerBaseCharacterDataSOAddressable;
        [SerializeField] private NTDictionary<string, BaseCharacterDataSO> CardPlayerBaseCharacterDataSO;

        public List<Sprite> OriginSpritesCircle;
        public List<Sprite> OriginSpritesSquare;
        public List<Sprite> OriginSpritesFlag;
        #endregion


        public static CardPlayerManager Instance;
        protected override void Awake()
        {
            base.Awake();
            if (CardPlayerManager.Instance != null)
            {
                Debug.LogWarning("Only 1 instance allow");
                return;
            }
            CardPlayerManager.Instance = this;
        }

        #region Function

        public IEnumerator LoadData()
        {
            yield return null;
            this.CardPlayerDataDic = new NTDictionary<string, CardPlayerData>();
            JSONNode jdata = JSONNode.Parse(DataCenterManager.Instance.GetData(DataName.CardPlayerData));
            foreach (JSONNode item in jdata)
            {
                CardPlayerData cardPlayerData = JsonUtility.FromJson<CardPlayerData>(item.ToString());
                this.CardPlayerDataDic.Add(cardPlayerData.Index.ToString(), cardPlayerData);
            }

            this.CardEvolveData = new CardEvolveData();
            jdata = JSONNode.Parse(DataCenterManager.Instance.GetData(DataName.CardEvolveData));
            this.CardEvolveData = JsonUtility.FromJson<CardEvolveData>(jdata.ToString());
            this.CardPlayerPathEvolveDic = new NTDictionary<string, CardPlayerIndex[]>();
            foreach (JSONNode item in jdata["PathEvolve"])
            {
                List<CardPlayerIndex> pathEvolve = new List<CardPlayerIndex>();
                foreach (JSONNode index in item)
                {
                    pathEvolve.Add((CardPlayerIndex)index.AsInt);
                }
                foreach (CardPlayerIndex index in pathEvolve)
                {
                    this.CardPlayerPathEvolveDic.Add(index.ToString(), pathEvolve.ToArray());
                }
            }

            this.CardLevelDataDic = new NTDictionary<string, CardLevelData>();
            jdata = JSONNode.Parse(DataCenterManager.Instance.GetData(DataName.CardLevelData));
            foreach (JSONNode item in jdata)
            {
                CardLevelData cardLevelData = JsonUtility.FromJson<CardLevelData>(item.ToString());
                this.CardLevelDataDic.Add(cardLevelData.Level.ToString(), cardLevelData);
            }

            this.CardUpStarDataDic = new NTDictionary<string, CardUpStar>();
            jdata = JSONNode.Parse(DataCenterManager.Instance.GetData(DataName.CardUpStarData));
            foreach (JSONNode item in jdata)
            {
                CardUpStar cardUpStar = JsonUtility.FromJson<CardUpStar>(item.ToString());
                this.CardUpStarDataDic.Add(cardUpStar.Star.ToString(), cardUpStar);
            }

            this.CardSkillPassiveDataDic = new NTDictionary<string, CardSkillPassive>();
            jdata = JSONNode.Parse(DataCenterManager.Instance.GetData(DataName.CardSkillPassiveData));
            foreach (JSONNode item in jdata)
            {
                CardSkillPassive cardSkillPassive = JsonUtility.FromJson<CardSkillPassive>(item.ToString());
                this.CardSkillPassiveDataDic.Add(cardSkillPassive.Index.ToString(), cardSkillPassive);
            }

            this.CardSkillActiveDataDic = new NTDictionary<string, CardSkillActive>();
            jdata = JSONNode.Parse(DataCenterManager.Instance.GetData(DataName.CardSkillData));
            foreach (JSONNode item in jdata)
            {
                CardSkillActive cardSkillActive = JsonUtility.FromJson<CardSkillActive>(item.ToString());
                this.CardSkillActiveDataDic.Add(cardSkillActive.Index.ToString(), cardSkillActive);
            }

            this.SkillLevelByStarData = JsonUtility.FromJson<SkillLevelByStar>(DataCenterManager.Instance.GetData(DataName.SkillLevelByStarData));

            this.CardSummonDatas = new List<CardSummonData>();
            jdata = JSONNode.Parse(DataCenterManager.Instance.GetData(DataName.CardSummonData));
            foreach (JSONNode item in jdata)
            {
                CardSummonData cardSummonData = JsonUtility.FromJson<CardSummonData>(item.ToString());
                this.CardSummonDatas.Add(cardSummonData);
            }

            this.CardTierOriginDatas = new List<CardTierOriginData>();
            jdata = JSONNode.Parse(DataCenterManager.Instance.GetData(DataName.CardTierOriginData));
            foreach (JSONNode item in jdata)
            {
                CardTierOriginData cardTierOriginData = JsonUtility.FromJson<CardTierOriginData>(item.ToString());
                this.CardTierOriginDatas.Add(cardTierOriginData);
            }

            int amount = 0;

            AddressablesLoader.Instance.LoadAssetsFromLocal(AddressablesAssetsConfig.CardPlayerAvatarSprite, (result) =>
            {
                this.CardPlayerAvatarSpriteAddressable = result.GetComponent<ListSpriteAddressable>();
                this.CardPlayerAvatarSpriteAddressable.transform.SetParent(transform);
                this.CardPlayerAvatarSpriteDic = new NTDictionary<string, Sprite>();
                foreach (var item in this.CardPlayerAvatarSpriteAddressable.ListSprite)
                {
                    this.CardPlayerAvatarSpriteDic.Add(item.name, item);
                }
                amount++;
            });

            AddressablesLoader.Instance.LoadAssetsFromLocal(AddressablesAssetsConfig.CardPlayerAvatarTrans, (result) =>
            {
                this.CardPlayerAvatarTransAddressable = result.GetComponent<ListTransformAddressable>();
                this.CardPlayerAvatarTransAddressable.transform.SetParent(transform);
                this.CardPlayerAvatarDic = new NTDictionary<string, Transform>();
                foreach (Transform item in this.CardPlayerAvatarTransAddressable.ListTransform)
                {
                    this.CardPlayerAvatarDic.Add(item.name, item);
                }
                amount++;
            });

            AddressablesLoader.Instance.LoadAssetsFromLocal(AddressablesAssetsConfig.CardPlayerSkeletonOnMapTrans, (result) =>
            {
                this.CardPlayerSkeletonOnMapTransAddressable = result.GetComponent<ListTransformAddressable>();
                this.CardPlayerSkeletonOnMapTransAddressable.transform.SetParent(transform);
                this.CardPlayerSkeletonOnMapDic = new NTDictionary<string, Transform>();
                foreach (Transform item in this.CardPlayerSkeletonOnMapTransAddressable.ListTransform)
                {
                    this.CardPlayerSkeletonOnMapDic.Add(item.name, item);
                }
                amount++;
            });

            AddressablesLoader.Instance.LoadAssetsFromLocal(AddressablesAssetsConfig.CardPlayerSkeletonGraphic, (result) =>
            {
                this.CardPlayerSkeletonGraphicAddressable = result.GetComponent<ListTransformAddressable>();
                this.CardPlayerSkeletonGraphicAddressable.transform.SetParent(transform);
                this.CardPlayerSkeletonGraphicDic = new NTDictionary<string, RectTransform>();
                foreach (RectTransform item in this.CardPlayerSkeletonGraphicAddressable.ListTransform)
                {
                    this.CardPlayerSkeletonGraphicDic.Add(item.name, item);
                }
                amount++;
            });

            AddressablesLoader.Instance.LoadAssetsFromLocal(AddressablesAssetsConfig.CardPlayerBaseCharacterDataSOAddressable, (result) =>
            {
                this.CardPlayerBaseCharacterDataSOAddressable = result.GetComponent<ListScriptableObjectAddressable>();
                this.CardPlayerBaseCharacterDataSOAddressable.transform.SetParent(transform);
                this.CardPlayerBaseCharacterDataSO = new NTDictionary<string, BaseCharacterDataSO>();
                foreach (BaseCharacterDataSO item in this.CardPlayerBaseCharacterDataSOAddressable.ListScriptableObject)
                {
                    this.CardPlayerBaseCharacterDataSO.Add(item.baseData.Index.ToString(), item);
                }
                amount++;
            });

            yield return new WaitUntil(() => amount >= 5);
        }

        public void UpdateCardPlayer(CardPlayer[] cardPlayers)
        {
            foreach (CardPlayer item in cardPlayers)
            {
                if (this.CardPlayerDic.Get(item._id) != null)
                {
                    this.CardPlayerDic.Get(item._id).UpdateData(item);
                }
                else
                {
                    this.CardPlayerDic.Add(item._id, item);
                }
                CardPlayer cardPlayer = this.CardPlayerDic.Get(item._id);
                UpdateCacheCardPlayer(cardPlayer);
                AchievementManager.Instance.DoAchievement(AchievementPlayerIndex.ApprenticeTrainer, -1, cardPlayer.Star+1);
            }
        }

        public void UpdateSummonHistory(SummonHistory[] summonHistory)
        {
            foreach (SummonHistory item in summonHistory)
            {
                this.SummonHistory.Add(item.Type.ToString(), item);
            }
        }


        public void UpdateCacheCardPlayer(CardPlayer cardPlayer)
        {
            cardPlayer.CardPlayerData = this.CardPlayerDataDic.Get(cardPlayer.Index.ToString());
            cardPlayer.CardSkillLv = this.GetCardSkill(cardPlayer.Index, cardPlayer.Star);
            (long atk, long def, long hp, long spd) = this.GetCardStatBaseLv_Star(cardPlayer.Index, cardPlayer.Lv, cardPlayer.Star);
            cardPlayer.BaseStats = new BattleEngine.BattleStats();
            cardPlayer.BaseStats.ATK = atk;
            cardPlayer.BaseStats.DEF = def;
            cardPlayer.BaseStats.HP = hp;
            cardPlayer.BaseStats.SPD = spd;

            cardPlayer.SkillStats = new BattleEngine.BattleStats();
            foreach (CardSkillPassiveLv item in cardPlayer.CardSkillLv.Passive)
            {
                if (item.IsLock) continue;
                BattleEngine.BattleStats.Add(cardPlayer.SkillStats, this.GetCardPlayerPassiveSkillStats(item, cardPlayer.BaseStats));
            }

            cardPlayer.TotalStats = new BattleEngine.BattleStats();

            BattleEngine.BattleStats.Add(cardPlayer.TotalStats, cardPlayer.BaseStats);
            BattleEngine.BattleStats.Add(cardPlayer.TotalStats, cardPlayer.SkillStats);
        }

        public void Logout()
        {
            this.CardPlayerDic.Clear();
            this.SummonHistory.Clear();
        }

        public void Summon(int time, Action<CardPlayer[]> callback = null)
        {
            // StartCoroutine(IESummon(time, callback));
        }

        public void ShowPopupStory(CardPlayerIndex cardIndex, int star = 0, int level = 0, float scale = 1)
        {
            PopupManager.Instance.OnUI(PopupCode.MonsterStoryUI, null, (popup) =>
            {
                MonsterStoryUI monsterStoryUI = popup as MonsterStoryUI;
                monsterStoryUI.SetData(cardIndex, star, level, scale);
            });
        }

        public void BaseAttackAudio(CardPlayerIndex index)
        {
            CardPlayerData cardPlayerData = this.GetCardPlayerDataByIndex(index);
            if (cardPlayerData == null) return;
            StartCoroutine(NTFunction.WaitSecond(cardPlayerData.SoundDelayBaseAttack, () =>
            {
                AudioCtrl.Instance.Play(cardPlayerData.SoundNameBaseAttack);
            }));
        }

        public void SkillActiveAudio(CardPlayerIndex index)
        {
            CardPlayerData cardPlayerData = this.GetCardPlayerDataByIndex(index);
            if (cardPlayerData == null) return;
            StartCoroutine(NTFunction.WaitSecond(cardPlayerData.SoundDelaySkillAttack, () =>
            {
                AudioCtrl.Instance.Play(cardPlayerData.SoundNameSkillAttack);
            }));
        }

        #endregion


        #region API
        public void SummonCard(SummonType summonType, int time, Action<RewardSummon> callback = null)
        {
            StartCoroutine(IESummonCard(summonType, time, false, callback));
        }

        public void SummonCardAdv(SummonType summonType, int time, Action<RewardSummon> callback = null)
        {
            StartCoroutine(IESummonCard(summonType, time, true, callback));
        }

        public IEnumerator IESummonCard(SummonType summonType, int time, bool isAdv, Action<RewardSummon> callback = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.Instance.GetUserID();
            jdata["summonType"] = (int)summonType;
            jdata["time"] = time;
            jdata["isAdv"] = isAdv;

            yield return Rubik.Server.APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + CardPlayerConfig.API_Summon_Card, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                callback?.Invoke(apiResponseData.RewardSummon);
            });
        }

        public IEnumerator IELevelUp(string cardID, Action callback = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.Instance.GetUserID();
            jdata["cardID"] = cardID;
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + CardPlayerConfig.API_LevelUp, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                callback?.Invoke();
                callback = null;
            });
            callback?.Invoke();
        }

        public IEnumerator IEEvolveCard(string cardID, Action callback = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.Instance.GetUserID();
            jdata["cardID"] = cardID;
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + CardPlayerConfig.API_EvolveCard, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                callback?.Invoke();
            });
        }

        public IEnumerator IEUpStarCard(string cardID, Action callback = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.Instance.GetUserID();
            jdata["cardID"] = cardID;
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + CardPlayerConfig.API_UpStarCard, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                callback?.Invoke();
                callback = null;
            });
            callback?.Invoke();
        }

        public IEnumerator IECombineCardShard(ItemType itemType, Action<CardPlayer[]> callback = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.Instance.GetUserID();
            jdata["cardShardIndex"] = (int)itemType;
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + CardPlayerConfig.API_CombineCardShard, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                callback?.Invoke(apiResponseData.CombineResult);
            });
        }

        public IEnumerator IEMergeCardShard(ItemType itemType, Action<ItemData[]> callback = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.Instance.GetUserID();
            jdata["cardShardIndex"] = (int)itemType;
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + CardPlayerConfig.API_MergeCardShard, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                callback?.Invoke(apiResponseData.MergeResult);
            });
        }

        #endregion

        #region Getter

        public CardPlayer GetCardByID(string cardID)
        {
            return this.CardPlayerDic.Get(cardID);
        }

        public string GetCardName(CardPlayerIndex index)
        {
            CardPlayerData cardPlayerData = this.CardPlayerDataDic.Get(index.ToString());
            if (cardPlayerData == null) return "";
            return Lean.Localization.LeanLocalization.GetTranslationText("monster" + (int)index + "_name", cardPlayerData.Name);
        }

        public string GetCardDescription(CardPlayerIndex index)
        {
            CardPlayerData cardPlayerData = this.CardPlayerDataDic.Get(index.ToString());
            if (cardPlayerData == null) return "";
            return Lean.Localization.LeanLocalization.GetTranslationText("monster" + (int)index + "_des", index.ToString());
        }

        public BaseCharacterDataSO GetCharacterByIndex(int index)
        {
            BaseCharacterDataSO data = this.CardPlayerBaseCharacterDataSO.Get(index.ToString());
            if (data == null)
            {
                NTLog.LogError("BaseCharacterDataSO not found: " + index);
                return null;
            }
            return data;
        }
        public void SetSkeletonAnimationData(SkeletonAnimation ske, int index)
        {
            var skeData = GetCharacterByIndex(index);

            ske.skeletonDataAsset = skeData.skeAsset;
            ske.Initialize(true);
            if (skeData.baseData.skinIndex > -1)
                ske.Skeleton.SetSkin(skeData.baseData.skinIndex.ToString());



        }
        public void SetSkeletonAnimationData(SkeletonGraphic ske, int index)
        {
            var skeData = GetCharacterByIndex(index);
            ske.skeletonDataAsset = skeData.skeAsset;
            ske.Initialize(true);
            if (skeData.baseData.skinIndex > -1)
                ske.Skeleton.SetSkin(skeData.baseData.skinIndex.ToString());

        }
        public Sprite GetCardPlayerSprite(CardPlayerIndex index)
        {
            CardPlayerData cardPlayerData = this.CardPlayerDataDic.Get(index.ToString());
            if (cardPlayerData == null) return null;
            return this.CardPlayerAvatarSpriteDic.Get(cardPlayerData.ResourceName);
        }

        public Transform InstantiatePlayerAvatar(CardPlayerIndex index)
        {
            CardPlayerData cardPlayerData = this.CardPlayerDataDic.Get(index.ToString());
            Transform avatar = ObjectPoolingManager.Instance.PullObjectFromPooling(ObjectPoolingConfig.CardPlayerAvatarCircle + ":" + cardPlayerData.ResourceName);
            if (avatar == null)
            {
                Transform avatarTrans = this.CardPlayerAvatarDic.Get(cardPlayerData.ResourceName);
                if (avatarTrans == null)
                {
                    NTLog.LogError("CardPlayerAvatarDic not found: " + cardPlayerData.ResourceName);
                    return null;
                }
                avatar = Instantiate(avatarTrans);
                avatar.gameObject.SetActive(true);
            }
            avatar.transform.name = ObjectPoolingConfig.CardPlayerAvatarCircle + ":" + cardPlayerData.ResourceName;
            NTLog.LogMessage(" Spawn : " + avatar.transform.name);
            return avatar;
        }

        public Transform InstantiatePlayerSkeletonOnMap(CardPlayerIndex index)
        {
            CardPlayerData cardPlayerData = this.GetCardPlayerDataByIndex(index);
            Transform avatar = ObjectPoolingManager.Instance.PullObjectFromPooling(ObjectPoolingConfig.CardPlayerSkeletonOnMap + ":" + cardPlayerData.ResourceName);
            if (avatar == null)
            {
                avatar = Instantiate(this.CardPlayerSkeletonOnMapDic.Get(cardPlayerData.ResourceName));
                avatar.gameObject.SetActive(true);
            }
            avatar.name = ObjectPoolingConfig.CardPlayerSkeletonOnMap + ":" + cardPlayerData.ResourceName;
            return avatar;
        }

        public int BackShardWhenEvolve(int star)
        {
            var total_shard = 0;
            for (int index = 0; index < star; index++)
            {
                CardUpStar data_up_star = this.GetCardUpStarDataByStar(index);
                if (data_up_star == null) continue;
                total_shard += data_up_star.Shard;
            }
            return total_shard;
        }

        public bool CanEvolve(CardPlayerIndex index)
        {
            CardPlayerData cardPlayerData = this.CardPlayerDataDic.Get(index.ToString());
            if (cardPlayerData == null) return false;
            return cardPlayerData.CanEvolve;
        }

        public List<ItemData> GetPriceEvolveCard(CardPlayerIndex index)
        {
            List<ItemData> price = new List<ItemData>();
            CardPlayerData cardPlayerData = this.CardPlayerDataDic.Get(index.ToString());
            if (cardPlayerData == null)
            {
                NTLog.LogError("CardPlayerData not found: " + index);
                return price;
            }
            CardEvolveData cardEvolveData = this.CardEvolveData;
            if (cardEvolveData == null)
            {
                NTLog.LogError("CardEvolveData not found: " + index);
                return price;
            }
            EvolveTierPrice evolveTierPrice = cardEvolveData.Price.ToList().Find(x => x.Tier == cardPlayerData.Tier);
            if (evolveTierPrice == null)
            {
                NTLog.LogError("EvolveTierPrice not found: " + index);
                return price;
            }
            price.Add(new ItemData(cardPlayerData.ShardCardType, evolveTierPrice.Shard));
            ItemData[] price_item = evolveTierPrice.Price;
            for (int i = 0; i < price_item.Length; i++)
            {
                price.Add(new ItemData(price_item[i].Type, price_item[i].Amount));
            }
            return price;
        }

        public CardPlayerIndex GetNextEvolveCard(CardPlayerIndex index)
        {
            CardPlayerData cardPlayerData = this.CardPlayerDataDic.Get(index.ToString());
            if (cardPlayerData == null) return index;
            return cardPlayerData.EvolveTarget;
        }

        public CardUpStar GetCardUpStarDataByStar(int star)
        {
            return this.CardUpStarDataDic.Get(star.ToString());
        }

        public bool IsMaxStar(int star)
        {
            CardUpStar data_up_star = this.GetCardUpStarDataByStar(star);
            if (data_up_star == null) return true;
            return data_up_star.Max;
        }

        public List<ItemData> GetPriceUpStarCard(CardPlayerIndex index, int star)
        {
            List<ItemData> price = new List<ItemData>();
            CardPlayerData cardPlayerData = this.CardPlayerDataDic.Get(index.ToString());
            if (cardPlayerData == null) return price;
            CardUpStar data_up_star = this.GetCardUpStarDataByStar(star);
            if (data_up_star == null) return price;
            price.Add(new ItemData(cardPlayerData.ShardCardType, data_up_star.Shard));
            for (int i = 0; i < data_up_star.Cost.Length; i++)
            {
                price.Add(new ItemData(data_up_star.Cost[i].Type, data_up_star.Cost[i].Amount));
            }
            return price;

        }

        public CardPlayerData GetCardPlayerDataByIndex(CardPlayerIndex index)
        {
            return this.CardPlayerDataDic.Get(index.ToString());
        }

        public List<CardPlayer> GetListCardPlayer()
        {
            return this.CardPlayerDic.ToList();
        }

        public CardPlayerIndex[] GetPathEvolve(CardPlayerIndex index)
        {
            return this.CardPlayerPathEvolveDic.Get(index.ToString());
        }

        public RectTransform InstantiatePlayerSkeletonGraphic(CardPlayerIndex index)
        {
            CardPlayerData cardPlayerData = this.GetCardPlayerDataByIndex(index);
            Transform ske = ObjectPoolingManager.Instance.PullObjectFromPooling(ObjectPoolingConfig.CardPlayerSkeletonGraphic + ":" + cardPlayerData.ResourceName);
            if (ske == null)
            {
                ske = Instantiate(this.CardPlayerSkeletonGraphicDic.Get(cardPlayerData.ResourceName));
                ske.gameObject.SetActive(true);
            }
            ske.transform.name = ObjectPoolingConfig.CardPlayerSkeletonGraphic + ":" + cardPlayerData.ResourceName;
            return ske.GetComponent<RectTransform>();
        }

        public CardLevelData GetCardLevelDataByLevel(int level)
        {
            return this.CardLevelDataDic.Get(level.ToString());
        }

        public (long atk, long def, long hp, long spd) GetCardStatBaseLv_Star(CardPlayerIndex index, int level, int star = 0)
        {
            CardPlayerData cardPlayerData = this.GetCardPlayerDataByIndex(index);
            if (cardPlayerData == null) return (0, 0, 0, 0);
            float upStatLevel = this.GetScaleStatByLevel(level);
            float upStatByStar = this.GetScaleStatByStar(star);
            long atk = (long)((cardPlayerData.ATK * (1 + upStatLevel)) * (1 + upStatByStar));
            long def = (long)((cardPlayerData.DEF * (1 + upStatLevel)) * (1 + upStatByStar));
            long spd = (long)((cardPlayerData.SPD + level * 2) * (1 + upStatByStar));
            long hp = (long)((cardPlayerData.HP * (1 + upStatLevel)) * (1 + upStatByStar));

            return (atk, def, hp, spd);
        }

        public (long atk, long def, long hp, long spd) GetCardIncreaseStatByNextLevel(CardPlayerIndex index, int level, int star)
        {
            (long atk, long def, long hp, long spd) = this.GetCardStatBaseLv_Star(index, level, star);
            (long atk_next, long def_next, long hp_next, long spd_next) = this.GetCardStatBaseLv_Star(index, level + 1, star);
            long atk_increase = atk_next - atk;
            long def_increase = def_next - def;
            long spd_increase = spd_next - spd;
            long hp_increase = hp_next - hp;
            return (atk_increase, def_increase, hp_increase, spd_increase);
        }

        public (long atk, long def, long hp, long spd) GetCardIncreaseStatByNextStar(CardPlayerIndex index, int level, int star)
        {
            (long atk, long def, long hp, long spd) = this.GetCardStatBaseLv_Star(index, level, star);
            (long atk_next, long def_next, long hp_next, long spd_next) = this.GetCardStatBaseLv_Star(index, level, star + 1);
            long atk_increase = atk_next - atk;
            long def_increase = def_next - def;
            long spd_increase = spd_next - spd;
            long hp_increase = hp_next - hp;
            return (atk_increase, def_increase, hp_increase, spd_increase);
        }


        public float GetScaleStatByLevel(int level)
        {
            CardLevelData cardLevelData = this.GetCardLevelDataByLevel(level);
            if (cardLevelData == null) return 0;
            return cardLevelData.UpStat;
        }

        public float GetScaleStatByStar(int star)
        {
            CardUpStar data_up_star = this.GetCardUpStarDataByStar(star);
            if (data_up_star == null) return 0;
            return data_up_star.UpStat;
        }

        //  Skill
        public TypeActive GetCardSkillActiveByIndex(CardPlayerIndex index)
        {
            CardPlayerData cardPlayerData = this.GetCardPlayerDataByIndex(index);
            if (cardPlayerData == null) return TypeActive.Basic_skill;
            return cardPlayerData.Skill;
        }

        public List<TypePassive> GetListCardSkillPassive(CardPlayerIndex index)
        {
            CardPlayerData cardPlayerData = this.GetCardPlayerDataByIndex(index);
            if (cardPlayerData == null) return new List<TypePassive>();
            return cardPlayerData.Passive.ToList();
        }

        public string GetSkillPassiveName(TypePassive index)
        {
            CardSkillPassive cardSkillPassive = this.CardSkillPassiveDataDic.Get(index.ToString());
            if (cardSkillPassive == null) return "";
            string name = Lean.Localization.LeanLocalization.GetTranslationText("skill_passive_name_" + (int)index, cardSkillPassive.Name);
            NTLog.LogMessage("skill_passive_name_" + (int)index + " : " + name);
            return name;
        }

        public string GetSkillPassiveDetail(TypePassive index, int level)
        {
            CardSkillPassive cardSkillPassive = this.CardSkillPassiveDataDic.Get(index.ToString());
            if (cardSkillPassive == null) return "";
            SkillValueByLevel value = cardSkillPassive.Value.ToList().Find(x => x.Level == level);
            if (value == null) value = cardSkillPassive.Value.Last();
            string detail = Lean.Localization.LeanLocalization.GetTranslationText("skill_passive_detail_" + (int)index, cardSkillPassive.Detail);
            NTLog.LogMessage("skill_passive_detail_" + (int)index + " : " + detail + ": " + value.Value.Length);
            List<string> ls_value = new List<string>();
            for (int i = 0; i < value.Value.Length; i++)
            {
                ls_value.Add(value.Value[i].ToString());
            }
            return String.Format(detail, ls_value.ToArray());
        }

        public Sprite GetSkillPassiveImage(TypePassive index)
        {
            CardSkillPassive cardSkillPassive = this.CardSkillPassiveDataDic.Get(index.ToString());
            if (cardSkillPassive == null) return null;
            return SkillManager.Instance.GetSkillImage(cardSkillPassive.Image);
        }

        public string GetSkillActiveName(TypeActive index)
        {
            CardSkillActive cardSkillActive = this.CardSkillActiveDataDic.Get(index.ToString());
            if (cardSkillActive == null) return "";
            string name = Lean.Localization.LeanLocalization.GetTranslationText("skill_active_name_" + (int)index, cardSkillActive.Name);
            NTLog.LogMessage("skill_active_name_" + (int)index + " : " + name);
            return name;
        }

        public string GetSkillActiveDetail(TypeActive index, int level)
        {
            CardSkillActive cardSkillActive = this.CardSkillActiveDataDic.Get(index.ToString());
            if (cardSkillActive == null) return "";
            SkillValueByLevel value = cardSkillActive.Value.ToList().Find(x => x.Level == level);
            if (value == null) value = cardSkillActive.Value.Last();
            string detail = Lean.Localization.LeanLocalization.GetTranslationText("skill_active_detail_" + (int)index, cardSkillActive.Detail);
            NTLog.LogMessage("skill_active_detail_" + (int)index + " : " + detail + ": " + value.Value.Length);
            List<string> ls_value = new List<string>();
            for (int i = 0; i < value.Value.Length; i++)
            {
                ls_value.Add(value.Value[i].ToString());
            }
            return String.Format(detail, ls_value.ToArray());
        }

        public Sprite GetSkillActiveImage(TypeActive index)
        {
            CardSkillActive cardSkillActive = this.CardSkillActiveDataDic.Get(index.ToString());
            if (cardSkillActive == null) return null;
            return SkillManager.Instance.GetSkillImage(cardSkillActive.Image);
        }

        public CardSkillLv GetCardSkill(CardPlayerIndex cardIndex, int star)
        {
            CardPlayerData cardPlayerData = this.GetCardPlayerDataByIndex(cardIndex);
            if (cardPlayerData == null) return new CardSkillLv();

            // Active skill
            var battle_skill = new CardSkillLv();
            CardSkillActive cardSkillActive = this.CardSkillActiveDataDic.Get(cardPlayerData.Skill.ToString());
            if (cardSkillActive == null)
            {
                NTLog.LogError("CardPlayerCtrl" + "GetSkillLevel" + "Active data not found");
                cardSkillActive = this.CardSkillActiveDataDic.Get(TypeActive.Basic_skill.ToString());
            }
            battle_skill.Active = new CardSkillActiveLv();
            battle_skill.Active.Index = cardPlayerData.Skill;
            battle_skill.Active.Level = 0;
            battle_skill.Active.IsLock = true;

            // Passive skill
            List<CardSkillPassive> passive_datas = new List<CardSkillPassive>();
            battle_skill.Passive = new List<CardSkillPassiveLv>();
            for (int index = 0; index < cardPlayerData.Passive.Length; index++)
            {
                CardSkillPassive cardSkillPassive = this.CardSkillPassiveDataDic.Get(cardPlayerData.Passive[index].ToString());
                if (cardSkillPassive == null)
                {
                    NTLog.LogError("CardPlayerCtrl" + "GetSkillLevel" + "Passive data not found");
                    break;
                }
                passive_datas.Add(cardSkillPassive);
                CardSkillPassiveLv passive_skill = new CardSkillPassiveLv();
                passive_skill.Index = cardPlayerData.Passive[index];
                passive_skill.Level = 0;
                passive_skill.IsLock = true;
                battle_skill.Passive.Add(passive_skill);
            }

            battle_skill.Active.StarUnlock = this.SkillLevelByStarData.Active[0];
            for (int index = 0; index < this.SkillLevelByStarData.Active.Length; index++)
            {
                int element = this.SkillLevelByStarData.Active[index];
                if (star >= element)
                {
                    battle_skill.Active.Level = index;
                    battle_skill.Active.IsLock = false;
                }
            }
            int max_level_active = this.GetMaxLevelSkillActive(cardSkillActive.Index);
            if (battle_skill.Active.Level >= max_level_active) battle_skill.Active.Level = max_level_active;

            for (int index = 0; index < passive_datas.Count; index++)
            {
                int[] stars = this.SkillLevelByStarData.Passive_1;
                if (index == 1) stars = this.SkillLevelByStarData.Passive_2;
                else if (index == 2) stars = this.SkillLevelByStarData.Passive_3;

                battle_skill.Passive[index].StarUnlock = stars[0];
                for (int star_index = 0; star_index < stars.Length; star_index++)
                {
                    if (star >= stars[star_index])
                    {
                        battle_skill.Passive[index].Level = star_index;
                        battle_skill.Passive[index].IsLock = false;
                    }
                }
                int max_level_passive = this.GetMaxLevelSkillPassive(passive_datas[index].Index);
                if (battle_skill.Passive[index].Level >= max_level_passive) battle_skill.Passive[index].Level = max_level_passive;
            }

            if (CardPlayerConfig.PortalBossCardPlayerIndex.Contains(cardPlayerData.Index) || CardPlayerConfig.ClanBossCardPlayerIndex.Contains(cardPlayerData.Index))
            {
                foreach (CardSkillPassiveLv skill in battle_skill.Passive)
                {
                    if (skill.Level < 0) skill.Level = 0;
                    if (skill.IsLock) skill.IsLock = false;
                }
            }

            return battle_skill;
        }

        public string GetActiveSkillNameByCardIndex(CardPlayerIndex index)
        {
            CardPlayerData cardPlayerData = this.GetCardPlayerDataByIndex(index);
            if (cardPlayerData == null) return "";
            return this.GetSkillActiveName(cardPlayerData.Skill);
        }

        public int[] GetActiveSkillValue(TypeActive skillIndex, int level)
        {
            CardSkillActive cardSkillActive = this.CardSkillActiveDataDic.Get(skillIndex.ToString());
            if (cardSkillActive == null) return new int[0];
            SkillValueByLevel value = cardSkillActive.Value.ToList().Find(x => x.Level == level);
            if (value == null) return new int[0];
            return value.Value;
        }

        public int[] GetPassiveSkillValue(TypePassive passiveIndex, int level)
        {
            CardSkillPassive cardSkillPassive = this.CardSkillPassiveDataDic.Get(passiveIndex.ToString());
            if (cardSkillPassive == null)
            {
                NTLog.LogError("CardPlayerManager|" + "GetPassiveSkillValue|" + "Passive data not found : " + passiveIndex + " " + level);
                return new int[0];
            }
            SkillValueByLevel value = cardSkillPassive.Value.ToList().Find(x => x.Level == level);
            if (value == null)
            {
                NTLog.LogError("CardPlayerManager|" + "GetPassiveSkillValue|" + "Passive value not found : " + passiveIndex + " " + level);
                return new int[0];
            }
            return value.Value;
        }

        public bool IsMaxSkillActive(TypeActive skillIndex, int level)
        {
            CardSkillActive cardSkillActive = this.CardSkillActiveDataDic.Get(skillIndex.ToString());
            if (cardSkillActive == null) return false;
            return level >= cardSkillActive.MaxLv;
        }

        public bool IsMaxSkillPassive(TypePassive passiveIndex, int level)
        {
            CardSkillPassive cardSkillPassive = this.CardSkillPassiveDataDic.Get(passiveIndex.ToString());
            if (cardSkillPassive == null) return false;
            return level >= cardSkillPassive.MaxLv;
        }

        public int GetMaxLevelSkillActive(TypeActive skillIndex)
        {
            CardSkillActive cardSkillActive = this.CardSkillActiveDataDic.Get(skillIndex.ToString());
            if (cardSkillActive == null) return 0;
            return cardSkillActive.MaxLv;
        }

        public int GetMaxLevelSkillPassive(TypePassive passiveIndex)
        {
            CardSkillPassive cardSkillPassive = this.CardSkillPassiveDataDic.Get(passiveIndex.ToString());
            if (cardSkillPassive == null) return 0;
            return cardSkillPassive.MaxLv;
        }


        public BattleEngine.BattleStats GetCardPlayerPassiveSkillStats(CardSkillPassiveLv passiveSkill, BattleEngine.BattleStats baseStats)
        {
            BattleEngine.BattleStats skillStats = new BattleEngine.BattleStats();
            int[] value = this.GetPassiveSkillValue(passiveSkill.Index, passiveSkill.Level);
            switch (passiveSkill.Index)
            {
                case TypePassive.Buff_20p_DEF_20p_HP:
                    skillStats.DEF = (long)(baseStats.DEF * (value[0] / 100f));
                    skillStats.HP = (long)(baseStats.HP * (value[1] / 100f));
                    break;
                case TypePassive.Buff_18p_ATK_13p_HP:
                    skillStats.ATK = (long)(baseStats.ATK * (value[0] / 100f));
                    skillStats.HP = (long)(baseStats.HP * (value[1] / 100f));
                    break;
                case TypePassive.ATK_20p_HP_10p_first_below_50p_HP_gain_100p_DR_2_round:
                    skillStats.ATK = (long)(baseStats.ATK * (value[0] / 100f));
                    skillStats.HP = (long)(baseStats.HP * (value[1] / 100f));
                    break;
                case TypePassive.Deal_45p_extra_damage_to_Bleed_enemies:
                    skillStats.DealToBleed = value[0];
                    break;
                case TypePassive.Buff_25p_HP_35p_DEF:
                    skillStats.HP = (long)(baseStats.HP * (value[0] / 100f));
                    skillStats.DEF = (long)(baseStats.DEF * (value[1] / 100f));
                    break;
                case TypePassive.Buff_35p_ATK:
                    skillStats.ATK = (long)(baseStats.ATK * (value[0] / 100f));
                    break;
                case TypePassive.Buff_10p_ATK_30p_CRT_20p_CRD:
                    skillStats.ATK = (long)(baseStats.ATK * (value[0] / 100f));
                    skillStats.CRT = value[1] / 100f;
                    skillStats.CRD = value[2] / 100f;
                    break;
                case TypePassive.Buff_20p_DEF_30_SPD:
                    skillStats.DEF = (long)(baseStats.DEF * (value[0] / 100f));
                    skillStats.SPD = value[1];
                    break;
                case TypePassive.Deal_70p_extra_damage_to_Frozen_enemies:
                    skillStats.DealToFrozen = value[0];
                    break;
                case TypePassive.Buff_30p_ATK_15p_DEF_20p_CRT_20p_CRD:
                    skillStats.ATK = (long)(baseStats.ATK * (value[0] / 100f));
                    skillStats.DEF = (long)(baseStats.DEF * (value[1] / 100f));
                    skillStats.CRT = value[2] / 100f;
                    skillStats.CRD = value[3] / 100f;
                    break;
                case TypePassive.Buff_30p_HP_20p_SkDR:
                    skillStats.HP = (long)(baseStats.HP * (value[0] / 100f));
                    skillStats.SkDR = value[1] / 100f;
                    break;
                case TypePassive.Gain_20p_ATK_and_15_SPD:
                    skillStats.ATK = (long)(baseStats.ATK * (value[0] / 100f));
                    skillStats.SPD = value[1];
                    break;
                case TypePassive.ATK_plus_10p_Crit_plus_30p_Crit_DMG_plus_30p:
                    skillStats.ATK = (long)(baseStats.ATK * (value[0] / 100f));
                    skillStats.CRT = value[1] / 100f;
                    skillStats.CRD = value[2] / 100f;
                    break;
                case TypePassive.HP_plus_0_ATK_plus_1_Block_plus_2:
                    skillStats.HP = (long)(baseStats.HP * (value[0] / 100f));
                    skillStats.ATK = (long)(baseStats.ATK * (value[1] / 100f));
                    skillStats.Block = value[2] / 100f;
                    break;
                case TypePassive.HP_plus_0_DEF_plus_1_Upon_death_unleashes_a_searing_curse_that_Burns_all_enemies_dealing_2_of_ATK_damage_per_round_for_3_turns:
                    skillStats.HP = (long)(baseStats.HP * (value[0] / 100f));
                    skillStats.DEF = (long)(baseStats.DEF * (value[1] / 100f));
                    skillStats.Block = value[2] / 100f;
                    break;
                case TypePassive.Increases_HP_0p_Damage_by_1_Speed_by_2_and_Stun_Resistance_by_3:
                    skillStats.HP = (long)(baseStats.HP * (value[0] / 100f));
                    skillStats.DMG = (long)(baseStats.DMG * (value[1] / 100f));
                    skillStats.SPD = value[2];
                    skillStats.StunRes = value[3] / 100f;
                    break;
                case TypePassive.IncreasesHP_by_0_ATK_by_1_and_Armor_by_2:
                    skillStats.HP = (long)(baseStats.HP * (value[0] / 100f));
                    skillStats.ATK = (long)(baseStats.ATK * (value[1] / 100f));
                    skillStats.DEF = (long)(baseStats.DEF * (value[2] / 100f));
                    break;
                case TypePassive.Increases_HP_by_0p_ATK_by_1_and_Crit_by_2:
                    skillStats.HP = (long)(baseStats.HP * (value[0] / 100f));
                    skillStats.ATK = (long)(baseStats.ATK * (value[1] / 100f));
                    skillStats.CRT = value[2] / 100f;
                    break;
            }

            return skillStats;
        }

        public Sprite GetOriginSpriteCircle(OriginType originType)
        {
            return this.OriginSpritesCircle[(int)originType];
        }

        public Sprite GetOriginSpriteSquare(OriginType originType)
        {
            return this.OriginSpritesSquare[(int)originType];
        }

        public Sprite GetOriginSpriteFlag(OriginType originType)
        {
            return this.OriginSpritesFlag[(int)originType];
        }

        public CardSummonData GetCardSummonDataBySummonType(SummonType summonType)
        {
            return this.CardSummonDatas.ToList().Find(x => x.Type == summonType);
        }

        public int GetCountEnsuareSummon(SummonType summonType)
        {
            CardSummonData cardSummonData = this.GetCardSummonDataBySummonType(summonType);
            if (cardSummonData == null) return 0;


            SummonHistory summonHistory = this.SummonHistory.Get(summonType.ToString());
            if (summonHistory == null) return 0;

            int count = cardSummonData.Ensuare - summonHistory.Count;
            if (count < 0) count = 0;
            return count;
        }

        public CardPlayerIndex[] GetCardTierOriginData(int star, OriginType originType)
        {
            CardTierOriginData cardTierOriginData = this.CardTierOriginDatas.ToList().Find(x => x.Origin == originType);
            if (cardTierOriginData == null)
            {
                NTLog.LogError("CardPlayerManager|" + "GetCardTierOriginData|" + "CardTierOriginData not found : " + originType);
                return this.CardTierOriginDatas[0].Card[0].CardIndexes;
            }

            RarityType rarityType = RarityType.Common;
            if (star < 5) rarityType = RarityType.Common;
            else if (star < 10) rarityType = RarityType.Uncommon;
            else if (star < 15) rarityType = RarityType.Rare;
            else if (star < 20) rarityType = RarityType.Epic;
            else rarityType = RarityType.Legendary;

            CardTierData cardTierData = cardTierOriginData.Card.ToList().Find(x => x.Tier == rarityType);
            if (cardTierData == null)
            {
                NTLog.LogError("CardPlayerManager|" + "GetCardTierOriginData|" + "CardTierData not found : " + star);
                return this.CardTierOriginDatas[0].Card[0].CardIndexes;
            }
            return cardTierData.CardIndexes;
        }

        public (int remain, int cap) GetAmountAdvSummon(SummonType summonType)
        {
            CardSummonData cardSummonData = this.GetCardSummonDataBySummonType(summonType);
            if (cardSummonData == null) return (0, 0);
            SummonHistory summonHistory = this.SummonHistory.Get(summonType.ToString());
            if (summonHistory == null) return (cardSummonData.AdvSummon, cardSummonData.AdvSummon);
            return (cardSummonData.AdvSummon - summonHistory.AdvSummon, cardSummonData.AdvSummon);
        }

        public CardPlayer GetFakeCardPlayerByIndex(CardPlayerIndex index)
        {
            CardPlayer cardPlayer = new CardPlayer();
            cardPlayer.Index = index;
            cardPlayer.Lv = 1;
            cardPlayer.Star = 0;
            cardPlayer.CardPlayerData = this.GetCardPlayerDataByIndex(index);
            cardPlayer.CardSkillLv = this.GetCardSkill(index, 0);
            return cardPlayer;
        }

        #endregion
    }
}
