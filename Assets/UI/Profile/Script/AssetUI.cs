using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Rubik.UI;
using GOA.WorldMap;
using GoShared;
using NTFunctions_old;
using System;
using Sirenix.OdinInspector;
using GOA.LandEvent;
using SimpleJSON;
using DG.Tweening;

namespace GOA.UIProfile
{
    [System.Serializable]
    public class LandData
    {
        public string _id;
        public double Longitude;
        public double Latitude;
        public string District;

        public LandData(string _id ,float longitude, float latitude, string district)
        {
            this._id = _id;
            this.Longitude = longitude;
            this.Latitude = latitude;
            this.District = district;
        }
    }

    public class AssetUI : LoadBehaviour
    {
        public TextMeshProUGUI TextEventName;

        public TextMeshProUGUI textStatus;
        public TextMeshProUGUI TextTime;
        public TextMeshProUGUI textName;
        public Image Ava;
        public List<Transform> Stars;

        public Image TimeCount;

        public LandData LandData;
        public LandEventData LandEventData;

        // public const float TimeEvent = 3*60*60;

        public void SetData(LandData landData)
        {
            this.LandData = landData;
            this.textName.text = LandData.District;
            this.textStatus.text = "<sprite=1> " + NTFunctions_old.NTFunction.CollapString(this.LandData.Longitude + "", 8) + " <sprite=0> " + NTFunctions_old.NTFunction.CollapString(this.LandData.Latitude + "", 8);
            gameObject.SetActive(true);
            this.TextEventName.text = Lean.Localization.LeanLocalization.GetTranslationText("event", "Event");
            this.UpdateData();
        }

        public void UpdateEventData()
        {
            this.LandEventData = LandEventManager.instance.GetLandEventData(UserData.UserData.Instance.data.UserId,this.LandData._id);
            if (this.LandEventData == null || this.LandEventData.Time == 0)
            {
                this.TimeCount.gameObject.SetActive(false);
                this.TextTime.gameObject.SetActive(false);
                return;
            }
            long timeDiff = NTFunction.GetUtcTimestamp() - this.LandEventData.Time;
            if (timeDiff > this.LandEventData.Duration)
            {
                this.TimeCount.gameObject.SetActive(false);
                this.TextTime.gameObject.SetActive(false);
                return;
            }
            this.TextTime.text = "<sprite=3> " + DateTimeOffset.FromUnixTimeSeconds(this.LandEventData.Time + this.LandEventData.Duration).ToString();
            this.TextTime.gameObject.SetActive(true);
            float per = 1 - timeDiff / (float)this.LandEventData.Duration;
            this.TimeCount.gameObject.SetActive(true);
            this.TimeCount.fillAmount = per;
            this.TextEventName.text = Lean.Localization.LeanLocalization.GetTranslationText(this.LandEventData.EventType.ToString(), "Event");
            this.TimeCount.DOFillAmount(0, this.LandEventData.Duration - timeDiff).OnComplete(()=>{
                this.UpdateData();
            });
        }

        public void UpdateData()
        {
            this.UpdateEventData();
        }

        public void OnClick()
        {
            // HUDCanvas.Instance.ShowNotification("Comming soon!");
            // TeleportSystem.instance.OffAllFlagTeleport();
            // GeoPointManager.Instance.IsOwnLand = true;
            TeleportSystem.instance.Teleport(new Coordinates(this.LandData.Latitude + UnityEngine.Random.Range(-0.0015f, 0.0015f), this.LandData.Longitude + UnityEngine.Random.Range(-0.0015f, 0.0015f)));
            try
            {
                ProfileUI profileUI = (ProfileUI)UIManager.instance.GetPopupUIByCode(PopupCode.ProfileUI);
                profileUI.OffUI();
            }
            catch (System.Exception)
            {
            }
        }

        public void OrganizeEvent()
        {
            if (this.LandEventData != null && NTFunction.GetUtcTimestamp() - this.LandEventData.Time < this.LandEventData.Duration)
            {
                return;
            }
            try
            {
                LandEventPopupUI landEventPopupUI = (LandEventPopupUI) UIManager.instance.GetPopupUIByCode(PopupCode.LandEventPopupUI);
                landEventPopupUI.ActConfirm = this.UpdateData;
                landEventPopupUI.OnUI(this.LandData);
            }
            catch (System.Exception e){
                Debug.LogWarning(e);
            }
        }
    }
}
