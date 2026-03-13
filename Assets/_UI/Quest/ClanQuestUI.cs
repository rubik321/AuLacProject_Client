using NTPackage.Functions;
using NTPackage.UI;
using UnityEngine;

namespace Rubik.Quest
{
    public class ClanQuestUI : PopupUI
    {
        public ClanQuestTab ClanQuestTab;

        [NTButton]
        public void TestOnUI(){
            this.OnUI();
        }

        public override void OnUI(object data = null, bool isDefaultSound = true)
        {
            base.OnUI(data, isDefaultSound);
            this.ClanQuestTab.UpdateData();
        }

        public override void ScriptOffUI()
        {
            base.ScriptOffUI();
            this.ClanQuestTab.OffUI();
        }
    }
}