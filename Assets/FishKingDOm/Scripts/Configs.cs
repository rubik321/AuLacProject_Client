using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Rubik.Config
{
    public class Configs : MonoBehaviour
    {
        public static string Home_Screen = "HomeScene";
        public static string Login_Screen = "Login";
        public static string Combat_Screen = "TestScene";
        public static string GamePlay_Screen = "GamePlay";
        public static string Campaign_Screen = "Campaign";

        public const int ScreenWidth = 1920;
        public const int ScreenHeight = 1080;
        public static Vector2 ScreenResolution = new Vector2(ScreenWidth, ScreenHeight);
        public static float HeroScaleInBattle = 0.1f;
    }
    public class SeverConfigs
    {
        public static string BASE_API_URL = "http://15.235.180.137:8088";

        public static string BASE_API_SELECT_CHARACTER = "/api/user/selectCharacter";
        public static string BASE_API_LOGIN = "/api/user/login";
        public static string BASE_API_REGISTER = "/api/user/register";

        public static string BASE_API_URL_HERO = "http://103.116.9.104:4001";
        public static string BASE_API_RANDOM_CARD = "/api/card/RandomCard";
        public static string BASE_API_GET_LIST_CARD = "/api/card/GetListCard";
        public static string BASE_API_GET_CARD_TEAM = "/api/card/GetCardTeam";
        public static string BASE_API_UPDATE_CARD_TEAM = "/api/card/UpdateCardTeam";
        public static string BASE_API_BATTLE_CAMPAIGN = "/api/card/BattleCampaign";
        public static string BASE_API_REWARD_CAMPAIGN = "/api/card/RewardCampaignBattle";

        public const string BASE_API_LeaderBoard = "/api/portal/getChessRank";

        public const string KING_FISH_URL_Tudt = "http://51.79.254.60:4008";
        public const string KING_FISH_URL_Admy = "http://103.116.9.104:4008";
        public const string KING_FISH_URL_Admy_v1 = "http://103.116.9.104:4108";
        public const string KING_FISH_URL_Local = "http://localhost:4008";
        public static string KING_FISH_URL{
            get{
                // return KING_FISH_URL_Local;
                return KING_FISH_URL_Admy_v1;
            }
        }

        public const string API_FinLord_GetData = "/api/king_fish/finlord/get_data";
        public const string API_FinLord_UpgradeLv = "/api/king_fish/finlord/upgrade_lv";
        public const string API_FinLord_UpgradeEnhanceLv = "/api/king_fish/finlord/upgrade_enhance_lv";

        public const string API_Card_AddRandomCard = "/api/king_fish/card/add_random_card";
        public const string API_Card_Summon = "/api/king_fish/card/summon";
        public const string API_Card_GetCards = "/api/king_fish/card/get_cards";
        public const string API_Card_UpdateCardTeam = "/api/king_fish/card/update_card_team";
        public const string API_Card_UpgradeLv = "/api/king_fish/card/upgrade_lv";
        public const string API_Card_Ascend = "/api/king_fish/card/ascend";
        public const string API_Card_UpgradeEnhanceLv = "/api/king_fish/card/upgrade_enhance_lv";
        public const string API_Card_UpgradeGearLv = "/api/king_fish/card/upgrade_gear_lv";
        public const string API_Card_ResetGearLv = "/api/king_fish/card/reset_gear_lv";
        public const string API_Card_QuickUpgradeGearLv = "/api/king_fish/card/quick_upgrade_gear_lv";

        public const string API_Account_Login = "/api/king_fish/account/login";
        public const string API_Account_Register = "/api/king_fish/account/register";
        public const string API_Account_LoginByDeviceID = "/api/king_fish/account/login_by_device_id";

        public const string API_UserData_Login = "/api/king_fish/user_data/login";
        public const string API_UserData_Get = "/api/king_fish/user_data/get";
        public const string API_UserData_UpStage = "/api/king_fish/user_data/up_stage";
        public const string API_UserData_UsingTorch = "/api/king_fish/user_data/using_torch";
        public const string API_UserData_GoldStorm = "/api/king_fish/user_data/gold_storm";

        public const string API_Chest_OpenChest = "/api/king_fish/chest/open_chest";


        public const string API_Shop_BuyItem = "/api/king_fish/shop/buy_item";

        public const string API_IdleLoot_UpLv = "/api/king_fish/idle_loot/up_lv";
        public const string API_IdleLoot_OfflineRewards = "/api/king_fish/idle_loot/offline_rewards";

        public const string API_DataCenter_CheckVersion = "/api/king_fish/data_center/check_version";

        public const string API_Quest_ClaimQuest = "/api/king_fish/quest/claim_quest";
        public const string API_Quest_ClaimWeek = "/api/king_fish/quest/claim_week_reward";
        public const string API_Quest_ClaimDay = "/api/king_fish/quest/claim_day_reward";
        public const string API_Quest_ClaimAchievement = "/api/king_fish/quest/claim_achievement";

        public const string API_Dungeon_GetData = "/api/king_fish/dungeon/get_data";
        public const string API_Dungeon_UpTowerStages = "/api/king_fish/dungeon/up_tower_stages";
        public const string API_Dungeon_GetArenaRank = "/api/king_fish/dungeon/get_arena_rank";
        public const string API_Dungeon_GetArenaBattle = "/api/king_fish/dungeon/get_arena_battle";
        public const string API_Dungeon_ArenaDoBattle = "/api/king_fish/dungeon/arena_do_battle";
        public const string API_Dungeon_ArenaHistory = "/api/king_fish/dungeon/arena_history";
        public const string API_Dungeon_GetBossData = "/api/king_fish/dungeon/get_boss_data";
        public const string API_Dungeon_DmgToBoss = "/api/king_fish/dungeon/dmg_to_boss";

    }

    public class AnimationConfigs
    {

        public static  string IDLE_DEFAULT = "idle";
        public static string IDLE = "stand";
        public static List<string> ListIdles = new List<string>() {  "idle", "Idle" ,"Idle_lighter","ilde"};

        public static string ATTACK_DEFAULT = "Attack";
        public static string ATTACK = "Hit2";
        public static List<string> ListAttacks = new List<string>() {  "Attack", "attack2", "attack","Hit2","Hit" ,"axe/attack 1", "sword/attack 1" };
        
        public static string ATTACK_UP = "attack_up";
        public static List<string> ListAttacksUp = new List<string>() {  "Attack", "attack2", "attack_up", "Hit2", "Hit", "axe/attack 1", "sword/attack 1" };

        public static string ATTACK_DOWN = "attack_down";
        public static List<string> ListAttacksDown = new List<string>() {  "Attack", "attack2", "attack_down", "Hit2", "Hit", "axe/attack 1", "sword/attack 1" };

        public static string HIT = "get_hit";
        public static string HIT_DEFAULT = "GetHit";
        public static List<string> ListHits = new List<string>() { "hurt", "hit" , "GetHit","get_hit" , "axe/get_hit", "sword/get_hit", "idle", "Idle" };

        public static string DIE_DEFAULT = "Run";
        public static string DIE = "Death";
        public static List<string> ListDies = new List<string>() { "attack__all", "dead", "die", "Idle", "get_hit" , "Idle_lighter" };

        public static string RUN = "run";
        public static string SKILL_BUFF = "skill01";

        public static string SKILL_ATTACK_DEFAULT = "sword/attack 2";
        public static string SKILL_ATTACK = "skill01";
        public static List<string> ListSkills = new List<string>() { "skill", "skill01", "skill1","Attack", "attack2", "attack", "Hit", "attack 1", "attack 2", "attack 3", "axe/attack 2", "sword/attack 2" };

        public const string JUMP = "into";

        public static string WIN_DEFAULT = "Idle";
        public static string WIN = "win";
        public static string BOX = "shake";
        public static string BOX_OPEN = "open_first";

    }
    public class EventConfigs
    {
        public static string ATTACK = "attack";
        public static string MOVE_TO_ATTACK = "up";
        public static string MOVE_TO_ATTACK_1 = "fast";



    }
    public class ColorConfigs
    {
        public static string TITLE_TXT = "#ffff";
        public static string CONTENT_TXT = "#ffff";
        public static string NAME_USER = "#ffff";

        public static string NAME_AXIE = "#ffff";
        public static string BUTTON_ON = "#ffff";
        public static string BUTTON_OFF = "#111";
        public static string MENU_TXT_ON = "#2E1C13";
        public static string MENU_TXT_OFF = "#786259";

    }
}