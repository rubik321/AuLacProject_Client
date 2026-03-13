using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NTPackage.UI;
using System;

namespace Rubik.UI
{
    public class MessagePanel : PopupUI
    {
        public TextMeshProUGUI txtTitle;
        public TextMeshProUGUI txtContent;
        public TextMeshProUGUI txtBtnClose;

        public Action btnClose;
      
        public void SetData(string title, string content, string btnCloseText = null, Action btnCloseAction = null)
        {
            txtTitle.text = title;
            txtContent.text = content;
            if(btnCloseText != null){
                txtBtnClose.text = btnCloseText;
            }else{
                txtBtnClose.text = Lean.Localization.LeanLocalization.GetTranslationText("confirm", "Confirm");
            }
            this.btnClose = btnCloseAction;
           
        }
      
        public override void ScriptOffUI()
        {
            base.ScriptOffUI();
            this.txtTitle.text = "";
            this.txtContent.text = "";
            this.btnClose = null;
        }

        public void OnClickBtnClose()
        {
            this.btnClose?.Invoke();
            this.OffUI();
        }
    }
}