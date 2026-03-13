using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using NTFunctions_old;
using TMPro;
using GOA.UserData;
using GOA.Config;

namespace GOA.WorldMap.Outpost{
    using Building;

    public class Outpost : Building
    {

        public Vector2 TileCoordinates;
        public const float TimeDelay = 0.2f;
        public bool Holding = false;

        public TextMeshProUGUI TextDetail;

        public string ChieftanID;
        public string GreaterID1;
        public string GreaterID2;
        public int rewards = 0;
        public int Lv = 20;

        public string Id;

        public MeshRenderer MeshRenderer;
        public Material MaterialOccupied;
        public Material MaterialNotOccupied;

        public bool IsOccupied = false;
        public Sprite SpriteOccupied;
        public Sprite SpriteNotOccupied;

        public Image BgIcon;

        public void SetData(Vector2 tileCoordinates) {
            this.TileCoordinates = tileCoordinates;
            ChieftanID = "M03" + (Mathf.FloorToInt(TileCoordinates.y) % 10 + 1).ToString("0000");
            GreaterID1 = "M02" +  (Mathf.FloorToInt(TileCoordinates.y) % 20 + 1).ToString("0000");
            GreaterID2 = "M02" +  (Mathf.FloorToInt(TileCoordinates.y/10) % 20 + 1).ToString("0000");
            rewards = Mathf.FloorToInt(TileCoordinates.x) % 40 + 5;
            this.Lv = Mathf.FloorToInt(TileCoordinates.x) % 10 + 20;
            this.TextDetail.text = string.Format(Lean.Localization.LeanLocalization.GetTranslationText("text_gold_outpost", "+{0}"), this.rewards);
            this.Id = Mathf.FloorToInt(this.TileCoordinates.x)+"&"+Mathf.FloorToInt(this.TileCoordinates.y);
            this.UpdateData();
        }

        public void UpdateData(){
            try
            {
                if(UserData.UserData.Instance.OutpostsOccupied[this.Id] == 1){
                    this.IsOccupied = true;
                }else{
                    this.IsOccupied = false; 
                }
            }
            catch (System.Exception)
            {
               this.IsOccupied = false; 
            }
            if(this.IsOccupied)
            {
                this.BgIcon.sprite = this.SpriteOccupied;
                this.MeshRenderer.material = this.MaterialOccupied;
            } 
            else {
                this.BgIcon.sprite = this.SpriteNotOccupied;
                this.MeshRenderer.material = this.MaterialNotOccupied;
            }
        }

        public void Chose(){
            this.Holding = true;
            StartCoroutine(this.CountHolding());
        }

        IEnumerator CountHolding(){
            yield return new WaitForSeconds(TimeDelay);
            this.Holding = false;
        }

        void OnMouseUp()
        {  
            if(this.IsOccupied) return;
            if(!this.Holding) return;
            OutpostUI outpostUI = (OutpostUI) UIManager.instance.GetPopupUIByCode(PopupCode.OutpostUI);
            if(outpostUI == null) return;
            outpostUI.OnUI(this);
        }

        public static int GetRewardByID(string id){
            string[] arr = id.Split("&");
            int x = int.Parse(arr[0]);
            return Mathf.FloorToInt(x) % 40 + 5;
        }
    }
}
