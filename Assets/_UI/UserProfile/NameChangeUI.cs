using System;
using System.Collections;
using System.Collections.Generic;
using NTPackage.Functions;
using NTPackage.UI;
using Rubik.ItemPlayer;
using Rubik.UI;
using TMPro;
using UnityEngine;

namespace Rubik.UserProfile
{
    public class NameChangeUI : PopupUI
    {
        public TMP_InputField InputFieldName;
        public TextMeshProUGUI TextFree;
        public ItemDataBarUI Price;

        public bool IsNotEnoughItem = false;

        public override void LoadComponents()
        {
            base.LoadComponents();
            this.popupCode = PopupCode.NameChangeUI;
        }

        public override void OnUI(object data = null, bool isDefaultSound = true)
        {
            base.OnUI(data, isDefaultSound);
            this.InputFieldName.text = "";
            this.IsNotEnoughItem = false;
            this.Show();
            if(UserProfileManager.Instance.IsFirstChangeName()){
                this.TextFree.gameObject.SetActive(true);
                this.Price.gameObject.SetActive(false);
            }else{
                this.TextFree.gameObject.SetActive(false);
                this.Price.gameObject.SetActive(true);
                this.Price.SetData(UserProfileManager.Instance.GetPriceChangeName());
                if(ItemDataManager.Instance.GetItem(UserProfileManager.Instance.GetPriceChangeName().Type).Amount < UserProfileManager.Instance.GetPriceChangeName().Amount){
                    this.Price.Amount.color = Color.red;
                    this.IsNotEnoughItem = true;
                }else{
                    this.Price.Amount.color = NTFunction.StringHexToColor("705748");
                    this.IsNotEnoughItem = false;
                }
            }
        }

        public void OnChangeName(){
            if (this.InputFieldName.text == ""){
                return;
            }
            if(this.InputFieldName.text.Length > 10){
                HUDCanvas.Instance.ShowNotification(Lean.Localization.LeanLocalization.GetTranslationText("name_too_long", "Name must be less than 10 characters"));
                return;
            }
            if(this.InputFieldName.text.Length < 3){
                HUDCanvas.Instance.ShowNotification(Lean.Localization.LeanLocalization.GetTranslationText("name_too_short", "Name must be at least 3 characters"));
                return;
            }
            if(this.IsNotEnoughItem){
                ItemDataManager.Instance.ShowDontEnoughItem(UserProfileManager.Instance.GetPriceChangeName().Type);
                return;
            }
            UserProfileManager.Instance.ChangeDisplayName(this.InputFieldName.text, () => {
                this.Hide();
            });
        }
    }
}
