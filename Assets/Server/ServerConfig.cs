using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerConfig : MonoBehaviour
{
    //public static string CHAT_URL = "ws://34.87.155.178:8088";
    public static string CHAT_URL = "ws://103.167.89.114:3000";

    public const string URL_SYNCDATA = "/user/SyncData";
    public const string URL_LOGIN = "/user/LoginDevice";
    public const string URL_LINKFACEBOOK = "/user/LinkFacebook";
    public const string URL_TEST_REMOVEDATA = "/user/ResetDataPlayer";
}
