using System;
using System.Collections;
using System.Collections.Generic;
using NTPackage;
using NTPackage.Functions;
using Rubik.Manager;
using SimpleJSON;
using UnityEngine;

namespace Rubik.Friend
{
    using Rubik._2DGPS.Chat;
    using Rubik.Chat;
    using Rubik.Config;
    using Rubik.Server;
    using Rubik.UI;
    using Rubik.UserDataPlayer;

    public class FriendConfig{
        public const string API_Friend_GetFriendList = "/api/multiplayer/friend/getFriendList";
        public const string API_Friend_SendFriendRequest = "/api/multiplayer/friend/sendFriendRequest";
        public const string API_Friend_AcceptFriendRequest = "/api/multiplayer/friend/acceptFriendRequest";
        public const string API_Friend_RejectFriendRequest = "/api/multiplayer/friend/rejectFriendRequest";
        public const string API_Friend_RemoveFriend = "/api/multiplayer/friend/removeFriend";
    }

    public class FriendManager : NTBehaviour
    {
        [SerializeField] private NTDictionary<string, FriendData> FriendDatas = new NTDictionary<string, FriendData>();

        // Cache
        [SerializeField] private NTDictionary<string, FriendData> Friends = new NTDictionary<string, FriendData>();
        [SerializeField] private NTDictionary<string, FriendData> FriendRequests = new NTDictionary<string, FriendData>();
        [SerializeField] private NTDictionary<string, FriendData> FriendRequestsSent = new NTDictionary<string, FriendData>();


        public static FriendManager Instance;
        protected override void Awake()
        {
            base.Awake();
            if (FriendManager.Instance != null)
            {
                NTLog.LogWarning("Only 1 Instance allow");
                return;
            }
            FriendManager.Instance = this;
        }

        #region Function

        public void Logout()
        {
            FriendDatas.Clear();
            Friends.Clear();
            FriendRequests.Clear();
            FriendRequestsSent.Clear();
        }

        public void UpdateFriendData(FriendData[] datas)
        {
            NTLog.LogMessage("UpdateFriendData: " + datas.Length);
            if (this.FriendDatas == null || this.FriendDatas.Count == 0)
            {
                this.FriendDatas = new NTDictionary<string, FriendData>();
                this.Friends = new NTDictionary<string, FriendData>();
                this.FriendRequests = new NTDictionary<string, FriendData>();
                this.FriendRequestsSent = new NTDictionary<string, FriendData>();
            }

            foreach (FriendData data in datas)
            {
                string partnerID = data.User1_ID.Equals(UserDataManager.Instance.GetUserID()) ? data.User2_ID : data.User1_ID;
                if (this.FriendDatas.Contains(partnerID))
                {
                    this.FriendDatas.Get(partnerID).UpdateData(data);
                }
                else
                {
                    this.FriendDatas.Add(partnerID, data);
                }

                if (data.Status == FriendStatus.Friend)
                {
                    if (this.Friends.Contains(partnerID))
                    {
                        this.Friends.Get(partnerID).UpdateData(data);
                    }
                    else
                    {
                        this.Friends.Add(partnerID, data);
                    }
                }

                if (data.Status == FriendStatus.Request)
                {
                    if (data.Follower == 1 && data.User1_ID.Equals(UserDataManager.Instance.GetUserID()))
                    {
                        this.FriendRequestsSent.Add(partnerID, data);
                    }
                    else
                    {
                        this.FriendRequests.Add(partnerID, data);
                    }
                }

                if (data.Status == FriendStatus.None)
                {
                    if (this.Friends.Contains(partnerID))
                    {
                        this.Friends.Remove(partnerID);
                    }
                    if (this.FriendRequests.Contains(partnerID))
                    {
                        this.FriendRequests.Remove(partnerID);
                    }
                    if (this.FriendRequestsSent.Contains(partnerID))
                    {
                        this.FriendRequestsSent.Remove(partnerID);
                    }
                }
            }
        }

        public void NewFriendRequest(string data){
            HUDCanvas.Instance.ShowNotification("You have a new friend request");
        }

        public void AcceptedFriendRequest(string data){
            HUDCanvas.Instance.ShowNotification("You was accepted as a friend");
        }

        public void RejectedFriendRequest(string data){
            HUDCanvas.Instance.ShowNotification("You was rejected as a friend");
        }

        #endregion
        
        #region Getter

        public FriendData GetFriendData(string partnerID)
        {
            return this.FriendDatas.Get(partnerID);
        }

        public FriendData GetFriend(string partnerID)
        {
            return this.Friends.Get(partnerID);
        }

        public List<FriendData> GetFriendList()
        {
            return this.Friends.ToList();
        }

        public FriendData GetFriendRequest(string partnerID)
        {
            return this.FriendRequests.Get(partnerID);
        }

        public List<FriendData> GetFriendRequestList()
        {
            return this.FriendRequests.ToList();
        }

        public FriendData GetFriendRequestSent(string partnerID)
        {
            return this.FriendRequestsSent.Get(partnerID);
        }

        public List<string> GetFriendChatChannelList(){
            List<string> channels = new List<string>();
            foreach (FriendData friendData in this.Friends.ToList())
            {
                channels.Add(this.GetFriendChatChannel(friendData._id));
            }
            return channels;
        }

        public string GetFriendChatChannel(string relationshipID){
            return "friend_chat_" + relationshipID;
        }
        #endregion

        #region API
        public IEnumerator GetFriendList(Action done)
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.Instance.GetUserID();
            yield return APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + FriendConfig.API_Friend_GetFriendList, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if(apiResponseData.Status == 0) return;
                done?.Invoke();
            });
        }

        public IEnumerator SendFriendRequest(string partnerID, Action done)
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.Instance.GetUserID();
            jdata["partnerID"] = partnerID;
            yield return APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + FriendConfig.API_Friend_SendFriendRequest, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if(apiResponseData.Status == 0) return;
                done?.Invoke();
            });
        }

        public IEnumerator AcceptFriendRequest(string partnerID, Action done)
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.Instance.GetUserID();
            jdata["partnerID"] = partnerID;
            yield return APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + FriendConfig.API_Friend_AcceptFriendRequest, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if(apiResponseData.Status == 0) return;
                StartCoroutine(ChatService.Instance.RegisterChannelFriendChat());
                done?.Invoke();
            });
        }

        public IEnumerator RejectFriendRequest(string partnerID, Action done)
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.Instance.GetUserID();
            jdata["partnerID"] = partnerID;
            yield return APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + FriendConfig.API_Friend_RejectFriendRequest, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if(apiResponseData.Status == 0) return;
                done?.Invoke();
            });
        }

        public IEnumerator RemoveFriend(string partnerID, Action done)
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.Instance.GetUserID();
            jdata["partnerID"] = partnerID;
            yield return APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + FriendConfig.API_Friend_RemoveFriend, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if(apiResponseData.Status == 0) return;
                StartCoroutine(ChatService.Instance.RegisterChannelFriendChat());
                done?.Invoke();
            });
        }

        #endregion
    }
}

