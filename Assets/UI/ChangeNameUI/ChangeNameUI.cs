using System.Collections;
using System.Collections.Generic;
using NTFunctions_old;
using UnityEngine;
using TMPro;

namespace GOA.ChangeName
{

    public class ChangeNameUI : PopupUI
    {
        public TMP_InputField InputField;

        public void OnUI(){
            this.InputField.text = "";
            this.Show();
        }

        public void Confirm(){
            if(this.InputField.text.Length == 0) return;
            APIManager.Instance.ChangeDisplayName(this.InputField.text, Success);
        }

        public void Success(){
            UserData.UserData.Instance.Inventory.AddInventoryByCode(Item.ItemCode.NameTag, -1);
            UserData.UserData.Instance.data.DisplayName = this.InputField.text;
            this.Hide();
            CharacterUIController.Instance.ChangeBottomField();
        }
    }
}
