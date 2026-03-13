using System;
using System.Collections;
using System.Collections.Generic;
using NTPackage.Functions;
using Rubik.UI;
using SimpleJSON;
using UnityEngine;

namespace Rubik.UserProfile
{
    using NTPackage.EventDispatcher;
    using Rubik.DataCenter;
    using Rubik.Manager;
    using Rubik.Config;
    using Rubik.UserDataPlayer;
    using Sirenix.Serialization;
    using UnityEngine.U2D;
    using NTPackage;
    using NTPackage.UI;
    using Rubik.ItemPlayer;
    using Rubik.Server;
    using System.Linq;
    using Rubik.AddressablesLoader;

    public class UserProfileConfig
    {
        public const string API_UserData_ChangeDisplayName = "/api/2D_GPS/user_data/change_display_name";
        public const string API_UserData_ChangeAvatar = "/api/2D_GPS/user_data/change_avatar";
        public const string API_UserData_ChangeAvatarBorder = "/api/2D_GPS/user_data/change_avatar_border";
        public const string API_UserData_ChangeSkin = "/api/2D_GPS/user_data/change_skin";
        public const string API_UserData_GetUserDataShort = "/api/2D_GPS/user_data/get_user_data_short";

        public const string API_UserData_BuyAvatar = "/api/2D_GPS/user_data/buy_avatar";
        public const string API_UserData_BuyAvatarBorder = "/api/2D_GPS/user_data/buy_avatar_border";
        public const string API_UserData_DoneTutorial = "/api/2D_GPS/user_data/done_tutorial";
    }

    public class UserProfileManager : NTBehaviour
    {
        #region Player Data
        public AvatarPlayer AvatarPlayer = new AvatarPlayer();
        public AvatarBorderPlayer AvatarBorderPlayer = new AvatarBorderPlayer();
        public EmojiPlayer EmojiPlayer = new EmojiPlayer();
        public List<TutorialType> TutorialDone;
        #endregion

        #region Game Data
        public ItemData CostChangeName;
        private NTDictionary<int, AvatarBorderData> AvatarBorderDatas;
        private NTDictionary<int, AvatarData> AvatarDatas;
        public List<LockFunction> LockFunctions;
        #endregion

        #region Resource
        [SerializeField] private ListSpriteAddressable PlayerAvatarSpriteAddressable;
        [SerializeField] private NTDictionary<string, Sprite> PlayerAvatarSpriteDic;
        [SerializeField] private ListSpriteAddressable PlayerFrameSpriteAddressable;
        [SerializeField] private NTDictionary<string, Sprite> PlayerFrameSpriteDic;
        public Sprite DefaultAvatar;
        public Sprite DefaultAvatarBorder;

        public List<GameObject> EmojiObjects;
        public List<Sprite> EmojiSprites;
        #endregion

        // Cache
        public NTDictionary<string, UserDataShort> UserDataShortCache;

        public static UserProfileManager Instance;
        protected override void Awake()
        {
            base.Awake();
            if (UserProfileManager.Instance != null)
            {
                NTLog.LogWarning("Only 1 Instance allow");
                return;
            }
            UserProfileManager.Instance = this;
        }

        #region Function

        public IEnumerator LoadData()
        {
            NTLog.LogMessage("UserProfileManager LoadData", gameObject);
            try
            {
                this.CostChangeName = JsonUtility.FromJson<ItemData>(DataCenterManager.Instance.GetData(DataName.CostChangeName));

            }
            catch (System.Exception)
            {
                this.CostChangeName = new ItemData();
            }
            JSONNode jdata = JSONNode.Parse(DataCenterManager.Instance.GetData(DataName.AvatarData));
            this.AvatarDatas = new NTDictionary<int, AvatarData>();
            foreach (JSONNode item in jdata)
            {
                AvatarData avatarData = JsonUtility.FromJson<AvatarData>(item.ToString());
                this.AvatarDatas.Add(avatarData.Index, avatarData);
            }

            jdata = JSONNode.Parse(DataCenterManager.Instance.GetData(DataName.AvatarBorderData));
            this.AvatarBorderDatas = new NTDictionary<int, AvatarBorderData>();
            foreach (JSONNode item in jdata)
            {
                AvatarBorderData avatarBorderData = JsonUtility.FromJson<AvatarBorderData>(item.ToString());
                this.AvatarBorderDatas.Add(avatarBorderData.Index, avatarBorderData);
            }

            this.LockFunctions = new List<LockFunction>();
            jdata = JSONNode.Parse(DataCenterManager.Instance.GetData(DataName.FunctionLockData));
            foreach (JSONNode item in jdata)
            {
                LockFunction lockFunction = JsonUtility.FromJson<LockFunction>(item.ToString());
                this.LockFunctions.Add(lockFunction);
            }

            int amount = 0;

            AddressablesLoader.Instance.LoadAssetsFromLocal(AddressablesAssetsConfig.PlayerAvatarSprite, (result) =>
            {
                this.PlayerAvatarSpriteAddressable = result.GetComponent<ListSpriteAddressable>();
                this.PlayerAvatarSpriteAddressable.transform.SetParent(transform);
                this.PlayerAvatarSpriteDic = new NTDictionary<string, Sprite>();
                foreach (var item in this.PlayerAvatarSpriteAddressable.ListSprite)
                {
                    this.PlayerAvatarSpriteDic.Add(item.name, item);
                }
                amount++;
            });

            AddressablesLoader.Instance.LoadAssetsFromLocal(AddressablesAssetsConfig.PlayerFrameSprite, (result) =>
            {
                this.PlayerFrameSpriteAddressable = result.GetComponent<ListSpriteAddressable>();
                this.PlayerFrameSpriteAddressable.transform.SetParent(transform);
                this.PlayerFrameSpriteDic = new NTDictionary<string, Sprite>();
                foreach (var item in this.PlayerFrameSpriteAddressable.ListSprite)
                {
                    this.PlayerFrameSpriteDic.Add(item.name, item);
                }
                amount++;
            });

            yield return new WaitUntil(() => amount >= 2);
        }

        public void Logout()
        {
            this.AvatarPlayer = new AvatarPlayer();
            this.AvatarBorderPlayer = new AvatarBorderPlayer();
            this.EmojiPlayer = new EmojiPlayer();
            this.TutorialDone = new List<TutorialType>();
        }

        public void Update_AvatarPlayer(AvatarPlayer avatarPlayer)
        {
            if (avatarPlayer.Current < -1)
            {
                return;
            }
            this.AvatarPlayer = avatarPlayer;
        }

        public void Update_AvatarBorderPlayer(AvatarBorderPlayer avatarBorderPlayer)
        {
            if (avatarBorderPlayer.Current < 0)
            {
                return;
            }
            this.AvatarBorderPlayer = avatarBorderPlayer;
        }

        public void Update_UserDataShort(UserDataShort[] userDataShorts)
        {
            foreach (UserDataShort userDataShort in userDataShorts)
            {
                this.UserDataShortCache.Add(userDataShort.UserID, userDataShort);
            }
        }

        public void Update_TutorialDone(TutorialType[] tutorialDone)
        {
            Debug.Log("Update_TutorialDone" + tutorialDone.Length);
            if(tutorialDone == null || tutorialDone.Length == 0) return;
            this.TutorialDone = tutorialDone.ToList();
        }

        public void ChangeDisplayName(string newDisplayName, System.Action callback = null)
        {
            StartCoroutine(IEChangeDisplayName(newDisplayName, callback));
        }

        public void ChangeAvatar(int index, System.Action callback = null)
        {
            StartCoroutine(IEChangeAvatar(index, callback));
        }

        public void ChangeAvatarBorder(int index, System.Action callback = null)
        {
            StartCoroutine(IEChangeAvatarBorder(index, callback));
        }

        public void ShowUserDataShortUI(string userID)
        {
            PopupManager.Instance.OnUI(PopupCode.UserDataShortUI, (object)userID);
        }

        public void DoneTutorial(TutorialType tutorialType, Action done = null){
            if(this.TutorialDone.Contains(tutorialType)) return;
            StartCoroutine(IEDoneTutorial(tutorialType, done));
        }

        #endregion

        #region API

        public IEnumerator IEChangeDisplayName(string newDisplayName, System.Action callback = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.Instance.GetUserID();
            jdata["display_name"] = newDisplayName;

            yield return APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + UserProfileConfig.API_UserData_ChangeDisplayName, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;

                if (callback != null)
                {
                    callback();
                }
                EventListenerManager.instance.PostEvent(EventCode.ChangeDisplayName, (object)newDisplayName);
            });
        }

        public IEnumerator IEChangeAvatar(int index, System.Action callback = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.Instance.GetUserID();
            jdata["index"] = index;
            yield return APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + UserProfileConfig.API_UserData_ChangeAvatar, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;

                callback?.Invoke();
                EventListenerManager.instance.PostEvent(EventCode.ChangeAvatar);
            });
        }

        public IEnumerator IEChangeAvatarBorder(int index, System.Action callback = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.Instance.GetUserID();
            jdata["index"] = index;

            yield return APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + UserProfileConfig.API_UserData_ChangeAvatarBorder, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;

                callback?.Invoke();
                EventListenerManager.instance.PostEvent(EventCode.ChangeAvatar, (object)null);
            });
        }

        public IEnumerator IEBuyAvatar(int index, System.Action callback = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.Instance.GetUserID();
            jdata["index"] = index;
            yield return APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + UserProfileConfig.API_UserData_BuyAvatar, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;

                callback?.Invoke();
            });
        }

        public IEnumerator IEBuyAvatarBorder(int index, System.Action callback = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.Instance.GetUserID();
            jdata["index"] = index;
            yield return APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + UserProfileConfig.API_UserData_BuyAvatarBorder, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;

                callback?.Invoke();
            });
        }
        public IEnumerator IEGetUserDataShort(string[] userIDs, System.Action<UserDataShort[]> callback = null)
        {
            List<string> userIDsList = new List<string>();
            foreach (string userID in userIDs)
            {
                UserDataShort userDataShort = this.UserDataShortCache.Get(userID);
                if (userDataShort != null)
                {
                    // 5 minutes
                    if (userDataShort.LastTimeGet < ServerManager.Instance.TimeServer - 5 * 60)
                    {
                        userIDsList.Add(userID);
                    }
                }
                else
                {
                    userIDsList.Add(userID);
                }
            }
            if (userIDsList.Count > 0)
            {
                JSONNode jdata = new JSONObject();
                jdata["userIDs"] = userIDsList.ToArray();
                yield return APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + UserProfileConfig.API_UserData_GetUserDataShort, (data) =>
                {
                    APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                    if (apiResponseData.Status == 0) return;
                });
            }
            List<UserDataShort> userDataShorts = new List<UserDataShort>();
            foreach (string userID in userIDs)
            {
                userDataShorts.Add(this.UserDataShortCache.Get(userID));
            }
            callback?.Invoke(userDataShorts.ToArray());
        }

        public IEnumerator IEDoneTutorial(TutorialType tutorialType, Action done = null){
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.Instance.GetUserID();
            jdata["index"] = (int)tutorialType;
            yield return APIManager.Instance.PostDataUrl(jdata.ToString(), URL_Config.BASE_API_URL + UserProfileConfig.API_UserData_DoneTutorial, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                done?.Invoke();
            });
        }

        #endregion

        #region Getter

        public ItemData GetPriceChangeName()
        {
            if (UserDataManager.Instance.UserData.FirstChangeName) return new ItemData(ItemType.Gem, 0);
            return this.CostChangeName;
        }

        public bool IsFirstChangeName()
        {
            return UserDataManager.Instance.UserData.FirstChangeName;
        }

        public int GetAvatarUsedIndex()
        {
            return this.AvatarPlayer.Current;
        }

        public int GetAvatarBorderUsedIndex()
        {
            return this.AvatarBorderPlayer.Current;
        }

        public Sprite GetAvatarSprite(int index)
        {
            AvatarData avatarData = this.GetAvatarData(index);
            if (avatarData == null)
            {
                NTLog.LogMessage("GetAvatarSprite: avatarData is null", gameObject);
                return this.DefaultAvatar;
            }
            Sprite sprite = this.PlayerAvatarSpriteDic.Get(avatarData.Sprite);
            if (sprite == null)
            {
                NTLog.LogMessage("GetAvatarSprite: sprite is null", gameObject);
                return this.DefaultAvatar;
            }
            return sprite;
        }

        public Sprite GetAvatarBorderSprite(int index)
        {
            AvatarBorderData avatarBorderData = this.GetAvatarBorderData(index);
            if (avatarBorderData == null)
            {
                NTLog.LogMessage("GetAvatarBorderSprite: avatarBorderData is null", gameObject);
                return this.DefaultAvatarBorder;
            }
            Sprite sprite = this.PlayerFrameSpriteDic.Get(avatarBorderData.Sprite);
            if (sprite == null)
            {
                NTLog.LogMessage("GetAvatarBorderSprite: sprite is null", gameObject);
                return this.DefaultAvatarBorder;
            }
            return sprite;
        }

        public bool IsAvatarBorderAvailable(int index)
        {
            return this.AvatarBorderPlayer.Own.Contains(index);
        }

        public bool IsAvatarAvailable(int index)
        {
            return this.AvatarPlayer.Own.Contains(index);
        }

        public AvatarBorderData GetAvatarBorderData(int index)
        {
            return this.AvatarBorderDatas.Get(index);
        }

        public AvatarData GetAvatarData(int index)
        {
            return this.AvatarDatas.Get(index);
        }

        public List<AvatarBorderData> GetListAvatarBorderData()
        {
            return this.AvatarBorderDatas.ToList();
        }

        public List<AvatarData> GetListAvatarData()
        {
            return this.AvatarDatas.ToList();
        }

        // Emoji
        public List<int> GetListEmojiAvailable()
        {
            return new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
        }

        public GameObject GetEmojiPrefabByIndex(int index)
        {
            return this.EmojiObjects[index % this.EmojiObjects.Count];
        }

        public Sprite GetEmojiSpriteByIndex(int index)
        {
            return this.EmojiSprites[index % this.EmojiSprites.Count];
        }

        public void GetUserDataShort(string userID, System.Action<UserDataShort> callback = null)
        {
            StartCoroutine(this.IEGetUserDataShort(new string[] { userID }, (userDataShorts) =>
            {
                if (userDataShorts.Length > 0)
                {
                    callback?.Invoke(userDataShorts[0]);
                }
                else
                {
                    callback?.Invoke(null);
                }
            }));
        }

        public void GetUserDataShorts(List<string> userIDs, System.Action<UserDataShort[]> callback = null)
        {
            StartCoroutine(this.IEGetUserDataShort(userIDs.ToArray(), callback));
        }

        public UserDataShort GetUserDataShortCache(string userID){
            return this.UserDataShortCache.Get(userID);
        }
        public int GetCountOfTutorials()
        {
            if(TutorialDone==null || TutorialDone.Count == 0)
            {
                return 0;
            }
            else
            {
                return TutorialDone.Count;
            }
        }
        public bool IsTutorialDone(TutorialType tutorialType){
            if(this.TutorialDone == null || this.TutorialDone.Count == 0) return false;
            return this.TutorialDone.Contains(tutorialType);
        }

        public bool IsFunctionLocked(LockFunctionType lockFunctionType){
            if(UserDataManager.Instance.GetRole() == Role.Tester) return false;
            if( lockFunctionType == LockFunctionType.None) return false;
            if (this.LockFunctions == null || this.LockFunctions.Count == 0) return false;
           
            LockFunction lockFunction = this.LockFunctions.Find(x => x.Type == lockFunctionType);
            if(lockFunction == null) return false;
            return UserDataManager.Instance.GetLevel() < lockFunction.LevelUnlock;
        }

        public int GetLevelUnlockFunction(LockFunctionType lockFunctionType){
            if(this.LockFunctions == null || this.LockFunctions.Count == 0) return 0;
            LockFunction lockFunction = this.LockFunctions.Find(x => x.Type == lockFunctionType);
            if(lockFunction == null) return 0;
            return lockFunction.LevelUnlock+1; 
        }
        #endregion
    }
}
