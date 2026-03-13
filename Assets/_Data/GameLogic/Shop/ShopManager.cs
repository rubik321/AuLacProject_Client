using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NTPackage;
using NTPackage.Functions;
using Rubik.DataCenter;
using SimpleJSON;
using Rubik.Config;
using Rubik.Manager;
using System;
using Rubik.UserDataPlayer;
using Rubik.IAP;

namespace Rubik.Myrk.Shop
{
    using Rubik.ItemPlayer;
    public class ShopConfig
    {
        public const string API_Shop_Buy = "/api/2D_GPS/shop/buy";
        public const string API_Buy_Daily_Shop = "/api/2D_GPS/shop/buy_daily_shop";
        public const string API_Buy_Clan_Shop = "/api/2D_GPS/shop/buy_clan_shop";
    }

    public class ShopManager : NTBehaviour
    {
        public DailyShop DailyShopData;

        public ShopData IapShopData;
        public ShopData ClanShop;

        public NTDictionary<string, ShopHistory> DailyShopHistory;
        public NTDictionary<string, ShopHistory> WeeklyShopHistory;

        [SerializeField] private List<Sprite> ShopItemIcons;
        [SerializeField] private NTDictionary<string, Sprite> ShopItemIconDict;

        public static ShopManager Instance;
        protected override void Awake()
        {
            base.Awake();
            if (ShopManager.Instance != null)
            {
                NTLog.LogWarning("Only 1 Instance allow");
                return;
            }
            ShopManager.Instance = this;
        }

        #region Function
        public IEnumerator LoadShopData()
        {
            this.IapShopData = JsonUtility.FromJson<ShopData>(DataCenterManager.Instance.GetData(DataName.IAP_Shop_Cfg));
            this.ClanShop = JsonUtility.FromJson<ShopData>(DataCenterManager.Instance.GetData(DataName.ClanShopData));
            yield return null;

            this.ShopItemIconDict = new NTDictionary<string, Sprite>();
            foreach (Sprite item in this.ShopItemIcons)
            {
                this.ShopItemIconDict.Add(item.name, item);
            }
        }

        public void Logout()
        {
            this.DailyShopData = new DailyShop();
            this.DailyShopHistory = new NTDictionary<string, ShopHistory>();
            this.WeeklyShopHistory = new NTDictionary<string, ShopHistory>();
        }

        public void UpdateDailyShopData(DailyShop dailyShop)
        {
            this.DailyShopData = dailyShop;
        }

        public void UpdateHistoryDailyShop(ShopHistory[] historyDailyShop)
        {
            foreach (ShopHistory history in historyDailyShop)
            {
                this.DailyShopHistory.Add(history.Index, history);
            }
        }

        public void UpdateHistoryWeeklyShop(ShopHistory[] historyWeeklyShop)
        {
            foreach (ShopHistory history in historyWeeklyShop)
            {
                this.WeeklyShopHistory.Add(history.Index, history);
            }
        }
        #endregion
        public void Buy(string productId, string packageId, ItemData[] productPrice, Action<bool> done = null)
        {
            if (productId == null || productId == "")
            {
                StartCoroutine(IEBuy(packageId, (result) =>
                {
                    if (result)
                    {
                        foreach (ItemData item in productPrice)
                        {
                            if(item == null) continue;
                            if (item.Type == ItemType.Gem)
                            {
                                AppsFlyerManager.TrackingEvent(AppsflyerEvents.gems_spent, "buy_shop", (int)item.Amount);
                            }
                        }
                        done?.Invoke(true);
                    }
                    else
                    {
                        done?.Invoke(false);
                    }
                }));
            }
            else
            {
                IAP_Controller.Instance.Purchase(productId, packageId, done);
            }
        }

        public void BuyClanShop(string index, Action done = null)
        {
            StartCoroutine(IEBuyClanShop(index, done));
        }


        #region API
        public IEnumerator IEBuy(string index, Action<bool> done = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = Rubik.UserDataPlayer.UserDataManager.Instance.GetUserId();
            jdata["index"] = index;
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + ShopConfig.API_Shop_Buy, (data) =>
            {
                APIResponseData aPIResponse = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (aPIResponse.Status == 1)
                {
                    done?.Invoke(true);
                }
                else
                {
                    done?.Invoke(false);
                }

            });
            done?.Invoke(false);
        }

        public IEnumerator BuyDailyShop(string index, Action done = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = Rubik.UserDataPlayer.UserDataManager.Instance.GetUserId();
            jdata["index"] = index;
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + ShopConfig.API_Buy_Daily_Shop, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                done?.Invoke();
            });
        }

        public IEnumerator IEBuyClanShop(string index, Action done = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = Rubik.UserDataPlayer.UserDataManager.Instance.GetUserId();
            jdata["index"] = index;
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + ShopConfig.API_Buy_Clan_Shop, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                done?.Invoke();
            });
        }
        #endregion

        #region Getter
        public Sprite GetShopItemIcon(string index)
        {
            return this.ShopItemIconDict.Get(index);
        }

        public string GetShopItemTitle(string index)
        {
            return Lean.Localization.LeanLocalization.GetTranslationText("package_" + index + "_name", index);
        }

        public string GetShopItemDescription(string index)
        {
            return Lean.Localization.LeanLocalization.GetTranslationText("package_" + index + "_des", index);
        }

        public int GetAmountBought(string index)
        {
            if (this.DailyShopHistory.Contains(index))
                return this.DailyShopHistory.Get(index).BuyTime;
            if (this.WeeklyShopHistory.Contains(index))
                return this.WeeklyShopHistory.Get(index).BuyTime;
            return 0;
        }
        #endregion
    }
}

