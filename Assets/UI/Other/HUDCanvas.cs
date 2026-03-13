using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Localization;
using NTPackage.UI;
using UnityEngine;
namespace Rubik.UI
{
    public class HUDCanvas : MonoBehaviour
    {
        public static HUDCanvas Instance; 
        [SerializeField] LoadingPanel loadingPanel;
        [SerializeField] MessagePanel messagePanel;
        [SerializeField] Dungeon dungeon;
        // Start is called before the first frame update
        void Awake()
        {
            Instance = this;
            
        }
        public void ShowLoadingPanel()
        {
            loadingPanel.OnUI(null, false);
        }
        public void HideLoadingPanel()
        {
            loadingPanel.OffUIWithoutSound();
        }
        public void ShowNotification(string str, string title = null, string btnCloseText = null, Action btnCloseAction = null)
        {
            if (title == null)
            {
                title = LeanLocalization.GetTranslationText("message", "Message");
            }
            // Rubik.Common.AudioHelper.AudioCtrl.instance.Play(Rubik.Common.AudioHelper.AudioName.UI_Popup_Panel_Error);
            messagePanel.SetData(title, str, btnCloseText, btnCloseAction);
            messagePanel.OnUI();
            messagePanel.transform.SetAsLastSibling();
        }
       
        public void HideNotification()
        {
            // messagePanel.HideMessage();
        }
        public void ShowReward(List<(string,int)> lsReward)
        {
            // Rubik.Common.AudioHelper.AudioCtrl.instance.Play(Rubik.Common.AudioHelper.AudioName.UI_Popup_Panel_Error);
            // messagePanel.ShowReward(lsReward);
        }
        public void ShowDungeon()
        {
            dungeon.ShowDungeon();
        }
        public void HideDungeon()
        {
            dungeon.HideDungeon();
        }

        public void ShowToolTip(string title, string description)
        {
            PopupManager.Instance.OnUI(PopupCode.ToolTipUI, null, (popup) =>
            {
                ToolTipUI toolTipUI = popup as ToolTipUI;
                toolTipUI.SetData(title, description);
            });
        }
    }

}
