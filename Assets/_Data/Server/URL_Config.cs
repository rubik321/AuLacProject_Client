using System.Collections;
using System.Collections.Generic;
using Rubik.SystemData;
using UnityEngine;

namespace Rubik.Config
{
    public class URL_Config
    {
        public const string Test_Sever = "http://15.235.180.137:4080";
        public const string LocalHost_Sever = "http://localhost:8080";
        public static string BASE_API_URL
        {
            get
            {
                return SystemManager.Instance.SystemData.API_Url;
                // return LocalHost_Sever;
                // return DataCenterManager.instance.SystemData.API_Url;
                // return Live_Sever;
                // return Test_Sever;
                // return NT_Sever;
            }
        }

        public const string Test_Sever_Socket = "ws://15.235.180.137:4000";
        public const string LocalHost_Socket = "ws://localhost:4000";
        public static string Socket_URL
        {
            get
            {
                return SystemManager.Instance.SystemData.Socket_Url;
                // return LocalHost_Socket;
                // return DataCenterManager.instance.SystemData.Socket_Url;
                // return Test_Sever_Socket;
                // return Live_Sever_Socket;
            }
        }
    }
}