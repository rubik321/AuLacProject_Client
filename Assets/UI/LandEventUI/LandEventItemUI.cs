using System;
using System.Collections;
using System.Collections.Generic;
using GOA.WorldMap;
using GoShared;
using NTFunctions_old;
using Rubik.Chat;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace GOA.LandEvent
{
    public class LandEventItemUI : LoadBehaviour
    {
        public LandEventData LandEventData;

        public TextMeshProUGUI TextName;
        public TextMeshProUGUI TextOrganizerName;
        public TextMeshProUGUI TextTimeRemain;
        public TextMeshProUGUI TextPeopleJoined;
        public TextMeshProUGUI TextLocation;

        public List<Transform> IconEvent;

        [Button]
        public override void LoadComponents()
        {
            base.LoadComponents();
            this.LoadTextName();
            this.LoadTextOrganizerName();
            this.LoadTextTimeRemain();
            this.LoadTextPeopleJoined();
            this.LoadTextLocation();
            this.LoadIconEvent();
        }

        protected void LoadTextName()
        {
            if (TextName != null) return;
            this.TextName = transform.Find("Panel").Find("TextName(TMP)").GetComponent<TextMeshProUGUI>();
        }
        protected void LoadTextOrganizerName()
        {
            if (TextOrganizerName != null) return;
            this.TextOrganizerName = transform.Find("Panel").Find("TextOrganizerName(TMP)").GetComponent<TextMeshProUGUI>();
        }
        protected void LoadTextTimeRemain()
        {
            if (TextTimeRemain != null) return;
            this.TextTimeRemain = transform.Find("Panel").Find("TextTimeRemain(TMP)").GetComponent<TextMeshProUGUI>();
        }
        protected void LoadTextPeopleJoined()
        {
            if (TextPeopleJoined != null) return;
            this.TextPeopleJoined = transform.Find("Panel").Find("TextPeopleJoined(TMP)").GetComponent<TextMeshProUGUI>();
        }
        protected void LoadTextLocation()
        {
            if (TextLocation != null) return;
            this.TextLocation = transform.Find("Panel").Find("TextLocation(TMP)").GetComponent<TextMeshProUGUI>();
        }

        protected void LoadIconEvent()
        {
            this.IconEvent.Clear();
            foreach (Transform item in transform.Find("Panel").Find("Slot").Find("Mask"))
            {
                this.IconEvent.Add(item);
            }
        }

        public void SetData(LandEventData landEventData)
        {
            gameObject.SetActive(false);
            if (NTFunction.GetUtcTimestamp() - landEventData.Time > landEventData.Duration)
            {
                return;
            }
            this.LandEventData = landEventData;
            this.TextName.text = landEventData.UserName+"'s "+Lean.Localization.LeanLocalization.GetTranslationText(landEventData.EventType.ToString(), "Hunting");;
            this.TextOrganizerName.text = "<sprite=2> " + landEventData.UserName;
            long timeRemain = this.LandEventData.Time + this.LandEventData.Duration - NTFunction.GetUtcTimestamp();
            this.TextTimeRemain.text = "<sprite=3> " + DateTimeOffset.FromUnixTimeSeconds(this.LandEventData.Time + this.LandEventData.Duration).ToString();
            this.TextPeopleJoined.text = "<sprite=4> " + landEventData.PeopleJoined;
            this.TextLocation.text = "<sprite=1> " + NTFunctions_old.NTFunction.CollapString(this.LandEventData.Longitude + "", 8) + " <sprite=0> " + NTFunctions_old.NTFunction.CollapString(this.LandEventData.Latitude + "", 8);
            foreach (Transform item in IconEvent)
            {
                item.gameObject.SetActive(false);
            }
            this.IconEvent[(int)landEventData.EventType].gameObject.SetActive(true);
            gameObject.SetActive(true);
        }

        public void Onclick()
        {
            // HUDCanvas.Instance.ShowNotification("Comming soon!");
            // TeleportSystem.instance.OffAllFlagTeleport();
            ChatManager.Instance.EventChannelJoin = ChatChannelConfig.GetLandEvent(this.LandEventData.GetLandEventID());
            // GeoPointManager.Instance.EventType = this.LandEventData.EventType;
            TeleportSystem.instance.Teleport(new Coordinates(this.LandEventData.Latitude + UnityEngine.Random.Range(-0.002f, 0.002f), this.LandEventData.Longitude + UnityEngine.Random.Range(-0.002f, 0.002f)));
            try
            {
                LandEventUI landEventUI = (LandEventUI)UIManager.instance.GetPopupUIByCode(PopupCode.LandEventUI);
                landEventUI.OffUI();
            }
            catch (System.Exception)
            {
            }
        }
    }
}
