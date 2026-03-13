using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NTFunctions_old;
using Rubik.UIController;
using Rubik.Combat;
using NTPackage.UI;

namespace GOA.UIWorldMap{
    public class BtnCharacterUI : BaseButton
    {
        protected override void OnClick()
        {
            base.OnClick();
          
            try
            {
                WorldMapUIController.Instance.OnButtonCharacter_Onclick();
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e);
            }
        }

        protected override void OnSound()
        {
           // Rubik.Common.AudioHelper.AudioCtrl.instance.Play(Rubik.Common.AudioHelper.AudioName.UI_Button_Worldmap_2);
        }
    }
}