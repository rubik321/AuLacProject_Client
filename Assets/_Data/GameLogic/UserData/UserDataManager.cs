using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NTPackage;
using NTPackage.EventDispatcher;
using NTPackage.Functions;
using Rubik.Account;
using Rubik.DataCenter;
using Rubik.Manager;
using Rubik.Config;
using SimpleJSON;
using UnityEngine;

namespace Rubik.UserDataPlayer
{
    using ItemPlayer;
    using NTPackage.UI;
    using Rubik._2DGPS.UserData;
    using Rubik.CardPlayer;
    using Rubik.CharacterPlayer;
    using Rubik.Myrk.BattleTeam;
    using Rubik.ServerGame;
    using Rubik.UI;
    using UnityEngine.Video;

    public class UserDataConfig
    {
        public const string API_UserData_Login = "/api/2D_GPS/user_data/login";
        public const string API_UserData_ExpandInventoryBag = "/api/2D_GPS/user_data/expand_inventory_bag";
        public const string API_UserData_ResetDay = "/api/2D_GPS/user_data/reset_day";

        // Developer
        public const string API_UserData_IncreaseExp = "/api/2D_GPS/user_data/increase_exp";
    }

    public class UserDataManager : NTBehaviour
    {

        #region Player Data 
        public UserDataResponse UserData;
        public SkinData SkinData;
        public UserInventoryBag UserInventoryBag;
        public long DailyVersion;
        public List<UserAdvLimit> DailyUserAdvLimit;
        #endregion

        #region Game Data 
        public List<ExpData> ExpData;
        public InventoryBagData InventoryBagData;
        public EnergyData EnergyData;

        public EnergyResponse UserEnergy;
        public List<AdvLimitData> AdvLimitData;
        #endregion

        public static UserDataManager Instance;
        protected override void Awake()
        {
            base.Awake();
            UserDataManager.Instance = this;
        }

        public override void LoadComponents()
        {
            base.LoadComponents();
        }

        #region Function

        public IEnumerator LoadData()
        {
            NTLog.LogMessage("LoadData UserDataManager");
            this.ExpData = new List<ExpData>();
            JSONNode jdata = JSON.Parse(DataCenterManager.Instance.GetData(DataName.ExpPlayerData));
            foreach (JSONNode item in jdata.AsArray)
            {
                ExpData expData = JsonUtility.FromJson<ExpData>(item.ToString());
                this.ExpData.Add(expData);
            }
            this.InventoryBagData = JsonUtility.FromJson<InventoryBagData>(DataCenterManager.Instance.GetData(DataName.InventoryBagData));
            this.EnergyData = JsonUtility.FromJson<EnergyData>(DataCenterManager.Instance.GetData(DataName.EnergyData));
            this.AdvLimitData = new List<AdvLimitData>();
            JSONNode jdataAdvLimit = JSON.Parse(DataCenterManager.Instance.GetData(DataName.AdvLimitData));
            foreach (JSONNode item in jdataAdvLimit.AsArray)
            {
                AdvLimitData advLimitData = JsonUtility.FromJson<AdvLimitData>(item.ToString());
                this.AdvLimitData.Add(advLimitData);
            }
            yield return null;
        }

        public void Init()
        {

        }

        public void Logout()
        {
            this.UserData = new UserDataResponse();
            this.UserInventoryBag = new UserInventoryBag();
            this.DailyVersion = 0;
            if (this.CorCheckDailyVersion != null)
            {
                StopCoroutine(this.CorCheckDailyVersion);
            }
            this.DailyUserAdvLimit = new List<UserAdvLimit>();
        }

        public Coroutine CorCheckDailyVersion;
        public IEnumerator IECheckDailyVersion()
        {
            while (true)
            {
                yield return new WaitForSeconds(1);
                if (this.DailyVersion != this.GetDailyVersion())
                {
                    if (this.DailyVersion < 1) break;
                    NTLog.LogMessage("Check Daily Version: " + this.DailyVersion + "|" + this.GetDailyVersion());
                    StartCoroutine(this.Login());
                    break;
                }

            }
        }
        public void UpdateDailyVersion(long dailyVersion)
        {
            if (dailyVersion > 0)
            {
                this.DailyVersion = dailyVersion;
                if (this.CorCheckDailyVersion != null)
                {
                    StopCoroutine(this.CorCheckDailyVersion);
                }
                this.CorCheckDailyVersion = StartCoroutine(this.IECheckDailyVersion());
            }
        }

        public void UpdateUserData(UserDataResponse userDataResponse)
        {
            if (userDataResponse == null || userDataResponse._id == null || userDataResponse._id.Length == 0) return;
            this.UserData = userDataResponse;
            if(this.UserData.IsNewDay){
                AppsFlyerManager.TrackingEvent(AppsflyerEvents.daily_login, "day", ServerGameManager.Instance.GetPlayerGroupServer());
            }
            EventListenerManager.instance.PostEvent(EventCode.UpdateUserData);
        }

        public void UpdateName(DisplayNameData nameData)
        {
            if (nameData.DisplayeName.Length > 0) this.UserData.DisplayName = nameData.DisplayeName;
            this.UserData.FirstChangeName = nameData.FirstChange;
            EventListenerManager.instance.PostEvent(EventCode.UpdateUserData);
        }

        public void UpdateUserEnergy(EnergyResponse energyResponse)
        {
            if (energyResponse == null || energyResponse.EnergyRecover == null || energyResponse.EnergyRecover < 1) return;
            this.UserEnergy = energyResponse;
        }

        public void UpdateUserAdvLimit(UserAdvLimit[] userAdvLimit)
        {
            if (userAdvLimit == null || userAdvLimit.Length == 0) return;
            foreach (UserAdvLimit item in userAdvLimit)
            {
                UserAdvLimit userAdvLimitData = this.DailyUserAdvLimit.Find(x => x._id == item._id);
                if (userAdvLimitData == null)
                {
                    this.DailyUserAdvLimit.Add(item);
                }
                else
                {
                    userAdvLimitData.Amount = item.Amount;
                }
            }
        }

        public string GetUserID()
        {
            return this.UserData._id;
        }

        public void ExpandInventoryBag(Action done = null)
        {
            this.StartCoroutine(this.IEExpandInventoryBag(done));
        }

        public void UpdateInventoryBag(UserInventoryBag userInventoryBag)
        {
            if (userInventoryBag == null || userInventoryBag._id == null || userInventoryBag._id.Length == 0) return;
            this.UserInventoryBag = userInventoryBag;
        }

        public int ExpIncrease = 0;
        [NTButton]
        public void IncreaseExp()
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = this.GetUserID();
            jdata["exp"] = this.ExpIncrease;
            if (this.ExpIncrease <= 0) return;
            StartCoroutine(Rubik.Server.APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + UserDataConfig.API_UserData_IncreaseExp, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                this.ExpIncrease = 0;
            }));
        }

        #endregion

        #region API
        public IEnumerator Login(Action<bool> done = null)
        {
            if (AccountManager.Instance.Account._id.Length > 0)
            {
                JSONNode jdata = new JSONObject();
                jdata["accountID"] = AccountManager.Instance.Account._id;
                yield return Rubik.Server.APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + UserDataConfig.API_UserData_Login, (data) =>
                {
                    APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                    if (apiResponseData.Status == 0) return;
                    done?.Invoke(true);
                    done = null;
                });
                done?.Invoke(false);
            }
        }

        public IEnumerator IEExpandInventoryBag(Action done = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = this.GetUserID();
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + UserDataConfig.API_UserData_ExpandInventoryBag, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                done?.Invoke();
                done = null;
            });
        }

        public IEnumerator IEResetDay(Action done = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = this.GetUserID();
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + UserDataConfig.API_UserData_ResetDay, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                done?.Invoke();
                done = null;
            });
        }

        #endregion

        #region Get
        public int GetServerPlay()
        {
            if (this.UserData == null) return 0;
            return this.UserData.Server;
        }

        public string GetDisplayName()
        {
            return this.UserData.DisplayName;
        }

        public string GetUserId()
        {
            return this.UserData._id;
        }

        public int GetLevel()
        {
            return this.UserData.Level;
        }

        public (long exp, long expNext, int level) GetPlayerLevel()
        {
            ExpData expData = this.ExpData.Find(e => e.Level == this.UserData.Level);
            if (expData == null) return (0, 0, this.UserData.Level);
            long exp = this.UserData.Exp;
            long expNext = expData.Exp;
            return (exp, expNext, expData.Level);
        }

        public bool IsMaxLevel(int level)
        {
            ExpData expData = this.ExpData.Find(e => e.Level == level);
            if (expData == null) return true;
            return expData.Max;
        }

        public ItemData GetPriceExpandInventoryBag()
        {
            return this.InventoryBagData.PriceExpand;
        }

        public int GetMaxExpandInventoryBag()
        {
            return this.InventoryBagData.MaxExpand;
        }

        public int GetSlotIncreaseInventoryBag()
        {
            return this.InventoryBagData.SlotIncrease;
        }

        public int GetCurrentSlotInventoryBag()
        {
            return this.UserInventoryBag.TimeExpand * this.InventoryBagData.SlotIncrease + this.InventoryBagData.StartSlot;
        }

        public bool IsCapSlotInventoryBag()
        {
            int amountItemSlot = ItemDataManager.Instance.GetItemBag().Count;
            return amountItemSlot >= this.GetCurrentSlotInventoryBag();
        }

        public void ShowNotificationCapSlotInventoryBag()
        {
            PopupManager.Instance.OnUI(PopupCode.MessageOptionPanel, null, (popupUI) =>
            {
                MessageOptionPanel messageOptionPanel = popupUI as MessageOptionPanel;
                messageOptionPanel.SetData(Lean.Localization.LeanLocalization.GetTranslationText("title_cap_slot_inventory_bag", "Bag Overflowing!"), Lean.Localization.LeanLocalization.GetTranslationText("notify_cap_slot_inventory_bag", "Your Bag is full! Please clear your bag first before continuing."));
                messageOptionPanel.SetActionConfirm(() =>
                {
                    PopupManager.Instance.OnUI(PopupCode.Inventory_UI);
                }, Lean.Localization.LeanLocalization.GetTranslationText("btn_go_to_bag", "Go to Bag"));
                messageOptionPanel.SetActionReject(() =>
                {
                    popupUI.OffUI();
                }, Lean.Localization.LeanLocalization.GetTranslationText("btn_cancel", "Cancel"));
            });
        }

        public (int energy, int energyMax, int energyRecover) GetUserEnergyItem()
        {
            EnergyResponse energyResponse = this.GetUserEnergy(this.UserEnergy.Energy, this.UserEnergy.EnergyRecover);
            int energy = (int)energyResponse.Energy;
            int energyMax = (int)this.GetEnergyMax();
            int energyRecover = (int)(energyResponse.EnergyRecover + this.EnergyData.EnergyRecoverTime - ServerManager.Instance.GetTimeServer());
            if (energy >= energyMax)
            {
                energyRecover = -1;
            }
            return (energy, energyMax, energyRecover);
        }

        public EnergyResponse GetUserEnergy(long energy, long energyRecover)
        {
            var energy_response = new EnergyResponse();
            energy_response.Energy = energy;
            energy_response.EnergyRecover = energyRecover;
            int energy_rec = Mathf.FloorToInt((ServerManager.Instance.GetTimeServer() - energyRecover) / this.EnergyData.EnergyRecoverTime);
            if (energy_rec < 1)
            {
                return energy_response;
            }

            if (energy + energy_rec > this.GetEnergyMax())
            {
                energy_rec = (int)this.GetEnergyMax() - (int)energy;
                if (energy_rec < 1)
                {
                    energy_rec = 0;
                }
                energy_response.EnergyRecover = ServerManager.Instance.GetTimeServer();
            }
            else
            {
                energy_response.EnergyRecover = energyRecover + energy_rec * this.EnergyData.EnergyRecoverTime;
            }
            energy_response.Energy += energy_rec;
            return energy_response;
        }

        public long GetEnergyMax()
        {
            return this.EnergyData.EnergyMax + (int)(this.UserData.Level / this.EnergyData.IncreaseEveryLevel);
        }

        public bool CanSkipBattle()
        {
            if (this.GetPlayerLevel().level < 4)
            {
                HUDCanvas.Instance.ShowNotification(Lean.Localization.LeanLocalization.GetTranslationText("level_5_to_skip_battle", "Battle skipping is unlocked at level 5. Keep progressing!"));
                return false;
            }
            return true;
        }

        public bool IsRole(Role[] roles)
        {
            if (roles == null || roles.Length == 0) return false;
            return roles.Contains(this.UserData.Role);
        }

        public Role GetRole()
        {
            return this.UserData.Role;
        }

        public long GetDailyVersion()
        {
            return NTFunction.GetTotalDay(ServerManager.Instance.GetTimeServer());
        }

        public bool IsPurchaseRemoveAds()
        {
            return this.UserData.RemoveAds;
        }

        public int GetRemainDailyAdvLimit(string _id)
        {
            AdvLimitData advLimitData = this.AdvLimitData.Find(x => x._id == _id);
            if (advLimitData == null)
            {
                NTLog.LogError("GetRemainDailyAdvLimit not found: " + _id);
                return 0;
            }
            UserAdvLimit userAdvLimit = this.DailyUserAdvLimit.Find(x => x._id == _id);
            if (userAdvLimit == null)
            {
                return advLimitData.Cap;
            }
            return advLimitData.Cap - userAdvLimit.Amount < 0 ? 0 : advLimitData.Cap - userAdvLimit.Amount;
        }

        #endregion

        #region Set


        #endregion
    }
}
