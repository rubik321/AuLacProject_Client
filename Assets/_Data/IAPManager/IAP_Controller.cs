using System;
using System.Collections;
using System.Collections.Generic;
using NTPackage;
using NTPackage.EventDispatcher;
using NTPackage.Functions;
using Rubik.Config;
using Rubik.Manager;
using Rubik.UI;
using SimpleJSON;
using UnityEngine;

namespace Rubik.IAP
{

    public class IAP_Config
    {
        public static string API_Buy = "/api/2D_GPS/iap/buy";
        public static string API_Restore = "/api/2D_GPS/iap/restore";

        public const string IAP_BattlePass1 = "com.rubik.myrk.battlepass1";
        public const string IAP_RemoveAds = "com.rubik.myrk.removeads";
    }

    [System.Serializable]
    public class QueueIAP
    {
        public string ProductID;
        public double Time;
    }

    public class IAP_Controller : NTBehaviour
    {
        public static IAP_Controller Instance;

        public List<QueueIAP> QueueIAP;
        public string PackageId;
        public Action<bool> OnPurchase;

        protected override void Awake()
        {
            base.Awake();
            if (IAP_Controller.Instance != null)
            {
                NTLog.LogError("Only 1 Instance allow");
                return;
            }
            IAP_Controller.Instance = this;
        }

        #region Function
        public void Init()
        {
            this.QueueIAP = new List<QueueIAP>();
        }

        public void Purchase(string productID, string packageId, Action<bool> onPurchase)
        {
            this.PackageId = packageId;
            this.OnPurchase = onPurchase;
            IAPManager.Instance.BuyProduct(productID);
        }

        public void PurchaseSuccess(string productID, string transactionID, bool isRestore = false)
        {
            this.QueueIAP.Add(new QueueIAP
            {
                ProductID = productID,
                Time = NTFunction.GetUtcTimestamp()
            });
            if (isRestore)
            {
                StartCoroutine(IERestore(productID, this.PackageId, transactionID, (success) =>
                {
                    if (success)
                    {
                        this.QueueIAP.RemoveAll(x => x.ProductID == productID);
                        EventListenerManager.instance.PostEventWithKey(EventCode.IAP_PurchaseSuccess, productID);
                    }
                    this.OnPurchase?.Invoke(success);
                }));
            }
            else
            {
                StartCoroutine(IEBuy(productID, this.PackageId, transactionID, (success) =>
                {
                    if (success)
                    {
                        this.QueueIAP.RemoveAll(x => x.ProductID == productID);
                        EventListenerManager.instance.PostEventWithKey(EventCode.IAP_PurchaseSuccess, productID);
                    }
                    this.OnPurchase?.Invoke(success);
                }));
            }

        }

        public void PurchaseFailed(string productID)
        {
            // TODO: Handle purchase failed
        }

        #endregion

        #region API
        public IEnumerator IEBuy(string productID, string packageId, string transactionID, Action<bool> callback)
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = Rubik.UserDataPlayer.UserDataManager.Instance.GetUserId();
            jdata["productId"] = productID;
            jdata["packageId"] = packageId;
            jdata["platform"] = (int)Application.platform;
            jdata["transactionID"] = transactionID;
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + IAP_Config.API_Buy, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                callback?.Invoke(true);
                callback = null;
            });
            callback?.Invoke(false);
        }

        public IEnumerator IERestore(string productID, string packageId, string transactionID, Action<bool> callback)
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = Rubik.UserDataPlayer.UserDataManager.Instance.GetUserId();
            jdata["productId"] = productID;
            jdata["packageId"] = packageId;
            jdata["platform"] = (int)Application.platform;
            jdata["transactionID"] = transactionID;
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + IAP_Config.API_Restore, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0)
                {
                    callback?.Invoke(false);
                    callback = null;
                    return;
                }
                string des = "";
                if (productID == IAP_Config.IAP_RemoveAds)
                {
                    des = Lean.Localization.LeanLocalization.GetTranslationText("title_remove_ads", "Remove ADS");
                    HUDCanvas.Instance.ShowNotification(des, Lean.Localization.LeanLocalization.GetTranslationText("restore_iap_success", "Restore Success!"));
                }
                des += " " + Lean.Localization.LeanLocalization.GetTranslationText("restore_iap_success", "Restore Success!");
                HUDCanvas.Instance.ShowNotification(Lean.Localization.LeanLocalization.GetTranslationText("restore_iap_success", des));
                callback?.Invoke(true);
            });
        }

        #endregion
    }
}


