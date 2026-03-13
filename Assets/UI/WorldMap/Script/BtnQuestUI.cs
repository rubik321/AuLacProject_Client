using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NTFunctions_old;

namespace GOA.UIWorldMap{
    public class BtnQuestUI : BaseButton
    {
        protected override void OnClick()
        {
            base.OnClick();
            try
            {
                UIManager.instance.OnButtonQuest_Onclick();
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e);
            }
        }

        protected override void OnSound()
        {
            Rubik.Common.AudioHelper.AudioCtrl.Instance.Play(Rubik.Common.AudioHelper.AudioName.UI_Button_Worldmap_2);
        }
    }
}