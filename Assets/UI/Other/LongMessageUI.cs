using System;
using NTPackage.UI;
using TMPro;
using UnityEngine;

namespace Rubik.UI
{
    public class LongMessageUI : PopupUI
    {
        public TextMeshProUGUI GuideTitle;
        public TextMeshProUGUI GuideDesc; 
        public Action OnConfirm;
        public Action OnClose;
        public Action OnOutSide;
        public TextMeshProUGUI TextConfirm;

        public void SetData(string title, string content, string btnConfirmText = null, Action onConfirm = null, Action onClose = null, Action onOutSide = null){
            this.OnConfirm = onConfirm;
            this.OnClose = onClose;
            this.OnOutSide = onOutSide;
            this.GuideTitle.text = title;
            this.GuideDesc.text = content;
            this.OnConfirm = onConfirm;
            this.OnClose = onClose;
            this.OnOutSide = onOutSide;
            if(btnConfirmText != null){
                this.TextConfirm.text = btnConfirmText;
            }else{
                this.TextConfirm.text = Lean.Localization.LeanLocalization.GetTranslationText("confirm", "Confirm");
            }
        }

        public void OnClickConfirm(){
            this.OnConfirm?.Invoke();
            this.OffUI();
        }

        public void OnClickClose(){
            this.OnClose?.Invoke();
            this.OffUI();
        }

        public void OnClickOutSide(){
            if(this.OnOutSide != null){
                this.OnOutSide?.Invoke();
                this.OffUI();
            }
        }
    }
}