using Colyseus;
using Colyseus.Schema;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Sirenix.OdinInspector;
using NTFunctions_old;
using GOA.UserData;
using GOA.Config;
using Rubik.UI;
using UnityEngine.SceneManagement;
using NTPackage_old.EventDispatcher;
using GOA.WorldMap;
using Rubik.Combat;

namespace Rubik.Chat
{
    public class HistoryFriendBoxChat
    {
        public static int Version = 2;
        public List<FriendBoxChatData> FriendBoxChatDatas;
        public int ClientVersion = 0;
    }

    [System.Serializable]
    public class FriendBoxChatData
    {
        public PlayerChat PlayerChat;
        public long Time = new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();
        public bool IsNew = false;
    }

    [System.Serializable]
    public class PlayerChat
    {
        public string channelId;
        public string channelName;
        public string senderName;
        public string senderId;
        public string msg;
    }

    public class GetHistoryRequest
    {
        public string channelId;
    }

    [System.Serializable]
    public class ChatHistory
    {
        public int status;
        public string channelId;
        public PlayerChat[] history;
    }

    public class ChatManager : MonoBehaviour
    {
        private static ChatManager _instance;

        public Dictionary<string, FriendBoxChatData> FriendBoxChatDataDictionary = new Dictionary<string, FriendBoxChatData>();

        public string Key_HistoryFriendBoxChat
        {
            get
            {
                return UserData.Instance.data.UserId + "HistoryFriendBoxChat";
            }
        }

        private ColyseusClient _client;

        public ColyseusRoom<MetroState> _room;

        public delegate void AddChat(string senderName, string content, string senderId);
        public event AddChat onChatMsg;
        [SerializeField]
        private ChatUI chatUI;
        public ChatUI ChatUI
        {
            get
            {
                if (this.chatUI == null)
                {
                    this.chatUI = (ChatUI)UIManager.instance.GetPopupUIByCode(PopupCode.ChatUI);
                }
                return this.chatUI;
            }
        }

        //private GameState gameState = GameState.Loading;
        //public PVPRoomController Room => _room;


        public static ChatManager Instance => _instance;

        public string EventChannelJoin = "";

        private void Awake()
        {
            _instance = this;
        }

        // public void Start()
        // {
        //     _ = InitClientChat();
        //     this.GetHistoryFriendBoxChat();
        // }
        public bool IsInit = false;
        public void Init()
        {
            NTPackage_old.Functions.NTLog.LogMessage("Init ChatManager", gameObject);
            this.IsInit = true;
            _ = InitClientChat();
            this.GetHistoryFriendBoxChat();
        }

        void OnApplicationQuit()
        {
            this.UnInit();
        }

        public void UnInit()
        {
            try
            {
                this.IsInit = false;
                _ = _room.Leave();

            }
            catch (System.Exception e)
            {
                NTPackage_old.Functions.NTLog.LogError(e.ToString(), gameObject);
            }
        }

        public void GetHistoryFriendBoxChat()
        {
            try
            {
                string key = this.Key_HistoryFriendBoxChat;
                HistoryFriendBoxChat historyFriendBoxChat = JsonUtility.FromJson<HistoryFriendBoxChat>(PlayerPrefs.GetString(key));
                if (historyFriendBoxChat.ClientVersion != HistoryFriendBoxChat.Version) return;
                foreach (FriendBoxChatData item in historyFriendBoxChat.FriendBoxChatDatas)
                {
                    this.FriendBoxChatDataDictionary[item.PlayerChat.channelId] = item;
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e);
            }

        }

        public bool isConnect = false;
        public bool isConnecting = false;
        private void FixedUpdate()
        {
            if (!this.isConnect && this.IsInit) this.Reconnect();
        }

        public async Task InitClientChat()
        {
            try
            {
                this.isConnecting = true;
                //_client = new ColyseusClient("ws://34.87.155.178:8080");
                _client = new ColyseusClient(ServerConfig.CHAT_URL);

                Dictionary<string, object> loginData = new Dictionary<string, object>();

                loginData.Add("userID", UserData.Instance.data.UserId);
                loginData.Add("device", SystemInfo.deviceName);
                loginData.Add("userName", UserData.Instance.data.UserName);
                _room = await _client.JoinOrCreate<MetroState>("chat", loginData);
                _room.OnLeave += (code) =>
                {
                    Debug.Log("client left the room");
                    this.isConnect = false;
                };
                Debug.Log("_room" + _room.Id);

                _room.OnMessage<PlayerChat>("message", playerChat);

                _room.OnMessage<ChatHistory>("getHistory", historyResponse);
                _room.OnMessage<ChatHistory>("duplicateDevice", (data) =>
                {
                    Debug.LogWarning("duplicateDevice");
                    HUDCanvas.Instance.ShowNotification(Lean.Localization.LeanLocalization.GetTranslationText("login_duplicate_device", "You have been logged out, because you logged in with another device."));
                    SceneManager.LoadScene(Configs.Login_Screen);
                    this.UnInit();
                    return;
                });
                this.isConnect = true;
                this.isConnecting = false;
            }
            catch (System.Exception e)
            {
                NTPackage_old.Functions.NTLog.LogError(e.ToString(), gameObject);
                this.isConnect = false;
                this.isConnecting = false;
            }


        }

        [Button]
        private async void Reconnect()
        {
            if (this.isConnecting) return;
            try
            {
                this.isConnecting = true;
                await this.InitClientChat();
                Debug.Log("Reconnect suc");
            }
            catch (System.Exception e)
            {
                Debug.Log("Reconnect fail: " + e);
                this.isConnecting = false;
            }
        }

        public void playerChat(PlayerChat chat)
        {
            // Debug.Log("playerChat" + "== id: " + chat.senderId + "== name: " + chat.senderName + "== msg: " + chat.msg);
            // Debug.Log(JsonUtility.ToJson(chat));
            if (chat.channelId.Equals("global"))
            {
                EventListenerManager.instance.PostEvent(EventCode.ReciveChatGlobal);
            }
            try
            {
                if (this.ChatUI.gameObject.activeSelf) this.ChatUI.ReceiveChat(chat);
            }
            catch (Exception e)
            {
                // throw e;
                Debug.LogWarning(e);
            }
            if (this.EventChannelJoin.Length > 0 && chat.channelId.Equals(this.EventChannelJoin))
            {
                this.ReciveChatEvent(chat.msg);
                return;
            }
            int index = chat.channelId.IndexOf('&');
            if (index > 0)
            {
                EventListenerManager.instance.PostEvent(EventCode.ReciveChatFriend);
                string[] userNames = chat.channelId.Split("&");
                string userName1 = userNames[0];
                string userName2 = userNames[1];
                Debug.LogWarning(userName1 + " " + userName2);
                if (userName1.Equals(UserData.Instance.data.UserName) || userName2.Equals(UserData.Instance.data.UserName))
                {
                    try
                    {
                        FriendBoxChatData friendBoxChatData = new FriendBoxChatData();
                        friendBoxChatData.PlayerChat = chat;
                        friendBoxChatData.Time = DateTime.Now.ToFileTime();
                        friendBoxChatData.IsNew = true;
                        this.FriendBoxChatDataDictionary[chat.channelId] = friendBoxChatData;
                        this.UpdateHistoryChat();
                        if (this.ChatUI.whisperUI.gameObject.activeSelf)
                            this.ChatUI.whisperUI.UpdateData();
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogWarning(e);
                    }
                }
            }
            onChatMsg?.Invoke(chat.senderName, chat.msg, chat.senderId);
        }

        public void UpdateHistoryChat()
        {
            HistoryFriendBoxChat historyFriendBoxChat = new HistoryFriendBoxChat();
            historyFriendBoxChat.FriendBoxChatDatas = new List<FriendBoxChatData>(this.FriendBoxChatDataDictionary.Values);
            historyFriendBoxChat.ClientVersion = HistoryFriendBoxChat.Version;
            string key = this.Key_HistoryFriendBoxChat;
            PlayerPrefs.SetString(key, JsonUtility.ToJson(historyFriendBoxChat));
        }

        List<ChatHistory> listHis;
        public void historyResponse(ChatHistory chatHistory)
        {
            Debug.Log("historyResponse " + chatHistory.status + "== id: " + chatHistory.channelId);
            if (chatHistory.status == 0)
            {
                Debug.Log("channel chat chưa có history");
            }
            else
            {
                if (this.EventChannelJoin.Length > 0 && chatHistory.channelId.Equals(this.EventChannelJoin))
                {
                    List<string> listMsg = new List<string>();
                    for (int i = 0; i < chatHistory.history.Length; i++)
                    {
                        listMsg.Add(chatHistory.history[i].msg);
                    }
                    WorldMapMaster.instance.UpdatePlayer(listMsg);
                }
                for (int i = 0; i < chatHistory.history.Length; i++)
                {
                    Debug.Log(chatHistory.history[i].senderName + " - " + chatHistory.history[i].msg);
                    this.ChatUI.ReceiveChat(chatHistory.history[i]);
                }


            }
        }


        [Button]
        public PlayerChat sendChat(string playerChatContent)
        {
            Debug.Log("send chat");
            PlayerChat playerChat = new PlayerChat()
            {
                msg = playerChatContent,
                senderName = "tuan",
                senderId = "001",
                channelId = "global"
            };
            _ = _room.Send("sendMessage", playerChat);

            //Demo lấy history của channel có id là "global"

            _ = _room.Send("getHistory", new GetHistoryRequest()
            {
                //Muốn lấy lịch sử của room office hay garden thì truyen key vào đây
                channelId = "global"
            });
            return playerChat;
        }

        public void sendChatChannel(string playerChatContent, string channelId = "global")
        {
            Debug.Log("send chat");
            _ = _room.Send("sendMessage", new PlayerChat()
            {
                msg = playerChatContent,
                senderName = UserData.Instance.data.DisplayName,
                senderId = UserData.Instance.data.UserId,
                channelId = channelId
            });
        }

        public void sendChatEvent(string playerData)
        {
            if(this.EventChannelJoin.Length == 0) return;
            _ = _room.Send("sendMessage", new PlayerChat()
            {
                msg = playerData,
                senderName = UserData.Instance.data.UserName,
                senderId = UserData.Instance.data.UserId,
                channelId = this.EventChannelJoin
            });
        }
        public void ReciveChatEvent(string playerData)
        {
            Debug.Log(playerData);
            try
            {
                WorldMapMaster.instance.GetPlayerData(playerData);
            }
            catch (System.Exception){}
        }

        public async void TakeHistoryPlayer()
        {
            if(this.EventChannelJoin.Length == 0) return;
            await this._room.Send("getHistory", new GetHistoryRequest()
            {
                //Muốn lấy lịch sử của room office hay garden thì truyen key vào đây
                channelId = this.EventChannelJoin
            });
        }

        public void SendMsgJoinEvent()
        {
            PlayerData playerData = new PlayerData();
            playerData.Join = true;
            playerData.Action = "Idle";
            playerData.UserName = UserData.Instance.data.UserName;
            playerData.UserId = UserData.Instance.data.UserId;
            playerData.Class = "Sword";
            playerData.GearCodes = new List<string>();
            foreach (GearData item in UserData.Instance.gearData.Data.GearData)
            {
                if (item.Equiped)
                {
                    playerData.GearCodes.Add(item.GearCode);
                    if (item.Slot == 3)
                    {
                        playerData.Class = AssetLoader.Instance.GetWeaponType(item.GearCode).ToString();
                    }
                }
            }
            playerData.latitude = GameMaster.instance.locationManager.currentLocation.latitude;
            playerData.longitude = GameMaster.instance.locationManager.currentLocation.longitude;
            this.sendChatEvent(JsonUtility.ToJson(playerData));
        }
    }
}


