using System;
using System.Collections.Generic;
using NTPackage.Functions;
using UnityEngine;

namespace Rubik.Myrk.Portal
{
    public class SelectionLevelUI : MonoBehaviour
    {
        public SelectionLevelItem SelectionLevelItemPrefab;
        public List<SelectionLevelItem> LevelItems;
        public Transform Holder;
        public int CurrentLevel;
        public Action<int> ChangeLevel;

        public void SetData(int maxLevel){
            this.Clear();
            for(int i = 0; i <= maxLevel; i++){
                SelectionLevelItem item = ObjectPoolingManager.Instance.InstantiateObject<SelectionLevelItem>(ObjectPoolingConfig.SelectionLevelItem, this.SelectionLevelItemPrefab.transform);
                item.SetData(i, this);
                this.LevelItems.Add(item);
                item.transform.SetParent(this.Holder);
                NTFunction.ResetPosition(item.transform);
            }
            this.UpdateData();
        }

        public void UpdateData(){
            foreach(SelectionLevelItem item in this.LevelItems){
                if(item.Level == this.CurrentLevel){
                    item.Chose();
                }
                else{
                    item.Unchose();
                }
            }
        }

        public void _OnclickLevel(int level){
            this.CurrentLevel = level;
            this.ChangeLevel?.Invoke(level);
            this.UpdateData();
        }

        public void Clear(){
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.Holder.transform);
        }
    }
}