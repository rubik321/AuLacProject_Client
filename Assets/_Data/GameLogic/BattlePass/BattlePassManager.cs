using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NTPackage.EventDispatcher;
using NTPackage.Functions;
using NTPackage.UI;
using Rubik.Config;
using Rubik.DataCenter;
using Rubik.IAP;
using Rubik.ItemPlayer;
using Rubik.Manager;
using Rubik.UI;
using Rubik.UserDataPlayer;
using SimpleJSON;
using UnityEngine;

namespace Rubik.Myrk.BattlePass
{
    public class BattlePassConfig
    {
        public const string API_ClaimReward = "/api/2D_GPS/battle_pass/claim_reward";
    }

    public class BattlePassManager : NTBehaviour
    {

        #region Player Data
        [Header("Player Data")]
        public BattlePassUpdate BattlePass;
        #endregion

        #region Game Data
        [Header("Game Data")]
        public BattlePassData BattlePassData;
        #endregion


        #region Function
        public static BattlePassManager Instance;
        protected override void Awake()
        {
            base.Awake();
            if (BattlePassManager.Instance != null)
            {
                NTLog.LogError("Only 1 Instance allow");
                return;
            }
            BattlePassManager.Instance = this;
        }

        public void Logout()
        {
            this.BattlePass = new BattlePassUpdate();
        }

        public void LoadData()
        {
            this.BattlePassData = JsonUtility.FromJson<BattlePassData>(DataCenterManager.Instance.GetData(DataName.BattlePassData));
        }

        public void UpdateBattlePass(BattlePassUpdate battlePassUpdate)
        {
            if (battlePassUpdate != null && battlePassUpdate.BattlePassVersion > 0)
            {
                this.BattlePass = battlePassUpdate;
                EventListenerManager.instance.PostEvent(EventCode.BattlePassUpdate, this.BattlePass);
            }
        }

        public void ClaimReward(int level, BattlePassType type)
        {
            if (UserDataManager.Instance.IsCapSlotInventoryBag())
            {
                UserDataManager.Instance.ShowNotificationCapSlotInventoryBag();
                return;
            }
            StartCoroutine(this.IEClaimReward(level, type, false, (success) =>
            {
                if (success)
                {
                    this.BattlePass.BattlePassFreeClaimed[level] = true;
                }
            }));
        }

        public void ClaimRewardAdv(int level, BattlePassType type)
        {
            if (UserDataManager.Instance.IsCapSlotInventoryBag())
            {
                UserDataManager.Instance.ShowNotificationCapSlotInventoryBag();
                return;
            }
            LevelPlayAds.Instance.OnShowReward(() =>
            {
                AppsFlyerManager.TrackingAds("Battle_pass");
                StartCoroutine(this.IEClaimReward(level, type, true, (success) =>
            {
                if (success)
                {
                    this.BattlePass.BattlePassFreeClaimed[level] = true;
                }
            }));
            });

        }

        #endregion

        #region API
        public IEnumerator IEClaimReward(int level, BattlePassType type, bool isAdv, Action<bool> done = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.Instance.GetUserID();
            JSONArray jsonArray = new JSONArray();
            jsonArray.Add(JSONNode.Parse(JsonUtility.ToJson(new BattlePassRewardClaimData()
            {
                Level = level,
                Type = type,
            })));

            jdata["datas"] = jsonArray;
            jdata["isAdv"] = isAdv;
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + BattlePassConfig.API_ClaimReward, (data) =>
                {
                    APIResponseData aPIResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                    if(aPIResponseData.Status == 0) return;
                    done?.Invoke(true);
                    done = null;
                });
            done?.Invoke(false);
        }
        #endregion

        #region IAP
        [NTButton]
        public void UnlockPremium(Action done = null)
        {
            PopupManager.Instance.OnUI(PopupCode.MessageOptionPanel, null, (data) =>
            {
                MessageOptionPanel messageOptionPanel = data as MessageOptionPanel;
                string title = Lean.Localization.LeanLocalization.GetTranslationText("unlock_golden_pass_title", "Unlock Golden Pass");
                string content = Lean.Localization.LeanLocalization.GetTranslationText("unlock_golden_pass_des", "Unlock the Golden Pass for only {0} to claim exclusive rewards, and rare treasures on your journey!");
                content = string.Format(content, BattlePassManager.Instance.GetPriceUnlockPremium());
                messageOptionPanel.SetData(title, content);
                messageOptionPanel.SetActionConfirm(() =>
                {
                    IAP_Controller.Instance.Purchase(IAP_Config.IAP_BattlePass1, "1", (success) =>
                {
                    if (success)
                    {
                    }
                    done?.Invoke();
                });
                }, Lean.Localization.LeanLocalization.GetTranslationText("btn_agree", "Agree"));
                messageOptionPanel.SetActionReject(() =>
                {
                    data.OffUI();
                }, Lean.Localization.LeanLocalization.GetTranslationText("btn_cancel", "Cancel"));
            });
        }
        #endregion

        #region Get
        public bool IsPremium()
        {
            return this.BattlePass.BattlePassUnlocked.ToList().Contains(BattlePassType.Premium);
        }

        public BattlePassStatus GetStatusFree(int level)
        {
            if (this.BattlePass.BattlePassFreeClaimed[level])
            {
                return BattlePassStatus.Claimed;
            }
            else
            {
                long battlePassPoint = ItemDataManager.Instance.GetItem(ItemType.BattlePassPoint).Amount;
                if (battlePassPoint >= this.BattlePassData.Free.Reward[level].ExpRequire)
                {
                    return BattlePassStatus.Claim;
                }
                else
                {
                    return BattlePassStatus.NotClaim;
                }
            }
        }

        public BattlePassStatus GetStatusPremium(int level)
        {
            if (this.BattlePass.BattlePassPremiumClaimed[level])
            {
                return BattlePassStatus.Claimed;
            }
            else
            {
                if (!this.IsPremium())
                {
                    return BattlePassStatus.Locked;
                }
                long battlePassPoint = ItemDataManager.Instance.GetItem(ItemType.BattlePassPoint).Amount;
                if (battlePassPoint >= this.BattlePassData.Premium.Reward[level].ExpRequire)
                {
                    return BattlePassStatus.Claim;
                }
                else
                {
                    return BattlePassStatus.NotClaim;
                }
            }
        }

        public int GetBattlePassVersion()
        {
            return (int)((NTFunction.GetTotalDay(ServerManager.Instance.GetTimeServer()) / 20) + 0);
        }

        public long GetBattlePassTimeRemaining()
        {
            long day_remain = (this.GetBattlePassVersion() + 1) * 20 - NTFunction.GetTotalDay(ServerManager.Instance.GetTimeServer());
            return (day_remain - 1) * 24 * 60 * 60 +  ServerManager.Instance.GetNextTimeNewDay();
        }

        public string GetPriceUnlockPremium()
        {
            return IAPManager.Instance.getPriceProduct(IAP_Config.IAP_BattlePass1);
        }
        #endregion
    }
}