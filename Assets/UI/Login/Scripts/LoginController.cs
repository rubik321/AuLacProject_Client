using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using BestHTTP;
using Newtonsoft.Json;
using System;
using Colyseus;
using SimpleJSON;
using Sirenix.OdinInspector;
using Rubik.UI;
using GOA.Config;
using GOA.UserData;
using Rubik.Chat;

public class LoginController : MonoBehaviour
{
    string jsonRegister,jsonlogin,userID;
    public GameObject gLogin;
    //public LoadingScene loading;
    //[SerializeField] GameObject panelLock;
    //[SerializeField] GameObject buttonLoginQR;
    //[SerializeField] Text textVer;
    // [SerializeField] GameObject panelLoading;
    // Start is called before the first frame update
    AudioClip clipBgmMain;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(Application.targetFrameRate <= 50)
        {
            Application.targetFrameRate = 50;
        }
    }
    [Button]
    public void Register(string userName,string pass, string displayName)
    {
        UserRegister user = new UserRegister();
        //user.email = "tuanva@gmail.com";
        //user.userName = "tuanva";
        //user.passWord = "123456";
        user.email = "tuanva@gmail.com";
        user.userName = userName;
        user.passWord = pass;
        user.displayName = displayName;
        jsonRegister = JsonUtility.ToJson(user);
        StartCoroutine(WaitForregister());
    }

    private IEnumerator WaitForregister()
    {
        //panelLock.SetActive(true);
        var uwr = new UnityWebRequest(SeverConfigs.BASE_API_URL+SeverConfigs.BASE_API_REGISTER_BEFE, "POST");
        // var uwr = new UnityWebRequest("https://api/sign-in", "POST");

        //var uwr = new UnityWebRequest(Config.IsTest ? Config.testAuth : Config.serverPublishLogin, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonRegister);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");
        Debug.Log(jsonRegister);
        //Send the request then wait here until it returns
        HUDCanvas.Instance.ShowLoadingPanel();
        yield return uwr.SendWebRequest();
        HUDCanvas.Instance.HideLoadingPanel();
        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
            //panelLock.SetActive(false);
            //GameController.Instance.IsLogout = false;
            //NoticePopup.ins.ShowError("Please check your connection!");
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            
            JSONNode info = JSON.Parse(uwr.downloadHandler.text);
            string resultLG = info["Status"];
            if (resultLG == "0")
            {
                string mess = info["Error"]["Message"];
                HUDCanvas.Instance.ShowNotification(mess);

            }
            else
            {
                
               
                UserData.Instance.GetUserData(info);
                UserData.Instance.GetInventoryData(info);
                // userID = info["Data"]["UserID"];
                gLogin.GetComponent<Rubik.UI.LoginPanel>().ShowPanelClass();
                PlayerPrefs.SetInt("FirstLoginApp"+UserData.Instance.data.UserName, 1);
                QuestManager.Instance.SetDailyQuest();
            }
        }
    }
    public void Login(string userName, string pass)
    {
        UserRegister user = new UserRegister();
        user.email = userName;
        //user.userName = "tuanva";
        //user.passWord = "123456";
        //user.email = "tuanva@gmail.com";
        //user.userName = userName;
        user.passWord = pass;
        jsonlogin = JsonUtility.ToJson(user);
        StartCoroutine(WaitForLogin());
    }
    private IEnumerator WaitForLogin()
    {
        //panelLock.SetActive(true);
        var uwr = new UnityWebRequest(SeverConfigs.BASE_API_URL + SeverConfigs.BASE_API_LOGIN_BEFE, "POST");
        // var uwr = new UnityWebRequest("https://api/sign-in", "POST");

        //var uwr = new UnityWebRequest(Config.IsTest ? Config.testAuth : Config.serverPublishLogin, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonlogin);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");
        Debug.Log(jsonlogin);
        //Send the request then wait here until it returns
        HUDCanvas.Instance.ShowLoadingPanel();
        yield return uwr.SendWebRequest();
        HUDCanvas.Instance.HideLoadingPanel();
        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
            //panelLock.SetActive(false);
            //GameController.Instance.IsLogout = false;
            //NoticePopup.ins.ShowError("Please check your connection!");
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);

            JSONNode info = JSON.Parse(uwr.downloadHandler.text);
            string resultLG = info["Status"];
            string charData = info["Data"]["CharacterData"];
            string gearData = info["Data"]["GearData"];
            //Debug.Log("Login result: " + resultLG);
            if (resultLG == "0")
            {
                string mess = info["Error"]["Message"];
                HUDCanvas.Instance.ShowNotification(mess);

            }
            else 
            {
                UserData.Instance.GetUserData(info);
                ChatManager.Instance.Init();
                if (charData == null||gearData==null)
                {
                    userID = info["Data"]["UserData"]["UserID"];
                    UserData.Instance.data.UserId = userID;
                    Debug.Log(userID);
                    gLogin.GetComponent<Rubik.UI.LoginPanel>().ShowPanelClass();
                }
                else
                {
                    string temp = info["Data"]["Notify"]["Title"];
                    Debug.Log("Notifly: " + info["Data"]["Notify"]);
                    if (!string.IsNullOrEmpty(temp) )
                    {
                        UserData.Instance.isNotifyShow = true;
                        HUDCanvas.Instance.ShowNotification(info["Data"]["Notify"]["Content"], info["Data"]["Notify"]["Title"], null, () => {
                            UserData.Instance.isNotifyShow = false;
                        });
                    }
                    // UserData.Instance.GetUserData(info);
                    UserData.Instance.GetCharacterData(info);
                    UserData.Instance.GetInventoryData(info);
                    UserData.Instance.gearData = JsonUtility.FromJson<GearDatas>(uwr.downloadHandler.text);

                    foreach (GearData data in UserData.Instance.gearData.Data.GearData)
                    {
                        data.SetUp();
                    }
                    UserData.Instance.GetStatsGearData();
                    UserData.Instance.GetSkillsData(info["Data"].ToString());
                    //UnityEngine.SceneManagement.SceneManager.LoadScene(Configs.WorldMap_Screen);
                    try
                    {
                        bl_SceneLoaderManager.LoadScene(Configs.WorldMap_Screen);
                        Rubik.Common.AudioHelper.AudioCtrl.Instance.Play(Rubik.Common.AudioHelper.AudioName.Test);
                    }
                    catch (System.Exception e)
                    {
                        NTPackage_old.Functions.NTLog.LogError(e.ToString(), gameObject);
                    }
                }
            }
        }
    }
    private IEnumerator Waitforclass()
    {
        //panelLock.SetActive(true);
        var uwr = new UnityWebRequest(SeverConfigs.BASE_API_URL + SeverConfigs.BASE_API_SELECT_CHARACTER, "POST");
        // var uwr = new UnityWebRequest("https://api/sign-in", "POST");

        //var uwr = new UnityWebRequest(Config.IsTest ? Config.testAuth : Config.serverPublishLogin, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonClass);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");
        Debug.Log(jsonClass);
        //Send the request then wait here until it returns
        HUDCanvas.Instance.ShowLoadingPanel();
        yield return uwr.SendWebRequest();
        HUDCanvas.Instance.HideLoadingPanel();
        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
            //panelLock.SetActive(false);
            //GameController.Instance.IsLogout = false;
            //NoticePopup.ins.ShowError("Please check your connection!");
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);

            JSONNode info = JSON.Parse(uwr.downloadHandler.text);
            string resultLG = info["Status"];
            if (resultLG == "0")
            {
                string mess = info["Error"]["Message"];
                HUDCanvas.Instance.ShowNotification(mess);
                //panelLock.SetActive(false);
            }
            else
            {
                //UserData.Instance.GetUserData(info);
               
                UserData.Instance.GetCharacterData(info);
                // UserData.Instance.GetInventoryData(info);
                try
                {
                    bl_SceneLoaderManager.LoadScene(Configs.WorldMap_Screen);
                    Rubik.Common.AudioHelper.AudioCtrl.Instance.Play(Rubik.Common.AudioHelper.AudioName.Test);
                }
                catch (System.Exception e)
                {
                    NTPackage_old.Functions.NTLog.LogError(e.ToString(), gameObject);
                }
            }

        }
    }
    string jsonClass;
    public void ChooseClass(int index = 0)
    {
        UserClass user = new UserClass();
        user.charID = index.ToString();
        user.userID = UserData.Instance.data.UserId;
        jsonClass = JsonUtility.ToJson(user);
        StartCoroutine(Waitforclass());
    }
}
