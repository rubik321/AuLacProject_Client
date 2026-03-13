using GOA.Item;
using NTPackage.Functions;
using NTPackage.UI;
using Rubik.CharacterGear;
using Rubik.Common.AudioHelper;
using Rubik.ItemPlayer;
using Rubik.RewardData;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ComfirmUI : PopupUI
{

    public TextMeshProUGUI Title;

    public BagInventoryItemUI RewardDataItem;
    public ItemDataUI itemData;
     Action yesAction = null;
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.popupCode = PopupCode.ConfirmUI;
    }
    public void SetAction(Action action = null)
    {
        if (action != null)
        {
           
            yesAction = action;
        }
    }
    public override void OnUI(object data = null, bool isDefaultSound = true)
    {
        base.OnUI(data, isDefaultSound);
        if (data == null)
            return;
        CharacterGear rewardData = (CharacterGear)data;
        //Title.text = rewardData.Title;
       // BagInventoryItemUI rewardDataItemUI = ObjectPoolingManager.Instance.PullObjectFromPooling<BagInventoryItemUI>(ObjectPoolingConfig.RewardDataItemUI);
        RewardDataItem.gameObject.SetActive(true);
        itemData.gameObject.SetActive(false);
        RewardDataItem.SetData(rewardData);
    }
    public void SetItem(Rubik.ItemPlayer.ItemData data)
    {
        itemData.gameObject.SetActive(true);
        RewardDataItem.gameObject.SetActive(false);
        itemData.SetData(data);
    }
    public void OnYessButton()
    {
        AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
        if (yesAction != null)
        {

            yesAction.Invoke();
        }
    }
    public override void OffUI()
    {
        base.OffUI();

    }
}
