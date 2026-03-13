using System.Collections;
using System.Collections.Generic;
using NTPackage.UI;
using Rubik.Myrk.Clan;
using UnityEngine;

namespace Rubik._2DGPS.Chat
{
    public class ChatUIAnim{
        public static string OnUI = "OnUI";
        public static string OffUI = "OffUI";
    }

    public class ChatUI : PopupUI
    {
        public MultiTabUI MultiTabUI;

        public Animator Animator;

        public int TabIndex = 0;

        public override void LoadComponents()
        {
            base.LoadComponents();
            this.popupCode = PopupCode.ChatUI;
        }

        public override void OnUI(object data = null, bool isDefaultSound = true)
        {
            this.MultiTabUI.tabUIs.ForEach(tab =>
            {
                tab.OffUI();
            });
            base.OnUI(data, isDefaultSound);
            this.Animator.Play(ChatUIAnim.OnUI);
            StartCoroutine(this.LoadChat());
            this.TabIndex = data == null ? 0 : (int)data;
        }

        public override void OffUI()
        {
            this.MultiTabUI.tabUIs.ForEach(tab =>
            {
                tab.OffUI();
            });
            this.Animator.Play(ChatUIAnim.OffUI);
            this.MultiTabUI.OffAllTab();
            StartCoroutine(this.OffChatAnim());
        }

        public void OnFriendChat(string partnerID){
            // TODO: Open Chat with partnerID
        }

        public IEnumerator LoadChat(){
            if(this.TabIndex == 0 && !ChatService.Instance.IsRegisterChannel(ChatService.Instance.GetServerChatChannel())){
                ChatService.Instance.RegisterChannel(ChatService.Instance.GetServerChatChannel());
            }
            if(this.TabIndex == 1 && ClanManager.Instance.IsClan() && !ChatService.Instance.IsRegisterChannel(ChatService.Instance.GetClanChatChannel())){
                ChatService.Instance.RegisterChannel(ChatService.Instance.GetClanChatChannel());
            }
            AnimationClip[] clips = this.Animator.runtimeAnimatorController.animationClips;
            foreach (AnimationClip clip in clips)
            {
                if (clip.name == ChatUIAnim.OnUI)
                {
                    yield return new WaitForSeconds(clip.length);
                    break;
                }
            }
            this.MultiTabUI.OnUI(this.TabIndex);
        }

        public IEnumerator OffChatAnim(){
            AnimationClip[] clips = this.Animator.runtimeAnimatorController.animationClips;
            foreach (AnimationClip clip in clips)
            {
                if (clip.name == ChatUIAnim.OffUI)
                {
                    yield return new WaitForSeconds(clip.length);
                    break;
                }
            }
            base.OffUI();
        }
    }
}