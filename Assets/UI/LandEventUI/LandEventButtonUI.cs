using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GOA.Item;
using GOA.Mat;
using NTFunctions_old;
using NTPackage_old.Functions;
using NTPackage_old.UI;
using Rubik.Common;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace GOA.LandEvent
{
    public class LandEventButtonUI : NTButtonEffect
    {
        public EventType EventType = EventType.Hunter;

        public Image Icon;
        public Image Shadow;
        public MatListUI MatListUI;

        public bool IsEnoughMat = true;

        [Button]
        public override void LoadComponents()
        {
            base.LoadComponents();
            this.LoadIcon();
        }

        protected void LoadIcon(){
            if(this.Icon != null) return;
            this.Icon = transform.Find("Inner").Find("Icon").GetComponent<Image>();
        }

        public void Init(){
            this.IsEnoughMat = true;
            LandEventInfo landEventInfo = LandEventManager.instance.LandEventInfoDic.Get(this.EventType.ToString());
            if(landEventInfo == null){
                NTLog.LogError("Not found"+this.EventType, gameObject);
                gameObject.SetActive(false);
                return;
            }
            List<(Sprite, string, Color)> values = new List<(Sprite, string, Color)>();
            foreach (MatRequire item in landEventInfo.MatRequire)
            {
                GOA.Item.ItemInfoData itemData = ItemAsset.instance.GetItemDataByCode(item.Code);
                Sprite icon = SpriteHelper.Instance.GetSprite(itemData.Images);
                int amountInventory = UserData.UserData.Instance.Inventory.GetInventoryByCode(item.Code);
                string detail  = item.Amount+"";
                Color color = Color.white;
                if(item.Amount > amountInventory){
                    color = Color.red;
                    this.IsEnoughMat = false;
                }
                values.Add((icon, detail, color));
            }
            this.MatListUI.SetData(values);
        }

        public override void Chose(){
            this.Icon.color = new Color(this.Icon.color.r,this.Icon.color.g,this.Icon.color.b,0.5f);
            this.Icon.DOColor(new Color(this.Icon.color.r,this.Icon.color.g,this.Icon.color.b,1), 0.7f);
            this.Shadow.color = new Color(this.Shadow.color.r,this.Shadow.color.g,this.Shadow.color.b,0f);
            this.Shadow.DOColor(new Color(this.Shadow.color.r,this.Shadow.color.g,this.Shadow.color.b,1), 0.7f);
        }

        public override void UnChose()
        {
            this.Icon.DOComplete();
            this.Shadow.DOComplete();
            this.Icon.color = new Color(this.Icon.color.r,this.Icon.color.g,this.Icon.color.b,0.5f);
            this.Shadow.color = new Color(this.Shadow.color.r,this.Shadow.color.g,this.Shadow.color.b,0f);
        }


    }
}
