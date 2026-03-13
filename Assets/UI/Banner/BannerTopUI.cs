using System.Collections.Generic;
using NTPackage;
using NTPackage.Functions;
using NTPackage.UI;
using Rubik.Manager;
using UnityEngine;

namespace Rubik.Banner
{
    public class BannerTopConfig{
        public const string ConnectUnstable = "ConnectUnstable";
        public const string LocationUnstable = "LocationUnstable";
        public const string WarrningGuest = "WarrningGuest";
        public const string WarningCapSlotInventoryBag = "WarningCapSlotInventoryBag";
    }

    [System.Serializable]
    public class BannerTopData{
        public string ID;
        public string Message;
        public float Time;
    }

    public class BannerTopUI : PopupUI
    {
        public BannerItem BannerItemPrefab;
        public NTDictionary<string, BannerTopData> BannerCache = new NTDictionary<string, BannerTopData>();
        public List<BannerItem> BannerItems;
        public Transform BannerHolder;
        public float CheckTime = 5f;
        public float CountTime = 0f;

        protected override void Update()
        {
            base.Update();
            this.CountTime += Time.deltaTime;
            if(this.CountTime >= this.CheckTime){
                this.CountTime = 0f;
                List<BannerTopData> bannerTopDatas = this.BannerCache.ToList();
                for (int i = 0; i < bannerTopDatas.Count; i++)
                {
                    if(Time.time - bannerTopDatas[i].Time >= this.CheckTime){
                        this.RemoveBanner(bannerTopDatas[i].ID);
                    }
                }
            }
            
        }

        public override void OnUI(object data = null, bool isDefaultSound = true)
        {
            base.OnUI(data, false);
        }

        public override void ScriptOffUI()
        {
            base.ScriptOffUI();
            this.BannerItems.Clear();
            this.BannerCache.Clear();
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.BannerHolder);
        }

        public override void OffUIWithoutSound(){
            base.OffUIWithoutSound();
            this.BannerItems.Clear();
            this.BannerCache.Clear();
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.BannerHolder);
        }

        public override void HideNone(){
            base.HideNone();
            this.BannerItems.Clear();
            this.BannerCache.Clear();
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.BannerHolder);
        }

        public void AddBanner(string id, string message)
        {
            BannerTopData data = this.BannerCache.Get(id);
            if(data != null){
                data.Message = message;
                BannerItem bannerItem_old = this.BannerItems.Find(x => x.Data.ID == id);
                if(bannerItem_old != null){
                    bannerItem_old.SetData(data);
                }
                data.Time = Time.time;
                return;
            }
            if (this.BannerItems.Count == 0)
            {
                this.OnUI();
            }
            BannerItem bannerItem = ObjectPoolingManager.Instance.InstantiateObject<BannerItem>(ObjectPoolingConfig.BannerItem, this.BannerItemPrefab.transform);
            BannerTopData bannerTopData = new BannerTopData();
            bannerTopData.ID = id;
            bannerTopData.Message = message;
            bannerTopData.Time = Time.time;
            bannerItem.SetData(bannerTopData);
            this.BannerItems.Add(bannerItem);
            bannerItem.transform.SetParent(this.BannerHolder);
            NTFunction.ResetPosition(bannerItem.transform);
            this.BannerCache.Add(id, bannerTopData);
        }

        public void RemoveBanner(string id){
            if(this.BannerCache.Get(id) == null){
                return;
            }
            this.BannerCache.Remove(id);
            BannerItem bannerItem = this.BannerItems.Find(x => x.Data.ID == id);
            if(bannerItem != null){
                ObjectPoolingManager.Instance.PushObjectIntoPooling(bannerItem.transform);
                this.BannerItems.Remove(bannerItem);
            }
            if(this.BannerItems.Count == 0){
                this.OffUIWithoutSound();
            }
        }

        
    }
}