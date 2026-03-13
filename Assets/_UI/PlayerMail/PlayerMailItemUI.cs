using NTPackage.EventDispatcher;
using NTPackage.Functions;
using NTPackage.UI;
using Rubik.Combat;
using Rubik.Common.AudioHelper;
using Rubik.UserDataPlayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Rubik.PlayerMail
{
    public class PlayerMailItemUI : NTButtonEffect
    {
        public TextMeshProUGUI TextTitle;
        public TextMeshProUGUI TextDetail;
        public TextMeshProUGUI TextTime;
        public Transform BtnDelete;
        public Transform BtnClaim;
        public PlayerMail PlayerMail;

        public Transform UnreadMail;
        public Transform ReadMail;
        public Transform SelectedMail;

        public PlayerMailUI PlayerMailUI;

        public void SetData(PlayerMail playerMail, PlayerMailUI playerMailUI)
        {
            this.PlayerMailUI = playerMailUI;
            this.PlayerMail = playerMail;
            EventListenerManager.instance.Register(EventCode.BattleCard_UpdateMail, this.PlayerMail._id, (data) =>
            {
                this.UpdateData();
            });
            this.TextTitle.text = Lean.Localization.LeanLocalization.GetTranslationText(this.PlayerMail.Title, PlayerMail.Title);
            string detail = String.Format(Lean.Localization.LeanLocalization.GetTranslationText(this.PlayerMail.Detail, PlayerMail.Detail), this.PlayerMail.InjectString.ToArray());
            this.TextDetail.text = NTFunction.CollapString(detail, 64);
            this.TextTime.text = NTFunction.UnixTimestampToDateTime((long)this.PlayerMail.TimeSend).ToLocalTime().ToString();
            this.UpdateData();
        }

        public void UpdateData()
        {
            this.PlayerMail = PlayerMailManager.instance.GetPlayerMail(this.PlayerMail._id);
            if(this.PlayerMail == null || this.PlayerMail.IsDelete){
                gameObject.SetActive(false);
                return;
            }
            this.BtnClaim.gameObject.SetActive(false);
            this.BtnDelete.gameObject.SetActive(false);
            if(this.PlayerMail.Attached.IsEmpty()){
                this.PlayerMail.IsRecieve = true;
            }
            if (this.PlayerMail.IsRecieve)
            {
                this.BtnClaim.gameObject.SetActive(false);
            }
            else
            {
                this.BtnClaim.gameObject.SetActive(true);
            }
            if (this.PlayerMail.IsRead)
            {
                this.UnreadMail.gameObject.SetActive(false);
                this.ReadMail.gameObject.SetActive(true);
                this.BtnDelete.gameObject.SetActive(true);
            }
            else
            {
                this.UnreadMail.gameObject.SetActive(true);
                this.ReadMail.gameObject.SetActive(false);
                this.BtnDelete.gameObject.SetActive(false);
            }
            if(this.PlayerMail.IsDelete){
                gameObject.SetActive(false);
            }
            if(this.PlayerMailUI.SelectedMail == this.PlayerMail._id){
                this.SelectedMail.gameObject.SetActive(true);
            }
            else
            {
                this.SelectedMail.gameObject.SetActive(false);
            }
        }

        public void Claim()
        {
            string[] ids = new string[]{this.PlayerMail._id};
            bool isMailClick = false;
            if (UserDataManager.Instance.IsCapSlotInventoryBag())
            {
                UserDataManager.Instance.ShowNotificationCapSlotInventoryBag();
                return;
            }
            StartCoroutine(PlayerMailManager.instance.RecieveMail(ids, () =>
            {
                AudioCtrl.Instance.Play(AudioName.Achievement_Sound);
                this.UpdateData();
                this.PlayerMailUI.UpdateData();
                isMailClick = true;



            }));
            DG.Tweening.DOVirtual.DelayedCall(1.2f, () =>
            {
                if (AssetLoader.Instance.IsTut&& !isMailClick)
                {
                    PopupManager.Instance.GetPopupUI(PopupCode.PlayerMailUI).OffUI();
                    //PopupManager.Instance.OnUI(PopupCode.TutorialUI);
                    AssetLoader.Instance.IsTut = false;
                    var temp = PopupManager.Instance.GetPopupUI(PopupCode.TutorialUI).GetComponent<TutorialUI>();
                    if (temp.isCanNextTut)
                        PopupManager.Instance.OnUI(PopupCode.TutorialUI);

                }
            });
        }

        public void Delete()
        {
            if (!this.PlayerMail.IsRecieve) return;
            string[] ids = new string[]{this.PlayerMail._id};
            StartCoroutine(PlayerMailManager.instance.DeleteMail(ids, () =>
            {
                this.UpdateData();
                this.PlayerMailUI.UpdateData();
            }));
        }

        public void ShowDetail()
        {
            this.PlayerMailUI.SelectMail(this);
        }
    }
}