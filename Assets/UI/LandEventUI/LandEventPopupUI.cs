using System;
using System.Collections;
using System.Collections.Generic;
using GOA.UIProfile;
using NTFunctions_old;
using Rubik.UI;
using SimpleJSON;
using UnityEngine;

namespace GOA.LandEvent
{
    public class LandEventPopupUI : PopupUI
    {
        public EventType EventType = EventType.Hunter;

        public LandData LandData;

        public List<LandEventButtonUI> LandEventButtonUIs;

        public Transform BtnConfirm;

        public override void LoadComponents()
        {
            base.LoadComponents();
            this.LoadLandEventButtonUI();
        }

        protected void LoadLandEventButtonUI()
        {
            this.LandEventButtonUIs.Clear();
            foreach (Transform item in transform.Find("Panel").Find("Content"))
            {
                if (item.TryGetComponent<LandEventButtonUI>(out LandEventButtonUI landEventButtonUI))
                {
                    this.LandEventButtonUIs.Add(landEventButtonUI);
                    landEventButtonUI.Onclick.RemoveAllListeners();
                    landEventButtonUI.Onclick.AddListener(() =>
                    {
                        this.ChoseEvent(landEventButtonUI.EventType);
                    });
                }
            }
        }

        public void OnUI(LandData landData)
        {
            this.LandData = landData;
            foreach (LandEventButtonUI item in this.LandEventButtonUIs)
            {
                item.Init();
            }
            this.ChoseEvent();
            this.Show();
        }

        public void ChoseEvent(EventType eventType = EventType.Hunter)
        {
            if (eventType != EventType.Hunter)
            {
                HUDCanvas.Instance.ShowNotification("Comming soon!");
            }
            else
            {
                this.EventType = eventType;
            }
            foreach (LandEventButtonUI item in this.LandEventButtonUIs)
            {
                if (item.EventType == this.EventType)
                {
                    item.Chose();
                    this.BtnConfirm.gameObject.SetActive(item.IsEnoughMat);
                    // this.BtnConfirm.gameObject.SetActive(true);
                }
                else item.UnChose();
            }
        }

        public Action ActConfirm;
        public void Confirm()
        {
            this.OffUI();
            APIManager.Instance.LandOrganizingEvent(
                UserData.UserData.Instance.data.UserId, UserData.UserData.Instance.data.UserName,
                this.LandData._id, this.LandData.Latitude, this.LandData.Longitude, (int)this.EventType, (data) =>
                {
                    JSONNode jdata = JSONNode.Parse(data);
                    LandEventData landEventData = JsonUtility.FromJson<LandEventData>(jdata["Data"].ToString());
                    if (landEventData.Duration < 1) landEventData.Duration = LandEventManager.DefaultTimeEvent;
                    LandEventManager.instance.AddLandEventData(landEventData);
                    this.ActConfirm?.Invoke();
                    this.ActConfirm = null;
                }
            );
        }
    }
}
