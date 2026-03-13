using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NTPackage.Functions;
using NTPackage.UI;
using Rubik.UserDataPlayer;
using Rubik.UserProfile;
using UnityEngine;

namespace Rubik.Myrk.Clan
{
    public class ClanBossRankAnim
    {
        public const string OnUI = "OnUI";
        public const string OffUI = "OffUI";
    }

    public class ClanBossRankUI : PopupUI
    {
        public ClanBossRankItem ClanBossRankItemPrefab;
        public List<ClanBossRankItem> ClanBossRankItems;
        public Transform Content;
        public ClanBossRankItem PlayerRankItem;
        public Transform Empty;
        public Transform Rank;

        public Animator Animator;

        public override void OnUI(object data = null, bool isDefaultSound = true)
        {
            base.OnUI(data, isDefaultSound);
            this.Animator.Play(ClanBossRankAnim.OnUI);
        }

        public override void ScriptOffUI()
        {
            if (this.ScreenDim != null) this.ScreenDim.gameObject.SetActive(false);
            this.Animator.Play(ClanBossRankAnim.OffUI);
            StartCoroutine(this.OffClanBossRankAnim());
        }

        public override void UpdateData(object data = null)
        {
            base.UpdateData(data);
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.Content);
            this.ClanBossRankItems.Clear();
            this.Empty.gameObject.SetActive(false);
            this.Rank.gameObject.SetActive(false);
            StartCoroutine(ClanManager.Instance.IEGetClanBossRank(() =>
            {
                List<UserRank> clanRanks = new List<UserRank>();
                if(ClanManager.Instance.ClanBossRank != null && ClanManager.Instance.ClanBossRank.UserRanks != null && ClanManager.Instance.ClanBossRank.UserRanks.Length > 0){
                    clanRanks = ClanManager.Instance.ClanBossRank.UserRanks.ToList();
                }
                List<string> userIDs = new List<string>();
                foreach (UserRank clanRank in clanRanks)
                {
                    userIDs.Add(clanRank.UserID);
                }
                if(userIDs.Count == 0){
                    this.Empty.gameObject.SetActive(true);
                }else{
                    this.Rank.gameObject.SetActive(true);
                }
                UserProfileManager.Instance.GetUserDataShorts(userIDs, (userProfiles) =>
                {
                    foreach (UserRank clanRank in clanRanks)
                    {
                        UserDataShort userDataShort = UserProfileManager.Instance.GetUserDataShortCache(clanRank.UserID);
                        if (userDataShort == null) continue;
                        ClanBossRankItem clanBossRankItem = ObjectPoolingManager.Instance.InstantiateObject<ClanBossRankItem>(ObjectPoolingConfig.ClanBossRankItem, this.ClanBossRankItemPrefab.transform);
                        clanBossRankItem.SetData(userDataShort.DisplayName, userDataShort.Level, userDataShort.Avatar, userDataShort.AvatarBorder, clanRank.Rank, clanRank.Score);
                        this.ClanBossRankItems.Add(clanBossRankItem);
                        clanBossRankItem.transform.SetParent(this.Content);
                        NTFunction.ResetPosition(clanBossRankItem.transform);
                    }
                });
                this.PlayerRankItem.SetPlayerRank(ClanManager.Instance.ClanBossRank.Rank, ClanManager.Instance.ClanBossRank.Score);
            }));
        }

        public IEnumerator OffClanBossRankAnim()
        {
            AnimationClip[] clips = this.Animator.runtimeAnimatorController.animationClips;
            foreach (AnimationClip clip in clips)
            {
                if (clip.name == ClanBossRankAnim.OffUI)
                {
                    yield return new WaitForSeconds(clip.length);
                    break;
                }
            }
            base.ScriptOffUI();
        }
    }

}
