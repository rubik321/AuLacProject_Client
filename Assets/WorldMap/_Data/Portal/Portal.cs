using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NTFunctions_old;
using GoShared;
using SimpleJSON;
using GOA.UserData;
using GOA.Config;
using GOA.Portal;

namespace GOA.Portal
{
    using NTPackage.UI;
    using Rubik.Myrk;
    using Rubik.Myrk.Portal;
    using WorldMap;
    public enum Rarity
    {
        White = 0,
        Green = 1,
        Blue = 2,
        Purple = 3,
        God = 4
    }

    [System.Serializable]
    public class GeoPointData
    {
        public string _id;
        public string Index;
        public double longitude;
        public double latitude;
        public int Type;
        public int PointID;
        public bool Opened = false;

        public string Country;
        public string State;
        public string City;

        public int BuidingType;
        public string Ownwer;
        public string OwnerName;

        public Rarity Rarity
        {
            get
            {
                if (this.Index.Equals("M040001"))
                {
                    return Rarity.Green;
                }
                if (this.Index.Equals("M040002") || this.Index.Equals("M040003"))
                {
                    return Rarity.Blue;
                }
                if (this.Index.Equals("M040004") || this.Index.Equals("M040005"))
                {
                    return Rarity.Purple;
                }
                if (this.Index.Equals("M040006"))
                {
                    return Rarity.God;
                }
                return Rarity.Green;
            }
        }
    }

    public class Portal : MonoBehaviour
    {
        public GeoPointData PortalData;
        public Coordinates coordinates;
        public bool IsLook = true;
        public DateTime TimeUnlook = DateTime.Now;
        public int Lv;

        public Sprite UnlockIcon;
        public Sprite LockIcon;
        public Image BgIcon;

        public const float TimeDelay = 0.2f;
        public bool Holding = false;

        public Transform Reaper;

        public Image Icon;
        public Transform Portal_God_Tier;
        public const string Color__God_Tier = "FFA100";
        public Transform Portal_legendary;
        public const string Color_legendary = "FF00C6";
        public Transform Portal_Rare;
        public const string Color_Rare = "00C9FF";
        public Transform Portal_Common;
        public const string Color_Common = "05FF00";

        public void Chose()
        {
            this.Holding = true;
            StartCoroutine(this.CountHolding());
        }

        protected virtual IEnumerator CountHolding()
        {
            yield return new WaitForSeconds(TimeDelay);
            this.Holding = false;
        }

        protected virtual void OnMouseUp()
        {
            if (!this.Holding) return;
            PopupManager.Instance.OnUI(PopupCode.PortalOnMapUI, null, popupUI => {
                PortalOnMapUI portalOnMapUI = (PortalOnMapUI)popupUI;
                portalOnMapUI.SetData(this.PortalData.PointID);
            });
            // PortalUnlookUI portalUnlookUI = (PortalUnlookUI)UIManager.instance.GetPopupUIByCode(PopupCode.PortalUnlookUI);
            // Debug.LogWarning(portalUnlookUI.name);
            // if (portalUnlookUI == null) return;
            // portalUnlookUI.OnUI(this);
        }

        public void SetData(GeoPointData portalData)
        {
            this.PortalData = portalData;
            this.coordinates.latitude = portalData.latitude;
            this.coordinates.longitude = portalData.longitude;
            Vector3 pos = this.coordinates.convertCoordinateToVector(0);
            transform.position = pos;
            this.Portal_Common.gameObject.SetActive(false);
            this.Portal_Rare.gameObject.SetActive(false);
            this.Portal_legendary.gameObject.SetActive(false);
            this.Portal_God_Tier.gameObject.SetActive(false);
            switch (portalData.Rarity)
            {
                case Rarity.Blue:
                    this.Portal_Rare.gameObject.SetActive(true);
                    this.Icon.color = NTFunction.StringHexToColor(Color_Rare);
                    break;
                case Rarity.Purple:
                    this.Portal_legendary.gameObject.SetActive(true);
                    this.Icon.color = NTFunction.StringHexToColor(Color_legendary);
                    break;
                case Rarity.God:
                    this.Portal_God_Tier.gameObject.SetActive(true);
                    this.Icon.color = NTFunction.StringHexToColor(Color__God_Tier);
                    break;

                default:
                    this.Portal_Common.gameObject.SetActive(true);
                    this.Icon.color = NTFunction.StringHexToColor(Color_Common);
                    break;
            }
            JSONNode data = new JSONObject();
            data["userID"] = UserData.UserData.Instance.data.UserId;
            data["portalID"] = this.PortalData.PointID;
            StartCoroutine(APIManager.Instance.PostDataUrl(data.ToString(), SeverConfigs.BASE_API_URL + SeverConfigs.GetPortalDataAPI, callback =>
            {
                JSONNode data = JSONNode.Parse(callback.downloadHandler.text);
                PortalAttackData portalAttackData = JsonUtility.FromJson<PortalAttackData>(data["Data"].ToString());
                if (portalAttackData.Detail.Status == PortalStatus.Open
                    || portalAttackData.Detail.Status == PortalStatus.Defeated
                    || portalAttackData.Detail.Status == PortalStatus.Destroy)
                {
                    this.PortalData.Opened = true;
                }
                else
                {
                    this.PortalData.Opened = false;
                }
                this.UpdateData();
            }));
            this.UpdateData();
        }

        public void UpdateData()
        {
            Vector3 pos = this.coordinates.convertCoordinateToVector(0);
            transform.position = pos;
            this.Reaper.gameObject.SetActive(this.PortalData.Opened);
            this.BgIcon.sprite = this.LockIcon;

            if (this.PortalData.Opened) return;
            this.BgIcon.sprite = this.UnlockIcon;
            MobSO mobSO = MobAssets.instance.GetMobScriptableObjectByIndex(this.PortalData.Index);
            if (mobSO == null) return;
            NTFunction.ClearChild(this.Reaper);
            GameObject reaper = Instantiate(mobSO.Model);
            reaper.transform.SetParent(this.Reaper);
            NTFunction.ResetPosition(reaper.transform);
        }
    }
}
