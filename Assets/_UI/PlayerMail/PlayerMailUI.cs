using NTPackage.Functions;
using NTPackage.UI;
using Rubik.Common.AudioHelper;
using Rubik.UserDataPlayer;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Rubik.PlayerMail
{
    public class PlayerMailAnim
    {
        public const string Anim_Open = "OnUI";
        public const string Anim_Close = "OffUI";
        public const string Anim_Select = "OnDetail";
        public const string Anim_HideDetail = "OffDetail";
    }


    public class PlayerMailUI : PopupUI
    {
        public NTButtonEffect BtnRecieveAll;
        public NTButtonEffect BtnDeleteAll;
        public PlayerMailItemUI PlayerMailItemUI;
        public List<PlayerMailItemUI> PlayerMailItemUIList;
        public Transform Holder;

        public Animator Anim;

        public Transform Empty;


        public string SelectedMail;
        public PlayerMailInfoUI PlayerMailInfoUI;

        public override void OnUI(object data = null, bool isDefaultSound = true)
        {
            base.OnUI(data, isDefaultSound);
            this.Anim.Play(PlayerMailAnim.Anim_Open);
        }

        public override void ScriptOffUI()
        {
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.Holder);
            this.SelectedMail = "";
            this.Anim.Play(PlayerMailAnim.Anim_Close);
            StartCoroutine(this.OffPlayerMailAnim());
        }

        public override void UpdateData(object data = null)
        {
            base.UpdateData(data);
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.Holder);
            this.PlayerMailItemUIList.Clear();
            List<PlayerMail> playerMails = PlayerMailManager.instance.PlayerMails.ToList();
            playerMails.Sort((a, b) => { return a.TimeSend > b.TimeSend ? -1 : 1; });
            this.Empty.gameObject.SetActive(true);
            foreach (PlayerMail item in playerMails)
            {
                if (item.IsDelete) continue;
                PlayerMailItemUI playerMailItemUI = ObjectPoolingManager.Instance.PullObjectFromPooling<PlayerMailItemUI>(ObjectPoolingConfig.PlayerMailItemUI);
                if (playerMailItemUI == null)
                {
                    playerMailItemUI = Instantiate(this.PlayerMailItemUI);
                    playerMailItemUI.transform.name = ObjectPoolingConfig.PlayerMailItemUI;
                }
                playerMailItemUI.transform.SetParent(this.Holder);
                playerMailItemUI.gameObject.SetActive(true);
                NTFunction.ResetPosition(playerMailItemUI.transform);
                playerMailItemUI.SetData(item, this);
                this.PlayerMailItemUIList.Add(playerMailItemUI);
                this.Empty.gameObject.SetActive(false);
            }

            this.BtnRecieveAll.SetActive(PlayerMailManager.instance.CanRecieveMail());
            this.BtnDeleteAll.SetActive(PlayerMailManager.instance.CanDeleteMail());
            this.PlayerMailInfoUI.UpdateData();
        }


        public void SelectMail(PlayerMailItemUI playerMailItemUI)
        {
            AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
            if (this.SelectedMail.Length == 0)
            {
                this.Anim.Play(PlayerMailAnim.Anim_Select);
            }
            if (this.SelectedMail == playerMailItemUI.PlayerMail._id)
            {
                this._OnHideDetail();
                return;
            }
            this.SelectedMail = playerMailItemUI.PlayerMail._id;
            this.PlayerMailInfoUI.SetData(playerMailItemUI);
            foreach (PlayerMailItemUI item in this.PlayerMailItemUIList)
            {
                item.UpdateData();
            }

            StartCoroutine(PlayerMailManager.instance.ReadMail(playerMailItemUI.PlayerMail._id, () =>
            {
                playerMailItemUI.UpdateData();
            }));
        }

        public void _OnClickRecieveAll()
        {
            if (UserDataManager.Instance.IsCapSlotInventoryBag())
            {
                UserDataManager.Instance.ShowNotificationCapSlotInventoryBag();
                return;
            }
            AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
            PlayerMailManager.instance.RecieveAllMail(() =>
            {
                AudioCtrl.Instance.Play(AudioName.Achievement_Sound);
                this.UpdateData();
            });
        }

        public void _OnClickDeleteAll()
        {
            AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
            PlayerMailManager.instance.DeleteAllMail(() =>
            {
                this.UpdateData();
                this._OnHideDetail();
            });
        }

        public void _OnHideDetail()
        {
            AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
            if (this.SelectedMail.Length > 0)
            {
                this.SelectedMail = "";
                this.Anim.Play(PlayerMailAnim.Anim_HideDetail);
                foreach (PlayerMailItemUI item in this.PlayerMailItemUIList)
                {
                    item.UpdateData();
                }
                this.PlayerMailInfoUI.OffUI();
            }
        }
    
        public IEnumerator OffPlayerMailAnim(){
            AnimationClip[] clips = this.Anim.runtimeAnimatorController.animationClips;
            foreach (AnimationClip clip in clips)
            {
                if (clip.name == PlayerMailAnim.Anim_Close)
                {
                    yield return new WaitForSeconds(clip.length);
                    break;
                }
            }
            base.ScriptOffUI();
        }
    }
}