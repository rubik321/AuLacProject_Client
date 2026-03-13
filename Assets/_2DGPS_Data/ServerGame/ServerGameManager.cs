using System.Collections;
using System.Collections.Generic;
using NTPackage_old.Functions;
using SimpleJSON;
using UnityEngine;
using NTFunctions_old;
using GOA.Config;

namespace Rubik._2DGPS.ServerGame
{
    using UserData;
    using DataCenter;

    public class ServerGameManager : LoadBehaviour
    {
        public NTDictionary<ServerGame> ServerGameDic;

        public static ServerGameManager instance;
        protected override void Awake()
        {
            base.Awake();
            if (ServerGameManager.instance != null)
            {
                NTLog.LogWarning("Only 1 instance allow");
                return;
            }
            ServerGameManager.instance = this;
        }

        public IEnumerator LoadData()
        {
            this.LoadServerGame();
            yield return null;
        }

        public void LoadServerGame()
        {
            this.ServerGameDic = new NTDictionary<ServerGame>();
            JSONNode jdata = JSONNode.Parse(DataCenterManager.instance.GetData(DataName.ServerGame));
            foreach (JSONNode item in jdata)
            {
                ServerGame serverGame = JsonUtility.FromJson<ServerGame>(item.ToString());
                this.ServerGameDic.Add(serverGame.Server.ToString(), serverGame);
            }
        }

        public int GetChannel()
        {
            ServerGame data = ServerGameDic.Get(UserDataManager.instance.UserData.Server.ToString());
            if (data == null)
            {
                return 0;
            }
            return data.Channel;
        }

        public int GetCrossChannel()
        {
            ServerGame data = ServerGameDic.Get(UserDataManager.instance.UserData.Server.ToString());
            if (data == null)
            {
                return 0;
            }
            return data.GroupChannel;
        }
    }
}

