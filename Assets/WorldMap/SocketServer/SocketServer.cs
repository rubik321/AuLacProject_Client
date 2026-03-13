using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BestHTTP.SocketIO3;
using SimpleJSON;
using NTFunctions_old;
using Rubik.Friend;

namespace Rubik.SocketServer{
    public class SocketServer : LoadBehaviour
    {
        public SocketManager socketManager;
        public string friendEvent = "friend"; 

        public static SocketServer instance;
        protected override void Awake()
        {
            base.Awake();
            if (SocketServer.instance != null) Debug.LogError("Only 1 SocketServer allow");
            SocketServer.instance = this;
        }

        protected override void Start(){
            // socketManager = new SocketManager(new System.Uri("http://localhost:8080"));
            socketManager.Socket.On(SocketIOEventTypes.Connect, () => Debug.Log(socketManager.Socket.Id));
            
            socketManager.Socket.On<JSONNode>(friendEvent, (data)=>{
                
            });
        }

        public void SendMessage(string eventSocket, JSONNode msg){
            socketManager.Socket.Emit(friendEvent, msg);
        }
    }
}
