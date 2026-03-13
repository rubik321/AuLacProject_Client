using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NTFunctions_old;
using Rubik.UI;
using SimpleJSON;

namespace GOA.Portal{
    using Config;

    public class NoticUseKeyOpenPortalUI : PopupUI
    {
        public Portal Portal;
        public void OnUI(Portal portal){
            if(!this.CanShow()) return;
            this.Portal = portal;
            this.Show();
        }

        public void Yes(){
            if(UserData.UserData.Instance.Inventory.PortalKey < 1){
                HUDCanvas.Instance.ShowNotification(Lean.Localization.LeanLocalization.GetTranslationText("not_enough_portal_keys", "Not Enough Portal Keys"));
                return;
            }
            UserData.UserData.Instance.Inventory.AddInventoryByID("I550001", -1);
            JSONNode data = new JSONObject();
            data["portalID"] = this.Portal.PortalData.PointID;
            data["userID"] = UserData.UserData.Instance.data.UserId;
            JSONNode dataUser = new JSONObject();
            dataUser["Name"] = UserData.UserData.Instance.data.UserName;
            dataUser["Avatar"] = 0;
            data["data"] = dataUser.ToString();
            StartCoroutine(APIManager.Instance.PostDataUrl(data.ToString(), SeverConfigs.BASE_API_URL + SeverConfigs.OpenPortalAPI, callback=>{
                PortalUnlookUI portalUnlookUI = (PortalUnlookUI) UIManager.instance.GetPopupUIByCode(PopupCode.PortalUnlookUI);
                if(portalUnlookUI != null) portalUnlookUI.OnUI(this.Portal);
                this.OffUI();
            }));
            // UserData.UserData.Instance.Inventory.PortalKey -= 1;
            // Debug.LogWarning(UserData.UserData.Instance.Inventory.PortalKey);
            // PlayerPrefs.SetInt("InventoryKey", PlayerPrefs.GetInt("InventoryKey") - 1);
        }

        public void No(){
            this.OffUI();
        }
    }
}
