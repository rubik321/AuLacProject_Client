using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BestHTTP.SocketIO3;
using SimpleJSON;

namespace NTPackage_old.Server{
    [System.Serializable]
    public class SocketServer
    {
        public SocketManager socketManager;
        public string respone;
        public string Url;
        [SerializeField]
        private string SocketId;
        [SerializeField]
        public bool isConnected;
        public string socketChannel = "listening";

        public string MessageQueue;

        public void ConnectToServer()
        {
            if(isConnected){
                Debug.LogWarning("Connected");
                return;
            }
            this.Disconnect();
            Debug.LogWarning("Connecting");
            socketManager = new SocketManager(new Uri(this.Url));
            socketManager.Socket.On(SocketIOEventTypes.Connect, () => {
                this.SocketId = socketManager.Socket.Id;
                this.isConnected = true;
                Debug.Log("Socket connected: "+ socketManager.Socket.Id);
            });
            socketManager.Socket.On<string>(this.socketChannel, (data)=>{
                    this.respone = data;
                    if(onListen!=null){
                        onListen(data);
                    }
            });
            socketManager.Socket.On(SocketIOEventTypes.Disconnect, ()=>{
                Debug.LogWarning("Drop connect");
                this.Disconnect();
                if(this.onDisconnect != null){
                    this.onDisconnect();
                }
            });
        }

        public Action <string> onListen = null;
        public Action onDisconnect = null;

        public void SendMessageSocket(string channel ,string message){
            if(socketManager == null) {
                this.MessageQueue = message;
                return;
            };
            if(!this.isConnected) {
                this.MessageQueue = message;
                return;
            };
            this.MessageQueue = null;
            this.socketManager.Socket.Emit(channel, message);
        }

        public void Disconnect(){
            try
            {
                this.isConnected = false;
                this.socketManager.Close();
                this.socketManager.Socket.Disconnect();
                Debug.LogWarning("Disconnect");
            }
            catch (System.Exception){}
        }
    }
}
