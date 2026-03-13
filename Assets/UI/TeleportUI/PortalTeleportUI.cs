using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NTFunctions_old;
using TMPro;
using GoShared;
using Rubik.Chat;
using SimpleJSON;
using Rubik.UI;
using Rubik.Combat;
using GOA.WorldMap.PortalGlobal;

namespace GOA.WorldMap
{

    public class PortalTeleportUI : PopupUI
    {
        public TextMeshProUGUI TextDetail;

        public void OnUI()
        {
            this.ShowUI();
        }

        public void ShowUI()
        {
            // this.TextDetail.text = string.Format(Lean.Localization.LeanLocalization.GetTranslationText("portal_detail", "A Portal has appeared. \n Defend against Chthonians Invasion."), GeoPointManager.Instance.GlobalPortalData.Name);
            // this.Show();
        }

        public void OnclickTeleport()
        {
            // TeleportSystem.instance.OffAllFlagTeleport();
            ChatManager.Instance.EventChannelJoin = ChatChannelConfig.GetGlobalPortal();
            // GeoPointManager.Instance.EventType = LandEvent.EventType.None;
            // Rubik.Common.AudioHelper.AudioCtrl.Instance.Play(Rubik.Common.AudioHelper.AudioName.UI_Button_Confirm);
            // TeleportSystem.instance.Teleport(new Coordinates(
            //     GeoPointManager.Instance.GlobalPortalData.latitude - Random.Range(-0.002f, 0.002f),
            //     GeoPointManager.Instance.GlobalPortalData.longitude - Random.Range(-0.002f, 0.002f)
            // ));
            this.OffUI();
        }
    }
}
