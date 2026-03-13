using System.Collections;
using System.Collections.Generic;
using NTPackage.UI;
using Rubik.BattleEngine;
using Rubik.Myrk.Monster;
using Rubik.UserDataPlayer;
using Rubik.UserProfile;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rubik.Myrk.Battle
{
    using DG.Tweening;
    using Rubik.CardPlayer;
    using Rubik.ItemPlayer;

    public class BattleLoadingAnim
    {
        public const string OnUI = "BLOnUI";
        public const string OffUI = "BLOffUI";
    }

    public class BattleLoadingUI : PopupUI
    {
        public List<Sprite> ListBattleType;
        public Image ImageBattleType;
        public Animator Anim;

        public AvatarPlayerUI AvatarPlayerUI;
        public TextMeshProUGUI PlayerName;

        public AvatarPlayerUI AvatarEnemyUI;
        public TextMeshProUGUI EnemyName;

        public ItemDataUI AvatarMonsterUI;
        public override void OnUI(object data = null, bool isDefaultSound = true)
        {
            base.OnUI(data, isDefaultSound);
            this.Anim.Play(BattleLoadingAnim.OnUI);
        }

        public override void ScriptOffUI()
        {
            this.Anim.Play(BattleLoadingAnim.OffUI);
            StartCoroutine(this.OffAnim());
            this.ImageBattleType.transform.localScale = Vector3.one;
            this.ImageBattleType.transform.DOScale(1.2f,1f).SetLoops(-1,LoopType.Yoyo);
        }

        public void SetData(BattleType battleType, UserDataShort enemy, MonsterData monster)
        {
            this.ImageBattleType.sprite = this.ListBattleType[(int)battleType % this.ListBattleType.Count];
            this.PlayerName.text = UserDataManager.Instance.GetDisplayName();
            this.AvatarPlayerUI.SetData(UserProfileManager.Instance.GetAvatarUsedIndex(), UserProfileManager.Instance.GetAvatarBorderUsedIndex(), UserDataManager.Instance.GetDisplayName(), UserDataManager.Instance.GetPlayerLevel().level);
            this.AvatarEnemyUI.gameObject.SetActive(false);
            this.AvatarMonsterUI.gameObject.SetActive(false);
            if (enemy != null)
            {
                this.AvatarEnemyUI.SetData(enemy.Avatar, enemy.AvatarBorder, enemy.DisplayName, enemy.Level);
                this.EnemyName.text = enemy.DisplayName;
                this.AvatarEnemyUI.gameObject.SetActive(true);
            }
            if (monster != null)
            {
                CardPlayer cardPlayer = CardPlayerManager.Instance.GetFakeCardPlayerByIndex(monster.Index);
                cardPlayer.Lv = monster.Lv;
                cardPlayer.Star = monster.Star;
                this.AvatarMonsterUI.SetData(cardPlayer, false);
                this.AvatarMonsterUI.gameObject.SetActive(true);
            }
        }

        public IEnumerator OffAnim()
        {
            AnimationClip[] clips = this.Anim.runtimeAnimatorController.animationClips;
            foreach (AnimationClip clip in clips)
            {
                if (clip.name == BattleLoadingAnim.OffUI)
                {
                    yield return new WaitForSeconds(clip.length);
                    break;
                }
            }
            this.ImageBattleType.transform.DOKill();
            this.ImageBattleType.transform.localScale = Vector3.one;
            base.ScriptOffUI();
        }
    }
}