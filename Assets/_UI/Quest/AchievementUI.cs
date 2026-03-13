using GOA.UserData;
using NTPackage.Functions;
using NTPackage.UI;
using Pixelplacement;
using Rubik.Manager;
using Rubik.Myrk.BattlePass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rubik.Quest
{
    public class AchievementUI : PopupUI
    {
        public ItemAchimentUI ItemAchimentPrefab;
        public Transform AchievementHolder;
        
        public List<AchievementPlayer> Achiments;
        public Animator Anim;
        public override void OnUI(object data = null, bool isDefaultSound = true)
        {
            base.OnUI(data, isDefaultSound);
            this.Anim.Play(BattlePassAnim.Anim_Open);
        }
        public override void ScriptOffUI()
        {
            //base.OffUI();
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.AchievementHolder);
            this.Anim.Play(BattlePassAnim.Anim_Close);
            StartCoroutine(this.OffBattlePassAnim());
        }

        public override void UpdateData(object data = null)
        {
            base.UpdateData(data);
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.AchievementHolder);
            Achiments = AchievementManager.Instance.GetAchievement();
            Achiments.Sort((a, b) =>
            {
                return AchievementManager.Instance.CompareTo(a, b);
            });
            foreach (AchievementPlayer achiment in Achiments)
            {
                ItemAchimentUI itemAchiment = ObjectPoolingManager.Instance.PullObjectFromPooling<ItemAchimentUI>(ObjectPoolingConfig.ItemAchimentUI);
                if (itemAchiment == null)
                {
                    itemAchiment = Instantiate(this.ItemAchimentPrefab, this.AchievementHolder);
                }
                itemAchiment.SetUp(achiment.Index);
                itemAchiment.transform.SetParent(this.AchievementHolder);
                itemAchiment.transform.name = ObjectPoolingConfig.ItemAchimentUI;
                NTFunction.ResetPosition(itemAchiment.transform);
            }
        }
        public IEnumerator OffBattlePassAnim()
        {
            AnimationClip[] clips = this.Anim.runtimeAnimatorController.animationClips;
            foreach (AnimationClip clip in clips)
            {
                if (clip.name == BattlePassAnim.Anim_Close)
                {
                    yield return new WaitForSeconds(clip.length);
                    break;
                }
            }
            base.ScriptOffUI();
        }
    }
}
