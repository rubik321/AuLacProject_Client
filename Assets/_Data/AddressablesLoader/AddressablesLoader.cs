using System;
using System.Collections;
using System.Collections.Generic;
using NTPackage;
using NTPackage.Functions;
using Rubik.SystemData;
using Rubik.UI;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace Rubik.AddressablesLoader
{
    public enum LoadStatus
    {
        None,
        Loading,
        Loaded,
        Failed
    }

    public class AddressablesAssetsConfig
    {
        public static string CardPlayerAvatarTrans = "Assets/_Data/AddressablesLoader/Prefabs/CardPlayer/CardPlayerAvatarTrans.prefab";
        public static string CardPlayerAvatarSprite = "Assets/_Data/AddressablesLoader/Prefabs/CardPlayer/CardPlayerAvatarSprite.prefab";
        public static string CardPlayerSkeletonOnMapTrans = "Assets/_Data/AddressablesLoader/Prefabs/CardPlayer/CardPlayerSkeletonOnMapTrans.prefab";
        public static string CardPlayerSkeletonGraphic = "Assets/_Data/AddressablesLoader/Prefabs/CardPlayer/CardPlayerSkeletonGraphic.prefab";
        public static string CardPlayerBaseCharacterDataSOAddressable = "Assets/_Data/AddressablesLoader/Prefabs/CardPlayer/CardPlayerBaseCharacterDataSOAddressable.prefab";
        
        public static string IconSkill = "Assets/_Data/AddressablesLoader/Prefabs/Skill/IconSkill.prefab";
        
        public static string ItemIconAddressableSprite = "Assets/_Data/AddressablesLoader/Prefabs/Item/ItemIconAddressableSprite.prefab";
        public static string TMP_SpriteAsset_Item = "Assets/_Data/AddressablesLoader/Prefabs/TMP_SpriteAsset/TMP_SpriteAsset_Item.prefab";
        
        public static string PlayerAvatarSprite = "Assets/_Data/AddressablesLoader/Prefabs/Player/PlayerAvatarSprite.prefab";
        public static string PlayerFrameSprite = "Assets/_Data/AddressablesLoader/Prefabs/Player/PlayerFrameSprite.prefab";
        
        public static string DataCenterHolder = "Assets/_Data/GameLogic/DataCenter/DataCenterHolder.prefab";

        public static string GameLanguage = "Assets/_Data/Localization/Localization/GameLanguage.prefab";

        public static string AchievementSprite = "Assets/_Data/AddressablesLoader/Prefabs/Achiement/IconAchiementSprite.prefab";

        public static string LandSprite = "Assets/_Data/AddressablesLoader/Prefabs/Land/LandSprite.prefab";

        // Popup

        public static string PopupTrans = "Assets/_Data/AddressablesLoader/Prefabs/Popup/PopupTrans.prefab";
        public static string HUDPopup = "Assets/_Data/AddressablesLoader/Prefabs/Popup/HUDPopup.prefab";

        // Url
        public static string BaseUrl = "http://15.235.180.137:4002/Myrk/";
    }

    public class AddressablesLoader : NTBehaviour
    {

        public AsyncOperationHandle<IList<GameObject>> Handles;

        public bool DoneLoad = false;
        public bool LoadSuccess = false;

        public static AddressablesLoader Instance;
        protected override void Awake()
        {
            base.Awake();
            if (AddressablesLoader.Instance != null)
            {
                NTLog.LogWarning("Only 1 Instance allow");
                return;
            }
            AddressablesLoader.Instance = this;
        }

        #region Function
        public void LoadAsset()
        {
            Addressables.InternalIdTransformFunc = TransformAddress;
            try
            {
                List<string> ids = new List<string>();
                // Assets
                ids.Add(AddressablesAssetsConfig.CardPlayerAvatarTrans);
                ids.Add(AddressablesAssetsConfig.CardPlayerAvatarSprite);
                ids.Add(AddressablesAssetsConfig.CardPlayerSkeletonOnMapTrans);
                ids.Add(AddressablesAssetsConfig.CardPlayerBaseCharacterDataSOAddressable);
                ids.Add(AddressablesAssetsConfig.CardPlayerSkeletonGraphic);
                ids.Add(AddressablesAssetsConfig.IconSkill);
                ids.Add(AddressablesAssetsConfig.ItemIconAddressableSprite);
                ids.Add(AddressablesAssetsConfig.TMP_SpriteAsset_Item);
                ids.Add(AddressablesAssetsConfig.PlayerAvatarSprite);
                ids.Add(AddressablesAssetsConfig.PlayerFrameSprite);
                ids.Add(AddressablesAssetsConfig.AchievementSprite);
                ids.Add(AddressablesAssetsConfig.LandSprite);
                // Data
                ids.Add(AddressablesAssetsConfig.DataCenterHolder);
                // Language
                ids.Add(AddressablesAssetsConfig.GameLanguage);
                // Popup
                ids.Add(AddressablesAssetsConfig.PopupTrans);
                ids.Add(AddressablesAssetsConfig.HUDPopup);

                this.DoneLoad = false;
                this.LoadSuccess = false;
                this.PreloadLoadAssets(ids);
            }
            catch (System.Exception e)
            {
                NTLog.LogError(e.Message);
                this.LoadSuccess = false;
                this.DoneLoad = true;
            }

        }

        private string TransformAddress(IResourceLocation location)
        {
            string baseUrl = AddressablesAssetsConfig.BaseUrl;
            if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer)
            {
                baseUrl = SystemManager.Instance.SystemData.IOS_Addressable;
            }
            else
            {
                baseUrl = SystemManager.Instance.SystemData.Android_Addressable;
            }
            if(baseUrl == null || baseUrl == "")
            {
                baseUrl = AddressablesAssetsConfig.BaseUrl;
            }

            string internalId = location.InternalId;

            // Handle default CDN paths or ServerData references
            if (internalId.StartsWith("http"))
            {
                // Replace any existing URL with your server base
                if (internalId.Contains("ServerData"))
                {
                    int index = internalId.LastIndexOf("ServerData/");
                    string relative = internalId.Substring(index);
                    internalId = baseUrl + relative;
                }
                else
                {
                    // fallback if no ServerData in path
                    internalId = baseUrl + internalId.Substring(internalId.LastIndexOf('/') + 1);
                }
            }
            else if (internalId.Contains("ServerData"))
            {
                // local build path — convert to remote
                int index = internalId.LastIndexOf("ServerData/");
                string relative = internalId.Substring(index);
                internalId = baseUrl + relative;
            }

            return internalId;
        }

        #endregion

        #region Download
        // Using for load cache assets
        public void PreloadLoadAssets(List<string> ids)
        {
            foreach (string item in ids)
            {
                NTLog.LogMessage(item + " assets loadeding.");

            }
            this.Handles = Addressables.LoadAssetsAsync<GameObject>(ids, OnAssetLoaded, Addressables.MergeMode.Union, false);
            this.Handles.Completed += (operation) => PreloadOnAllAssetsLoaded(operation);
        }

        private void OnAssetLoaded(GameObject result)
        {
            NTLog.LogMessage(result.name + " assets loaded successfully.");
        }

        private void PreloadOnAllAssetsLoaded(AsyncOperationHandle<IList<GameObject>> handle)
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                NTLog.LogMessage(handle.Result.Count + " All assets loaded successfully.");
                this.DoneLoad = true;
                this.LoadSuccess = true;
            }
            else
            {
                NTLog.LogError("Failed to load one or more assets.");
                this.DoneLoad = true;
                this.LoadSuccess = false;
            }

            // Optionally release the handle after use
            Addressables.Release(handle);
        }

        public void LoadAssetsFromLocal(string id, Action<GameObject> done = null)
        {
            AsyncOperationHandle<long> sizeHandle = Addressables.GetDownloadSizeAsync(id);
            sizeHandle.Completed += (operation) =>
            {
                if (operation.Result == 0)
                {
                    NTLog.LogMessage("Download size: " + operation.Result);
                    Addressables.InstantiateAsync(id).Completed += handle =>
                    {
                        if (handle.Status == AsyncOperationStatus.Succeeded)
                        {
                            GameObject obj = handle.Result;
                            done?.Invoke(obj);
                        }
                        else
                        {
                            NTLog.LogError("Failed to load assets from local InstantiateAsync: " + id);
                            this.LoadAssetFromServer(id, done);
                        }
                    };
                }
                else
                {
                    NTLog.LogError("Failed to load assets from local : " + id);
                    this.LoadAssetFromServer(id, done);
                }
            };
        }

        public void LoadAssetFromServer(string id, Action<GameObject> done = null)
        {
            Addressables.LoadAssetAsync<GameObject>(id).Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    GameObject obj = handle.Result;
                    done?.Invoke(obj);
                }
                else
                {
                    NTLog.LogError("Failed to load assets from server: " + id);
                    done?.Invoke(null);
                }
            };
        }

        #endregion

        #region Getter
        public string GetProcessValue()
        {
            try
            {
                if (AddressablesLoader.Instance.Handles.GetDownloadStatus().DownloadedBytes < 10) return "";
                return BytesToReadable(AddressablesLoader.Instance.Handles.GetDownloadStatus().DownloadedBytes) + "/" + BytesToReadable(AddressablesLoader.Instance.Handles.GetDownloadStatus().TotalBytes);
            }
            catch (System.Exception)
            {
                return "";
            }
        }

        public double GetProcessPercent()
        {
            try
            {
                double per = (double)AddressablesLoader.Instance.Handles.GetDownloadStatus().DownloadedBytes / (double)(AddressablesLoader.Instance.Handles.GetDownloadStatus().TotalBytes + 1);
                if (per > 1) return 1;
                if (per < 0) return 0;
                return per;
            }
            catch (System.Exception)
            {
                return 0;
            }
        }

        // Funtion change Bytes to KB, MB, GB, TB
        public string BytesToReadable(long bytes)
        {
            string[] Suffix = { "B", "KB", "MB", "GB", "TB" };
            int i = 0;
            double dblSByte = bytes;
            if (bytes > 1024)
                for (i = 0; (int)(bytes / 1024) > 0; i++, bytes /= 1024)
                    dblSByte = bytes / 1024.0;
            return String.Format("{0:0.##} {1}", dblSByte, Suffix[i]);
        }

        #endregion
    }
}