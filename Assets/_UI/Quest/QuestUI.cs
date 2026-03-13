using GOA.UserData;
using NTPackage.Functions;
using NTPackage.UI;
using Pixelplacement;
using Rubik.Common.AudioHelper;
using Rubik.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rubik.Quest
{
    public class QuestUI : PopupUI
    {
        public MultiTabUI MultiTabUI;
        public TabInven[] lsTabs;
        public GameObject timerGo;
        public override void OnUI(object data = null, bool isDefaultSound = true)
        {
            base.OnUI(data, isDefaultSound);
            this.MultiTabUI.OnUI();
        }

        public override void ScriptOffUI()
        {
            base.ScriptOffUI();
            this.MultiTabUI.OffAllTab();
        }
        public void SetTab(int index)
        {
            AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
            foreach (TabInven tab in lsTabs)
            {
                tab.TabOn(false);
            }
            lsTabs[index].TabOn(true);
            timerGo.SetActive(index == 0 ? true : false);
        }
    }


}