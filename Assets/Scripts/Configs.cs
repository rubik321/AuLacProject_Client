using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GOA.Config
{
    public class Configs : MonoBehaviour
    {
        public static string WorldMap_Screen = "WorldMapScenes";
        public static string Login_Screen = "Login";
        public static string Combat_Screen = "Campaign";
        public static string Adventure_Screen = "Adventure";
        public static string Campaign_Screen = "LevelMap";
        public const string URL_TERMS_OF_SERVICE = "https://gateofabyss.com/terms-and-conditions/";
        public const string URL_POLICIES_ON_PRIVACY = "https://gateofabyss.com/terms-and-conditions/";

        public const float RangeBuildBuilding = 800;

        public const int CapShopBuilding = 5;
        public const int CapBlackSmithBuilding = 5;
        public const int CapDungeonBuilding = 5;
        public const int CapCastleBuilding = 5;

    }
    public class SeverConfigs
    {
        public const string GOA_Sever = "http://15.235.180.137:8080";
        public const string Test_Sever = "http://15.235.180.137:7008";
        public const string LocalHost_Sever = "http://localhost:7008";
        public const string Hung_Sever = "http://103.167.89.114:8080";

        public static string BASE_API_URL{
            get{
                // return LocalHost_Sever;
                 return Test_Sever; 
                //return GOA_Sever;
            }
        }

        public const string GPS2D_Server = "http://103.167.89.114:7008";
        // public const string GPS2D_Server = "http://localhost:7008";

        // public static string BASE_API_URL = "http://localhost:8080";
        // public static string BASE_API_URL = "http://51.79.254.60:8080"

        public static string GOA_API_URL = "https://be.gateofabyss.com";
        public static string LOCALHOST_API_URL = "http://localhost:8080";
        public static string BASE_API_SELECT_CHARACTER = "/api/user/selectCharacter";
        public static string BASE_API_LOGIN = "/api/user/login";
        public static string BASE_API_LOGIN_BEFE = "/api/user/befelogin";
        public static string BASE_API_REGISTER = "/api/user/register";
        public static string BASE_API_REGISTER_BEFE = "/api/user/befeRegister";
        public static string BASE_API_ATTACKMOB = "/api/user/AttackMob";
        public static string BASE_API_GET_USER_GEAR = "/api/user/GetUserGear";
        public static string BASE_API_EQUIP_GEAR = "/api/user/EquipGear";
        public static string BASE_API_SELL_GEAR = "/api/user/SellGear";
        public static string BASE_API_UNEQUIP_GEAR = "/api/user/UnEquipGear";
        public static string BASE_API_EQUIP_ORB = "/api/user/EquipOrb";
        
        public static string BASE_API_GET_DUNGEON = "/api/dungeon/get_data"; 
        public static string BASE_API_JOIN_DUNGEON = "/api/dungeon/join";

        public static string BASE_API_SEND_ADVENTURE = "/api/user/SendAdventure";
        public static string BASE_API_CLAIM_ADVENTURE = "/api/user/ClaimAdventure";
        public static string BASE_API_EQUIP_COMPANION = "/api/user/EquipCompanion";
        public static string BASE_API_UNEQUIP_COMPANION = "/api/user/UnEquipCompanion";
        public static string BASE_API_UNEQUIP_ORB = "/api/user/UnEquipOrb";
        public static string BASE_API_GET_USER_ORB = "/api/user/GetUserOrb";
        public static string BASE_API_GET_USER_COMPANIONS = "/api/user/GetUserCompanion";
        public static string BASE_API_CLAIM_DAILY_QUEST = "/api/user/claimDailyQuest";
        public static string BASE_API_FIND_LOCATION = "/api/tools/findLocation";
        public const string ChangeDisplayName = "/api/user/changeDisplayName";

        public const string GetFriendListAPI = "/api/friend/getFriendList";
        public const string SendFriendRequestAPI = "/api/friend/sendFriendRequest";
        public const string GetFriendRequestAPI = "/api/friend/getFriendRequest";
        public const string AcceptFriendRequestAPI = "/api/friend/acceptFriendRequest";
        public const string GetRelationship = "/api/friend/getRelationship";

        public const string GetOutpostListAPI = "/api/outpost/getOutpostList";
        public const string DefeatOutpostListAPI = "/api/outpost/defeatOutpost";

        public const string GetPortalDataAPI = "/api/portal/getDataPortal";
        public const string OpenPortalAPI = "/api/portal/openPortal";
        public const string AttackPortalAPI = "/api/portal/attackPortal";

        public const string BuyItemAPI = "/api/user/BuyItem";

        public const string GetGlobalPortalAPI = "/api/event/getGlobalPortal";

        public const string GetLandsAPI = "/api/game/assets/lands";
        public const string GetNFTGenesisAPI = "/api/game/assets/nftGenesis";

        public const string GetRewardAPI = "/api/reward/get_reward";

        public const string ReloadCompanionAPI = "/api/companion/reloadCompanion";

        public static string BASE_API_GET_USER_USE_ITEM = "/api/user/UseItem";
        public const string GetPlayerShopData = "/api/building_shop/getPlayerShopData";
        public const string BuyPlayerShopItem = "/api/building_shop/buyPlayerShopItem";
        public const string GetNeutralShopData = "/api/building_shop/getNeutralShopData";
        public const string BuyNeutralShopItem = "/api/building_shop/buyNeutralShopItem";
        public const string GetWanderingDealerData = "/api/building_shop/getWanderingDealerData";
        public const string BuyWanderingDealerItem = "/api/building_shop/buyWanderingDealerItem";

        public const string BuildBuildingPlayer = "/api/tools/buildBuildingPlayer";

        public const string Land_OrganizingEvent = "/api/lands/organizing_event";
        public const string Land_GetEvent = "/api/lands/get_event";
        public const string Land_GetUserEvent = "/api/lands/get_user_event";
        public const string Land_GetEventRank = "/api/lands/get_event_rank";

        public const string Blacksmith_UpgradeGear = "/api/black_smith/upgrade_gear";
        public const string Blacksmith_UpStarGear = "/api/black_smith/up_star_gear";
        public const string Blacksmith_FusionGear = "/api/black_smith/fusion_gear";
        public const string Blacksmith_FragmentGear = "/api/black_smith/fragment_gear";
        public const string API_Card_AddRandomCard = "/api/card_fk/add_random_card";
        public const string API_Card_Summon = "/api/card_fk/summon";
        public const string API_Card_GetCards = "/api/card_fk/get_cards";
        public const string API_Card_UpdateCardTeam = "/api/card_fk/update_card_team";
        public const string API_Card_UpgradeLv = "/api/card_fk/upgrade_lv";
        public const string API_Card_Ascend = "/api/card_fk/ascend";
        public const string API_Card_UpgradeEnhanceLv = "/api/card_fk/upgrade_enhance_lv";
        public const string API_Card_UpgradeGearLv = "/api/card_fk/upgrade_gear_lv";
        public const string API_Card_ResetGearLv = "/api/card_fk/reset_gear_lv";
        public const string API_Card_QuickUpgradeGearLv = "/api/card_fk/quick_upgrade_gear_lv";
        public const string API_Chest_OpenChest = "/api/chest_fk/open_chest";

        public const string _2DGPS_DataCenter_CheckVersion = "/api/2d_gps/data_center/check_version";

        public const string _2DGPS_API_Account_Login = "/api/2d_gps/account/login";
        public const string _2DGPS_API_Account_Register = "/api/2d_gps/account/register";
        public const string _2DGPS_API_Account_LoginByDeviceID = "/api/2d_gps/account/login_by_device_id";

        public const string _2DGPS_API_UserData_Get = "/api/2d_gps/user_data/get";
        public const string _2DGPS_API_UserData_Login = "/api/2d_gps/user_data/login";

        public const string _2DGPS_API_Card_AddRandomCard = "/api/2d_gps/card/add_random_card";
        public const string _2DGPS_API_GetCards= "/api/2d_gps/card/get_cards";
        public const string _2DGPS_API_SummonCard= "/api/2d_gps/card/summon";
        public const string _2DGPS_API_UpdateCardTeam= "/api/2d_gps/card/update_card_team";

        public const string _2DGPS_API_Chest_OpenChest = "/api/2d_gps/chest/open_chest";

        public const string _2DGPS_API_Campaign_PassCampaign = "/api/2d_gps/campaign/pass_campaign";

        public const string _2DGPS_API_Character_AddRandomCharacter = "/api/2d_gps/character/add_rand_character";
        public const string _2DGPS_API_Character_GetCharacters = "/api/2d_gps/character/get_characters";
        public const string _2DGPS_API_Character_UpgradeLvCharacter = "/api/2d_gps/character/upgrade_lv_character";
        public const string _2DGPS_API_Character_EquipGear= "/api/2d_gps/character/equip_character_gear";

        public const string _2DGPS_API_Gear_AddRandomGear = "/api/2d_gps/gear/add_rand_gear";
        public const string _2DGPS_API_Gear_GetGears = "/api/2d_gps/gear/get_gears";
        public const string _2DGPS_API_Gear_UpgradeLvGear = "/api/2d_gps/gear/upgrade_lv_gear";
    }

    public class MobConfigs
    {
        public static int LimitTopRegularMob = 30;
        public static int LimitDownRegularMob = 8;
        public static int LimitTopGreaterMob = 0;
    }
}