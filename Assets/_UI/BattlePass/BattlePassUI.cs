using NTPackage.Functions;
using NTPackage.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Collections;
using NTPackage.EventDispatcher;
using Rubik.UI;

namespace Rubik.Myrk.BattlePass
{

    public class BattlePassAnim
    {
        public const string Anim_Open = "OnUI";
        public const string Anim_Close = "OffUI";
    }

    public class BattlePassUI : PopupUI
    {
        public TextMeshProUGUI TextCountTime;
        public Coroutine CorCountTime;

        public Transform PriceUnlockPremium;
        public TextMeshProUGUI TextPriceUnlockPremium;
        public Animator Anim;
        public long TimeRemaining;

        
        public List<BattlePassMilestoneItem> BattlePassMilestoneItemList;

        [NTButton]
        public void TestOnUI(){
            this.OnUI();
        }

        public override void OnUI(object data = null, bool isDefaultSound = true)
        {
            base.OnUI(data, isDefaultSound);
            EventListenerManager.instance.Register(EventCode.BattlePassUpdate, "BattlePassUI", this.UpdateData);
            this.Anim.Play(BattlePassAnim.Anim_Open);
        }


        public override void OffUI()
        {
            EventListenerManager.instance.RemoveListener(EventCode.BattlePassUpdate, "BattlePassUI");
            this.Anim.Play(BattlePassAnim.Anim_Close);
            StartCoroutine(this.OffBattlePassAnim());
        }

        public override void UpdateData(object data = null)
        {
            base.UpdateData(data);
            if(this.CorCountTime != null){
                StopCoroutine(this.CorCountTime);
            }
            this.CorCountTime = StartCoroutine(this.IECountTime());

            if(BattlePassManager.Instance.IsPremium()){
                this.PriceUnlockPremium.gameObject.SetActive(false);
            }else{
                this.PriceUnlockPremium.gameObject.SetActive(true);
                this.TextPriceUnlockPremium.text = BattlePassManager.Instance.GetPriceUnlockPremium();
            }

            BattlePassPath freePath = BattlePassManager.Instance.BattlePassData.Free;
            BattlePassPath premiumPath = BattlePassManager.Instance.BattlePassData.Premium;

            List<BattlePassMilestoneData> milestones = new List<BattlePassMilestoneData>();

            for (int i = 0; i < freePath.Reward.Length; i++)
            {
                BattlePassMilestoneData milestone = new BattlePassMilestoneData(freePath.Reward[i].Level, freePath.Reward[i].RewardItems[0], premiumPath.Reward[i].RewardItems[0], freePath.Reward[i].ExpRequire);
                milestones.Add(milestone);
            }

            for (int i = 0; i < this.BattlePassMilestoneItemList.Count; i++)
            {
                if(i < milestones.Count - 1){
                    this.BattlePassMilestoneItemList[i].SetData(milestones[i], milestones[i + 1]);
                }else{
                    this.BattlePassMilestoneItemList[i].SetData(milestones[i], null);
                }
            }
        }

        public IEnumerator IECountTime(){
            while(true){
                this.TimeRemaining = BattlePassManager.Instance.GetBattlePassTimeRemaining();
                this.TextCountTime.text = NTFunction.Format_Time(this.TimeRemaining, 2);
                yield return new WaitForSeconds(1);
                if(this.TimeRemaining <= 0){
                    this.TextCountTime.text = Lean.Localization.LeanLocalization.GetTranslationText("battle_pass_end_season", "Battle Pass End Season");
                    break;
                }
            }
        }

        public void OnclickUnlockPremium(){
            if(this.TimeRemaining <= 0){
                HUDCanvas.Instance.ShowNotification(Lean.Localization.LeanLocalization.GetTranslationText("battle_pass_end_season", "Battle Pass End Season"));
                return;
            }
            if(BattlePassManager.Instance.IsPremium()){
                return;
            }else{
                BattlePassManager.Instance.UnlockPremium(()=>{
                    this.UpdateData();
                });
            }
        }

        public IEnumerator OffBattlePassAnim(){
            AnimationClip[] clips = this.Anim.runtimeAnimatorController.animationClips;
            foreach (AnimationClip clip in clips)
            {
                if (clip.name == BattlePassAnim.Anim_Close)
                {
                    yield return new WaitForSeconds(clip.length);
                    break;
                }
            }
            base.OffUI();
        }

        public void OnclickFreeItem(int level, bool isAdv = false){
            if(this.TimeRemaining <= 0){
                HUDCanvas.Instance.ShowNotification(Lean.Localization.LeanLocalization.GetTranslationText("battle_pass_end_season", "Battle Pass End Season"));
                return;
            }
            if(isAdv){
                BattlePassManager.Instance.ClaimRewardAdv(level, BattlePassType.Free);
            }else{
                BattlePassManager.Instance.ClaimReward(level, BattlePassType.Free);
            }
        }

        public void OnclickPremiumItem(int level, bool isAdv = false){
            if(this.TimeRemaining <= 0){
                HUDCanvas.Instance.ShowNotification(Lean.Localization.LeanLocalization.GetTranslationText("battle_pass_end_season", "Battle Pass End Season"));
                return;
            }

            if(isAdv){
                BattlePassManager.Instance.ClaimRewardAdv(level, BattlePassType.Premium);
            }else{
                BattlePassManager.Instance.ClaimReward(level, BattlePassType.Premium);
            }
        }
    }
}
