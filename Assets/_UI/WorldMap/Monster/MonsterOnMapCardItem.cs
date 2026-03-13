using System.Collections;
using System.Collections.Generic;
using NTPackage.Functions;
using NTPackage.UI;
using Rubik.CardPlayer;
using Rubik.UI.Statitic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rubik.Myrk.Monster
{
    public class MonsterOnMapCardItem : MonoBehaviour
    {
        public Transform SlotInfo;
        public Transform AvatarHolder;
        public MonsterData MonsterData;
        public TextMeshProUGUI TextLevel;
        public StarUI StarUI;
        public void SetData(MonsterData monsterData){
            if(monsterData == null || monsterData._id == null || monsterData._id == "")
            {
                this.MonsterData = null;
                this.SlotInfo.gameObject.SetActive(false);
                return;
            }
            this.SlotInfo.gameObject.SetActive(true);
            this.MonsterData = monsterData;
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.AvatarHolder);
            Transform avatar = CardPlayerManager.Instance.InstantiatePlayerAvatar(monsterData.Index);
            avatar.SetParent(this.AvatarHolder);
            NTFunction.ResetPosition(avatar);
            this.TextLevel.text = "Lv. " + (monsterData.Lv+1).ToString();
            this.StarUI.SetStar(monsterData.Star);
        }

        public void OnClickCard(){
            if(this.MonsterData == null) return;
            CardPlayerManager.Instance.ShowPopupStory(this.MonsterData.Index, this.MonsterData.Star, this.MonsterData.Lv, this.MonsterData.Scale);
        }
    }
}