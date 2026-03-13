using System.Collections.Generic;
using NTPackage.Functions;
using NTPackage.UI;
using UnityEngine;

namespace Rubik.Myrk.Outpost
{
    using Rubik.ItemPlayer;
    public class OutpostOnMapRewardUI : PopupUI
    {
        public OutpostOnMapRewardItem OutpostOnMapRewardItemCurrent;
        public OutpostOnMapRewardItem OutpostOnMapRewardItemPrefab;
        public List<OutpostOnMapRewardItem> OutpostOnMapRewardItems = new List<OutpostOnMapRewardItem>();
        public Transform Holder;

        public override void OnUI(object data = null, bool isDefaultSound = true){
            base.OnUI(data, isDefaultSound);
        }

        public override void UpdateData(object data = null)
        {
            base.UpdateData(data);
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(Holder);
            long amount = OutpostWorldMapManager.Instance.GetAmountOutpost();
            int count = 0;
            List<OutpostData> outpostDatas = new List<OutpostData>();
            for (int i = 0; i < OutpostWorldMapManager.Instance.OutpostDatas.Count; i++)
            {
                OutpostData outpostData = OutpostWorldMapManager.Instance.OutpostDatas[i];
                if(amount <= outpostData.Max){
                    outpostDatas.Add(outpostData);
                    count++;
                }
                if(count >= 10) break;
            }
            for (int i = 0; i < outpostDatas.Count; i++)
            {
                OutpostOnMapRewardItem outpostOnMapRewardItem = ObjectPoolingManager.Instance.InstantiateObject<OutpostOnMapRewardItem>(ObjectPoolingConfig.OutpostOnMapRewardItem,this.OutpostOnMapRewardItemPrefab.transform);
                outpostOnMapRewardItem.SetData(outpostDatas[i].Min + " - " + outpostDatas[i].Max, outpostDatas[i].DailyReward);
                OutpostOnMapRewardItems.Add(outpostOnMapRewardItem);
                outpostOnMapRewardItem.SetBG(i);
                outpostOnMapRewardItem.transform.SetParent(this.Holder);
                NTFunction.ResetPosition(outpostOnMapRewardItem.transform);
            }
            if(amount > 0){
                this.OutpostOnMapRewardItemCurrent.SetData(amount.ToString(), outpostDatas[0].DailyReward);
            }else{
                this.OutpostOnMapRewardItemCurrent.SetData("0", new List<ItemData>());
            }
        }

        public override void ScriptOffUI()
        {
            base.ScriptOffUI();
            foreach (OutpostOnMapRewardItem item in OutpostOnMapRewardItems)
            {
                item.Clear();
            }
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(Holder);
        }
    }
}