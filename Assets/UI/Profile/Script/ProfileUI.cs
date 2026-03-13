using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NTFunctions_old;
using GOA.UserData;
using TMPro;
using DG.Tweening;

namespace GOA.UIProfile
{
    public class ProfileUI : PopupUI
    {
        [SerializeField]
        private Data _data;
        public TextMeshProUGUI textNameUser;
        public TextMeshProUGUI textGmail;
        public TextMeshProUGUI textLv;
        public Image avatar; 

        public ProfileDetailUI profileDetailUI;

        public Transform Content;
        public AssetUI LandAssetSample;
        public List<AssetUI> AssetUIs;

        public TextMeshProUGUI CountPageText;
        
        public int curPage = 1;

        public int IsNext = 1;

        public override void LoadComponents()
        {
            base.LoadComponents();
            this.lvUI = new PopupLv().GetValue(transform.name);
        }


        public void OnUI(Data data){
            return;
            if(!this.CanShow()) return;
            this._data = data;
            this.Show();
        }

        public override void UpdateData()
        {
            base.UpdateData();
            //APIManager.Instance.GetLand(curPage, 10, ()=>{
            //    for (int i = 0; i < this.AssetUIs.Count; i++)
            //    {
            //        try
            //        {  
            //            try
            //            {
            //                Transform trans = this.AssetUIs[i].transform.Find("Panel");
            //                trans.DOComplete();
            //                float time = 0.2f*(i-this.Content.localPosition.y/180);
            //                if(time<= 0) time = 0;
            //                trans.localPosition = new Vector3(1200*this.IsNext, trans.localPosition.y, trans.localPosition.z);
            //                trans.DOLocalMoveX(1200*this.IsNext, 0.1f*time).OnComplete(()=>{
            //                    trans.DOLocalMoveX(0, 0.2f);
            //                });
            //            }
            //            catch (System.Exception){}
            //            this.AssetUIs[i].SetData(UserData.UserData.Instance.LandDatas[i]);
            //        }
            //        catch (System.Exception)
            //        {
            //            this.AssetUIs[i].gameObject.SetActive(false);
            //        }
            //    }
            //});
            this.UpdateText();
        }
        
        protected void UpdateText(){
            this.textNameUser.text = this._data.DisplayName.Length > 0 ? this._data.DisplayName : this._data.UserName;
            this.textLv.text = UserData.UserData.Instance.characterData.Level.ToString();
            this.textGmail.text = this._data.UserName;
            if(UserData.UserData.Instance.CountLand <= 0){
                this.CountPageText.text = "--/--";
            }else{
                int maxPage = UserData.UserData.Instance.CountLand / 10;
                if(UserData.UserData.Instance.CountLand %10 != 0) maxPage++;
                this.CountPageText.text = this.curPage+"/"+maxPage;
            }
        }

        public void NextPage(){
            this.IsNext = 1;
            this.curPage ++;
            int maxPage = UserData.UserData.Instance.CountLand / 10;
            if(UserData.UserData.Instance.CountLand %10 != 0) maxPage++;
            if(this.curPage > maxPage) this.curPage = 1;
            this.UpdateData();
        }

        public void BackPage(){
            this.IsNext = -1;
            this.curPage --;
            if(this.curPage < 1){
                int maxPage = UserData.UserData.Instance.CountLand / 10;
                if(UserData.UserData.Instance.CountLand %10 != 0) maxPage++;
                curPage = maxPage;
            }
            if(curPage < 1) curPage = 1;
            this.UpdateData();
        }
    }

}
