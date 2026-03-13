using System;
using System.Collections;
using System.Collections.Generic;
using NTPackage.UI;
using UnityEngine;
using TMPro;

namespace Rubik.UI
{
    using Rubik.ItemPlayer;
    using UnityEngine.UI;

    public class MessageOptionPanel : PopupUI
    {
        public TextMeshProUGUI TextContent;
        public TextMeshProUGUI TextTitle;

        public ItemDataBarUI ItemDataBarUI;

        public Action ActionReject;
        public Action ActionConfirm;

        public NTButtonEffect ButtonReject;
        public NTButtonEffect ButtonConfirm;

        public TextMeshProUGUI TextReject;
        public TextMeshProUGUI TextConfirm;
        public TextMeshProUGUI TextTime;
        public bool IsBlockClickScreenDim = false;

        public override void OnUI(object data = null, bool isDefaultSound = true)
        {
            base.OnUI(data, isDefaultSound);
            this.ItemDataBarUI.gameObject.SetActive(false);
            this.ButtonReject.SetActive(false);
            this.ButtonConfirm.SetActive(false);
            TextTime.text = "";
            this.IsBlockClickScreenDim = false;
        }

        public void SetData(string title, string content){
            this.TextTitle.text = title;
            this.TextContent.text = content;
        }

        public void SetItemData(ItemData itemData){
            this.ItemDataBarUI.gameObject.SetActive(true);
            this.ItemDataBarUI.SetData(itemData);
        }

        public void SetActionReject(Action action, string text){
            this.ButtonReject.SetActive(true);
            this.ButtonReject.Onclick.RemoveAllListeners();
            this.ButtonReject.Onclick.AddListener(() => {
                this.ActionReject?.Invoke();
                this.OffUI();
            });
            this.TextReject.text = text;
            this.ActionReject = action;
        }

        public void SetActionConfirm(Action action, string text){
            this.ButtonConfirm.SetActive(true);
            this.ButtonConfirm.Onclick.RemoveAllListeners();
            this.ButtonConfirm.Onclick.AddListener(() => {
                this.ActionConfirm?.Invoke();
                this.OffUI();
            });
            this.TextConfirm.text = text;
            this.ActionConfirm = action;
        }
        public void SetTimeConfirm()
        {
            StopAllCoroutines();
            StartCoroutine(StartCount());
        }
        IEnumerator StartCount()
        {
            int time = 5;
            this.ButtonConfirm.SetActive(false);
            while (time >= 0) {
                TextTime.text = time.ToString();
                yield return new WaitForSeconds(1);
                time--;
            }
            TextTime.text = "";
            this.ButtonConfirm.SetActive(true);
        }

        public void _OnClickScreenDim(){
            if(this.IsBlockClickScreenDim){
                return;
            }
            this.OffUI();
        }
    }
}
