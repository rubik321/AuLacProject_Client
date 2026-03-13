using System;
using System.Collections;
using System.Collections.Generic;
using NTPackage.Functions;
using Rubik.Manager;
using Rubik.Config;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using Rubik.UI;

namespace Rubik.Account
{
    public class AccountConfig
    {
        public const string API_Account_Login = "/api/2D_GPS/account/login";
        public const string API_Account_Register = "/api/2D_GPS/account/register";
        public const string API_Account_LoginByDeviceID = "/api/2D_GPS/account/login_by_device_id";
        public const string API_Account_LoginByToken = "/api/2D_GPS/account/login_by_token";
        public const string API_Account_Delete_Account = "/api/2D_GPS/account/delete_account";
        public const string API_Account_Link_Account = "/api/2D_GPS/account/link_account";
        public const string API_Account_Confirm_Delete_Account = "/api/2D_GPS/account/confirm_delete_account";
        public const string API_Account_LoginByGooglePlay = "/api/2D_GPS/account/login_by_google_play";
        public const string API_Account_LinkGooglePlay = "/api/2D_GPS/account/link_google_play";
        public const string API_Account_UnLinkGooglePlay = "/api/2D_GPS/account/un_link_google_play";
        public const string API_Account_LoginByApple = "/api/2D_GPS/account/login_by_apple";
        public const string API_Account_LinkApple = "/api/2D_GPS/account/link_apple";


        public const string KeyToken = "2D_GPS:KeyToken";
        public const string IsAutoLogin = "2D_GPS:IsAutoLogin";

        public static Regex ValidPattern = new Regex("^[a-zA-Z0-9_]{6,24}$");
        public static string ErrorColor = "D43749";
        public static string SuccessColor = "2EE021";
    }
    public class AccountManager : NTBehaviour
    {
        public Account Account;

        public static AccountManager Instance;
        protected override void Awake()
        {
            base.Awake();
            if (AccountManager.Instance != null)
            {
                NTLog.LogWarning("Only 1 instance allow");
                return;
            }
            AccountManager.Instance = this;
        }

        protected override void Start()
        {
            base.Start();
        }

        public void Logout()
        {
            this.Account = new Account();
            this.SetIsAutoLoginSelection(false);
            this.SetToken("");
        }

        public void LinkGooglePlay(Action<bool> done = null)
        {
            GoogleAuthen.GoogleAuthen.Instance.LoginGooglePlayGames((playerId, playerName, email, success) =>
            {
                if (success)
                {
                    StartCoroutine(IELinkGooglePlay(playerId, playerName, email, (response) =>
                    {
                        done?.Invoke(success);
                        done = null;
                    }));
                }
                else
                {
                    HUDCanvas.Instance.ShowNotification(Lean.Localization.LeanLocalization.GetTranslationText("link_google_play_error_content", "Failed to link Google Play account"), Lean.Localization.LeanLocalization.GetTranslationText("link_google_play_error_title", "Link Google Play"));
                }
            });
        }

        public void UnLinkGooglePlay(Action<bool> done = null)
        {
            GoogleAuthen.GoogleAuthen.Instance.LoginGooglePlayGames((playerId, playerName, email, success) =>
            {
                if (success)
                {
                    StartCoroutine(IEUnLinkGooglePlay(email, (response) =>
                    {
                        done?.Invoke(success);
                        done = null;
                    }));
                }
                else
                {
                    HUDCanvas.Instance.ShowNotification(Lean.Localization.LeanLocalization.GetTranslationText("un_link_google_play_error_content", "Failed to unlink Google Play account"), Lean.Localization.LeanLocalization.GetTranslationText("un_link_google_play_error_title", "Unlink Google Play"));
                }
            });
        }

        public void LoginGooglePlay(Action<AuthenResponse> done = null)
        {
            GoogleAuthen.GoogleAuthen.Instance.LoginGooglePlayGames((playerId, playerName, email, success) =>
            {
                if (success)
                {
                    StartCoroutine(IELoginByGooglePlay(playerId, playerName, email, (response) =>
                    {
                        done?.Invoke(response);
                        done = null;
                    }));
                }
                else
                {
                    // HUDCanvas.Instance.ShowNotification(Lean.Localization.LeanLocalization.GetTranslationText("login_google_play_error", "Failed to login Google Play account"), Lean.Localization.LeanLocalization.GetTranslationText("login_google_play_error_title", "Login Google Play"));
                    AuthenResponse authenResponse = new AuthenResponse();
                    authenResponse.Status = 0;
                    authenResponse.Message = Lean.Localization.LeanLocalization.GetTranslationText("login_google_play_error", "Failed to login Google Play account");
                    done?.Invoke(authenResponse);
                    done = null;
                }
            });
        }

        public void LoginGameCenter(Action<AuthenResponse> done = null)
        {
            AppleAuthen.AppleAuthen.Instance.LoginGameCenter((playerId, playerName, success) =>
            {
                if (success)
                {
                    StartCoroutine(IELoginByApple(playerId, playerName, (response) =>
                    {
                        done?.Invoke(response);
                        done = null;
                    }));
                }
                else
                {
                    HUDCanvas.Instance.ShowNotification(Lean.Localization.LeanLocalization.GetTranslationText("login_apple_error", "Failed to login Apple account"), Lean.Localization.LeanLocalization.GetTranslationText("login_apple_error_title", "Login Apple"));
                }
            });
        }

        public void LinkGameCenter(Action<bool> done = null)
        {
            AppleAuthen.AppleAuthen.Instance.LoginGameCenter((playerId, playerName, success) =>
            {
                if (success)
                {
                    StartCoroutine(IELinkApple(playerId, playerName, (response) =>
                    {
                        done?.Invoke(success);
                        done = null;
                    }));
                }
                else
                {
                    HUDCanvas.Instance.ShowNotification(Lean.Localization.LeanLocalization.GetTranslationText("link_apple_error", "Failed to link Apple account"), Lean.Localization.LeanLocalization.GetTranslationText("link_apple_error_title", "Link Apple"));
                }
            });
        }

        public IEnumerator IELogin(string username, string password, Action<AuthenResponse> done = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["username"] = username;
            jdata["password"] = password;
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + AccountConfig.API_Account_Login, (data) =>
            {
                jdata = JSONNode.Parse(data.downloadHandler.text);
                AuthenResponse authenResponse = JsonUtility.FromJson<AuthenResponse>(jdata["Data"].ToString());
                if (authenResponse.Status == 1)
                {
                    this.Account = authenResponse.Account;
                    this.SetToken(authenResponse.Token);
                    done?.Invoke(authenResponse);
                    done = null;
                }
                done?.Invoke(authenResponse);
                done = null;
            });
            if(done != null){
                AuthenResponse authenResponse = new AuthenResponse();
                authenResponse.Status = 0;
                authenResponse.Message = "login_fail"; // Login failed. Please check your connection and try again!
                done?.Invoke(authenResponse);
                done = null;
            }
        }

        public IEnumerator IERegister(string username, string password, string email, Action<AuthenResponse> done = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["email"] = email;
            jdata["username"] = username;
            jdata["password"] = password;
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + AccountConfig.API_Account_Register, (data) =>
            {
                jdata = JSONNode.Parse(data.downloadHandler.text);
                AuthenResponse authenResponse = JsonUtility.FromJson<AuthenResponse>(jdata["Data"].ToString());
                if (authenResponse.Status == 1)
                {
                    this.Account = authenResponse.Account;
                    this.SetToken(authenResponse.Token);
                    done?.Invoke(authenResponse);
                    done = null;
                }
                done?.Invoke(authenResponse);
                done = null;
            });
            if(done != null){
                AuthenResponse authenResponse = new AuthenResponse();
                authenResponse.Status = 0;
                authenResponse.Message = "register_fail"; // Register failed. Please check your connection and try again!
                done?.Invoke(authenResponse);
                done = null;
            }
        }

        public IEnumerator IELinkAccount(string username, string password, Action<AuthenResponse> done = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["accountID"] = this.Account._id;
            jdata["username"] = username;
            jdata["password"] = password;
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + AccountConfig.API_Account_Link_Account, (data) =>
            {
                jdata = JSONNode.Parse(data.downloadHandler.text);
                AuthenResponse authenResponse = JsonUtility.FromJson<AuthenResponse>(jdata["Data"].ToString());
                if (authenResponse.Status == 1)
                {
                    this.Account = authenResponse.Account;
                    this.SetToken(authenResponse.Token);
                    done?.Invoke(authenResponse);
                    done = null;
                }
                done?.Invoke(authenResponse);
                done = null;
            });
        }

        public IEnumerator LoginByDeviceID(string deviceID, Action<AuthenResponse> done = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["deviceID"] = deviceID;
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + AccountConfig.API_Account_LoginByDeviceID, (data) =>
            {
                jdata = JSONNode.Parse(data.downloadHandler.text);
                AuthenResponse authenResponse = JsonUtility.FromJson<AuthenResponse>(jdata["Data"].ToString());
                if (authenResponse.Status == 1)
                {
                    this.Account = authenResponse.Account;
                    this.SetToken(authenResponse.Token);
                    done?.Invoke(authenResponse);
                    done = null;
                }
                done?.Invoke(authenResponse);
            });
            if(done != null){
                AuthenResponse authenResponse = new AuthenResponse();
                authenResponse.Status = 0;
                authenResponse.Message = "login_fail"; // Login failed. Please check your connection and try again!
                done?.Invoke(authenResponse);
                done = null;
            }
        }

        public IEnumerator LoginByToken(string token, Action<AuthenResponse> done = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["token"] = token;
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + AccountConfig.API_Account_LoginByToken, (data) =>
            {
                jdata = JSONNode.Parse(data.downloadHandler.text);
                AuthenResponse authenResponse = JsonUtility.FromJson<AuthenResponse>(jdata["Data"].ToString());
                if (authenResponse.Status == 1)
                {
                    this.Account = authenResponse.Account;
                    this.SetToken(authenResponse.Token);
                    done?.Invoke(authenResponse);
                    done = null;
                }
                done?.Invoke(authenResponse);
            });
            if(done != null){
                AuthenResponse authenResponse = new AuthenResponse();
                authenResponse.Status = 0;
                authenResponse.Message = "login_fail"; // Login failed. Please check your connection and try again!
                done?.Invoke(authenResponse);
                done = null;
            }
        }

        public IEnumerator IEDeleteAccount(string username, string password, Action<AuthenResponse> done = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["username"] = username;
            jdata["password"] = password;
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + AccountConfig.API_Account_Delete_Account, (data) =>
            {
                jdata = JSONNode.Parse(data.downloadHandler.text);
                AuthenResponse authenResponse = JsonUtility.FromJson<AuthenResponse>(jdata["Data"].ToString());
                if (authenResponse.Status == 1)
                {
                    done?.Invoke(authenResponse);
                    done = null;
                }
                done?.Invoke(authenResponse);
            });
        }

        public IEnumerator IEConfirmDeleteAccount(Action<AuthenResponse> done = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["accountID"] = this.Account._id;
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + AccountConfig.API_Account_Confirm_Delete_Account, (data) =>
            {
                jdata = JSONNode.Parse(data.downloadHandler.text);
                AuthenResponse authenResponse = JsonUtility.FromJson<AuthenResponse>(jdata["Data"].ToString());
                done?.Invoke(authenResponse);
                done = null;
            });
        }

        public IEnumerator IELoginByGooglePlay(string token, string displayName, string email, Action<AuthenResponse> done = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["token"] = token;
            jdata["displayName"] = displayName;
            jdata["email"] = email;
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + AccountConfig.API_Account_LoginByGooglePlay, (data) =>
            {
                jdata = JSONNode.Parse(data.downloadHandler.text);
                AuthenResponse authenResponse = JsonUtility.FromJson<AuthenResponse>(jdata["Data"].ToString());
                if (authenResponse.Status == 1)
                {
                    this.Account = authenResponse.Account;
                    this.SetToken(authenResponse.Token);
                    done?.Invoke(authenResponse);
                    done = null;
                }
                done?.Invoke(authenResponse);
            });
            if(done != null){
                AuthenResponse authenResponse = new AuthenResponse();
                authenResponse.Status = 0;
                authenResponse.Message = "login_fail"; // Login failed. Please check your connection and try again!
                done?.Invoke(authenResponse);
                done = null;
            }
        }

        public IEnumerator IELinkGooglePlay(string token, string displayName, string email, Action<AuthenResponse> done = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["accountID"] = this.Account._id;
            jdata["token"] = token;
            jdata["displayName"] = displayName;
            jdata["email"] = email;
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + AccountConfig.API_Account_LinkGooglePlay, (data) =>
            {
                jdata = JSONNode.Parse(data.downloadHandler.text);
                AuthenResponse authenResponse = JsonUtility.FromJson<AuthenResponse>(jdata["Data"].ToString());
                if (authenResponse.Status == 1)
                {
                    this.Account = authenResponse.Account;
                    this.SetToken(authenResponse.Token);
                    done?.Invoke(authenResponse);
                    done = null;
                }
                done?.Invoke(authenResponse);
            });
        }

        public IEnumerator IEUnLinkGooglePlay(string email, Action<AuthenResponse> done = null){
            JSONNode jdata = new JSONObject();
            jdata["accountID"] = this.Account._id;
            jdata["email"] = email;
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + AccountConfig.API_Account_UnLinkGooglePlay, (data) =>
            {
                jdata = JSONNode.Parse(data.downloadHandler.text);
                AuthenResponse authenResponse = JsonUtility.FromJson<AuthenResponse>(jdata["Data"].ToString());   
                if (authenResponse.Status == 1)
                {
                    done?.Invoke(authenResponse);
                    done = null;
                }
                done?.Invoke(authenResponse);
            });
        }

        public IEnumerator IELoginByApple(string token, string displayName, Action<AuthenResponse> done = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["token"] = token;
            jdata["displayName"] = displayName;
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + AccountConfig.API_Account_LoginByApple, (data) =>
            {
                jdata = JSONNode.Parse(data.downloadHandler.text);
                AuthenResponse authenResponse = JsonUtility.FromJson<AuthenResponse>(jdata["Data"].ToString());
                if (authenResponse.Status == 1)
                {
                    this.Account = authenResponse.Account;
                    this.SetToken(authenResponse.Token);
                    done?.Invoke(authenResponse);
                    done = null;
                }
                done?.Invoke(authenResponse);
            });
            if(done != null){
                AuthenResponse authenResponse = new AuthenResponse();
                authenResponse.Status = 0;
                authenResponse.Message = "login_fail"; // Login failed. Please check your connection and try again!
                done?.Invoke(authenResponse);
                done = null;
            }
        }

        public IEnumerator IELinkApple(string token, string displayName, Action<AuthenResponse> done = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["accountID"] = this.Account._id;
            jdata["token"] = token;
            jdata["displayName"] = displayName;
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + AccountConfig.API_Account_LinkApple, (data) =>
            {
                jdata = JSONNode.Parse(data.downloadHandler.text);
                AuthenResponse authenResponse = JsonUtility.FromJson<AuthenResponse>(jdata["Data"].ToString());
                if (authenResponse.Status == 1)
                {
                    this.Account = authenResponse.Account;
                    this.SetToken(authenResponse.Token);
                    done?.Invoke(authenResponse);
                    done = null;
                }
                done?.Invoke(authenResponse);
            });
        }

        #region Getter
        public string GetToken()
        {
            return PlayerPrefs.GetString(AccountConfig.KeyToken, "");
        }

        public bool IsAutoLogin()
        {
            return true;
            // return PlayerPrefs.GetInt(AccountConfig.IsAutoLogin) == 1 && this.GetToken() != "";
        }

        public bool IsAutoLoginSelection()
        {
            return PlayerPrefs.GetInt(AccountConfig.IsAutoLogin) == 1;
        }

        public bool IsLinkAccount()
        {
            if (this.IsLinkMyrk())
            {
                return true;
            }
            if (this.IsLinkGooglePlay())
            {
                return true;
            }
            if (this.Account.GoogleEmail != null && this.Account.GoogleEmail != "")
            {
                return true;
            }
            if (this.Account.AppleID != null && this.Account.AppleID != "")
            {
                return true;
            }
            return false;
        }

        public bool IsLinkMyrk()
        {
            if (this.Account.Username != null && this.Account.Username != "")
            {
                return true;
            }
            return false;
        }

        public bool IsLinkGooglePlay()
        {
            if (this.Account.GooglePlayID != null && this.Account.GooglePlayID != "")
            {
                return true;
            }
            return false;
        }

        public bool IsLinkGameCenter()
        {
            if (this.Account.AppleID != null && this.Account.AppleID != "")
            {
                return true;
            }else{
                return false;
            }
        }

        public bool IsLogin()
        {
            if (this.Account == null || this.Account._id == null || this.Account._id == "") return false;
            return true;
        }

        #endregion

        #region Setter
        public void SetToken(string token)
        {
            PlayerPrefs.SetString(AccountConfig.KeyToken, token);
        }

        public void SetIsAutoLoginSelection(bool isAutoLogin)
        {
            PlayerPrefs.SetInt(AccountConfig.IsAutoLogin, isAutoLogin ? 1 : 0);
        }

        #endregion

    }
}