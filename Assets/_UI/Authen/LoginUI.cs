using NTPackage.UI;
using Rubik.Common.AudioHelper;
using Rubik.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rubik.Authen
{
    public class LoginUIAnim
    {
        public const string Anim_None = "None";
        public const string Anim_Login = "Login";
        public const string Anim_Register = "Register";
    }
    public class LoginUI : PopupUI
    {
        public LoginTab LoginTab;
        public RegisterTab RegisterTab;

        public Animator Anim;

        public override void OnUI(object data = null, bool isDefaultSound = true)
        {
            base.OnUI(data, isDefaultSound);
            this.LoginTab.OnUI();
            this.Anim.Play(LoginUIAnim.Anim_None);
            this.LoginTab.transform.SetAsLastSibling();
            this.RegisterTab.transform.SetAsFirstSibling();
        }

        public override void ScriptOffUI()
        {
            base.ScriptOffUI();
        }

        public void _OnclickQuickPlay()
        {
            AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
            if (Application.platform == RuntimePlatform.IPhonePlayer ||
           Application.platform == RuntimePlatform.OSXPlayer)
            {
                AppleAuthen.AppleAuthen.Instance.LoginGameCenter((playerId, playerName, success) =>
                {
                    if (success)
                    {
                        ServerManager.Instance.LoginByApple();
                    }
                    else
                    {
                        ServerManager.Instance.LoginByDeviceID(true);
                    }
                });
            }
            else
            {
                GoogleAuthen.GoogleAuthen.Instance.LoginGooglePlayGames((playerId, playerName, email, success) =>
                {
                    if (success)
                    {
                        ServerManager.Instance.LoginByGooglePlay();
                    }
                    else
                    {
                        ServerManager.Instance.LoginByDeviceID(true);
                    }
                });

            }

        }

        public void _OnClickLogin()
        {
            AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
            this.LoginTab.OnUI();
            this.Anim.Play(LoginUIAnim.Anim_Login);
            StartCoroutine(SetLogin());
        }

        public IEnumerator SetLogin()
        {
            yield return new WaitForSeconds(0.3f);
            this.LoginTab.transform.SetAsLastSibling();
        }

        public void _OnClickRegister()
        {
            AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
            this.RegisterTab.OnUI();
            this.Anim.Play(LoginUIAnim.Anim_Register);
            StartCoroutine(SetRegister());
        }

        public IEnumerator SetRegister()
        {
            yield return new WaitForSeconds(0.3f);
            this.RegisterTab.transform.SetAsLastSibling();
        }
    }
}
