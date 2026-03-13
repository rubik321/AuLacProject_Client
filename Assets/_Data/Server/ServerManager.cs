using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using SimpleJSON;


namespace Rubik.Manager
{
    using NTPackage.Functions;
    using NTPackage.EventDispatcher;
    using NTPackage.UI;
    using UnityEngine.SceneManagement;
    using System;
    using System.Collections.Generic;
    using Rubik.Config;
    using System.Threading.Tasks;
    using Colyseus;
    using Rubik.Friend;
    using Rubik.SystemData;
    using Rubik.Chat;
    using Rubik.Loading;
    using Rubik.DataCenter;
    using Rubik.UserDataPlayer;
    using Rubik.UserProfile;
    using Rubik.ItemPlayer;
    using Rubik.CharacterPlayer;
    using Rubik.Account;
    using Rubik.RewardData;
    using Rubik.CharacterGear;
    using Rubik.Myrk.Shop;
    using Rubik.CardPlayer;
    using Rubik.Quest;
    using Rubik.UI;
    using Rubik.Myrk;
    using Rubik.MsgDelivery;
    using Rubik.ServerGame;
    using Rubik.Myrk.Monster;
    using Rubik.PlayerLand;
    using Rubik.PlayerMail;
    using Rubik.IAP;
    using Rubik.Myrk.PackageIAP;
    using Lean.Localization;
    using Rubik.Common.AudioHelper;
    using Rubik.Myrk.Clan;
    using Rubik.Myrk.BattleTeam;
    using Rubik.BattleEngine;
    using Rubik.Myrk.PlayerChest;
    using Rubik.Myrk.DailyReward;
    using Rubik.Myrk.BattlePass;
    using Rubik.Myrk.GeoPoint;
    using Rubik.Myrk.Portal;
    using System.Linq;
    using Rubik.Myrk.Outpost;
    using Rubik.AddressablesLoader;
    using TMPro;
    using Rubik.Myrk.Arena;
    using Rubik.Myrk.Skill;
    using Rubik.Localization;

    public class APIResponse
    {
        public APIResponseData Data;
    }

    [System.Serializable]
    public class APIResponseData
    {
        public string Url_API;
        public int Status;
        public string Error;

        public UserDataResponse UserDataResponse = null;
        public ItemData[] Update_Item;
        public EnergyResponse UserEnergy = null;
        public DisplayNameData Update_DisplayName = null;
        public AvatarPlayer AvatarUpdate = null;
        public AvatarBorderPlayer AvatarBorderUpdate = null;
        public CharacterPlayer[] Update_CharacterPlayer = null;
        public UserDataShort[] UserDataShorts = null;
        public CharacterGear[] Update_CharacterGear = null;
        public CardPlayer[] Update_CardPlayer = null;

        // Battle Team
        public BattleTeamData[] Update_BattleTeam = null;
        public BattleTeamSelected BattleTeamSelected = null;
        public UpdateBattleShortTeam UpdateBattleShortTeam = null;

        public DailyQuestPlayer[] DailyQuest = null;
        public AchievementPlayer[] Achievements = null;
        public AchievementBadgePlayer AchievementBadgePlayer = null;
        public FriendData[] Update_Friend = null;
        public PlayerLand[] PlayerLands = null;
        public DailyShop DailyShop = null;
        public ShopHistory[] HistoryDailyShop = null;
        public ShopHistory[] HistoryWeeklyShop = null;
        public PlayerMail[] Update_PlayerMail = null;
        // Package IAP
        public PackageIAP[] PackageIAP = null;
        // Inventory Bag
        public UserInventoryBag InventoryBag = null;

        // Battle
        public BattleResult BattleResult = null;
        public BattleStatus BattleMonsterStatus = null;
        public ResultAttackMonsterResponse ResultAttackMonsterResponse = null;
        public ClanBossBattleResult ClanBossBattleResult = null;
        public ArenaBattleResult ArenaBattleResult = null;

        public TileGeoPoint[] TileGeoPoint = null;

        // Chest
        public PlayerChestResponse Update_PlayerChest = null;

        // Clan
        public UserClan PlayerUserClan = null;
        public UserClan[] PlayerClanRequest = null;
        public long ClanLastTimeJoined = 0;
        public Clan PlayerClan = null;
        public ClanDonateType[] ClanDonate = null;
        public ClanMemberInfo[] ClanRequest = null;
        public ClanRank[] ClansTopLevel = null;
        public ClanInfo[] ClanInfos = null;
        public ListClanMemberInfo[] ClanMemberList = null;
        public ClanInfo[] ClanFinding = null;
        public PlayerClanBossResponse PlayerClanBoss = null;
        public UserRankResponse ClanBossRank = null;

        // Summon
        public SummonHistory[] SummonHistory = null;

        // Daily Reward
        public DailyReward DailyReward = null;

        // Battle Pass
        public BattlePassUpdate Update_BattlePass = null;

        //Portal
        public PortalBossAttackRespone PortalBossAttackRespone = null;
        public PortalBossRank[] PortalBossRanks = null;
        public PortalAttackData[] PortalAttackDatas = null;
        public PortalHistoryRespone PortalHistories = null;
        public PortalBossBattleResult PortalBossBattleResult = null;
        public PortalRandomResponse PortalRandomResponse = null;


        // UI
        public RewardData[] RewardDatas = null;
        public RewardSummon RewardSummon = null;
        public CardPlayer[] CombineResult = null;
        public ItemData[] MergeResult = null;

        // Tutorial
        public TutorialType[] TutorialVersion = null;

        // Outpost
        public UserOutpost Outpost = null;
        public OutpostBattleResult OutpostBattleResult = null;

        // Arena
        public ArenaResponse ArenaResponse = null;
        public UserRankResponse ArenaRankAll = null;

        // Exchange
        public UserExchange[] ExchangeDaily = null;

        // Adv Limit
        public UserAdvLimit[] DailyUserAdvLimit = null;

        public long DailyVersion;
        public long TimeServer;
    }

    public class ServerManager : NTBehaviour
    {
        #region Loading
        public const float TimeWait = .4f;
        public const float CheckVersion = 1;
        public const float AssetsLoading = 2;
        public const float LoadingGameData = 3;
        public const float LoginDeviceID = 4;
        public const float LoginToken = 5;
        public const float JoinSever = 6;
        public const float JoinGame = 7;
        public const float DoneLoad = 7.999f;
        public const float MaxLoad = 8;
        #endregion

        public bool IsLoad = false;

        public bool LoadAddressableDone = false;

        public long TimeServer = 0;
        public Coroutine CorTimeServer;

        public TMP_SpriteAsset TMP_SpriteAsset;

        public static ServerManager Instance;
        protected override void Awake()
        {
            base.Awake();
            if (ServerManager.Instance != null)
            {
                NTLog.LogWarning("Only 1 instance allow");
                return;
            }
            ServerManager.Instance = this;
        }

        // Start is called before the first frame update
        protected override void Start()
        {
            StartCoroutine(NTFunction.WaitSecond(1f, () =>
            {
                PopupManager.Instance.OnUI(PopupCode.LongMessageUI, null, (PopupUI popupUI) =>
                {
                    LongMessageUI longMessageUI = popupUI as LongMessageUI;
                    longMessageUI.SetData(
                        Lean.Localization.LeanLocalization.GetTranslationText("location_service_title", "Unleash the Adventure!"),
                        Lean.Localization.LeanLocalization.GetTranslationText("location_service_detail", "Embark on an epic quest filled with thrilling battles and hidden treasures! To enhance your gameplay experience, we request you give permission to access your location. This will allow us to unleash nearby monsters and unveil mysterious Astrals. \nSo prepare yourself for action-packed battles against monsters lurking in your vicinity! Use the Astrals to travel to exciting new realms and challenges! Enabling capitalized allows you to embark on this unforgettable adventure. You can always adjust your location settings later."),
                        Lean.Localization.LeanLocalization.GetTranslationText("btn_allow", "Allow"),
                        () =>
                        {
                            Input.location.Start();
                            GeoPointManager.Instance.Init();
                        },
                        () =>
                        {
                            GeoPointManager.Instance.Init();
                        },
                        null
                    );
                });
            }));

            if (IsLoad) return;
            StartCoroutine(LoadData());
            if (this.CorTimeServer != null) StopCoroutine(this.CorTimeServer);
            this.CorTimeServer = StartCoroutine(this.CountTimeServer());
            AudioCtrl.Instance.Play(AudioName.BGM_Splash);
        }

        private IEnumerator CountTimeServer()
        {
            while (true)
            {
                yield return new WaitForSeconds(1);
                this.TimeServer++;
            }
        }

        void OnApplicationQuit()
        {
            Debug.Log("On OnApplicationQuit ");
            // this.TMP_SpriteAsset.fallbackSpriteAssets.Clear();
        }

        void OnplicationFocus(bool focus)
        {
            if (!this.IsLoad) return;
            Debug.Log("On focusssss " + focus);
            if (focus)
            {

                StartCoroutine(DataCenterManager.Instance.IEGetTimeServer());
            }
        }
        void OnApplicationPause(bool focus)
        {
            if (!this.IsLoad) return;
            Debug.Log("On OnApplicationPause " + focus);
            if (!focus)
            {

                StartCoroutine(DataCenterManager.Instance.IEGetTimeServer());
            }
        }

        #region Join Server
        public IEnumerator Play()
        {
            NTLog.LogMessage("Play");
            yield return null;
            if (AccountManager.Instance.Account == null || AccountManager.Instance.Account._id.Length == 0)
            {
                PopupManager.Instance.OnUI(PopupCode.LoginUI);
                PopupManager.Instance.OffUI(PopupCode.LoadingUI);
            }
            else
            {
                // Join Server
                EventListenerManager.instance.PostEvent(EventCode.LoadingUI_DoneLoad, new LoadingData(JoinSever / MaxLoad, Lean.Localization.LeanLocalization.GetTranslationText("loading_join_server", "Join Sever")));
                int login_result = -1;
                yield return UserDataManager.Instance.Login((suc) =>
                {
                    if (suc) login_result = 1;
                    else login_result = 0;
                });
                PopupManager.Instance.OnUI(PopupCode.LoadingUI);
                yield return new WaitUntil(() => login_result != -1);
                if (login_result == 0)
                {
                    // Back to Login Screen if Join Server Failed

                    yield break;
                }
                PopupManager.Instance.OffUI(PopupCode.LoginUI);
                yield return IEChosingPlayerLand();

                PopupManager.Instance.OnUI(PopupCode.StartingUI);
                PopupManager.Instance.OffUI(PopupCode.LoadingUI);
            }
        }

        public IEnumerator IEJoinGame()
        {
            EventListenerManager.instance.PostEvent(EventCode.LoadingUI_DoneLoad, new LoadingData(JoinGame / MaxLoad, Lean.Localization.LeanLocalization.GetTranslationText("loading_join_game", "Join Game")));
            yield return JoinSocket();
            // yield return new WaitForSeconds(TimeWait);

            // Update Cache
            BattleTeamManager.Instance.GetBattleTeamDataSelected();

            // Change to Home Screen
            SceneController.Instance.LoadScene(SceneConfig.WorldMap_Screen);
            yield return WaitForSceneLoad(SceneConfig.WorldMap_Screen);


            EventListenerManager.instance.PostEvent(EventCode.LoadingUI_DoneLoad, new LoadingData(DoneLoad / MaxLoad, Lean.Localization.LeanLocalization.GetTranslationText("loading_done", "Loading")));

            yield return new WaitForSeconds(TimeWait);
            PopupManager.Instance.OffUI(PopupCode.LoadingUI);
        }

        public IEnumerator IEChosingPlayerLand()
        {
            while (true)
            {
                if (PlayerLandManager.Instance.GetPlayerLandSelected() != null)
                {
                    PopupManager.Instance.OffUI(PopupCode.LandSelectionUI);
                    break;
                }

                if (!PopupManager.Instance.GetPopupUIByCode(PopupCode.LandSelectionUI).IsShow())
                {
                    PopupManager.Instance.OnUI(PopupCode.LandSelectionUI, null, (PopupUI popupUI) =>
                    {

                    });
                }

                yield return new WaitForSeconds(1);
            }
        }
        #endregion

        public IEnumerator JoinSocket()
        {
            ColyseusRoomManager.Instance.Init();
            MsgDeliveryRoom.Instance.Init();
            yield return null;
        }

        #region LoadData
        public IEnumerator LoadData()
        {
            // wait for logo screen
            // yield return new WaitForSeconds(3);
            NTLog.LogMessage("LoadData");
            LeanLocalization.UpdateTranslations();
            this.IsLoad = true;
            this.LoadAddressableDone = false;

            IAP_Controller.Instance.Init();

            yield return SystemManager.Instance.IEInit();
            yield return LoadAddressable();
            yield return new WaitForSeconds(TimeWait);
            EventListenerManager.instance.PostEvent(EventCode.LoadingUI_DoneLoad, new LoadingData(AssetsLoading / MaxLoad, Lean.Localization.LeanLocalization.GetTranslationText("loading_assets", "Assets Loading")));
            yield return PopupManager.Instance.LoadData();
            yield return LocalizationManager.Instance.IELoadGameLanguage();
            yield return DataCenterManager.Instance.IELoadDataCenterHolder();
            yield return new WaitForSeconds(TimeWait);
            EventListenerManager.instance.PostEvent(EventCode.LoadingUI_DoneLoad, new LoadingData(CheckVersion / MaxLoad, Lean.Localization.LeanLocalization.GetTranslationText("loading_checkversion", "Check Version")));
            yield return DataCenterManager.Instance.CheckVersion();
            EventListenerManager.instance.PostEvent(EventCode.LoadingUI_DoneLoad, new LoadingData(LoadingGameData / MaxLoad, Lean.Localization.LeanLocalization.GetTranslationText("loading_gamedata", "Game Data Loading")));
            yield return new WaitForSeconds(TimeWait);
            yield return UserDataManager.Instance.LoadData();
            yield return UserProfileManager.Instance.LoadData();
            yield return ItemDataManager.Instance.LoadData();
            yield return CharacterPlayerManager.Instance.LoadData();
            yield return CharacterGearManager.Instance.LoadData();
            yield return ShopManager.Instance.LoadShopData();
            yield return CardPlayerManager.Instance.LoadData();
            yield return QuestManager.Instance.LoadData();
            yield return AchievementManager.Instance.LoadData();
            yield return SkillManager.Instance.LoadData();
            yield return PlayerLandManager.Instance.LoadData();
            BattleEngineController.Instance.LoadData();
            PackageIAPManager.Instance.LoadData();
            PortalWorldMapManager.Instance.LoadData();
            ServerGameManager.Instance.LoadData();
            ClanManager.Instance.LoadData();
            PlayerChestManager.Instance.LoadData();
            DailyRewardManager.Instance.LoadData();
            BattlePassManager.Instance.LoadData();
            OutpostWorldMapManager.Instance.LoadData();
            yield return new WaitForSeconds(TimeWait);
            MonsterManager.Instance.LoadData();
            ArenaManager.Instance.LoadData();
            yield return LoadTMP_SpriteAsset();
            this.AutoLogin();
        }
        #endregion

        public IEnumerator LoadAddressable()
        {
            // Load Addressable
            AddressablesLoader.Instance.LoadAsset();
            while (true)
            {
                if (!AddressablesLoader.Instance.DoneLoad)
                {
                    float process = (float)AddressablesLoader.Instance.GetProcessPercent();
                    process = (AssetsLoading + process) / MaxLoad;
                    EventListenerManager.instance.PostEvent(EventCode.LoadingUI_DoneLoad, new LoadingData(process, Lean.Localization.LeanLocalization.GetTranslationText("loading_addressable", "Loading Addressable ") + AddressablesLoader.Instance.GetProcessValue()));
                }
                else
                {
                    if (AddressablesLoader.Instance.DoneLoad)
                    {
                        if (AddressablesLoader.Instance.LoadSuccess)
                        {
                            break;
                        }
                        else
                        {
                            NTLog.LogError("Load Addressable Failed");
                            AddressablesLoader.Instance.DoneLoad = false;
                            PopupManager.Instance.OnUI(PopupCode.MessageOptionPanel, null, (PopupUI popupUI) =>
                            {
                                MessageOptionPanel messageOptionPanel = popupUI as MessageOptionPanel;
                                messageOptionPanel.IsBlockClickScreenDim = true;
                                messageOptionPanel.SetData(Lean.Localization.LeanLocalization.GetTranslationText("addressable_load_fail_title", "Download Failed"), Lean.Localization.LeanLocalization.GetTranslationText("addressable_load_fail_message", "Resource download failed. Check your connection and attempt the download again."));
                                messageOptionPanel.SetActionConfirm(() =>
                                {
                                    AddressablesLoader.Instance.LoadAsset();
                                }, Lean.Localization.LeanLocalization.GetTranslationText("btn_try_again", "Try Again"));
                                messageOptionPanel.SetActionReject(() =>
                                {
                                    Application.Quit();
                                }, Lean.Localization.LeanLocalization.GetTranslationText("btn_exit", "Exit"));
                            });
                        }
                    }
                }
                yield return new WaitForSeconds(.5f);
            }
            yield return null;
        }

        public IEnumerator LoadTMP_SpriteAsset()
        {
            // this.TMP_SpriteAsset.fallbackSpriteAssets.Clear();
            AddressablesLoader.Instance.LoadAssetsFromLocal(AddressablesAssetsConfig.TMP_SpriteAsset_Item, (result) =>
            {
                ListTMP_SpriteAssetAddressable listTMP_SpriteAssetAddressable = result.GetComponent<ListTMP_SpriteAssetAddressable>();
                foreach (TMP_SpriteAsset item in listTMP_SpriteAssetAddressable.ListTMP_SpriteAsset)
                {
                    // this.TMP_SpriteAsset.fallbackSpriteAssets.Add(item);
                }
            });
            yield return null;
        }

        public void AutoLogin()
        {
            NTLog.LogMessage("AutoLogin");
            if (AccountManager.Instance.IsAutoLogin() && AccountManager.Instance.GetToken() != "")
            {
                this.LoginByToken();
            }
            else
            {
                PopupManager.Instance.OnUI(PopupCode.LoginUI);
                PopupManager.Instance.OffUI(PopupCode.LoadingUI);
            }
        }

        [Button]
        public void GameStart()
        {
            NTLog.LogMessage("GameStart");
            if (AccountManager.Instance.Account != null && AccountManager.Instance.Account._id != null && AccountManager.Instance.Account._id.Length > 0)
            {
                StartCoroutine(this.Play());
            }
            else
            {
                this.LoginByDeviceID(true);
            }
        }

        public void LoginByDeviceID(bool isPlay = false)
        {
            EventListenerManager.instance.PostEvent(EventCode.LoadingUI_DoneLoad, new LoadingData(LoginDeviceID / MaxLoad, Lean.Localization.LeanLocalization.GetTranslationText("loading_login_deviceid", "Login DeviceID")));
            StartCoroutine(AccountManager.Instance.LoginByDeviceID(UnityEngine.SystemInfo.deviceUniqueIdentifier, (authenResponse) =>
            {
                if (authenResponse.Status == 1 && isPlay)
                    StartCoroutine(Play());
                else
                {
                    PopupManager.Instance.OnUI(PopupCode.LoginUI);
                    PopupManager.Instance.OffUI(PopupCode.LoadingUI);
                }
            }));
        }

        public void LoginByGooglePlay()
        {
            EventListenerManager.instance.PostEvent(EventCode.LoadingUI_DoneLoad, new LoadingData(LoginDeviceID / MaxLoad, Lean.Localization.LeanLocalization.GetTranslationText("loading_login_googleplay", "Login GooglePlay")));
            AccountManager.Instance.LoginGooglePlay((authenResponse) =>
            {
                if (authenResponse.Status == 1)
                {
                    StartCoroutine(Play());
                }
                else
                {
                    PopupManager.Instance.OnUI(PopupCode.LoginUI);
                    PopupManager.Instance.OffUI(PopupCode.LoadingUI);
                }
            });
        }

        public void LoginByApple()
        {
            EventListenerManager.instance.PostEvent(EventCode.LoadingUI_DoneLoad, new LoadingData(LoginDeviceID / MaxLoad, Lean.Localization.LeanLocalization.GetTranslationText("loading_login_apple", "Login Apple")));
            AccountManager.Instance.LoginGameCenter((authenResponse) =>
            {
                if (authenResponse.Status == 1)
                {
                    StartCoroutine(Play());
                }
                else
                {
                    PopupManager.Instance.OnUI(PopupCode.LoginUI);
                    PopupManager.Instance.OffUI(PopupCode.LoadingUI);
                }
            });
        }

        public void LoginByToken()
        {
            EventListenerManager.instance.PostEvent(EventCode.LoadingUI_DoneLoad, new LoadingData(LoginToken / MaxLoad, Lean.Localization.LeanLocalization.GetTranslationText("loading_login_token", "Login Token")));
            StartCoroutine(AccountManager.Instance.LoginByToken(AccountManager.Instance.GetToken(), (authenResponse) =>
            {
                if (authenResponse.Status == 1)
                {
                    StartCoroutine(Play());
                }
                else
                {
                    PopupManager.Instance.OnUI(PopupCode.LoginUI);
                    PopupManager.Instance.OffUI(PopupCode.LoadingUI);
                }
            }));
        }


        public void LoginByUsernamePassword(string username, string password, Action<AuthenResponse> done = null)
        {
            EventListenerManager.instance.PostEvent(EventCode.LoadingUI_DoneLoad, new LoadingData(LoginToken / MaxLoad, Lean.Localization.LeanLocalization.GetTranslationText("loading_login_account", "Login Account")));
            StartCoroutine(AccountManager.Instance.IELogin(username, password, (authenResponse) =>
            {
                if (authenResponse.Status == 1)
                {
                    StartCoroutine(Play());
                    done?.Invoke(authenResponse);
                }
                else
                {
                    PopupManager.Instance.OnUI(PopupCode.LoginUI);
                    PopupManager.Instance.OffUI(PopupCode.LoadingUI);
                    done?.Invoke(authenResponse);
                }
            }));
        }

        [Button]
        public void LogOut()
        {
            PopupManager.Instance.OffAllPopupUI();
            PopupManager.Instance.OffUI(PopupCode.BannerTopUI);
            PopupManager.Instance.OnUI(PopupCode.LoadingUI);
            AccountManager.Instance.Logout();
            UserDataManager.Instance.Logout();
            UserProfileManager.Instance.Logout();
            ItemDataManager.Instance.Logout();
            CharacterPlayerManager.Instance.Logout();
            CharacterGearManager.Instance.Logout();
            MonsterManager.Instance.Logout();
            PlayerLandManager.Instance.LogOut();
            CardPlayerManager.Instance.Logout();
            ShopManager.Instance.Logout();
            PlayerMailManager.instance.Logout();
            PackageIAPManager.Instance.Logout();
            ClanManager.Instance.Logout();
            BattleTeamManager.Instance.Logout();
            PlayerChestManager.Instance.Logout();
            DailyRewardManager.Instance.Logout();
            BattlePassManager.Instance.Logout();
            OutpostWorldMapManager.Instance.Logout();
            GeoPointManager.Instance.Logout();
            SceneController.Instance.LoadScene(SceneConfig.Login_Screen);
            PopupManager.Instance.OffUI(PopupCode.LoadingUI);
            PopupManager.Instance.OnUI(PopupCode.LoginUI);
        }

        public APIResponseData LastAPIResponseData = new APIResponseData();

        public APIResponseData APIResponse(string data)
        {
            APIResponse apiResponse = JsonUtility.FromJson<APIResponse>(data);
            APIResponseData apiResponseData = apiResponse.Data;
#if UNITY_EDITOR
            this.LastAPIResponseData = apiResponseData;
#endif
            if (apiResponseData.Error != null && apiResponseData.Error.Length > 0)
            {
                PopupManager.Instance.OnUI(PopupCode.MessagePanel, null, (PopupUI popupUI) =>
                {
                    string error = Lean.Localization.LeanLocalization.GetTranslationText(apiResponseData.Error, apiResponseData.Error);
                    popupUI.GetComponent<MessagePanel>().SetData(Lean.Localization.LeanLocalization.GetTranslationText("error_title", "Error"), error);
                }, false);
                AudioCtrl.Instance.Play(AudioName.UI_Popup_Panel_Error);
            }
            // Log Data
            JSONNode jdata = JSONNode.Parse(data)["Data"];

            bool isUpdateBattleTeam = false;

            #region Stream Update
            // Update User Data Response
            try
            {
                if (apiResponseData.UserDataResponse != null && apiResponseData.UserDataResponse._id != null && apiResponseData.UserDataResponse._id.Length > 0)
                    UserDataManager.Instance.UpdateUserData(apiResponseData.UserDataResponse);
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString());
            }
            // Update Achievement
            try
            {
                if (apiResponseData.Achievements != null && apiResponseData.Achievements.Length > 0)
                    AchievementManager.Instance.UpdateAchievement(apiResponseData.Achievements);
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString());
            }
            // Update Achievement Badge Player
            try
            {
                if (apiResponseData.AchievementBadgePlayer != null)
                    AchievementManager.Instance.UpdateAchievementBadgePlayer(apiResponseData.AchievementBadgePlayer);
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString());
            }
            // Update Character Gear
            try
            {
                if (apiResponseData.Update_CharacterGear != null && apiResponseData.Update_CharacterGear.Length > 0)
                {
                    CharacterGearManager.Instance.UpdateGears(apiResponseData.Update_CharacterGear);
                    isUpdateBattleTeam = true;
                }
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString());
            }
            // Update Character Player
            try
            {
                if (apiResponseData.Update_CharacterPlayer != null && apiResponseData.Update_CharacterPlayer.Length > 0)
                {
                    CharacterPlayerManager.Instance.UpdateCharacterPlayer(apiResponseData.Update_CharacterPlayer);
                    isUpdateBattleTeam = true;
                }
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString());
            }
            // Update Card Player
            try
            {
                if (apiResponseData.Update_CardPlayer != null && apiResponseData.Update_CardPlayer.Length > 0)
                {
                    CardPlayerManager.Instance.UpdateCardPlayer(apiResponseData.Update_CardPlayer);
                    isUpdateBattleTeam = true;
                }
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString());
            }
            // Update Battle Team
            try
            {
                if (apiResponseData.Update_BattleTeam != null && apiResponseData.Update_BattleTeam.Length > 0)
                {
                    BattleTeamManager.Instance.UpdateBattleTeam(apiResponseData.Update_BattleTeam);
                    isUpdateBattleTeam = true;
                }
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString());
            }
            ;
            // Update Arena
            #endregion
            #region Normal Update
            // Update Inventory Bag

            try
            {
                if (apiResponseData.InventoryBag != null)
                    UserDataManager.Instance.UpdateInventoryBag(apiResponseData.InventoryBag);
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString());
            }
            // Update Item
            try
            {
                if (apiResponseData.Update_Item != null && apiResponseData.Update_Item.Length > 0)
                    ItemDataManager.Instance.UpdateData(apiResponseData.Update_Item);
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString());
            }
            ;

            // Update Display Name
            try
            {
                if (apiResponseData.Update_DisplayName != null && apiResponseData.Update_DisplayName.DisplayeName.Length > 0)
                    UserDataManager.Instance.UpdateName(apiResponseData.Update_DisplayName);
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString());
            }
            ;
            // Update Avatar
            try
            {
                if (apiResponseData.AvatarUpdate != null)
                    UserProfileManager.Instance.Update_AvatarPlayer(apiResponseData.AvatarUpdate);
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString());
            }
            ;
            try
            {
                if (apiResponseData.AvatarBorderUpdate != null)
                    UserProfileManager.Instance.Update_AvatarBorderPlayer(apiResponseData.AvatarBorderUpdate);
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString());
            }
            ;
            // Update Daily Quest
            try
            {
                if (apiResponseData.DailyQuest != null && apiResponseData.DailyQuest.Length > 0)
                    QuestManager.Instance.UpdateQuest(apiResponseData.DailyQuest);
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString());
            }
            // Update Friend
            try
            {
                if (apiResponseData.Update_Friend != null && apiResponseData.Update_Friend.Length > 0)
                    FriendManager.Instance.UpdateFriendData(apiResponseData.Update_Friend);
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString());
            }

            // Update Battle Monster Status
            try
            {
                if (apiResponseData.BattleMonsterStatus != null)
                    MonsterManager.Instance.UpdateBattleStatus(apiResponseData.BattleMonsterStatus);
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString());
            }

            // Update Player Land
            try
            {
                if (apiResponseData.PlayerLands != null && apiResponseData.PlayerLands.Length > 0)
                    PlayerLandManager.Instance.UpdatePlayerLand(apiResponseData.PlayerLands);
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString());
            }

            // Update Daily Shop
            try
            {
                if (apiResponseData.DailyShop != null)
                    ShopManager.Instance.UpdateDailyShopData(apiResponseData.DailyShop);
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString());
            }

            // Update History Daily Shop
            try
            {
                if (apiResponseData.HistoryDailyShop != null && apiResponseData.HistoryDailyShop.Length > 0)
                    ShopManager.Instance.UpdateHistoryDailyShop(apiResponseData.HistoryDailyShop);
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString());
            }

            // Update History Weekly Shop

            try
            {
                if (apiResponseData.HistoryWeeklyShop != null && apiResponseData.HistoryWeeklyShop.Length > 0)
                    ShopManager.Instance.UpdateHistoryWeeklyShop(apiResponseData.HistoryWeeklyShop);
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString());
            }

            // Update Player Mail
            try
            {
                if (apiResponseData.Update_PlayerMail != null && apiResponseData.Update_PlayerMail.Length > 0)
                    PlayerMailManager.instance.UpdatePlayerMail(apiResponseData.Update_PlayerMail);
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString());
            }

            // Update Package IAP
            try
            {
                if (apiResponseData.PackageIAP != null && apiResponseData.PackageIAP.Length > 0)
                    PackageIAPManager.Instance.UpdatePackageIAP(apiResponseData.PackageIAP);
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString());
            }

            // Update User Energy
            try
            {
                if (apiResponseData.UserEnergy != null)
                    UserDataManager.Instance.UpdateUserEnergy(apiResponseData.UserEnergy);
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString());
            }

            // Update Player Chest
            try
            {
                if (apiResponseData.Update_PlayerChest != null && apiResponseData.Update_PlayerChest._id != null && apiResponseData.Update_PlayerChest._id.Length > 0)
                    PlayerChestManager.Instance.UpdatePlayerChest(apiResponseData.Update_PlayerChest);
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString());
            }

            // Clan
            try
            {
                if (apiResponseData.ClanDonate != null && apiResponseData.ClanDonate.Length > 0)
                    ClanManager.Instance.UpdateClanDonate(apiResponseData.ClanDonate);
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString());
            }
            try
            {
                if (apiResponseData.PlayerClanRequest != null && apiResponseData.PlayerClanRequest.Length > 0)
                    ClanManager.Instance.UpdatePlayerClanRequest(apiResponseData.PlayerClanRequest);
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString());
            }
            try
            {
                if (apiResponseData.PlayerClan != null && apiResponseData.PlayerClan._id != null && apiResponseData.PlayerClan._id.Length > 0)
                    ClanManager.Instance.UpdateClan(apiResponseData.PlayerClan);
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString());
            }
            try
            {
                if (apiResponseData.PlayerUserClan != null && apiResponseData.PlayerUserClan._id != null && apiResponseData.PlayerUserClan._id.Length > 0)
                    ClanManager.Instance.UpdatePlayerUserClan(apiResponseData.PlayerUserClan);
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString());
            }
            try
            {
                if (apiResponseData.ClanLastTimeJoined > 0)
                    ClanManager.Instance.UpdateClanLastTimeJoined(apiResponseData.ClanLastTimeJoined);
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString());
            }
            try
            {
                if (apiResponseData.PlayerClanBoss != null && apiResponseData.PlayerClanBoss._id != null && apiResponseData.PlayerClanBoss._id.Length > 0)
                    ClanManager.Instance.UpdateClanBossResponse(apiResponseData.PlayerClanBoss);
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString());
            }
            try
            {
                if (apiResponseData.ClanRequest != null && apiResponseData.ClanRequest.Length > 0)
                    ClanManager.Instance.UpdateClanRequest(apiResponseData.ClanRequest);
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString());
            }
            try
            {
                if (apiResponseData.ClansTopLevel != null && apiResponseData.ClansTopLevel.Length > 0)
                    ClanManager.Instance.UpdateClansTopLevel(apiResponseData.ClansTopLevel);
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString());
            }
            try
            {
                if (apiResponseData.ClanInfos != null && apiResponseData.ClanInfos.Length > 0)
                    ClanManager.Instance.UpdateClanInfos(apiResponseData.ClanInfos);
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString());
            }
            try
            {
                if (apiResponseData.ClanMemberList != null && apiResponseData.ClanMemberList.Length > 0)
                    ClanManager.Instance.UpdateClanMemberList(apiResponseData.ClanMemberList);
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString());
            }
            try
            {
                if (apiResponseData.ClanFinding != null && apiResponseData.ClanFinding.Length > 0)
                    ClanManager.Instance.UpdateClanFinding(apiResponseData.ClanFinding);
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString());
            }
            try
            {
                if (apiResponseData.ClanBossBattleResult != null)
                    ClanManager.Instance.UpdateClanBossBattleResult(apiResponseData.ClanBossBattleResult);
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString());
            }
            try
            {
                if (apiResponseData.ClanBossRank != null && apiResponseData.ClanBossRank.UserRanks != null && apiResponseData.ClanBossRank.UserRanks.Length > 0)
                    ClanManager.Instance.UpdateClanBossRank(apiResponseData.ClanBossRank);
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString());
            }

            // Update Daily Reward
            try
            {
                if (apiResponseData.DailyReward != null)
                    DailyRewardManager.Instance.UpdateDailyReward(apiResponseData.DailyReward);
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString());
            }

            // Update Daily Version
            try
            {
                if (apiResponseData.DailyVersion > 0)
                    UserDataManager.Instance.UpdateDailyVersion(apiResponseData.DailyVersion);
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString());
            }

            // Update Battle Pass
            try
            {
                if (apiResponseData.Update_BattlePass != null)
                    BattlePassManager.Instance.UpdateBattlePass(apiResponseData.Update_BattlePass);
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString());
            }

            // Update Portal Boss Attack
            try
            {
                if (apiResponseData.PortalBossAttackRespone != null)
                    PortalWorldMapManager.Instance.UpdatePortalBossAttackRespone(apiResponseData.PortalBossAttackRespone);
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString());
            }

            // Update Geo Point
            try
            {
                if (apiResponseData.TileGeoPoint != null)
                    GeoPointManager.Instance.UpdateGeoPoint(apiResponseData.TileGeoPoint);
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString());
            }

            // Update User Data Short
            try
            {
                if (apiResponseData.UserDataShorts != null && apiResponseData.UserDataShorts.Length > 0)
                    UserProfileManager.Instance.Update_UserDataShort(apiResponseData.UserDataShorts);
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString());
            }

            // Update Summon History
            try
            {
                if (apiResponseData.SummonHistory != null && apiResponseData.SummonHistory.Length > 0)
                    CardPlayerManager.Instance.UpdateSummonHistory(apiResponseData.SummonHistory);
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString());
            }
            try
            {
                if (apiResponseData.BattleResult != null && apiResponseData.BattleResult._id != null && apiResponseData.BattleResult._id.Length > 0)
                    BattleEngineController.Instance.SetBattleResult(apiResponseData.BattleResult);
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString());
            }
            try
            {
                if (apiResponseData.ClanBossBattleResult != null && apiResponseData.ClanBossBattleResult.TotalDamage > 0)
                    BattleEngineController.Instance.SetClanBattleResult(apiResponseData.ClanBossBattleResult);
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString());
            }
            try
            {
                if (apiResponseData.PortalBossRanks != null && apiResponseData.PortalBossRanks.Length > 0)
                    PortalWorldMapManager.Instance.UpdatePortalBossRank(apiResponseData.PortalBossRanks.ToList());
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString());
            }
            try
            {
                if (apiResponseData.PortalAttackDatas != null && apiResponseData.PortalAttackDatas.Length > 0)
                    PortalWorldMapManager.Instance.UpdatePortalAttackData(apiResponseData.PortalAttackDatas.ToList());
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString());
            }
            try
            {
                if (apiResponseData.PortalHistories != null)
                    PortalWorldMapManager.Instance.UpdatePortalHistory(apiResponseData.PortalHistories);
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString());
            }
            try
            {
                if (apiResponseData.PortalBossBattleResult != null)
                    BattleEngineController.Instance.SetPortalBossBattleResult(apiResponseData.PortalBossBattleResult);
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString());
            }
            // Update Tutorial Done
            try
            {
                if (apiResponseData.TutorialVersion != null && apiResponseData.TutorialVersion.Length > 0)
                    UserProfileManager.Instance.Update_TutorialDone(apiResponseData.TutorialVersion);
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString());
            }

            // Update Outpost

            try
            {
                if (apiResponseData.Outpost != null)
                    OutpostWorldMapManager.Instance.UpdateUserOutpost(apiResponseData.Outpost);
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString());
            }

            try
            {
                if (apiResponseData.OutpostBattleResult != null)
                    BattleEngineController.Instance.SetOutpostBattleResult(apiResponseData.OutpostBattleResult);
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString());
            }

            // Update Exchange Daily
            try
            {
                if (apiResponseData.ExchangeDaily != null && apiResponseData.ExchangeDaily.Length > 0)
                    ItemDataManager.Instance.UpdateExchangeDaily(apiResponseData.ExchangeDaily);
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString());
            }
            try
            {
                if (apiResponseData.DailyUserAdvLimit != null && apiResponseData.DailyUserAdvLimit.Length > 0)
                    UserDataManager.Instance.UpdateUserAdvLimit(apiResponseData.DailyUserAdvLimit);
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString());
            }

            // Arena
            try
            {
                if (apiResponseData.ArenaResponse != null && apiResponseData.ArenaResponse.ArenaVersion > 0)
                    ArenaManager.Instance.UpdateArenaResponse(apiResponseData.ArenaResponse);
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString());
            }

            try
            {
                if (apiResponseData.ArenaRankAll != null && apiResponseData.ArenaRankAll._id != null && apiResponseData.ArenaRankAll._id.Length > 0)
                    ArenaManager.Instance.UpdateArenaRankAll(apiResponseData.ArenaRankAll);
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString());
            }
            try
            {
                if (apiResponseData.ArenaBattleResult != null && apiResponseData.ArenaBattleResult._id != null && apiResponseData.ArenaBattleResult._id.Length > 0)
                    ArenaManager.Instance.UpdateArenaBattleResult(apiResponseData.ArenaBattleResult);
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString());
            }

            try
            {
                if (apiResponseData.BattleTeamSelected != null && apiResponseData.BattleTeamSelected._id != null && apiResponseData.BattleTeamSelected._id.Length > 0)
                    BattleTeamManager.Instance.UpdateBattleTeamSelected(apiResponseData.BattleTeamSelected);
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString());
            }

            try
            {
                if (apiResponseData.UpdateBattleShortTeam != null)
                    BattleTeamManager.Instance.UpdateBattleShortTeam(apiResponseData.UpdateBattleShortTeam);
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString());
            }

            try
            {
                if (apiResponseData.PortalRandomResponse != null)
                    PortalWorldMapManager.Instance.UpdatePortalRandom(apiResponseData.PortalRandomResponse);
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.ToString());
            }


            #endregion

            #region UI
            // UI
            if (apiResponseData.RewardDatas != null && apiResponseData.RewardDatas.Length > 0)
            {
                // Todo: using when amount of reward data is equal or lower than 11
                try
                {
                    PopupManager.Instance.OnUI(PopupCode.RewardDataUI, null, (popup) =>
                    {
                        RewardDataUI rewardDataUI = popup as RewardDataUI;
                        rewardDataUI.SetData(apiResponseData.RewardDatas);
                    });
                }
                catch
                {

                }
            }
            #endregion

            if (isUpdateBattleTeam)
            {
                BattleTeamManager.Instance.GetBattleTeamDataSelected();
            }

            this.TimeServer = apiResponse.Data.TimeServer;
            return apiResponseData;
        }


        // Function to wait for scene to load successfully
        public IEnumerator WaitForSceneLoad(string sceneName, Action done = null)
        {
            yield return new WaitUntil(() => SceneManager.GetSceneByName(sceneName).isLoaded);
            done?.Invoke();
        }

        public void ShowNotificationDataError()
        {
            HUDCanvas.Instance.ShowNotification(LeanLocalization.GetTranslationText("data_error", "The connection is unstable. Please try again in a few minutes!"));
        }

        #region Getter

        public long GetTimeServer()
        {
            return this.TimeServer;
        }

        // Second
        public long GetNextTimeNewDay()
        {
            DateTime now = NTFunction.UnixTimestampToDateTime(this.TimeServer);
            DateTime newDay = now.AddDays(1);
            newDay = new DateTime(newDay.Year, newDay.Month, newDay.Day, 0, 0, 0);
            return (long)(newDay.Ticks - now.Ticks) / 10000000;
        }

        public string GetTimeOffline(long last)
        {
            return NTFunction.Format_Time(ServerManager.Instance.GetTimeServer() - last, 1);
        }

        public long GetTimeNewWeek()
        {
            int day = (int)NTFunction.GetTotalDay(this.TimeServer);
            int week = (int)NTFunction.GetTotalWeek(this.TimeServer) + 1;
            int nextDay = week * 7 + 4 - day - 1;
            return nextDay * 24 * 60 * 60 + this.GetNextTimeNewDay();
        }

        #endregion

        #region Setter
        #endregion

    }
}