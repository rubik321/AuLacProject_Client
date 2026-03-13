using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NTFunctions_old;
using TMPro;
using GOA.UserData;
using GOA.Config;
using SimpleJSON;
using GOA.Portal;
using Rubik.Format;

namespace GOA.Portal{
    using WorldMap;

    public class PortalUnlookUI : PopupUI
    {
        public Portal Portal;
        public GeoPointData PortalData;
        public PortalAttackData PortalAttackData;
        public MobInfo MobInfo;
        public TextMeshProUGUI NamePortal;
        public TextMeshProUGUI CountTime;
        public TextMeshProUGUI MaxHp;
        public TextMeshProUGUI CurHp;
        public Slider HPBar;
        public Image Ava;

        public Transform TransAttack;
        public Transform TransAttackCooldown;
        public TextMeshProUGUI TextTimeCooldown;
        public TextMeshProUGUI TextTimeClose;

        public MultiTabUI MultiTabUI;
        public Transform BtnTabs;
        public ContributorsTabUI ContributorsTabUI;

        public Transform UserContribute;
        public TextMeshProUGUI TextUserName;
        public TextMeshProUGUI TextUserRank;
        public TextMeshProUGUI TextUserScore;
        public TextMeshProUGUI TextPortalID;

        public Transform BtnLock;
        public Transform BtnUnlock;

        public float TimeCD;
        public float TimeClose;

        public Transform Top1;
        public Transform Top2_5;
        public Transform Top6_10;
        public Transform Top11_20;
        public Transform Join;

        public string KeyPortalDelayAttack(){
            return "Portal:"+this.PortalData.PointID+":"+UserData.UserData.Instance.data.UserId;
        }

        public override void LoadComponents()
        {
            base.LoadComponents();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            this.TimeCD -= Time.fixedDeltaTime;
            this.TimeClose -= Time.fixedDeltaTime;
            if(this.PortalAttackData.Detail.Status == PortalStatus.Defeated
                ||this.PortalAttackData.Detail.Status == PortalStatus.Destroy
            ){
                this.TransAttack.gameObject.SetActive(false);
                this.TransAttackCooldown.gameObject.SetActive(false);
            }else{
                if(this.TimeCD < 0){
                    this.TransAttack.gameObject.SetActive(true);
                    this.TransAttackCooldown.gameObject.SetActive(false);
                }else{
                    this.TextTimeCooldown.text = Lean.Localization.LeanLocalization.GetTranslationText("ready_again_in", "Ready again in") +" "+ NTFunction.FormatTimeHour(this.TimeCD);
                    this.TransAttack.gameObject.SetActive(false);
                    this.TransAttackCooldown.gameObject.SetActive(true);
                }
            }
            if(this.TimeClose < 0){
                this.TextTimeClose.text = Lean.Localization.LeanLocalization.GetTranslationText("closed", "Closed");
            }if(this.TimeClose > 0){
                this.TextTimeClose.text = Lean.Localization.LeanLocalization.GetTranslationText("close_in", "Close in") +" "+ NTFunction.FormatTimeHour(this.TimeClose);
            }
        }

        public void OnUI(Portal portal){
            //this.Portal = portal;
            //this.Portal.UpdateData();
            //MobSO mobSO = MobManager.instance.GetMobScriptableObjectByIndex(this.Portal.PortalData.Index);
            //this.Ava.sprite = mobSO.Avatar;
            //this.PortalData = this.Portal.PortalData;
            //this.MobInfo = UserData.UserData.Instance.DictionaryMobInfo[this.Portal.PortalData.Index];
            //this.NamePortal.text = Lean.Localization.LeanLocalization.GetTranslationText(this.Portal.PortalData.Index + "_name", "Portal");
            //this.MaxHp.text = this.MobInfo.HP.ToString();
            //JSONNode data = new JSONObject();
            //data["userID"] = UserData.UserData.Instance.data.UserId;
            //data["portalID"] = portal.PortalData.PointID;
            //this.TextPortalID.text = portal.PortalData.PointID.ToString();
            //StartCoroutine(APIManager.Instance.PostDataUrl(data.ToString() ,SeverConfigs.BASE_API_URL + SeverConfigs.GetPortalDataAPI, callback=>{
            //    JSONNode data = JSONNode.Parse(callback.downloadHandler.text);
            //    this.PortalAttackData = JsonUtility.FromJson<PortalAttackData>(data["Data"].ToString());
            //    if(this.PortalAttackData.Detail.Status == PortalStatus.Open 
            //        || this.PortalAttackData.Detail.Status == PortalStatus.Defeated 
            //        || this.PortalAttackData.Detail.Status ==PortalStatus.Destroy)
            //    {
            //        this.PortalData.Opened = true;
            //    }else{
            //        this.PortalData.Opened = false;
            //    }
            //    Debug.Log(this.PortalData.Opened+":"+this.PortalAttackData.Detail.Status, gameObject);
            //    float perHp = 1;
            //    float hpNumber = this.MobInfo.HP;
            //    try
            //    {
            //        if(this.PortalAttackData.Detail.Status == PortalStatus.Close){
            //            perHp = 1;
            //            this.PortalAttackData.Hp = this.MobInfo.HP.ToString();
            //        }
            //        hpNumber = float.Parse(this.PortalAttackData.Hp);
            //        if(hpNumber <= 0) hpNumber = 0;
            //        perHp = float.Parse(this.PortalAttackData.Hp) / (this.MobInfo.HP+1);
            //    }
            //    catch (System.Exception e)
            //    {
            //        Debug.LogWarning(e);
            //    }
            //    this.CurHp.text = hpNumber.ToString();
            //    if(perHp > 1) perHp = 1;
            //    if(perHp < 0) perHp = 0;
            //    this.HPBar.value = perHp;
            //    this.Show();
            //    this.TransAttack.gameObject.SetActive(true);
            //    this.TransAttackCooldown.gameObject.SetActive(false);
            //    if(this.Portal.PortalData.Opened){
            //        this.MultiTabUI.BtnTabOnclick(0);
            //        this.BtnTabs.gameObject.SetActive(true);
            //        this.BtnLock.gameObject.SetActive(false);
            //        this.BtnUnlock.gameObject.SetActive(true);
            //        this.ContributorsTabUI.Init(this.PortalAttackData.List);
            //        this.UserContribute.gameObject.SetActive(true);
            //    }else{
            //        this.MultiTabUI.BtnTabOnclick(1);
            //        this.BtnTabs.gameObject.SetActive(false);
            //        this.BtnLock.gameObject.SetActive(true);
            //        this.BtnUnlock.gameObject.SetActive(false);
            //        this.UserContribute.gameObject.SetActive(false);
            //    }

            //    this.Top1.gameObject.SetActive(false);
            //    this.Top2_5.gameObject.SetActive(false);
            //    this.Top6_10.gameObject.SetActive(false);
            //    this.Top11_20.gameObject.SetActive(false);
            //    this.Join.gameObject.SetActive(false);
            //    this.TextUserName.text = UserData.UserData.Instance.data.UserName;
            //    if(this.PortalAttackData.Score == null || this.PortalAttackData.Score.Length == 0){
            //        this.TextUserScore.text = "--";
            //        this.TextUserRank.text = "--";
            //    }else{
            //        this.TextUserScore.text = FormatData.GetFriendlyShortNumberFromString(this.PortalAttackData.Score);
            //        this.TextUserRank.text = (this.PortalAttackData.Rank+1).ToString();
            //        if(this.PortalAttackData.Rank > 99){
            //            this.TextUserRank.text = "99+";
            //        }
            //        if(this.PortalAttackData.Rank < 1) this.Top1.gameObject.SetActive(true);
            //        else if(this.PortalAttackData.Rank < 5) this.Top2_5.gameObject.SetActive(true);
            //        else if(this.PortalAttackData.Rank < 10) this.Top6_10.gameObject.SetActive(true);
            //        else if(this.PortalAttackData.Rank < 20) this.Top11_20.gameObject.SetActive(true);
            //        else this.Join.gameObject.SetActive(true);
            //    }
            //    try
            //    {
            //        string strTime = PlayerPrefs.GetString(KeyPortalDelayAttack());
            //        double timeCD = System.DateTime.Now.Subtract(System.DateTime.Parse(strTime)).TotalSeconds;
            //        this.TimeCD = 1800 - (float)timeCD;
            //    }
            //    catch (System.Exception)
            //    {
            //        this.TimeCD = 0;
            //    }
            //    this.TimeClose = 86400 - (NTFunction.GetUtcTimestamp() - this.PortalAttackData.Detail.TimeOpen);
            //    this.Portal.UpdateData();
            //}));
        }

        public override void OffUI()
        {
            base.OffUI();
            this.Portal.UpdateData();
        }

        public void Attack(){
            PlayerPrefs.SetString(KeyPortalDelayAttack(), System.DateTime.Now.ToString());
            UserData.UserData.Instance.DataInCombat.TypeCombat = MobTypeCode.MobReaperPortal;
            UserData.UserData.Instance.DataInCombat.mobsInCombat.Clear();
            MobInfo mobInfo = (MobInfo) this.MobInfo.Clone();
            mobInfo.CurHp = float.Parse(this.PortalAttackData.Hp);
            UserData.UserData.Instance.DataInCombat.mobsInCombat.Add(mobInfo);
            UserData.UserData.Instance.DataInCombat.PortalId = this.PortalData.PointID.ToString();
            //UnityEngine.SceneManagement.SceneManager.LoadScene(Configs.Combat_Screen);
            try
            {
                bl_SceneLoaderManager.LoadScene(Configs.Combat_Screen);
                Rubik.Common.AudioHelper.AudioCtrl.Instance.Play(Rubik.Common.AudioHelper.AudioName.Test);
            }
            catch (System.Exception e)
            {
                NTPackage_old.Functions.NTLog.LogError(e.ToString(), gameObject);
            }
        }
    
        public void OnclickOpen(){
            NoticUseKeyOpenPortalUI noticUseKeyOpenPortalUI = (NoticUseKeyOpenPortalUI) UIManager.instance.GetPopupUIByCode(PopupCode.NoticUseKeyOpenPortalUI);
            if(noticUseKeyOpenPortalUI != null) noticUseKeyOpenPortalUI.OnUI(this.Portal);
        }

    }
}
