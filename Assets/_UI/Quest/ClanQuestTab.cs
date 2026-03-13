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
    public class ClanQuestTab : NTBehaviour
    {
        public TextMeshProUGUI timeLeft;
        public ItemQuestUI ItemQuestPrefab;
        public Transform ClanQuestHolder;
        public Coroutine coroutineCountDown;
        public long countDown;
        public DateTime day;

        public List<DailyQuestPlayer> Quests;

        public void OffUI()
        {
            if (this.coroutineCountDown != null)
            {
                StopCoroutine(this.coroutineCountDown);
            }
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.ClanQuestHolder);
        }

        public void UpdateData()
        {
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.ClanQuestHolder);
            List<DailyQuestPlayerData> clanQuestPlayerData = QuestManager.Instance.GetClanQuestData();
            this.Quests = new List<DailyQuestPlayer>();
            foreach (DailyQuestPlayerData quest in clanQuestPlayerData)
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
                        itemQuest = Instantiate(this.ItemQuestPrefab, this.ClanQuestHolder);
                }
                itemQuest.SetUp(quest.Index,true);
                itemQuest.transform.SetParent(this.ClanQuestHolder);
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
                timeLeft.text = LeanLocalization.GetTranslationText("time_left","Time left ")  + ": <color=#BD7E92>" + NTFunction.FormatTimeHour(this.countDown) + "</color>";
                yield return new WaitForSeconds(1);
                this.countDown = ServerManager.Instance.GetNextTimeNewDay();
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
