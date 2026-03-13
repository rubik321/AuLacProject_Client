using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Pixelplacement;
using UnityEngine.UI;
using GOA.UserData;
using System;
using System.Globalization;
using NTPackage.UI;
using Rubik.Manager;
using NTPackage.Functions;

namespace Rubik.Quest
{
    public class AchimentTab : TabUI
    {
        public ItemAchimentUI ItemAchimentPrefab;
        public Transform AchievementHolder;
        
        public List<AchievementPlayer> Achiments;

        public override void OffUI()
        {
            base.OffUI();
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.AchievementHolder);
        }

        public override void UpdateData()
        {
            base.UpdateData();

            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.AchievementHolder);
            List<AchievementBadge> achievementBadges = AchievementManager.Instance.GetAchievementBadge();
            this.Achiments = new List<AchievementPlayer>();
            foreach (AchievementBadge achievementBadge in achievementBadges){
                AchievementPlayer achievementPlayer = AchievementManager.Instance.GetAchievement(achievementBadge.Index);
                if(achievementPlayer != null){
                    this.Achiments.Add(achievementPlayer);
                }
            }
            Achiments.Sort((a, b) =>
            {
                return AchievementManager.Instance.CompareTo(a, b);
            });
            foreach (AchievementPlayer achiment in Achiments)
            {
                ItemAchimentUI itemAchiment = ObjectPoolingManager.Instance.PullObjectFromPooling<ItemAchimentUI>(ObjectPoolingConfig.ItemAchimentUI);
                if (itemAchiment == null)
                {
                    itemAchiment = Instantiate(this.ItemAchimentPrefab, this.AchievementHolder);
                }
                itemAchiment.SetUp(achiment.Index);
                itemAchiment.transform.SetParent(this.AchievementHolder);
                itemAchiment.transform.name = ObjectPoolingConfig.ItemAchimentUI;
                NTFunction.ResetPosition(itemAchiment.transform);
            }
        }
    }
}
