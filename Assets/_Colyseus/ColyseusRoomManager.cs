using System.Collections;
using System.Collections.Generic;
using Colyseus;
using Rubik.Config;
using NTPackage.Functions;
using NTPackage.Functions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Rubik.Colyseus
{
    public class ColyseusRoomName{
        public const string LobbyGame = "LobbyGame";
        public const string MirrorBreak = "MirrorBreak";
        public const string MsgDelivery = "MsgDelivery";
        public const string DodgeConquer = "DodgeConquer";
        public const string Marathon = "Marathon";
        public const string JumpingGame = "JumpingGame";
        public const string HeatPanGame = "HeatPanGame";
        public const string BoomGame = "BoomGameRoom";
        public const string WaitingGame = "WaitingGame";
    }
    public class ColyseusRoomManager : NTBehaviour
    {
        public static ColyseusRoomManager Instance;
        public ColyseusClient Client;

        public string RoomId = "";

        protected override void Awake()
        {
            base.Awake();
            if (ColyseusRoomManager.Instance != null)
            {
                NTLog.LogWarning("Only 1 Instance allow");
                return;
            }
            ColyseusRoomManager.Instance = this;
        }

        [Button]
        public void Init(){
            this.Client = new ColyseusClient(URL_Config.Socket_URL);
        }
    }
}

