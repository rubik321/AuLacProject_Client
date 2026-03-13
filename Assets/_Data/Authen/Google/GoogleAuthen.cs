#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif
using NTPackage.Functions;
using Rubik.Server;
using Rubik.UI;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

namespace Rubik.GoogleAuthen
{
    public class GoogleAuthen : NTBehaviour
    {
        public string Token;
        public string Error;

        public const string webClientId = "797925948665-jjj57crbn7n07l0pqlboh4kojr18dqvm.apps.googleusercontent.com";

        public static GoogleAuthen Instance;
        protected override void Awake()
        {
            base.Awake();
            if (GoogleAuthen.Instance != null)
            {
                NTLog.LogError("Only 1 Instance allow");
                return;
            }
            GoogleAuthen.Instance = this;
        }

        [NTButton]
        public void TestLoginGooglePlayGames()
        {
            this.LoginGooglePlayGames((playerId, playerName, email, success) =>
            {
                if(success)
                {
                    NTLog.LogMessage("Player ID: " + playerId);
                    NTLog.LogMessage("Player Name: " + playerName);
                    NTLog.LogMessage("Email: " + email);
                }
                else
                {
                    NTLog.LogError("Failed to login Google Play Games");
                }
            });
        }

        public string AuthCode;
        [NTButton]
        public void TestSendAuthCodeToServer()
        {
            StartCoroutine(ExchangeCodeForToken(AuthCode));
        }

        public string AccessToken;
        [NTButton]
        public void TestGetUserInfo()
        {
            StartCoroutine(GetUserInfo(AccessToken));
        }


        public void LoginGooglePlayGames(Action<string, string, string, bool> done = null)
        {
#if UNITY_ANDROID
            NTLog.LogMessage("LoginGooglePlayGames");
            HUDCanvas.Instance.ShowLoadingPanel();
            try
            {
                PlayGamesPlatform.Activate();
                PlayGamesPlatform.Instance.Authenticate((success) =>
                {
                    if (success == SignInStatus.Success)
                    {
                        string playerId = PlayGamesPlatform.Instance.GetUserId();
                        string playerName = Social.localUser.userName;
                        NTLog.LogMessage("Player ID: " + playerId);
                        NTLog.LogMessage("Player Name: " + playerName);
                        this.GetEmail((email) =>
                        {
                            if(email != null && email.Length > 0)
                            {
                                done?.Invoke(playerId, playerName, email, true);
                                done = null;
                                HUDCanvas.Instance.HideLoadingPanel();
                            }
                            else
                            {
                                done?.Invoke(null, null, null, false);
                                done = null;
                                HUDCanvas.Instance.HideLoadingPanel();
                            }
                        });
                    }
                    else
                    {
                        Error = "Failed to retrieve Google play games authorization code";
                        NTLog.LogMessage("Login Unsuccessful");
                        done?.Invoke(null, null, null,false);
                        HUDCanvas.Instance.HideLoadingPanel();
                    }
                });
            }
            catch (System.Exception e)
            {
                HUDCanvas.Instance.HideLoadingPanel();
                NTLog.LogError("Google Play Games login failed: " + e.Message);
                done?.Invoke(null, null, null,false);
                done = null;
                return;
            }
            HUDCanvas.Instance.HideLoadingPanel();

#else
            done?.Invoke(null, null, null, false);
            done = null;
#endif
        }

        private void GetEmail(Action<string> done = null)
        {
#if UNITY_ANDROID
            PlayGamesPlatform.Instance.RequestServerSideAccess(true, new List<AuthScope> { AuthScope.EMAIL, AuthScope.PROFILE, AuthScope.OPEN_ID }, authResponse =>
            {
                StartCoroutine(ExchangeCodeForToken(authResponse.GetAuthCode(), done));
            });
#else
            done?.Invoke(null);
#endif
        }

        IEnumerator ExchangeCodeForToken(string code, Action<string> done = null)
        {
            WWWForm form = new WWWForm();
            form.AddField("code", code);
            form.AddField("client_id", webClientId);
            form.AddField("client_secret", "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");
            // form.AddField("redirect_uri", "797925948665-hrm6ol0kmom0bvqeovr806qgc8f8tqj4.apps.googleusercontent.com:/auth");
            form.AddField("redirect_uri", "https://localhost");
            form.AddField("grant_type", "authorization_code");

            using (UnityWebRequest request = UnityWebRequest.Post("https://oauth2.googleapis.com/token", form))
            {
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    NTLog.LogMessage("Token response: " + request.downloadHandler.text);

                    // Parse access_token từ JSON
                    JSONNode tokenData = JSON.Parse(request.downloadHandler.text);
                    NTLog.LogMessage("Token data: " + tokenData["access_token"].Value);
                    StartCoroutine(GetUserInfo(tokenData["access_token"].Value, (email)=>{
                        done?.Invoke(email);
                    }));
                }
                else
                {
                    NTLog.LogError("Token exchange failed: " + request.error + "\n" + request.downloadHandler.text);
                }
            }
        }

        IEnumerator GetUserInfo(string accessToken, Action<string> done = null)
        {
            UnityWebRequest request = UnityWebRequest.Get("https://www.googleapis.com/oauth2/v2/userinfo?alt=json");
            request.SetRequestHeader("Authorization", "Bearer " + accessToken);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                NTLog.LogMessage("User info: " + request.downloadHandler.text);
                JSONNode userInfo = JSON.Parse(request.downloadHandler.text);
                done?.Invoke(userInfo["email"].Value);
            }
            else
            {
                NTLog.LogError("Failed to get user info: " + request.responseCode + "\n" + request.downloadHandler.text);
                done?.Invoke("");
            }
        }

        [System.Serializable]
        public class TokenResponse
        {
            public string access_token;
            public string refresh_token;
            public string expires_in;
            public string token_type;
            public string id_token;
        }

    }
}