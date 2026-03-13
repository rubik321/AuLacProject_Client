using System;
using System.Collections;
using System.Collections.Generic;
using NTPackage.UI;
using TMPro;
using UnityEngine;

namespace Rubik.ItemPlayer
{
    public class ItemDataDetailUI : PopupUI
    {
        public ItemDataUI ItemDataUI;
        public TextMeshProUGUI Name;
        public TextMeshProUGUI Description;
        
        public ItemData ItemData;

        public Action ActionUse;
        public NTButtonEffect ButtonUse;

        public override void LoadComponents()
        {
            base.LoadComponents();
            this.popupCode = PopupCode.ItemDataDetailUI;
        }

        public override void OnUI(object data = null, bool isDefaultSound = true)
        {
            this.ItemData = (ItemData)data;
            this.ButtonUse.gameObject.SetActive(false);
            base.OnUI(data, isDefaultSound);
        }

        public override void UpdateData(object data = null)
        {
            base.UpdateData(data);
            this.ItemDataUI.SetData(this.ItemData);
            this.Name.text = ItemDataManager.Instance.GetItemName(this.ItemData.Type);
            this.Description.text = ItemDataManager.Instance.GetItemDescription(this.ItemData.Type);
        }

        public void SetInventoryAction(Action actionUse = null){
            if(ItemDataManager.Instance.GetItemDataInfo(this.ItemData.Type).IsUse){
                this.ButtonUse.gameObject.SetActive(true);
                this.ActionUse = actionUse;
            }
        }

        #region Action
        public void OnClickUse(){
            this.ActionUse?.Invoke();
            this.Hide();
        }
        #endregion
    }
}