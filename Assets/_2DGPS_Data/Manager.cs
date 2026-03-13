using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using NTPackage_old.Functions;
using NTPackage_old.UI;
using NTPackage_old.EventDispatcher;
using NTFunctions_old;

namespace Rubik._2DGPS.Manager
{
    using DataCenter;
    using ServerGame;
    using Account;
    using UserData;
    using Card;
    using WorldMap;
    using Campaign;
    using Rubik._2DGPS.Gear;
    using Rubik._2DGPS.Character;

    public class Manager : LoadBehaviour
    {
        public const string KeyDeviceID = "_2DGPS:DeviceID";
        public const string KeyLastServer = "_2DGPS:LastServer";
        public const int MaxLoad = 12;

        public bool IsLoad = false;

        public static Manager instance;
        protected override void Awake()
        {
            base.Awake();
            if (Manager.instance != null){
               NTLog.LogWarning("Only 1 instance allow");
               return;
             }
            Manager.instance = this;
        }

        // Start is called before the first frame update
        protected override void Start()
        {
            if(IsLoad) return;
            StartCoroutine(LoadData());
        }

        public IEnumerator Play(int server){
            yield return UserDataManager.instance.Login(server);
            yield return CardManager.instance.Init();
            yield return CharacterManager.instance.Init();
            yield return GearManager.instance.Init();
            bl_SceneLoaderManager.LoadScene("WorldMapScenes");
        }

        public IEnumerator LoadData(){
            this.IsLoad = true;
            yield return new WaitForSeconds(1);
            yield return DataCenterManager.instance.CheckVersion();
            yield return ServerGameManager.instance.LoadData();
            yield return UserDataManager.instance.LoadData();
            yield return CardManager.instance.LoadData();
            yield return WorldMapManager.instance.LoadData();
            yield return CampaignManager.instance.LoadData();
            yield return CharacterManager.instance.LoadData();
            yield return GearManager.instance.LoadData();
            StartCoroutine(LoginByDeviceID(UnityEngine.SystemInfo.deviceUniqueIdentifier));
        }

        // public IEnumerator AutoLogin(){
        //     EventListenerManager.instance.PostEvent(EventCode.FishKingdom_DoneLoad, new LoadingData(7f/MaxLoad, "AutoLogin"));
        //     string deviceID = PlayerPrefs.GetString(KeyDeviceID);
        //     if(deviceID == null || deviceID.Length == 0){
        //         PopupManager.instance.OffUI(PopupCode.LoadingUI);
        //         PopupManager.instance.OnUI(PopupCode.LoginUI);
        //     }else{
        //         PopupManager.instance.OffUI(PopupCode.LoginUI);
        //         yield return this.LoginByDeviceID(deviceID);
        //     }
        //     yield return null;
        // }

        public IEnumerator LoginByDeviceID(string deviceID){
            yield return AccountManager.instance.LoginByDeviceID(deviceID);
            PlayerPrefs.SetString(KeyDeviceID, deviceID);
            AutoJoinServer();
        }

        public void AutoJoinServer(){
            StartCoroutine(Play(0));
            // int lastServer = PlayerPrefs.GetInt(KeyLastServer);
            // if(lastServer <= 0){
            //     PopupManager.instance.OffUI(PopupCode.LoadingUI);
            //     PopupManager.instance.OnUI(PopupCode.ChoseServerUI);
            // }else{
            //     PopupManager.instance.OffUI(PopupCode.ChoseServerUI);
            //     StartCoroutine(Play(lastServer));
            // }
        }

        public void ChoseServer(int server){
            StartCoroutine(Play(server));
            PlayerPrefs.SetInt(KeyLastServer, server);
        }

        [Button]
        public void LogOut(){
            PlayerPrefs.DeleteKey(KeyDeviceID);
            PlayerPrefs.DeleteKey(KeyLastServer);
        }

    }
}
