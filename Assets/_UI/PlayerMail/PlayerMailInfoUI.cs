using NTPackage.Functions;
using NTPackage.UI;
using Rubik.UI;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rubik.PlayerMail
{
    using ItemPlayer;
    using Rubik.Common.AudioHelper;

    public class PlayerMailInfoUI : NTBehaviour, IPointerClickHandler
    {
        public TextMeshProUGUI TextTitle;
        public TextMeshProUGUI TextDetail;
        public PlayerMail PlayerMail;
        public PlayerMailItemUI PlayerMailItemUI;
        public PlayerMailAttachItemUI PlayerMailAttachItemUIPrefab;
        public Transform AttachContent;
        public Transform BtnClaim;

        public Canvas Canvas;

        public RectTransform TranInner;

        public PlayerMailUI PlayerMailUI;

        public void OffUI()
        {
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.AttachContent);
        }

        public void SetData(PlayerMailItemUI playerMailItemUI)
        {
            if(this.Canvas == null){
                this.Canvas = PopupManager.Instance.transform.GetComponent<Canvas>();
            }
            this.PlayerMailItemUI = playerMailItemUI;
            this.PlayerMail = this.PlayerMailItemUI.PlayerMail;
            this.TranInner.anchoredPosition = new Vector2(0, 0);
            this.UpdateData();
        }

        public void UpdateData()
        {
            if(this.PlayerMail == null || string.IsNullOrEmpty(this.PlayerMail._id)) return;
            this.TextTitle.text = Lean.Localization.LeanLocalization.GetTranslationText(this.PlayerMail.Title, PlayerMail.Title);
            this.TextDetail.text = string.Format(Lean.Localization.LeanLocalization.GetTranslationText(this.PlayerMail.Detail, PlayerMail.Detail), this.PlayerMail.InjectString.ToArray());
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.AttachContent);
            foreach (ItemData item in this.PlayerMail.Attached.Items)
            {
                PlayerMailAttachItemUI playerMailAttachItemUI = ObjectPoolingManager.Instance.InstantiateObject<PlayerMailAttachItemUI>(ObjectPoolingConfig.PlayerMailAttachItemUI, this.PlayerMailAttachItemUIPrefab.transform);
                playerMailAttachItemUI.SetData(item);
                playerMailAttachItemUI.transform.SetParent(this.AttachContent);
                NTFunction.ResetPosition(playerMailAttachItemUI.transform);
            }
            this.BtnClaim.gameObject.SetActive(!this.PlayerMail.IsRecieve);
        }

        public void Claim()
        {
            string[] ids = new string[] { this.PlayerMail._id };
            StartCoroutine(PlayerMailManager.instance.RecieveMail(ids, () =>
            {
                AudioCtrl.Instance.Play(AudioName.Achievement_Sound);
                this.PlayerMailItemUI.UpdateData();
                this.PlayerMailUI.UpdateData();
                this.UpdateData();
            }));

        }

        public void Delete()
        {
            if (!this.PlayerMail.IsRecieve) return;
            string[] ids = new string[] { this.PlayerMail._id };
            StartCoroutine(PlayerMailManager.instance.DeleteMail(ids, () =>
            {
                this.PlayerMailItemUI.UpdateData();
                this.PlayerMailUI._OnHideDetail();
            }));
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(this.Canvas == null) return;
            Vector3 mousePosition = new Vector3(eventData.position.x, eventData.position.y, 0);
            NTLog.LogMessage(mousePosition.ToString());
            var linkTaggedText = TMP_TextUtilities.FindIntersectingLink(this.TextDetail, mousePosition, Canvas.worldCamera);
            if (linkTaggedText == -1)
            {
                NTLog.LogMessage("not_found_link");
                return;
            }
            TMP_LinkInfo linkInfo = this.TextDetail.textInfo.linkInfo[linkTaggedText];
            Application.OpenURL(linkInfo.GetLinkID());

        }
    }
}