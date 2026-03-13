using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NTFunctions_old;
using TMPro;
using GOA.WorldMap;
using GOA.WorldMap.Outpost;
using GOA.Config;
using GOA.UserData;
using Rubik.UI;

namespace GOA.WorldMap.Outpost{
    public class OutpostUI : PopupUI
    {
        public Transform TransModel;
        public TextMeshProUGUI TextName;
        public TextMeshProUGUI TextInfo;
        public TextMeshProUGUI TextLv;

        public Outpost Outpost;
        public Transform Model;

        public void OnUI(Outpost outpost){
            if(!this.CanShow()) return;
            this.Outpost = outpost;
            try
            {
                Destroy(this.Model.GetChild(0).gameObject);
            }
            catch (System.Exception){}
            MobSO mobSO= MobManager.instance.GetMobScriptableObjectByIndex(this.Outpost.ChieftanID);
            //GameObject mob = Instantiate(mobSO.Model);
            //var children = mob.GetComponentsInChildren<Transform>(includeInactive: true);
            //foreach (var child in children)
            //{
            //    child.gameObject.layer = 5;
            //}
           // mob.transform.SetParent(this.Model);
            //NTFunction.ResetPosition(mob.transform);
            this.TextLv.text ="Lv."+ outpost.Lv.ToString();
            // this.TextInfo.text = string.Format(Lean.Localization.LeanLocalization.GetTranslationText("detail_outpost", "Reward {0}"), this.Outpost.rewards);
            this.TextInfo.text = Lean.Localization.LeanLocalization.GetTranslationText("detail_outpost", "The Outpost stands as a sinister testament to the Chthonians' dark conquest, serving as a strategic point to exert their malevolent influence over Earth.");
            this.TextName.text = Lean.Localization.LeanLocalization.GetTranslationText(mobSO.Index + "_name", "Outpost");
            this.Show();
        }

        public void Attack(){

            //UserData.UserData.Instance.DataInCombat.TypeCombat = MobTypeCode.MobChieftainOutpost;
            //UserData.UserData.Instance.DataInCombat.OutpostId = this.Outpost.Id;
            //UserData.UserData.Instance.DataInCombat.mobsInCombat.Clear();
            //MobInfo chieftan = MobManager.Calculator(UserData.UserData.Instance.DictionaryMobInfo[this.Outpost.ChieftanID], UserData.UserData.Instance.DictionaryMobStatsScaleByLevel[this.Outpost.ChieftanID],this.Outpost.Lv);
            //chieftan.CurHp = chieftan.HP;
            //MobInfo greater1 = MobManager.Calculator(UserData.UserData.Instance.DictionaryMobInfo[this.Outpost.GreaterID1], UserData.UserData.Instance.DictionaryMobStatsScaleByLevel[this.Outpost.GreaterID1],this.Outpost.Lv-5);
            //greater1.CurHp = greater1.HP;
            //MobInfo greater2 = MobManager.Calculator(UserData.UserData.Instance.DictionaryMobInfo[this.Outpost.GreaterID2], UserData.UserData.Instance.DictionaryMobStatsScaleByLevel[this.Outpost.GreaterID2],this.Outpost.Lv-5);
            //greater2.CurHp = greater2.HP;
            //UserData.UserData.Instance.DataInCombat.mobsInCombat.Add(chieftan);
            //UserData.UserData.Instance.DataInCombat.mobsInCombat.Add(greater1);
            //UserData.UserData.Instance.DataInCombat.mobsInCombat.Add(greater2);

            ////UnityEngine.SceneManagement.SceneManager.LoadScene(Configs.Combat_Screen);
            //try
            //{
            //    bl_SceneLoaderManager.LoadScene(Configs.Combat_Screen);
            //    Rubik.Common.AudioHelper.AudioCtrl.instance.Play(Rubik.Common.AudioHelper.AudioName.BGM_Outpost);
            //}
            //catch (System.Exception e)
            //{
            //    NTPackage.Functions.NTLog.LogError(e.ToString(), gameObject);
            //}
            HUDCanvas.Instance.ShowNotification("Don't enought key !");
        }
    }
}
