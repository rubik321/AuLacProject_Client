using System;
using System.Collections;
using System.Collections.Generic;
using NTPackage;
using NTPackage.EventDispatcher;
using NTPackage.Functions;
using Rubik.CardPlayer;
using Rubik.CharacterCloth;
using Rubik.DataCenter;
using Rubik.Manager;
using Rubik.UserDataPlayer;
using SimpleJSON;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Rubik.CharacterPlayer
{

    using CharacterGear;
    using NTPackage.UI;
    using Rubik.Myrk.BattleTeam;
    using Rubik.Myrk.Guide;
    using Rubik.Server;

    public class CharacterPlayerConfig
    {
        public const string API_Add_Random_CharacterPlayer = "/api/2D_GPS/character_player/add_random";
        public const string API_Select_CharacterPlayer = "/api/2D_GPS/character_player/select_character";
    }

    public class CharacterPlayerManager : NTBehaviour
    {
        #region Player Data
        [Header("Player Data")]
        [SerializeField] private NTDictionary<string, CharacterPlayer> CharacterPlayer;
        #endregion

        #region Game Data
        [Header("Game Data")]
        [SerializeField] private NTDictionary<string, CharacterPlayerData> CharacterPlayerData;
        #endregion

        public static CharacterPlayerManager Instance;
        protected override void Awake()
        {
            base.Awake();
            if (Instance != null)
            {
                NTLog.LogError("Only 1 Instance allow");
                return;
            }
            Instance = this;
        }

        #region Function
        public IEnumerator LoadData()
        {
            CharacterPlayer = new NTDictionary<string, CharacterPlayer>();

            CharacterPlayerData = new NTDictionary<string, CharacterPlayerData>();
            JSONNode jsonNode = JSONNode.Parse(DataCenterManager.Instance.GetData(DataName.CharacterPlayerData));
            foreach (JSONNode item in jsonNode)
            {
                CharacterPlayerData characterPlayerData = JsonUtility.FromJson<CharacterPlayerData>(item.ToString());
                CharacterPlayerData.Add(characterPlayerData.Index.ToString(), characterPlayerData);
            }
            yield return null;
        }

        public void Logout()
        {
            CharacterPlayer.Clear();
        }

        public void UpdateCharacterPlayer(CharacterPlayer[] characterPlayers)
        {
            if (characterPlayers == null || characterPlayers.Length == 0) return;

            foreach (CharacterPlayer characterPlayer in characterPlayers)
            {
                CharacterPlayer.Add(characterPlayer._id, characterPlayer);
            }
        }


        public void UpdateCacheCharacterPlayer(CharacterPlayer characterPlayer)
        {
            
        }

        public void OnTooltipStatsGuide(){
            string title = Lean.Localization.LeanLocalization.GetTranslationText("hero_stats_title");
            string desc = Lean.Localization.LeanLocalization.GetTranslationText("hero_stats_desc");
            PopupManager.Instance.OnUI(PopupCode.GuideUI, null, (popup) =>
            {
                GuideUI guideUI = popup as GuideUI;
                guideUI.SetData(title, desc);
            });
        }

        #endregion

        #region API
        public IEnumerator IEAddRandomCharacterPlayer(Action<CharacterPlayer[]> done = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.Instance.GetUserID();
            yield return APIManager.Instance.PostDataUrl(jdata.ToString(), Rubik.Config.URL_Config.BASE_API_URL + CharacterPlayerConfig.API_Add_Random_CharacterPlayer, (data) =>
            {
                APIResponseData apiResponse = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if(apiResponse.Status == 0) return;
                done?.Invoke(apiResponse.Update_CharacterPlayer);
            });
        }

        public IEnumerator IESelectCharacterPlayer(string characterPlayerID, Action done = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.Instance.GetUserID();
            jdata["characterPlayerID"] = characterPlayerID;
            yield return APIManager.Instance.PostDataUrl(jdata.ToString(), Rubik.Config.URL_Config.BASE_API_URL + CharacterPlayerConfig.API_Select_CharacterPlayer, (data) =>
            {
                APIResponseData apiResponse = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if(apiResponse.Status == 0) return;
                done?.Invoke();
                EventListenerManager.instance.PostEvent(EventCode.ChangeCharacter);
            });
        }
        #endregion

        #region Getter
        public CharacterPlayer GetCharacterPlayer(string _id)
        {
            return CharacterPlayer.Get(_id);
        }

        public CharacterPlayer GetFirstCharacterPlayer()
        {
            if (this.CharacterPlayer.Count == 0) return null;
            return this.CharacterPlayer.ToList()[0];
        }

        public List<CharacterPlayer> GetAllCharacterPlayer()
        {
            return CharacterPlayer.ToList();
        }
        #endregion

    }
}
