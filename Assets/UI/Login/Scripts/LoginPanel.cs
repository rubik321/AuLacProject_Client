using System.Collections;
using System.Collections.Generic;
using Pixelplacement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
namespace Rubik.UI
{
    public class LoginPanel : State
    {
        [SerializeField] GameObject panelLogin, panelSignUp,panelClass;
        [SerializeField] Toggle savePass;
        [SerializeField] LoginController loginController;
        // Start is called before the first frame update
        void Start()
        {
            passwordInput.contentType = TMP_InputField.ContentType.Password;
            savePass.onValueChanged.AddListener(delegate {
                ToggleValueChanged(savePass);
            });
            if (PlayerPrefs.GetInt("Save_User") == 1)
            {
                savePass.isOn = true;
                userNameInput.text =PlayerPrefs.GetString("User_Save");
                passwordInput.text= PlayerPrefs.GetString("Pass_Save");
            }
            else
            {
                savePass.isOn = false;
            }
            savePass.onValueChanged.AddListener((UnityAction<bool>)(call=>{
                Rubik.Common.AudioHelper.AudioCtrl.Instance.Play(Rubik.Common.AudioHelper.AudioName.UI_Checkbox);
            }));
        }

        public TMP_InputField userNameInput, passwordInput;
        public TextMeshProUGUI textError;
        void ValueChangeCheck()
        {

        }
        void ToggleValueChanged(Toggle change)
        {
            if (change.isOn)
            {
                PlayerPrefs.SetInt("Save_User", 1);
            }
            else
            {
                PlayerPrefs.SetInt("Save_User", 0);
                PlayerPrefs.SetString("User_Save", "");
                PlayerPrefs.SetString("Pass_Save", "");
            }
        }
        public void OnClickLogin()
        {
            Rubik.Common.AudioHelper.AudioCtrl.Instance.Play(Rubik.Common.AudioHelper.AudioName.UI_Button_Confirm);
            var userName = userNameInput.text;
            var password = passwordInput.text;
            if (userName.Length < 6)
            {
                ShowError("Username must > 6 character");
                return;
            }
            if (password.Length < 3)
            {
                ShowError("Password must > 3 character");
                return;
            }
            CheckSavePassword();
            loginController.Login(userName, password);
            //HUDCanvas.Instance.ShowLoadingPanel();
        }

        public void OnclickSignUp()
        {
           
        } 
        void ShowPanelLogin()
        {
            panelLogin.SetActive(true);
            panelSignUp.SetActive(false);
            panelClass.SetActive(false);
        }
        void ShowPanelSignUp()
        {
            panelLogin.SetActive(false);
            panelSignUp.SetActive(true);
            panelClass.SetActive(false);
        }
        public void ShowPanelClass()
        {
            panelLogin.SetActive(false);
            panelSignUp.SetActive(false);
            panelClass.SetActive(true);
        }
        public void btnSignUp_onclick()
        {
            Rubik.Common.AudioHelper.AudioCtrl.Instance.Play(Rubik.Common.AudioHelper.AudioName.UI_Button_Default);
            ShowPanelSignUp();
        }
        public void btnBack()
        {
            Rubik.Common.AudioHelper.AudioCtrl.Instance.Play(Rubik.Common.AudioHelper.AudioName.UI_Button_Back_Exit);
            ShowPanelLogin();
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
        public void CheckSavePassword()
        {
            if (savePass.isOn)
            {
                
                PlayerPrefs.SetString("User_Save", userNameInput.text);
                PlayerPrefs.SetString("Pass_Save", passwordInput.text);
            }
           
        }
        
    }
}