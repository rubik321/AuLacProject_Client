using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Rubik.DataCenter
{
    public enum DataName
    {
        DataVersion,
        VersionGame,
        CostChangeName,
        AvatarData,
        AvatarBorderData,
        ItemDataInfo,
        CharacterClothData,
        CharacterPlayerData,
        CharacterGearData,
        CharacterGearUpgradeLvData,
        IAP_Shop_Cfg,
        CardPlayerData,
        DailyQuestData,
        AchievementData,
        CardEvolveData,
        ServerGameData,
        CardUpStarData,
        BattleMonsterWorldMapData,
        CardSummonData,
        CardSkillData,
        CardSkillPassiveData,
        ExpPlayerData,
        CardLevelData,
        PackageIAPData,
        InventoryBagData,
        EnergyData,
        ClanData,
        CharacterGearStatsLvData,
        PlayerChestData,
        PlayerChestSlotData,
        ClanBossData,
        PlayerChestLevelData,
        PlayerChestDecreaseTimeData,
        DailyRewardData,
        BattlePassData,
        SkillLevelByStarData,
        BossPortalData,
        ClanShopData,
        OutpostData,
        CardTierOriginData,
        ItemExchangeData,
        ItemExchangeAdvData,
        FunctionLockData,
        ArenaRankingData,
        ArenaData,
        AdvLimitData,
        GearSkillActiveData,
        GearSkillPassiveData,
        CharacterGearRarityBonusData,
        AchievementBadgeData,
    }

    [System.Serializable]
    public class DataVersion
    {
        public int VersionGame = 0;
        public int AvatarData = 0;
        public int AvatarBorderData = 0;
        public int CostChangeName = 0;
        public int ItemDataInfo = 0;
        public int CharacterClothData = 0;
        public int CharacterPlayerData = 0;
        public int CharacterGearData = 0;
        public int CharacterGearUpgradeLvData = 0;
        public int IAP_Shop_Cfg = 0;
        public int CardPlayerData = 0;
        public int DailyQuestData = 0;
        public int AchievementData = 0;
        public int CardEvolveData = 0;
        public int ServerGameData = 0;
        public int CardUpStarData = 0;
        public int BattleMonsterWorldMapData = 0;
        public int CardSummonData = 0;
        public int CardSkillData = 0;
        public int CardSkillPassiveData = 0;
        public int ExpPlayerData = 0;
        public int CardLevelData = 0;
        public int PackageIAPData = 0;
        public int InventoryBagData = 0;
        public int EnergyData = 0;
        public int ClanData = 0;
        public int CharacterGearStatsLvData = 0;
        public int PlayerChestData = 0;
        public int PlayerChestSlotData = 0;
        public int ClanBossData = 0;
        public int PlayerChestLevelData = 0;
        public int PlayerChestDecreaseTimeData = 0;
        public int DailyRewardData = 0;
        public int BattlePassData = 0;
        public int SkillLevelByStarData = 0;
        public int BossPortalData = 0;
        public int ClanShopData = 0;
        public int OutpostData = 0;
        public int CardTierOriginData = 0;
        public int ItemExchangeData = 0;
        public int ItemExchangeAdvData = 0;
        public int FunctionLockData = 0;
        public int ArenaRankingData = 0;
        public int ArenaData = 0;
        public int AdvLimitData = 0;
        public int GearSkillActiveData = 0;
        public int GearSkillPassiveData = 0;
        public int CharacterGearRarityBonusData = 0;
        public int AchievementBadgeData = 0;
    }
}