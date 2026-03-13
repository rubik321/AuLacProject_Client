using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NTFunctions_old;
using GOA.Config;
using Rubik.Chat;
using GOA.LandEvent;

namespace GOA.UIMenu{
    public class NoticLogoutUI : PopupUI
    {
        public override void LoadComponents()
        {
            base.LoadComponents();
            this.lvUI = new PopupLv().GetValue(transform.name);
        }
        
        public void OnUI(){
            if(!this.CanShow()) return;
            this.Show();
        }

        public void OnclickYes(){
            //UnityEngine.SceneManagement.SceneManager.LoadScene(Configs.Login_Screen);
            try
            {
                bl_SceneLoaderManager.LoadScene(Configs.Login_Screen);
                Rubik.Common.AudioHelper.AudioCtrl.Instance.Play(Rubik.Common.AudioHelper.AudioName.Test);
                ChatManager.Instance.UnInit();
            }
            catch (System.Exception e)
            {
                NTPackage_old.Functions.NTLog.LogError(e.ToString(), gameObject);
            }
        }
        public void OnclickNo(){
            this.OffUI();
        }
    }
}