using System;
using System.Collections;
using NTPackage.Functions;
using NTPackage.UI;
using Rubik.Common.AudioHelper;
using Rubik.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Rubik.Config
{
    // Scene
    public class SceneConfig
    {
        public const string Splash_Screen = "Splash";
        public const string Login_Screen = "Login";
        public const string WorldMap_Screen = "WorldMapScenes";
        public const string Battle_Screen = "Campaign";
        public const string AR_Screen = "ARScene";
    }

    public class SceneController : NTBehaviour
    {
        public static SceneController Instance;
        protected override void Awake()
        {
            base.Awake();
            if (Instance != null){
                NTLog.LogWarning("Only 1 Instance allow");
                return;
            }
            Instance = this;
        }

        public void LoadScene(string screenName, Action done = null){
            switch (screenName)
            {
                case SceneConfig.Splash_Screen:
                    AudioCtrl.Instance.PlaySplashSound();
                    break;
                case SceneConfig.Login_Screen:
                    AudioCtrl.Instance.PlayLoginSound();
                    break;
                case SceneConfig.WorldMap_Screen:
                    AudioCtrl.Instance.PlayWorldMapSound();
                    break;
                case SceneConfig.Battle_Screen:
                    AudioCtrl.Instance.PlayBattleSound();
                    break;
                default:
                    break;
            }
            StartCoroutine(IELoadScene(screenName, done));
        }

        public IEnumerator IELoadScene(string screenName, Action done = null){
            HUDCanvas.Instance.ShowLoadingPanel();
            if(screenName == SceneConfig.Battle_Screen){
                
            }else{
                HUDCanvas.Instance.ShowLoadingPanel();
            }
            bl_SceneLoaderManager.LoadScene(screenName);
            yield return WaitForSceneLoad(screenName);
            NTLog.LogMessage("Loading Screen: " + screenName);
            done?.Invoke();
            if(screenName == SceneConfig.Battle_Screen){
                PopupManager.Instance.OffUI(PopupCode.BattleLoadingUI);
            } 
            HUDCanvas.Instance.HideLoadingPanel();
        }

        public IEnumerator WaitForSceneLoad(string sceneName, Action done = null)
        {
            yield return new WaitUntil(() => SceneManager.GetSceneByName(sceneName).isLoaded);
            done?.Invoke();
        }
    }
}