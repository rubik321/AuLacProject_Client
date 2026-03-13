using System.Collections;
using System.Collections.Generic;
using NTPackage.EventDispatcher;
using NTPackage.UI;
using Rubik.Myrk.Clan;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rubik._2DGPS.Chat
{
    using Rubik.Quest;
    public class ClanTab : TabUI
    {
        public bool IsInit = false;
        public ChatChannelUI ChatChannelUI;
        public Transform ClanAnnouce;
        public TextMeshProUGUI TextAnnouce; 
        
        public Transform Empty;


        public override void OnUI()
        {
            if(this.IsInit) return;
            this.IsInit = true;
            base.OnUI();
            this.SetData();
            EventListenerManager.instance.Register(EventCode.Clan_UpdateAnnounce, "ClanTab", this.OnUpdateAnnounce);
        }

        public override void OffUI()
        {
            base.OffUI();
            this.IsInit = false;
        }

        public override void SetData(object data = null)
        {
            base.SetData(data);
            string clanChannel = ChatService.Instance.GetClanChatChannel();
            if(clanChannel == null){
                this.Empty.gameObject.SetActive(true);
                this.ChatChannelUI.gameObject.SetActive(false);
                this.ChatChannelUI.UnSetData();
            }
            else{
                this.Empty.gameObject.SetActive(false);
                this.ChatChannelUI.SetData(clanChannel, this.OnSendChat);
                this.ChatChannelUI.gameObject.SetActive(true);

                string announce = ClanManager.Instance.GetClanAnnouce();
                if(announce == null || announce.Length == 0){
                    this.ClanAnnouce.gameObject.SetActive(false);
                }
                else{
                    this.ClanAnnouce.gameObject.SetActive(true);
                    this.TextAnnouce.text = announce;
                }
            }
        }

        public void OnUpdateAnnounce(object data){
            string announce = (string)data;
            if(announce!= null && announce.Length > 0){
                this.ClanAnnouce.gameObject.SetActive(true);
                this.TextAnnouce.text = announce;
            }else{
                this.ClanAnnouce.gameObject.SetActive(false);
            }
        }

        public void OnSendChat(){
            if(!QuestManager.Instance.IsDoneQuest(QuestIndex.ClanQuest_Chat)){
                StartCoroutine(QuestManager.Instance.IEClientDoQuest(QuestIndex.ClanQuest_Chat));
            }
        }
    }
}