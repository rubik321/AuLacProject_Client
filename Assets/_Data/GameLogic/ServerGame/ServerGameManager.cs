using System.Collections;
using System.Collections.Generic;
using NTPackage;
using NTPackage.Functions;
using Rubik.DataCenter;
using Rubik.UserDataPlayer;
using SimpleJSON;
using UnityEngine;

namespace Rubik.ServerGame
{
    public class ServerGameManager : NTBehaviour
    {
        public NTDictionary<int, ServerGameData> ServerGameData;

        public static ServerGameManager Instance;
        protected override void Awake()
        {
            base.Awake();
            if (ServerGameManager.Instance != null)
            {
                NTLog.LogError("Only 1 Instance allow");
                return;
            }
            ServerGameManager.Instance = this;
        }

        public void LoadData(){
            this.ServerGameData = new NTDictionary<int, ServerGameData>();
            JSONNode jdata = JSONNode.Parse(DataCenterManager.Instance.GetData(DataName.ServerGameData));
            foreach (JSONNode item in jdata.AsArray)
            {
                ServerGameData serverGameData = JsonUtility.FromJson<ServerGameData>(item.ToString());
                this.ServerGameData.Add(serverGameData.Server, serverGameData);
            }
        }

        #region Getter
        public int GetPlayerGroupServer(){
            return this.GetGroupServer(UserDataManager.Instance.UserData.Server);
        }

        public int GetPlayerServer(){
            return UserDataManager.Instance.UserData.Server;
        }

        public int GetGroupServer(int server)
        {
            ServerGameData serverGameData = this.ServerGameData.Get(server);
            if (serverGameData == null)
            {
                NTLog.LogError("ServerGameData not found");
                return 0;
            }
            return serverGameData.GroupServer;
        }
        
        #endregion
    }
}