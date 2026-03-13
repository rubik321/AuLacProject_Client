using System;
using UnityEngine;

namespace NTPackage.UI
{
    public class PopupCodeParser
    {
        public static PopupCode FromString(string name)
        {
            //name = name.ToLower();W
            return (PopupCode)Enum.Parse(typeof(PopupCode), name);
        }
    }

    [System.Serializable]
    public enum PopupCode
    {
        Unknown = 0,
        LoadingUI,
        GiftCodeUI,
        RewardDataUI,
        ItemDataDetailUI,
        UserDataShortUI,
        SummonUI,
        QuestUI,
        AvatarChangeUI,
        NameChangeUI,
        LoginUI,
        MessagePanel,
        SettingUI,
        MonsterOnMapUI,
        Emoji_Popup,
        ChatUI,
        PortalOnMapUI,
        MessageOptionPanel,
        PlayerMailInfoUI,
        PlayerMailUI,
        FriendUI,
        GearInfo_UI,
        CharacterGear_UI,
        ConfirmUI,
        Inventory_UI,
        CardPlayerInfoUI,
        LandSelectionUI,
        LineUpUI,
        MonsterUI,
        IAPShopUI,
        PackageIAPPopupUI,
        SkillInfoUI,
        BattleResultUI,
        EndGameUI,
        ShardUI,
        MonsterStoryUI,
        ToolTipUI,
        StartingUI,
        ExchangeItemUI,
        ClanUI,
        EffectDetailUI,
        PlayerChestUI,
        PlayerChestSelectUI,
        RateItemDataUI,
        DailyRewardUI,
        BattlePassUI,
        ClanHomeUI,
        ClanBossUI,
        MemberClanUI,
        ClanShopUI,
        ClanQuestUI,
        TutorialUI,
        ClanBossRankUI,
        MessageOptionAdvPanel,
        WatchAdsUI,
        OutpostOnMapUI,
        OutpostOnMapRewardUI,
        GuideUI,
        BannerTopUI,
        BattleLoadingUI,
        ArenaUI,
        LinkAccountUI,
        LoadingPanel,
        LineUpHeroUI,
        LongMessageUI,

        DeleteAccountUI,
        AchievementUI,
        PortalSearchUI,
        GearShortInfoUI,
        EventUI
    }
}
