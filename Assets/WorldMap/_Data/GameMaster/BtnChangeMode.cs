using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NTFunctions_old;
using TMPro;

namespace GOA.WorldMap
{
    public class BtnChangeMode : BaseButton
    {

        public Transform Region;
        public Transform Normal;

        protected override void Start() {
            this.Change();
        }

        protected override void OnClick()
        {
            base.OnClick();
            GameMaster.instance.ChangeViewMode();
            this.Change();
        }

        protected override void OnSound()
        {
            Rubik.Common.AudioHelper.AudioCtrl.Instance.Play(Rubik.Common.AudioHelper.AudioName.UI_Button_Worldmap_2);
        }

        public void Change(){
            if(GameMaster.instance.viewModeCode == ViewModeCode.per){
                this.Region.gameObject.SetActive(true);
                this.Normal.gameObject.SetActive(false);
            }else{
                this.Region.gameObject.SetActive(false);
                this.Normal.gameObject.SetActive(true);
            }
        }
    }
}
