using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NTFunctions_old;
using SimpleJSON;
using Rubik.SocketServer;
using System;
using GOA.UserData;
using GOA.Config;
using NTPackage_old.Functions;
using Rubik.UI;
using GOA.UIProfile;

namespace Rubik.Friend{
    [System.Serializable]

    public class FriendController : LoadBehaviour
    {
        public NTPackage_old.Server.SocketServer SocketServer;
        public string channel = "listening";

        public NTDictionary<string, bool> ListRelationship;

        public static FriendController instance;
        protected override void Awake()
        {
            base.Awake();
            if (FriendController.instance != null) Debug.LogError("Only 1 FriendController allow");
            FriendController.instance = this;
        }

        protected override void Start()
        {
            base.Start();
            this.ListRelationship = new NTDictionary<string, bool>();
            // this.InitSocket();
            StartCoroutine(this.Init());
        }

        IEnumerator Init(){
            yield return new WaitForSeconds(3);
            if(UserData.Instance.data.UserId.Length == 0){
                StartCoroutine(this.Init());
            }else{
                this.GetListRelationship();
            }
        }

        [ContextMenu("GetFriendList")]
        public void GetFriendList(Action<string> callback){
            JSONNode data = new JSONObject();
            data["userID"] = UserData.Instance.data.UserId;
            data["skip"] = 0;
            data["total"] = 40;
            StartCoroutine(APIManager.Instance.NTPostData(data.ToString(), SeverConfigs.GetFriendListAPI, (res)=>{
                callback(res);
            }));
        }

        //649d64d49c879de4091cdb3d
        [ContextMenu("SendFriendRequest")]
        public void TestSendFriendRequest(){
            this.SendFriendRequest("649d64d49c879de4091cdb3d");
        }
        public void SendFriendRequest(string friendID){
            if(this.ListRelationship.Dictionary.Count > 40){
                HUDCanvas.Instance.ShowNotification("Friends limit reached!");
                return;
            }
            JSONNode data = new JSONObject();
            data["userID"] = UserData.Instance.data.UserId;
            data["userName"] = UserData.Instance.data.UserName;
            data["friendID"] = friendID;
            StartCoroutine(APIManager.Instance.NTPostData(data.ToString(), SeverConfigs.SendFriendRequestAPI, (res)=>{
                JSONNode json = JSONNode.Parse(res);
                try
                {
                    FriendRequestUI friendRequestUI = (FriendRequestUI) UIManager.instance.GetPopupUIByCode(PopupCode.FriendRequestUI);
                    if(friendRequestUI == null) return;
                    friendRequestUI.OnUI();
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning(e);
                }
            }));
        }

        [ContextMenu("GetFriendRequest")]
        public void TestGetFriendRequest(){
            this.GetFriendRequest(res=>{});
        }
        public void GetFriendRequest(Action<string> callback){
            JSONNode data = new JSONObject();
            data["userID"] = UserData.Instance.data.UserId;
            data["skip"] = 0;
            data["total"] = 20;
            StartCoroutine(APIManager.Instance.NTPostData(data.ToString(), SeverConfigs.GetFriendRequestAPI, (res)=>{
                callback(res);
            }));
        }

        [ContextMenu("AcceptFriendRequest")]
        public void TestAcceptFriendRequest(){
            this.AcceptFriendRequest("66ddc156be0b4736514b3c23");
        }

        [ContextMenu("GetListRelationship")]
        public void GetListRelationship(){
           JSONNode data = new JSONObject();
            data["userID"] = UserData.Instance.data.UserId;
            StartCoroutine(APIManager.Instance.NTPostData(data.ToString(), SeverConfigs.GetRelationship, (res)=>{
                JSONNode jdata = JSONNode.Parse(res);
                foreach (JSONNode item in jdata["Data"]["list"])
                {
                    try
                    {
                        this.ListRelationship.Add(item["user1"], true);
                    }
                    catch (System.Exception){}
                    try
                    {
                        this.ListRelationship.Add(item["user2"], true);
                    }
                    catch (System.Exception){}
                }
            })); 
        }

        public void AcceptFriendRequest(string relationID){
            JSONNode data = new JSONObject();
            data["relationID"] = relationID;
            data["userName"] = UserData.Instance.data.UserName;
            StartCoroutine(APIManager.Instance.NTPostData(data.ToString(), SeverConfigs.AcceptFriendRequestAPI, (res)=>{

            }));
        }

        // public void InitSocket(){
        //     // this.SocketServer.Url = ServerConfig.CHAT_URL;
        //     this.SocketServer.Url = SeverConfigs.BASE_API_URL;
        //     this.SocketServer.onListen = (data)=>this.Router(data);
        //     this.SocketServer.ConnectToServer();
        // }

        // public void SendMessage(JSONNode msg){
        //     this.SocketServer.SendMessageSocket(this.channel, msg);
        // }
        // public void Router(JSONNode data){
        //     Debug.LogWarning(data.ToString());
        //     if(data[1]["Name"].Equals(FriendCmdId.SendFriendRequest)){
        //         this.SendFriendRequestResponse(data[2]);
        //         return;
        //     }
        //     if(data[1]["Name"].Equals(FriendCmdId.AcceptFriendRequest)){
        //         this.AcceptFriendRequestResponse(data[2]);
        //         return;
        //     }
        //     if(data[1]["Name"].Equals(FriendCmdId.CancelFriendRequest)){
        //         this.CancelFriendRequestResponse(data[2]);
        //         return;
        //     }
        //     if(data[1]["Name"].Equals(FriendCmdId.UnFriend)){
        //         this.UnfriendResponse(data[2]);
        //         return;
        //     }
        // }

        // public void GetListRelationship(){
            
        // }

        // public void GetListRelationshipRespone(JSONNode data){
        //     foreach (JSONNode item in data)
        //     {
        //         Relationship relationship = new Relationship();
        //         relationship.user1 = item["user1"];
        //         relationship.user2 = item["user2"];
        //         relationship.User1FollowUser2 = item["User1FollowUser2"];
        //         relationship.User2FollowUser1 = item["User2FollowUser1"];
        //         relationship.type = (RelationshipType) Int32.Parse(item["type"].ToString());
        //         relationship.time = item["time"];
        //         string key;
        //         if(UserData.Instance.data.UserId.Equals(relationship.user1)){
        //             key = relationship.user2;
        //         }else{
        //             key = relationship.user1;
        //         }
        //         disRelationship.Add(key, relationship);
        //     }
        // }

        // public void GetFriendList(int skip, int limit){
        //     JSONNode msg = new JSONObject();
        //     msg["Name"] = FriendCmdId.GetFriendList;
        //     msg["skip"] = skip;
        //     msg["limit"] = limit;

        //     SendMessage(msg);
        // }

        // public void FriendListResponse(JSONNode response){

        // }

        // public void SendFriendRequest(string idFriend){
        //     JSONNode msg = new JSONObject();
        //     msg["Name"] = FriendCmdId.SendFriendRequest;
        //     msg["UserId"] = idFriend;

        //     SendMessage(msg);
        // }
        // public void SendFriendRequestResponse(JSONNode response){
        //     if(response["Name"]){
        //         Debug.Log(response["Name"] +"Success");
        //     }else{
        //         Debug.LogWarning(response.ToString());
        //     }
        // }

        // public void AcceptFriendRequest(string idFriend){
        //     JSONNode msg = new JSONObject();
        //     msg["Name"] = FriendCmdId.AcceptFriendRequest;
        //     msg["UserId"] = idFriend;
        //     disRelationship[idFriend].type = RelationshipType.Friend;
        //     SendMessage(msg);
        // }
        // public void AcceptFriendRequestResponse(JSONNode response){
        //     if(response["Name"]){
                
        //         Debug.Log(response["Name"] +"Success");
        //     }else{
        //         Debug.LogWarning(response.ToString());
        //     }
        // }
        // public void CancelFriendRequest(string idFriend){
        //     JSONNode msg = new JSONObject();
        //     msg["Name"] = FriendCmdId.CancelFriendRequest;
        //     msg["UserId"] = idFriend;

        //     SendMessage(msg);
        // }

        // public void CancelFriendRequestResponse(JSONNode response){
        //     if(response["Name"]){
        //         if(UserData.Instance.data.UserId.Equals(response["user1"])){
        //             this.disRelationship.Remove(response["user2"]);
        //         }else{
        //             this.disRelationship.Remove(response["user1"]);
        //         }
        //         Debug.Log(response["Name"] +"Success");
        //     }else{
        //         Debug.LogWarning(response.ToString());
        //     }
        // }

        // public void Unfriend(string idFriend){
        //     JSONNode msg = new JSONObject();
        //     msg["Name"] = FriendCmdId.UnFriend;
        //     msg["UserId"] = idFriend;

        //    SendMessage(msg);
        // }

        // public void UnfriendResponse(JSONNode response){
        //     if(response["Name"]){
        //         if(UserData.Instance.data.UserId.Equals(response["user1"])){
        //             this.disRelationship.Remove(response["user2"]);
        //         }else{
        //             this.disRelationship.Remove(response["user1"]);
        //         }
        //         Debug.Log(response["Name"] +" Success");
        //     }else{
        //         Debug.LogWarning(response.ToString());
        //     }
        // }


        // public bool CheckInRelationship(string idUser){
        //     if(this.disRelationship[idUser]) return true;
        //     return false;
        // }

        // [ContextMenu("TestFriend")]
        // public void TestFriend(){
        //     this.SendFriendRequest("123456");
        // }

    }
}