using System.Collections;
using System.Collections.Generic;
using Pixelplacement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace Rubik.UI
{
    public class RegisterPanel : MonoBehaviour
    {
        public TMP_InputField userNameInput, displayNameInput,passwordInput, passwordConfirmInput;
        public TextMeshProUGUI textError;
        [SerializeField] LoginController loginController;
        void Start()
        {
            passwordInput.contentType = TMP_InputField.ContentType.Password;
            passwordConfirmInput.contentType = TMP_InputField.ContentType.Password;
        }
        public void RegisterOnclick()
        {
            Rubik.Common.AudioHelper.AudioCtrl.Instance.Play(Rubik.Common.AudioHelper.AudioName.UI_Button_Confirm);
            var userName = userNameInput.text;
            var password = passwordInput.text;
            var passwordConfirm = passwordConfirmInput.text;
            var displayName = displayNameInput.text;
            // if (displayName.Length < 4)
            // {
            //     ShowError("Display name must > 3 character");
            //     return;
            // }
            if (userName.Length < 6)
            {
                ShowError("Username must > 6 character");
                return;
            }
            if (password.Length < 6)
            {
                ShowError("Password must > 6 character");
                return;
            }
            if (password != passwordConfirm)
            {
                ShowError("Password and password confirm not match");
                return;
            }
            loginController.Register(userName,password, displayName);
        }
        public void ShowError(string err)
        {
            textError.transform.parent.gameObject.SetActive(true);
            textError.text = err;

            //Utils.instant.DelayCall(3, () =>
            //{

            //    textError.transform.parent.gameObject.SetActive(false);

            //});

        }

    }
}


