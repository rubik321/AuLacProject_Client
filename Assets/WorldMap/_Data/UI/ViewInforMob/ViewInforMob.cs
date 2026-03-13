using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GOA.WorldMap;
using GOA.Config;
using NTFunctions_old;
using UnityEngine.UI;
using GOA.UserData;
using NTPackage_old.EventDispatcher;
using GOA.Building;
using Spine.Unity;
namespace GOA.WorldMap
{
    public class ViewInforMob : PopupUI
    {
        public Mob mob;
        public TextMeshProUGUI textTypeMob;
        public TextMeshProUGUI textNameMob;
        public TextMeshProUGUI textLvMob;
        public TextMeshProUGUI textNumber;
        public Image avatar;
        public SkeletonGraphic skeAvatar;
        public override void LoadComponents()
        {
            base.LoadComponents();
            this.lvUI = new PopupLv().GetValue(transform.name);
            this.LoadTextNamePortal();
            this.LoadTextLvMob();
            this.LoadAvatar();
        }

        protected void LoadTextNamePortal(){
            if(textNameMob != null) return;
            this.textNameMob = transform.Find("Panel").Find("TextNamePortal(TMP)").GetComponent<TextMeshProUGUI>();
        }

        protected void LoadTextLvMob(){
            if(textLvMob != null) return;
            this.textLvMob = transform.Find("Panel").Find("PopUp").Find("TextLvMob(TMP)").GetComponent<TextMeshProUGUI>();
        }

        protected void LoadAvatar(){
            if(avatar != null) return;
            this.avatar = transform.Find("Panel").Find("PopUp").Find("AvatarMask").Find("Avatar").GetComponent<Image>();
        }

        public void OnUI(Mob mob){
            if(!this.CanShow()) return;
            this.mob = mob;
            this.GenerateMob();
            this.textNumber.text = "x" + UserData.UserData.Instance.DataInCombat.mobsInCombat.Count;
            this.Show();
            EventListenerManager.instance.PostEvent(EventCode.ViewInforMobOn);
        }

        public override void UpdateData(){
            if(this.mob == null) return;
            this.textNameMob.text = Lean.Localization.LeanLocalization.GetTranslationText(mob.mobData.MobSO.Index + "_name", "Mobs");
            this.textTypeMob.text =  Lean.Localization.LeanLocalization.GetTranslationText("type_mob_"+mob.mobData.MobSO.Type.ToString(), "Regular Mobs_x");
            this.textLvMob.text =  Lean.Localization.LeanLocalization.GetTranslationText("level", "Level") +" "+ this.mob.mobData.Lv;
            this.avatar.sprite = this.mob.mobData.MobSO.Avatar;
            if (this.mob.GetComponentInChildren<SkeletonAnimation>()!=null)
                skeAvatar.skeletonDataAsset = this.mob.GetComponentInChildren<SkeletonAnimation>().skeletonDataAsset;
            skeAvatar.Initialize(true);
        }
        public void btnAttack_Onclick()
        {
            Rubik.Common.AudioHelper.AudioCtrl.Instance.Play(Rubik.Common.AudioHelper.AudioName.UI_Button_Confirm);
            // UnityEngine.SceneManagement.SceneManager.LoadScene(Configs.Combat_Screen);
            try
            {
                UserData.UserData.Instance.eneyDatas = mob.mobData.MobSO.lsEnemies;
                StaticData.GameMode = GameMode.Normal;
                bl_SceneLoaderManager.LoadScene(Configs.Combat_Screen);
                Rubik.Common.AudioHelper.AudioCtrl.Instance.Play(Rubik.Common.AudioHelper.AudioName.Test);
            }
            catch (System.Exception e)
            {
                NTPackage_old.Functions.NTLog.LogError(e.ToString(), gameObject);
            }
        }

        public void GenerateMob(){
            UserData.UserData.Instance.DataInCombat.TypeCombat = mob.mobData.MobSO.Type;
            UserData.UserData.Instance.DataInCombat.mobsInCombat.Clear();
            MobInfo mobInfo = MobManager.Calculator(UserData.UserData.Instance.DictionaryMobInfo[this.mob.mobData.Index], UserData.UserData.Instance.DictionaryMobStatsScaleByLevel[this.mob.mobData.Index], this.mob.mobData.Lv);
            mobInfo.CurHp = mobInfo.HP;
            UserData.UserData.Instance.DataInCombat.mobsInCombat.Add(mobInfo);
            if(this.mob.mobData.MobSO.Type == MobTypeCode.RegularMobs) this.GenerateSuportRegular();
            if(this.mob.mobData.MobSO.Type == MobTypeCode.GreaterMobs) this.GenerateSuportGreater();
            UserData.UserData.Instance.DataInCombat.MobDataPrevious = this.mob.mobData;
        }
        public void GenerateSuportRegular(){
            System.Random random = new System.Random((int)(this.mob.mobData.coordinates.latitude*1000));
            int number = 3;
            int rand = random.Next(100);
            if(rand < 25) number = 0;
            else if(rand < 50) number = 1;
            else if(rand < 75) number = 2;
            else number = 3;
            for (int i = 0; i < number; i++)
            {
                string Index = MobManager.fixedIndexRegularMobs[random.Next(MobManager.fixedIndexRegularMobs.Length)];
                int lv = this.mob.mobData.Lv - random.Next(2)-1;
                if(lv <= 0) lv = 1;
                MobInfo mobInfo_1 = MobManager.Calculator(UserData.UserData.Instance.DictionaryMobInfo[Index], UserData.UserData.Instance.DictionaryMobStatsScaleByLevel[Index], lv);
                mobInfo_1.CurHp = mobInfo_1.HP;
                UserData.UserData.Instance.DataInCombat.mobsInCombat.Add(mobInfo_1);
            }
        }

        public void GenerateSuportGreater(){
            System.Random random = new System.Random((int)(this.mob.mobData.coordinates.latitude*1000));
            for (int i = 0; i < 2; i++)
            {
                string Index = MobManager.fixedIndexRegularMobs[random.Next(MobManager.fixedIndexRegularMobs.Length)];
                int lv = this.mob.mobData.Lv - 5;
                if(lv <= 0) lv = 1;
                MobInfo mobInfo_1 = MobManager.Calculator(UserData.UserData.Instance.DictionaryMobInfo[Index], UserData.UserData.Instance.DictionaryMobStatsScaleByLevel[Index], lv);
                UserData.UserData.Instance.DataInCombat.mobsInCombat.Add(mobInfo_1);
                mobInfo_1.CurHp = mobInfo_1.HP;
            }
        }
    }
}
