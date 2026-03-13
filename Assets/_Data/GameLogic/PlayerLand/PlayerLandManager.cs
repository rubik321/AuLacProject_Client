using System;
using System.Collections;
using System.Collections.Generic;
using NTPackage;
using NTPackage.Functions;
using Rubik.DataType;
using Rubik.Manager;
using Rubik.UserDataPlayer;
using SimpleJSON;
using UnityEngine;

namespace Rubik.PlayerLand
{
    using Rubik.AddressablesLoader;
    using Rubik.Server;
    public class PlayerLandConfig
    {
        public const string PlayerLandData = "/api/2D_GPS/player_land/chose";
    }

    public class PlayerLandManager : NTBehaviour
    {
        #region Data Player
        public NTDictionary<string, PlayerLand> DataPlayerLand;
        #endregion

        #region Resources
        public ListSpriteAddressable LandSpritesAddressable;
        #endregion

        public static PlayerLandManager Instance;
        protected override void Awake()
        {
            base.Awake();
            if (Instance != null)
            {
                NTLog.LogWarning("Only 1 instance allow");
                return;
            }
            Instance = this;
        }

        #region Function

        public IEnumerator LoadData()
        {
            int count = 0;
            AddressablesLoader.Instance.LoadAssetsFromLocal(AddressablesAssetsConfig.LandSprite, (result) =>
            {
                this.LandSpritesAddressable = result.GetComponent<ListSpriteAddressable>();
                this.LandSpritesAddressable.transform.SetParent(transform);
                count++;
            });
            yield return new WaitUntil(() => count >= 1);
        }

        public void LogOut()
        {
            this.DataPlayerLand.Clear();
        }

        public void UpdatePlayerLand(PlayerLand[] playerLands)
        {
            foreach (var playerLand in playerLands)
            {
                this.DataPlayerLand.Add(playerLand._id, playerLand);
            }
        }
        #endregion

        #region API
        public IEnumerator IEChosePlayerLand(RaceType race, Action done = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.Instance.GetUserID();
            jdata["index"] = (int)race;
            yield return APIManager.Instance.PostDataUrl(jdata.ToString(), Rubik.Config.URL_Config.BASE_API_URL + PlayerLandConfig.PlayerLandData, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                done?.Invoke();
            });
        }
        #endregion

        #region Getter
        public Sprite GetLandSprite(RaceType race)
        {
            return this.LandSpritesAddressable.ListSprite[(int)race % this.LandSpritesAddressable.ListSprite.Count];
        }

        public Sprite GetPlayerSprite()
        {
            return this.GetLandSprite(this.GetPlayerLandSelected()?.Index ?? RaceType.Gaia);
        }

        public PlayerLand GetPlayerLandSelected()
        {
            if (this.DataPlayerLand.Count == 0)
                return null;
            return this.DataPlayerLand.ToList()[0];
        }

        public string GetPlayerLandName(RaceType race)
        {
            return Lean.Localization.LeanLocalization.GetTranslationText("race_name_" + (int)race, race.ToString());
        }

        public string GetPlayerLandDescription(RaceType race)
        {
            return Lean.Localization.LeanLocalization.GetTranslationText("race_detail_" + (int)race, race.ToString());
        }
        #endregion
    }
}