using System;
using System.Collections.Generic;
using System.Collections;
using SimpleJSON;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Rubik.CharacterGear
{
    using System.Linq;
    using NTPackage;
    using NTPackage.Functions;
    using Rubik.CardPlayer;
    using Rubik.CharacterPlayer;
    using Rubik.DataCenter;
    using Rubik.DataType;
    using Rubik.ItemPlayer;
    using Rubik.Manager;
    using Rubik.Myrk.BattleTeam;
    using Rubik.Myrk.Skill;
    using Rubik.Server;
    using Rubik.UI.Statitic;
    using Rubik.UserDataPlayer;

    public class CharacterGearConfig
    {
        public const string API_Add_Random = "/api/2D_GPS/character_gear/add_rand";
        public const string API_Upgrade_Lv = "/api/2D_GPS/character_gear/upgrade_lv";
        public const string API_Sell = "/api/2D_GPS/character_gear/sell";
    }

    public class CharacterGearManager : NTBehaviour
    {
        #region Player Data
        [Header("Player Data")]
        public NTDictionary<string, CharacterGear> GearDic;
        #endregion

        #region Game Data
        [Header("Game Data")]
        public NTDictionary<string, CharacterGearData> GearDataDic;
        public NTDictionary<int, CharacterGearUpgradeLvData> CharacterGearUpgradeLvData;
        public NTDictionary<string, GearSkillActive> GearSkillActiveDataDic;
        public NTDictionary<string, GearSkillPassive> GearSkillPassiveDataDic;
        public NTDictionary<int, CharacterGearRarityBonus> CharacterGearRarityBonusDataDic;
        #endregion

        #region Resource Data
        [Header("Resource Data")]
        public List<Sprite> GearSprites;
        public NTDictionary<string, CharacterGearSpriteData> GearSpriteDataDic;
        public TextAsset GearSpriteDataTextAsset;

        public List<Sprite> BackgroundRarityGears;
        public List<Sprite> BorderRarityGears;
        #endregion

        public static CharacterGearManager Instance;
        protected override void Awake()
        {
            base.Awake();
            if (CharacterGearManager.Instance != null)
            {
                NTLog.LogWarning("Only 1 instance allow");
                return;
            }
            CharacterGearManager.Instance = this;
        }

        #region Function

        public void Logout()
        {
            this.GearDic.Clear();
        }

        public IEnumerator LoadData()
        {
            this.GearDataDic = new NTDictionary<string, CharacterGearData>();
            JSONNode jdata = JSONNode.Parse(DataCenterManager.Instance.GetData(DataName.CharacterGearData));
            foreach (JSONNode item in jdata)
            {
                CharacterGearData gearData = JsonUtility.FromJson<CharacterGearData>(item.ToString());
                this.GearDataDic.Add(gearData.Index.ToString(), gearData);
            }

            this.CharacterGearUpgradeLvData = new NTDictionary<int, CharacterGearUpgradeLvData>();
            jdata = JSONNode.Parse(DataCenterManager.Instance.GetData(DataName.CharacterGearUpgradeLvData));
            foreach (JSONNode item in jdata)
            {
                CharacterGearUpgradeLvData gearLvCost = JsonUtility.FromJson<CharacterGearUpgradeLvData>(item.ToString());
                this.CharacterGearUpgradeLvData.Add(gearLvCost.Level, gearLvCost);
            }

            this.GearSkillActiveDataDic = new NTDictionary<string, GearSkillActive>();
            jdata = JSONNode.Parse(DataCenterManager.Instance.GetData(DataName.GearSkillActiveData));
            foreach (JSONNode item in jdata)
            {
                GearSkillActive gearSkillActive = JsonUtility.FromJson<GearSkillActive>(item.ToString());
                this.GearSkillActiveDataDic.Add(gearSkillActive.Index.ToString(), gearSkillActive);
            }

            this.GearSkillPassiveDataDic = new NTDictionary<string, GearSkillPassive>();
            jdata = JSONNode.Parse(DataCenterManager.Instance.GetData(DataName.GearSkillPassiveData));
            foreach (JSONNode item in jdata)
            {
                GearSkillPassive gearSkillPassive = JsonUtility.FromJson<GearSkillPassive>(item.ToString());
                this.GearSkillPassiveDataDic.Add(gearSkillPassive.Index.ToString(), gearSkillPassive);
            }

            this.GearSpriteDataDic = new NTDictionary<string, CharacterGearSpriteData>();
            jdata = JSONNode.Parse(this.GearSpriteDataTextAsset.text);
            foreach (JSONNode item in jdata)
            {
                CharacterGearSpriteData characterGearSpriteData = JsonUtility.FromJson<CharacterGearSpriteData>(item.ToString());
                this.GearSpriteDataDic.Add(characterGearSpriteData.Index.ToString(), characterGearSpriteData);
                characterGearSpriteData.Sprite = this.GearSprites.Find(sprite => sprite.name == characterGearSpriteData.SpriteName);
                if (characterGearSpriteData.Sprite == null)
                {
                    NTLog.LogError("GearSpriteData not found: " + characterGearSpriteData.SpriteName);
                }
            }

            this.CharacterGearRarityBonusDataDic = new NTDictionary<int, CharacterGearRarityBonus>();
            jdata = JSONNode.Parse(DataCenterManager.Instance.GetData(DataName.CharacterGearRarityBonusData));
            foreach (JSONNode item in jdata)
            {
                CharacterGearRarityBonus gearRarityBonusData = JsonUtility.FromJson<CharacterGearRarityBonus>(item.ToString());
                this.CharacterGearRarityBonusDataDic.Add((int)gearRarityBonusData.Rarity, gearRarityBonusData);
            }
            yield return null;
        }

        public void UpdateGears(CharacterGear[] gears)
        {
            foreach (CharacterGear gear in gears)
            {
                if (gear.Delete)
                {
                    this.GearDic.Remove(gear._id);
                    continue;
                }
                if (this.GearDic.Get(gear._id) == null)
                {
                    this.GearDic.Add(gear._id, gear);
                }
                else
                {
                    this.GearDic.Get(gear._id).UpdateGear(gear);
                }
                UpdateCacheCharacterGear(this.GearDic.Get(gear._id));
            }
        }

        public void UpdateCacheCharacterGear(CharacterGear characterGear)
        {
            CharacterGearData gearData = CharacterGearManager.Instance.GetGearDataByIndex(characterGear.Index);
            characterGear.GearData = gearData;
            GearStats stats = CharacterGearManager.Instance.GetStatsByLevel(characterGear.Rarity, characterGear.Lv);
            characterGear.GearStats = new GearStats();
            characterGear.GearStats.HeroAtk = gearData.GearStats.HeroAtk * stats.HeroAtk;
            characterGear.GearStats.TeamHp = gearData.GearStats.TeamHp * stats.TeamHp;
            characterGear.GearStats.TeamSpeed = stats.TeamSpeed;


            GearStats statsNextLevel = CharacterGearManager.Instance.GetStatsByLevel(characterGear.Rarity, characterGear.Lv + 1);
            characterGear.GearStatsNextLevel = new GearStats();
            characterGear.GearStatsNextLevel.HeroAtk = gearData.GearStats.HeroAtk * statsNextLevel.HeroAtk;
            characterGear.GearStatsNextLevel.TeamHp = gearData.GearStats.TeamHp * statsNextLevel.TeamHp;
            characterGear.GearStatsNextLevel.TeamSpeed = statsNextLevel.TeamSpeed;
        }

        public bool CheckGearInList(CharacterGearIndex id)
        {
            foreach (CharacterGear gear in GearDic.ToList())
            {
                if (gear.Index == id)
                {
                    return true;
                }
            }
            return false;
        }
        public bool CheckGearInListIsUsed(CharacterGearIndex id)
        {
            foreach (CharacterGear gear in GearDic.ToList())
            {
                if (gear.Index == id && gear.IsEquiped)
                {
                    return true;
                }
            }
            return false;
        }
        public string GetIdByIndex(CharacterGearIndex index)
        {
            foreach (CharacterGear gear in GearDic.ToList())
            {
                if (gear.Index == index)
                {
                    return gear._id;
                }
            }
            return null;
        }
        #endregion

        #region API

        public IEnumerator IEAddRandomGear(Action<CharacterGear[]> done = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.Instance.GetUserID();
            yield return APIManager.Instance.PostDataUrl(jdata.ToString(), Rubik.Config.URL_Config.BASE_API_URL + CharacterGearConfig.API_Add_Random, (data) =>
            {
                APIResponseData apiResponse = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponse.Status == 0) return;
                done?.Invoke(apiResponse.Update_CharacterGear);
            });
        }

        public IEnumerator IEUpgradeLvGear(string gearID, Action done = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["gearID"] = gearID;
            yield return APIManager.Instance.PostDataUrl(jdata.ToString(), Rubik.Config.URL_Config.BASE_API_URL + CharacterGearConfig.API_Upgrade_Lv, (data) =>
            {
                APIResponseData apiResponse = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponse.Status == 0) return;
                done?.Invoke();
            });
        }

        public IEnumerator IESellGear(string gearID, Action done = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.Instance.GetUserID();
            jdata["gearID"] = gearID;
            yield return APIManager.Instance.PostDataUrl(jdata.ToString(), Rubik.Config.URL_Config.BASE_API_URL + CharacterGearConfig.API_Sell, (data) =>
            {
                APIResponseData apiResponse = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponse.Status == 0) return;
                done?.Invoke();
            });
        }

        #endregion

        #region Getter

        public CharacterGearData GetGearDataByIndex(CharacterGearIndex index)
        {
            return this.GearDataDic.Get(index.ToString());
        }

        public CharacterGear GetCharacterGearByID(string id)
        {
            return this.GearDic.Get(id);
        }

        public Sprite GetGearSpriteByIndex(CharacterGearIndex index)
        {
            CharacterGearSpriteData characterGearSpriteData = this.GearSpriteDataDic.Get(index.ToString());
            if (characterGearSpriteData == null)
            {
                NTLog.LogError("GearSpriteData not found: " + index.ToString());
                return null;
            }
            return characterGearSpriteData.Sprite;
        }

        public Sprite GetGearBackgroundRarityByRarity(RarityType rarity)
        {
            return this.BackgroundRarityGears[(int)rarity % this.BackgroundRarityGears.Count];
        }

        public Sprite GetGearBorderRarityByRarity(RarityType rarity)
        {
            return this.BorderRarityGears[(int)rarity % this.BorderRarityGears.Count];
        }

        public ItemData GetGearSellPrice()
        {
            return new Rubik.ItemPlayer.ItemData(ItemType.Coin, 50);
        }

        public ItemData[] GetGearUpgradeLvCost(int level)
        {
            CharacterGearUpgradeLvData gearLvCost = this.CharacterGearUpgradeLvData.Get(level);
            if (gearLvCost == null)
            {
                NTLog.LogError("GearLvCost not found: " + level);
                return null;
            }
            return gearLvCost.Cost;
        }

        public GearStats GetStatsByLevel(RarityType rarity, int level)
        {
            if (this.CharacterGearUpgradeLvData.Get(level) == null)
            {
                NTLog.LogError("GearStatsLv not found: " + level);
                return new GearStats();
            }
            CharacterGearRarityBonus gearRarityBonus = this.GetCharacterGearRarityBonusDataByRarity(rarity);
            float sclae = gearRarityBonus.Bonus;
            GearStats gearStats = new GearStats();
            gearStats.HeroAtk = (long)(this.CharacterGearUpgradeLvData.Get(level).HeroATK * (1 + sclae));
            gearStats.TeamHp = (long)(this.CharacterGearUpgradeLvData.Get(level).TeamHP * (1 + sclae));
            gearStats.TeamSpeed = (long)(gearRarityBonus.Stat.TeamSpeed);
            return gearStats;
        }

        public StatData GetMainStat(CharacterGearIndex index, RarityType rarity, int level)
        {
            CharacterGearData gearData = CharacterGearManager.Instance.GetGearDataByIndex(index);
            GearStats gearStats = CharacterGearManager.Instance.GetStatsByLevel(rarity, level);
            GearStats stats = NTFunction.Clone(gearStats);
            CharacterGearRarityBonus gearRarityBonus = this.GetCharacterGearRarityBonusDataByRarity(rarity);
            float sclae = gearRarityBonus.Bonus;
            stats.HeroAtk = (long)Mathf.Floor(gearData.GearStats.HeroAtk * stats.HeroAtk * (1 + sclae));
            stats.TeamHp = (long)Mathf.Floor(gearData.GearStats.TeamHp * stats.TeamHp * (1 + sclae));
            if (stats.HeroAtk > 1)
            {
                return new StatData(TypeStat.Mind, stats.HeroAtk);
            }
            if (stats.TeamHp > 1)
            {
                return new StatData(TypeStat.HP, stats.TeamHp);
            }
            if (stats.TeamSpeed > 1)
            {
                return new StatData(TypeStat.SPD, stats.TeamSpeed);
            }

            return new StatData();
        }

        public StatData GetMainStatsIncrease(CharacterGearIndex index, RarityType rarity, int level)
        {
            StatData mainStats = this.GetMainStat(index, rarity, level);
            StatData mainStatsIncrease = this.GetMainStat(index, rarity, level + 1);
            StatData statData = NTFunction.Clone(mainStatsIncrease);
            statData.Value = mainStatsIncrease.Value - mainStats.Value;
            return statData;
        }

        public StatData GetSubStats(RarityType rarity)
        {
            CharacterGearRarityBonus gearRarityBonus = this.GetCharacterGearRarityBonusDataByRarity(rarity);
            if (gearRarityBonus.Stat.HeroAtk > 0)
            {
                return new StatData(TypeStat.Mind, gearRarityBonus.Stat.HeroAtk);
            }
            if (gearRarityBonus.Stat.TeamHp > 0)
            {
                return new StatData(TypeStat.HP, gearRarityBonus.Stat.TeamHp);
            }
            if (gearRarityBonus.Stat.TeamSpeed > 0)
            {
                return new StatData(TypeStat.SPD, gearRarityBonus.Stat.TeamSpeed);
            }
            return new StatData();
        }

        public TypeActiveGear GetTypeActiveGearByIndex(CharacterGearIndex index)
        {
            CharacterGearData gearData = CharacterGearManager.Instance.GetGearDataByIndex(index);
            return gearData.SkillActive;
        }
        public TypePassiveGear GetTypePassiveGearByIndex(CharacterGearIndex index)
        {
            CharacterGearData gearData = CharacterGearManager.Instance.GetGearDataByIndex(index);
            return gearData.SkillPassive;
        }

        public string GetGearNameByIndex(CharacterGearIndex index)
        {
            return Lean.Localization.LeanLocalization.GetTranslationText("gear_" + (int)index + "_name", index.ToString());
        }
        public string GetGearDesByIndex(CharacterGearIndex index)
        {
            return Lean.Localization.LeanLocalization.GetTranslationText("gear_" + (int)index + "_des", index.ToString());
        }

        public bool IsActiveSkillGear(CharacterGearIndex index)
        {
            CharacterGearData gearData = CharacterGearManager.Instance.GetGearDataByIndex(index);
            return gearData.SkillActive != TypeActiveGear.None;
        }

        public GearSkillActiveLv GetGearSkillActiveLv(TypeActiveGear index, RarityType rarity)
        {
            GearSkillActiveLv gearSkillActiveLv = new GearSkillActiveLv();
            gearSkillActiveLv.Index = index;
            CharacterGearRarityBonus rarityBonus = this.GetCharacterGearRarityBonusDataByRarity(rarity);
            gearSkillActiveLv.Level = rarityBonus.SkillLevel;
            gearSkillActiveLv.Rarity = rarity;
            return gearSkillActiveLv;
        }

        public GearSkillPassiveLv GetGearSkillPassiveLv(TypePassiveGear index, RarityType rarity)
        {
            GearSkillPassiveLv gearSkillPassiveLv = new GearSkillPassiveLv();
            gearSkillPassiveLv.Index = index;
            CharacterGearRarityBonus rarityBonus = this.GetCharacterGearRarityBonusDataByRarity(rarity);
            gearSkillPassiveLv.Level = rarityBonus.SkillLevel;
            gearSkillPassiveLv.Rarity = rarity;
            return gearSkillPassiveLv;
        }

        public string GetGearSkillName(CharacterGearIndex index)
        {
            if (this.IsActiveSkillGear(index))
            {
                return this.GetGearSkillName(this.GetTypeActiveGearByIndex(index));
            }
            return this.GetGearSkillName(this.GetTypePassiveGearByIndex(index));
        }

        public string GetGearSkillName(TypeActiveGear index)
        {
            return Lean.Localization.LeanLocalization.GetTranslationText("gear_skill_active_name_" + (int)index, index.ToString());
        }
        public string GetGearSkillName(TypePassiveGear index)
        {
            return Lean.Localization.LeanLocalization.GetTranslationText("gear_skill_passive_name_" + (int)index, index.ToString());
        }

        public string GetGearSkillDetail(CharacterGearIndex index, RarityType rarity)
        {
            if (this.IsActiveSkillGear(index))
            {
                return this.GetGearSkillDetail(this.GetTypeActiveGearByIndex(index), rarity);
            }
            else
            {
                return this.GetGearSkillDetail(this.GetTypePassiveGearByIndex(index), rarity);
            }
        }

        public string GetGearSkillDetail(TypeActiveGear index, RarityType rarity)
        {
            string detail = Lean.Localization.LeanLocalization.GetTranslationText("gear_skill_active_detail_" + (int)index, index.ToString());
            SkillValueByLevel value = this.GetGearSkillValue(index, rarity);
            List<string> ls_value = new List<string>();
            for (int i = 0; i < value.Value.Length; i++)
            {
                ls_value.Add(value.Value[i].ToString());
            }
            return String.Format(detail, ls_value.ToArray());
        }

        public string GetGearSkillDetail(TypePassiveGear index, RarityType rarity)
        {
            string detail = Lean.Localization.LeanLocalization.GetTranslationText("gear_skill_passive_detail_" + (int)index, index.ToString());
            SkillValueByLevel value = this.GetGearSkillValue(index, rarity);
            List<string> ls_value = new List<string>();
            for (int i = 0; i < value.Value.Length; i++)
            {
                ls_value.Add(value.Value[i].ToString());
            }
            return String.Format(detail, ls_value.ToArray());
        }

        public SkillValueByLevel GetGearSkillValue(CharacterGearIndex index, RarityType rarity)
        {
            if (this.IsActiveSkillGear(index))
            {
                return this.GetGearSkillValue(this.GetTypeActiveGearByIndex(index), rarity);
            }
            else
            {
                return this.GetGearSkillValue(this.GetTypePassiveGearByIndex(index), rarity);
            }
        }

        public SkillValueByLevel GetGearSkillValue(TypeActiveGear index, RarityType rarity)
        {
            GearSkillActive gearSkillActive = this.GearSkillActiveDataDic.Get(index.ToString());
            CharacterGearRarityBonus rarityBonus = this.GetCharacterGearRarityBonusDataByRarity(rarity);
            SkillValueByLevel value = gearSkillActive.Value.ToList().Find(x => x.Level == rarityBonus.SkillLevel);
            return value;
        }
        public SkillValueByLevel GetGearSkillValue(TypePassiveGear index, RarityType rarity)
        {
            GearSkillPassive gearSkillPassive = this.GearSkillPassiveDataDic.Get(index.ToString());
            CharacterGearRarityBonus rarityBonus = this.GetCharacterGearRarityBonusDataByRarity(rarity);
            SkillValueByLevel value = gearSkillPassive.Value.ToList().Find(x => x.Level == rarityBonus.SkillLevel);
            return value;
        }

        public GearSkillActive GetGearSkillActive(CharacterGearIndex index)
        {
            return this.GearSkillActiveDataDic.Get(index.ToString());
        }

        public GearSkillPassive GetGearSkillPassive(CharacterGearIndex index)
        {
            return this.GearSkillPassiveDataDic.Get(index.ToString());
        }


        public GearSkillLv GetGearSkillLv(GearShortTeam[] gears)
        {
            GearSkillLv gearSkillLv = new GearSkillLv();
            foreach (GearShortTeam gear in gears)
            {
                if (gear.Index == CharacterGearIndex.None) continue;
                if (!IsMainGear(gear.Index)) continue;
                if (IsActiveSkillGear(gear.Index))
                {
                    gearSkillLv.Active = this.GetGearSkillActiveLv(this.GetTypeActiveGearByIndex(gear.Index), gear.Rarity);
                }
                else
                {
                    gearSkillLv.Passive.Add(this.GetGearSkillPassiveLv(this.GetTypePassiveGearByIndex(gear.Index), gear.Rarity));
                }
            }
            return gearSkillLv;
        }

        public Sprite GetSkillActiveImage(TypeActiveGear index)
        {
            GearSkillActive gearSkillActive = this.GearSkillActiveDataDic.Get(index.ToString());
            if (gearSkillActive == null) return null;
            return SkillManager.Instance.GetSkillImage(gearSkillActive.Image);
        }

        public Sprite GetSkillPassiveImage(TypePassiveGear index)
        {
            GearSkillPassive gearSkillPassive = this.GearSkillPassiveDataDic.Get(index.ToString());
            if (gearSkillPassive == null) return null;
            return SkillManager.Instance.GetSkillImage(gearSkillPassive.Image);
        }

        public CharacterGearRarityBonus GetCharacterGearRarityBonusDataByRarity(RarityType rarity)
        {
            if (this.CharacterGearRarityBonusDataDic.Get((int)rarity) == null)
            {
                NTLog.LogError("CharacterGearRarityBonusData not found: " + rarity);
                return new CharacterGearRarityBonus();
            }
            return this.CharacterGearRarityBonusDataDic.Get((int)rarity);
        }

        public bool IsMainGear(CharacterGearIndex index)
        {
            CharacterGearData gearData = this.GetGearDataByIndex(index);
            return gearData.Type == CharacterGearType.Hair || gearData.Type == CharacterGearType.Armor || gearData.Type == CharacterGearType.Weapon;
        }

        #endregion
    }
}