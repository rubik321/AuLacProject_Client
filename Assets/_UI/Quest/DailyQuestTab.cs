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
using Lean.Localization;
namespace Rubik.Quest
{
    public class DailyQuestTab : TabUI
    {
        public TextMeshProUGUI timeLeft;
        public ItemQuestUI ItemQuestPrefab;
        public Transform DailyQuestHolder;
        public Coroutine coroutineCountDown;
        public long countDown;
        public DateTime day;

        public List<DailyQuestPlayer> Quests;

        public override void OffUI()
        {
            base.OffUI();
            if (this.coroutineCountDown != null)
            {
                StopCoroutine(this.coroutineCountDown);
            }
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.DailyQuestHolder);
        }

        public override void UpdateData()
        {
            base.UpdateData();

            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.DailyQuestHolder);
            List<DailyQuestPlayerData> dailyQuestPlayerData = QuestManager.Instance.GetDailyQuestData();
            this.Quests = new List<DailyQuestPlayer>();
            foreach (DailyQuestPlayerData quest in dailyQuestPlayerData)
            {
                DailyQuestPlayer player = QuestManager.Instance.GetQuest(quest.Index);
                if(player == null) continue;
                this.Quests.Add(player);
            }
            Quests.Sort((a, b) =>
            {
                return QuestManager.Instance.CompareTo(a, b);
            });
            foreach (DailyQuestPlayer quest in Quests)
            {
                ItemQuestUI itemQuest = ObjectPoolingManager.Instance.PullObjectFromPooling<ItemQuestUI>(ObjectPoolingConfig.ItemQuestUI);
                if (itemQuest == null)
                {
                    itemQuest = Instantiate(this.ItemQuestPrefab, this.DailyQuestHolder);
                }
                itemQuest.SetUp(quest.Index);
                itemQuest.transform.SetParent(this.DailyQuestHolder);
                itemQuest.transform.name = ObjectPoolingConfig.ItemQuestUI;
                NTFunction.ResetPosition(itemQuest.transform);
            }

            this.countDown = ServerManager.Instance.GetNextTimeNewDay();
            if (this.coroutineCountDown != null)
            {
                StopCoroutine(this.coroutineCountDown);
            }
            this.coroutineCountDown = StartCoroutine(this.CotimeLeft());
        }

        IEnumerator CotimeLeft()
        {
            while (true)
            {
                this.countDown = ServerManager.Instance.GetNextTimeNewDay();
                timeLeft.text = LeanLocalization.GetTranslationText("time_left","Time left ")  + ": <color=#BD7E92>" + NTFunction.FormatTimeHour(this.countDown) + "</color>";
                yield return new WaitForSeconds(1);
                if (this.countDown < 0)
                {
                    yield return QuestManager.Instance.IEGetDailyQuest(() =>
                    {
                        this.UpdateData();
                    });
                    break;
                }
            }
        }
    }
}
