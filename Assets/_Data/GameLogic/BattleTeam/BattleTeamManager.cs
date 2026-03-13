using System.Collections;
using System.Collections.Generic;
using NTPackage;
using NTPackage.Functions;
using UnityEngine;

namespace Rubik.Myrk.BattleTeam
{
    using System;
    using System.Linq;
    using Rubik.CardPlayer;
    using Rubik.CharacterGear;
    using Rubik.CharacterPlayer;
    using Rubik.DataType;
    using Rubik.Manager;
    using Rubik.UserDataPlayer;
    using SimpleJSON;

    public class BattleTeamConfig
    {
        public const string API_Update_BattleTeam = "/api/2D_GPS/battle_team/update_team";
        public const string API_Update_BattleTeamSelected = "/api/2D_GPS/battle_team/update_battle_team_selected";
        public const string API_Update_BattleTeamAuto = "/api/2D_GPS/battle_team/update_battle_short_team";

        public static readonly int[] BattleTeamIndex = new int[] { 0, 1, 2, 3, 4 };
        public static int ArenaDefendTeamIndex = 101;

        public static string[] DefaultCharacterGearEquips = new string[] {
            "", // Hair
            "", // Armor
            "", // Weapon
            "", // Shield
            "", // Bracelet
            "", // Ring
            "", // Book
        };
    }

    public class BattleTeamManager : NTBehaviour
    {
        #region Player Data
        [Header("Player Data")]
        public NTDictionary<int, BattleTeamData> BattleTeamData;
        public BattleTeamData BattleTeamDataTemp;
        public BattleTeamSelected BattleTeamSelected;
        public UpdateBattleShortTeam BattleShortTeam;
        public bool IsInit = false;
        #endregion

        public Coroutine CorUpdateBattleShortTeam;

        public static BattleTeamManager Instance;
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
        public void UpdateBattleTeam(BattleTeamData[] battleTeamData)
        {
            this.IsInit = true;
            foreach (BattleTeamData data in battleTeamData)
            {
                if (this.BattleTeamData.Get(data.Index) == null)
                {
                    this.BattleTeamData.Add(data.Index, data);
                }
                else
                {
                    this.BattleTeamData.Get(data.Index).Update(data);
                }
            }
            if (this.CorUpdateBattleShortTeam != null)
            {
                StopCoroutine(this.CorUpdateBattleShortTeam);
                this.CorUpdateBattleShortTeam = null;
            }
            this.CorUpdateBattleShortTeam = StartCoroutine(IEUpdateBattleTeamAuto());
            this.UpdateCacheBattleTeam();
        }

        public void UpdateBattleTeamSelected(BattleTeamSelected battleTeamSelected)
        {
            if(battleTeamSelected == null || battleTeamSelected._id == null || battleTeamSelected._id == ""){
                return;
            }
            this.BattleTeamSelected = battleTeamSelected;
        }

        public void UpdateBattleShortTeam(UpdateBattleShortTeam updateBattleShortTeam)
        {
            if(updateBattleShortTeam == null || updateBattleShortTeam.BattleTeamShort == null || updateBattleShortTeam.BattleTeamShort.UserID == null || updateBattleShortTeam.BattleTeamShort.UserID == ""){
                return;
            }
            this.BattleShortTeam = updateBattleShortTeam;
        }

        public void Logout()
        {
            this.BattleTeamData.Clear();
            this.IsInit = false;
            this.BattleTeamSelected = null;
            this.BattleShortTeam = null;
            if (this.CorUpdateBattleShortTeam != null)
            {
                StopCoroutine(this.CorUpdateBattleShortTeam);
                this.CorUpdateBattleShortTeam = null;
            }
        }


        public void UpdateBattleCards(string[] cardIDs, int index)
        {
            NTLog.LogMessage("UpdateBattleCards: " + cardIDs.Length);
            if (this.BattleTeamDataTemp == null)
            {
                this.BattleTeamDataTemp = NTFunction.Clone(this.GetBattleTeamDataByIndex(index));
            }
            this.BattleTeamDataTemp.CardTeam = cardIDs;
            this.BattleTeamDataTemp.Index = index;
            this.UpdateCacheBattleTeamTemp();
        }

        public void ConfirmUpdateBattleCards(int index, Action<BattleTeamData> callback = null)
        {
            BattleTeamData battleTeamData = this.GetBattleTeamDataByIndex(index);
            if (battleTeamData == null)
            {
                NTLog.LogMessage("ConfirmUpdateBattleCards: battleTeamData is null:" + index);
                callback?.Invoke(null);
                return;
            }
            if (this.BattleTeamDataTemp == null)
            {
                NTLog.LogMessage("ConfirmUpdateBattleCards: this.BattleTeamDataTemp is null:" + index);
                return;
            }
            this.BattleTeamDataTemp.Index = index;
            if (battleTeamData.Compare(this.BattleTeamDataTemp))
            {
                NTLog.LogMessage("ConfirmUpdateBattleCards: battleTeamData is equal to this.BattleTeamDataTemp:" + index + ":"+battleTeamData.Compare(this.BattleTeamDataTemp));
                this.BattleTeamDataTemp = null;
                callback?.Invoke(battleTeamData);
                return;
            }
            StartCoroutine(IEUpdateBattleTeam(this.BattleTeamDataTemp, callback));
        }

        public void UpdateBattleGears(string[] gearIDs, int index)
        {
            if (this.BattleTeamDataTemp == null || this.BattleTeamDataTemp.CardTeam == null || this.BattleTeamDataTemp.CardTeam.Length == 0)
            {
                this.BattleTeamDataTemp = NTFunction.Clone(this.GetBattleTeamDataByIndex(index));
            }
            this.BattleTeamDataTemp.GearIDs = gearIDs;
            this.BattleTeamDataTemp.Index = index;
            this.UpdateCacheBattleTeamTemp();
        }

        public void ConfirmUpdateBattleGears(int index, Action<BattleTeamData> callback = null)
        {
            if (this.BattleTeamDataTemp == null || this.BattleTeamDataTemp.CardTeam == null || this.BattleTeamDataTemp.CardTeam.Length == 0) return;
            BattleTeamData battleTeamData = this.GetBattleTeamDataByIndex(index);
            if (battleTeamData == null)
            {
                callback?.Invoke(null);
                return;
            }
            if (this.BattleTeamDataTemp == null)
            {
                return;
            }
            this.BattleTeamDataTemp.Index = index;
            if (battleTeamData.Compare(this.BattleTeamDataTemp))
            {
                this.BattleTeamDataTemp = null;
                callback?.Invoke(battleTeamData);
                return;
            }
            StartCoroutine(IEUpdateBattleTeam(this.BattleTeamDataTemp, callback));
        }

        public void UpdateCacheBattleTeam()
        {
            NTLog.LogMessage("UpdateCacheBattleTeam");
            // UnCache Gear
            foreach (BattleTeamData data in this.BattleTeamData.ToList())
            {
                foreach (CharacterGear gear in data.CharacterGears)
                {
                    if (gear != null)
                    {
                        gear.Teams = new List<int>();
                    }
                }
            }
            // UnCache Card
            foreach (BattleTeamData data in this.BattleTeamData.ToList())
            {
                data.CardPlayers = new List<CardPlayer>();
                foreach (CardPlayer card in data.CardPlayers)
                {
                    if (card != null)
                    {
                        card.Teams = new List<BattleTeamData>();
                    }
                }
            }
            // UnCache Character
            foreach (BattleTeamData data in this.BattleTeamData.ToList())
            {
                if (data.CharacterPlayer != null)
                {
                    data.CharacterPlayer.Teams = new List<int>();
                }
            }

            // Cache Gear
            foreach (BattleTeamData data in this.BattleTeamData.ToList())
            {
                data.CharacterGears = new List<CharacterGear>();
                data.GearStats = new DataType.GearStats();
                foreach (string gearID in data.GearIDs)
                {
                    CharacterGear gear = CharacterGearManager.Instance.GetCharacterGearByID(gearID);
                    if (gear != null)
                    {
                        if (gear.Teams == null)
                        {
                            gear.Teams = new List<int>();
                        }
                        CharacterGearManager.Instance.UpdateCacheCharacterGear(gear);
                        gear.Teams.Add(data.Index);
                        data.CharacterGears.Add(gear);
                        data.GearStats.AddStats(gear.GearStats);
                    }
                    else
                    {
                        data.CharacterGears.Add(null);
                    }
                }
            }
            // Cache Card
            foreach (BattleTeamData data in this.BattleTeamData.ToList())
            {
                data.CardPlayers = new List<CardPlayer>();
                foreach (string cardID in data.CardTeam)
                {
                    CardPlayer card = CardPlayerManager.Instance.GetCardByID(cardID);
                    if (card != null)
                    {
                        if (card.Teams == null)
                        {
                            card.Teams = new List<BattleTeamData>();
                        }
                        card.Teams.Add(data);
                        data.CardPlayers.Add(card);
                    }
                    else
                    {
                        data.CardPlayers.Add(null);
                    }
                }
            }
            // Cache Character
            foreach (BattleTeamData data in this.BattleTeamData.ToList())
            {
                data.CharacterPlayer = CharacterPlayerManager.Instance.GetCharacterPlayer(data.CharacterID);
                if (data.CharacterPlayer != null)
                {
                    if (data.CharacterPlayer.Teams == null)
                    {
                        data.CharacterPlayer.Teams = new List<int>();
                    }
                    data.CharacterPlayer.Teams.Add(data.Index);
                }
                else
                {
                    data.CharacterPlayer = null;
                }
            }
        }

        public void UpdateCacheBattleTeamTemp()
        {
            NTLog.LogMessage("UpdateCacheBattleTeamTemp");
            if (this.BattleTeamDataTemp == null || this.BattleTeamDataTemp.CardTeam == null || this.BattleTeamDataTemp.CardTeam.Length == 0) return;

            // UnCache Gear
            if (this.BattleTeamDataTemp.CharacterGears != null && this.BattleTeamDataTemp.CharacterGears.Count > 0)
            {
                foreach (CharacterGear gear in this.BattleTeamDataTemp.CharacterGears)
                {
                    if (gear != null)
                    {
                        gear.Teams = new List<int>();
                    }
                }
            }

            // UnCache Card
            if (this.BattleTeamDataTemp.CardPlayers != null && this.BattleTeamDataTemp.CardPlayers.Count > 0)
            {
                foreach (CardPlayer card in this.BattleTeamDataTemp.CardPlayers)
                {
                    if (card != null)
                    {
                        card.Teams = new List<BattleTeamData>();
                    }
                }
            }

            this.BattleTeamDataTemp.CharacterGears = new List<CharacterGear>();
            this.BattleTeamDataTemp.CardPlayers = new List<CardPlayer>();
            this.BattleTeamDataTemp.CharacterPlayer = CharacterPlayerManager.Instance.GetCharacterPlayer(this.BattleTeamDataTemp.CharacterID);
            this.BattleTeamDataTemp.GearStats = new DataType.GearStats();

            // Cache Gear
            this.BattleTeamDataTemp.CharacterGears = new List<CharacterGear>();
            this.BattleTeamDataTemp.GearStats = new DataType.GearStats();
            foreach (string gearID in this.BattleTeamDataTemp.GearIDs)
            {
                CharacterGear gear = CharacterGearManager.Instance.GetCharacterGearByID(gearID);
                if (gear != null)
                {
                    if (gear.Teams == null)
                    {
                        gear.Teams = new List<int>();
                    }
                    CharacterGearManager.Instance.UpdateCacheCharacterGear(gear);
                    gear.Teams.Add(this.BattleTeamDataTemp.Index);
                    this.BattleTeamDataTemp.CharacterGears.Add(gear);
                    this.BattleTeamDataTemp.GearStats.AddStats(gear.GearStats);
                }
                else
                {
                    this.BattleTeamDataTemp.CharacterGears.Add(null);
                }
            }
            // Cache Card
            this.BattleTeamDataTemp.CardPlayers = new List<CardPlayer>();
            foreach (string cardID in this.BattleTeamDataTemp.CardTeam)
            {
                CardPlayer card = CardPlayerManager.Instance.GetCardByID(cardID);
                if (card != null)
                {
                    if (card.Teams == null)
                    {
                        card.Teams = new List<BattleTeamData>();
                    }
                    card.Teams.Add(this.BattleTeamDataTemp);
                    this.BattleTeamDataTemp.CardPlayers.Add(card);
                }
                else
                {
                    this.BattleTeamDataTemp.CardPlayers.Add(null);
                }
            }
            // Cache Character
            this.BattleTeamDataTemp.CharacterPlayer = CharacterPlayerManager.Instance.GetCharacterPlayer(this.BattleTeamDataTemp.CharacterID);
            if (this.BattleTeamDataTemp.CharacterPlayer != null)
            {
                if (this.BattleTeamDataTemp.CharacterPlayer.Teams == null)
                {
                    this.BattleTeamDataTemp.CharacterPlayer.Teams = new List<int>();
                }
                this.BattleTeamDataTemp.CharacterPlayer.Teams.Add(this.BattleTeamDataTemp.Index);
            }
            else
            {
                CharacterPlayer characterPlayer = CharacterPlayerManager.Instance.GetFirstCharacterPlayer();
                this.BattleTeamDataTemp.CharacterID = characterPlayer._id;
                this.BattleTeamDataTemp.CharacterPlayer = characterPlayer;
                this.BattleTeamDataTemp.CharacterPlayer.Teams.Add(this.BattleTeamDataTemp.Index);
            }
        }

        #endregion

        #region API
        public IEnumerator IEUpdateBattleTeam(BattleTeamData battleTeamData, Action<BattleTeamData> callback = null)
        {
            this.UpdateCacheBattleTeamTemp();
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.Instance.GetUserID();
            jdata["battleTeamData"] = battleTeamData.ToJson();
            jdata["battleShortTeam"] = JSONNode.Parse(JsonUtility.ToJson(battleTeamData.ToShortTeam()));
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(jdata.ToString(), Rubik.Config.URL_Config.BASE_API_URL + BattleTeamConfig.API_Update_BattleTeam, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                this.UpdateCacheBattleTeam();
                this.BattleTeamDataTemp = null;
                callback?.Invoke(apiResponseData.Update_BattleTeam[0]);
            });
        }

        public IEnumerator IEUpdateBattleTeamSelected(BattleTeamSelected battleTeamSelected, Action callback = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.Instance.GetUserID();
            jdata["battleSelected"] = battleTeamSelected.BattleSelected;
            jdata["arenaDefendSelected"] = battleTeamSelected.ArenaDefendSelected;
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(jdata.ToString(), Rubik.Config.URL_Config.BASE_API_URL + BattleTeamConfig.API_Update_BattleTeamSelected, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                callback?.Invoke();
            });
        }

        public IEnumerator IEUpdateBattleTeamAuto(){
            while (true)
            {
                yield return new WaitForSeconds(5);
                NTLog.LogMessage("IEUpdateBattleTeamAuto");
                if (this.BattleShortTeam != null)
                {
                    bool isUpdate = false;
                    if(this.BattleShortTeam.BattleTeamShort.Power != this.GetPower(this.GetBattleTeamDataSelected().ToShortTeam())){
                        isUpdate = true;
                    }
                    if(this.BattleShortTeam.ArenaDefendShortTeam.Power != this.GetPower(this.GetArenaDefendTeamDataSelected().ToShortTeam())){
                        isUpdate = true;
                    }
                    if(!isUpdate) continue;
                }
                UpdateBattleShortTeam battleShortTeam = new UpdateBattleShortTeam();
                battleShortTeam.BattleTeamShort = this.GetBattleTeamDataSelected().ToShortTeam();
                battleShortTeam.ArenaDefendShortTeam = this.GetArenaDefendTeamDataSelected().ToShortTeam();
                this.BattleShortTeam = battleShortTeam;
                StartCoroutine(IEUpdateBattleTeam(this.BattleShortTeam, null));
            }
        }

        public IEnumerator IEUpdateBattleTeam(UpdateBattleShortTeam updateBattleShortTeam, Action callback = null)
        {
            JSONNode jdata = new JSONObject();
            jdata["userID"] = UserDataManager.Instance.GetUserID();
            jdata["updateBattleShortTeam"] = JSONNode.Parse(JsonUtility.ToJson(updateBattleShortTeam));
            yield return Rubik.Server.APIManager.Instance.PostDataUrl(jdata.ToString(), Rubik.Config.URL_Config.BASE_API_URL + BattleTeamConfig.API_Update_BattleTeamAuto, (data) =>
            {
                APIResponseData apiResponseData = ServerManager.Instance.APIResponse(data.downloadHandler.text);
                if (apiResponseData.Status == 0) return;
                callback?.Invoke();
            }, false);
        }


        #endregion

        #region Getter
        // To do: need to update when battle team selected is changed
        public List<CardPlayer> GetBattleCardPlayer()
        {
            return GetBattleTeamDataSelected().CardPlayers;
        }

        public List<CardPlayer> GetArenaDefendCardPlayer()
        {
            return GetArenaDefendTeamDataSelected().CardPlayers;
        }

        public List<CardPlayer> GetBattleCardPlayerByIndex(int index)
        {
            return GetBattleTeamDataByIndex(index).CardPlayers;
        }


        public CharacterPlayer GetBattleCharacterPlayer()
        {
            return GetBattleTeamDataSelected().CharacterPlayer;
        }

        public CharacterPlayer GetArenaDefendCharacterPlayer()
        {
            return GetArenaDefendTeamDataSelected().CharacterPlayer;
        }

        public CharacterPlayer GetBattleCharacterPlayerByIndex(int index)
        {
            return GetBattleTeamDataByIndex(index).CharacterPlayer;
        }

        public List<CharacterGear> GetCharacterGearBattle()
        {
            List<CharacterGear> characterGears = new List<CharacterGear>();
            foreach (CharacterGear gear in GetBattleTeamDataSelected().CharacterGears)
            {
                if (gear != null && gear._id != null && gear._id != "")
                {
                    characterGears.Add(gear);
                }
            }
            return characterGears;
        }
        public List<string> GetCharacterGearIDsBattle()
        {
            List<string> gearIDs = new List<string>(BattleTeamConfig.DefaultCharacterGearEquips);
            for (int i = 0; i < GetBattleTeamDataSelected().GearIDs.Length; i++)
            {
                if (GetBattleTeamDataSelected().GearIDs[i] != null && GetBattleTeamDataSelected().GearIDs[i] != "")
                {
                    CharacterGear gear = CharacterGearManager.Instance.GetCharacterGearByID(GetBattleTeamDataSelected().GearIDs[i]);
                    if (gear != null)
                    {
                        gearIDs[i] = gear._id;
                    }
                    else
                    {
                        gearIDs[i] = "";
                    }
                }else{
                    gearIDs[i] = "";
                }
            }
            return gearIDs;
        }
        public int[] GetGearIndexsBattle()
        {
            BattleTeamData battleTeamData = GetBattleTeamDataSelected();
            if (battleTeamData == null)
            {
                return new int[0];
            }
            List<int> gearIndexs = new List<int>();
            foreach (CharacterGear gear in battleTeamData.CharacterGears)
            {
                if (gear == null)
                {
                    continue;
                }
                gearIndexs.Add((int)gear.Index);
            }
            return gearIndexs.ToArray();
        }

        public List<CharacterGear> GetCharacterGearArenaDefend()
        {
            return GetArenaDefendTeamDataSelected().CharacterGears;
        }
        public List<string> GetCharacterGearIDsArenaDefend()
        {
            List<string> gearIDs = new List<string>();
            foreach (string gearID in GetArenaDefendTeamDataSelected().GearIDs)
            {
                if (gearID != null && gearID != "")
                {
                    gearIDs.Add(gearID);
                }
            }
            return gearIDs;
        }
        public int[] GetGearIndexsArenaDefend()
        {
            BattleTeamData battleTeamData = GetArenaDefendTeamDataSelected();
            if (battleTeamData == null)
            {
                return new int[0];
            }
            List<int> gearIndexs = new List<int>();
            foreach (CharacterGear gear in battleTeamData.CharacterGears)
            {
                if (gear == null)
                {
                    continue;
                }
                gearIndexs.Add((int)gear.Index);
            }
            return gearIndexs.ToArray();
        }

        public List<CharacterGear> GetCharacterGearByIndex(int index)
        {
            return GetBattleTeamDataByIndex(index).CharacterGears;
        }
        public List<string> GetCharacterGearIDsByIndex(int index)
        {
            List<string> gearIDs = new List<string>();
            foreach (string gearID in GetBattleTeamDataByIndex(index).GearIDs)
            {
                if (gearID != null && gearID != "")
                {
                    gearIDs.Add(gearID);
                }
            }
            return gearIDs;
        }
        public int[] GetGearIndexsByIndex(int index)
        {
            BattleTeamData battleTeamData = GetBattleTeamDataByIndex(index);
            if (battleTeamData == null)
            {
                return new int[0];
            }
            List<int> gearIndexs = new List<int>();
            foreach (CharacterGear gear in battleTeamData.CharacterGears)
            {
                if (gear == null || gear._id == null || gear._id.Length == 0)
                {
                    continue;
                }
                gearIndexs.Add((int)gear.Index);
            }
            return gearIndexs.ToArray();
        }


        public DataType.GearStats GetHeroStatsBattle()
        {
            BattleTeamData battleTeamData = GetBattleTeamDataSelected();
            if (battleTeamData == null || battleTeamData.HeroStats == null)
            {
                return new DataType.GearStats();
            }
            return battleTeamData.GearStats;
        }

        public DataType.GearStats GetHeroStatsArenaDefend()
        {
            BattleTeamData battleTeamData = GetArenaDefendTeamDataSelected();
            if (battleTeamData == null || battleTeamData.HeroStats == null)
            {
                return new DataType.GearStats();
            }
            return battleTeamData.GearStats;
        }

        public DataType.GearStats GetHeroStatsByIndex(int index)
        {
            BattleTeamData battleTeamData = GetBattleTeamDataByIndex(index);
            if (battleTeamData == null || battleTeamData.HeroStats == null)
            {
                return new DataType.GearStats();
            }
            return battleTeamData.GearStats;
        }

        public BattleTeamData GetBattleTeamDataSelected()
        {
            BattleTeamData battleTeamData = this.BattleTeamData.Get(this.BattleTeamSelected.BattleSelected);
            if (battleTeamData == null)
            {
                battleTeamData = this.BattleTeamData.ToList()[0];
            }
            // this.UpdateCacheBattleTeam();
            return battleTeamData;
        }

        public BattleTeamData GetArenaDefendTeamDataSelected()
        {
            BattleTeamData battleTeamData = this.BattleTeamData.Get(BattleTeamConfig.ArenaDefendTeamIndex);
            if (battleTeamData == null)
            {
                battleTeamData = NTFunction.Clone(GetBattleTeamDataSelected());
                battleTeamData.Index = BattleTeamConfig.ArenaDefendTeamIndex;
                StartCoroutine(IEUpdateBattleTeam(battleTeamData));
                this.BattleTeamData.Add(BattleTeamConfig.ArenaDefendTeamIndex, battleTeamData);
            }
            battleTeamData.Index = BattleTeamConfig.ArenaDefendTeamIndex;
            return battleTeamData;
        }

        public BattleTeamData GetBattleTeamDataByIndex(int index)
        {
            BattleTeamData battleTeamData = this.BattleTeamData.Get(index);
            if (battleTeamData == null)
            {
                return new BattleTeamData();
            }
            return battleTeamData;
        }

        public BattleTeamData GetBattleTeamDataTemp(int index)
        {
            if (this.BattleTeamDataTemp == null || this.BattleTeamDataTemp.CardTeam == null || this.BattleTeamDataTemp.CardTeam.Length == 0)
            {
                this.BattleTeamDataTemp = NTFunction.Clone(this.GetBattleTeamDataByIndex(index));
                this.UpdateCacheBattleTeamTemp();
            }
            return this.BattleTeamDataTemp;
        }

        public long GetPower(BattleShortTeam battleShortTeam, float scale = 1)
        {
            long power = 0;
            foreach (CardShortTeam card in battleShortTeam.Cards)
            {
                power += this.GetCardPower(card);
            }
            if (battleShortTeam.Hero != null && battleShortTeam.Hero._id != null && battleShortTeam.Hero._id != "")
            {
                power += this.GetHeroPower(battleShortTeam.Hero);
            }
            if (battleShortTeam.Gears != null && battleShortTeam.Gears.Length > 0)
            {
                foreach (GearShortTeam gear in battleShortTeam.Gears)
                {
                    power += this.GetGearPower(gear);
                }
            }
            return (long)(power * scale);
        }

        public long GetCardPower(CardShortTeam cardShortTeam)
        {
            if (cardShortTeam == null)
            {
                return 0;
            }
            CardPlayer cardPlayer = CardPlayerManager.Instance.GetFakeCardPlayerByIndex(cardShortTeam.Index);
            cardPlayer.Lv = cardShortTeam.Level;
            cardPlayer.Star = cardShortTeam.Star;
            CardPlayerManager.Instance.UpdateCacheCardPlayer(cardPlayer);
            long power = cardPlayer.TotalStats.ATK * 5 + cardPlayer.TotalStats.DEF * 5 + cardPlayer.TotalStats.HP + cardPlayer.TotalStats.SPD;
            return power;
        }

        public long GetHeroPower(CharacterShortTeam characterShortTeam)
        {
            return 1;
        }

        public long GetGearPower(GearShortTeam gearShortTeam)
        {
            if (gearShortTeam == null)
            {
                return 0;
            }
            CharacterGearData gearData = CharacterGearManager.Instance.GetGearDataByIndex(gearShortTeam.Index);
            DataType.GearStats stats = CharacterGearManager.Instance.GetStatsByLevel(gearShortTeam.Rarity, gearShortTeam.Level);
            return (long)(stats.HeroAtk * 5 * gearData.GearStats.HeroAtk + stats.TeamHp * gearData.GearStats.TeamHp + stats.TeamSpeed);
        }

        public GearStats GetGearStatsBattle(BattleShortTeam battleShortTeam){
            GearStats gearStats = new GearStats();
            foreach (GearShortTeam gear in battleShortTeam.Gears)
            {
                CharacterGearData gearData = CharacterGearManager.Instance.GetGearDataByIndex(gear.Index);
                DataType.GearStats stats = CharacterGearManager.Instance.GetStatsByLevel(gear.Rarity, gear.Level);
                gearStats.AddStats(stats);
            }
            return gearStats;
        }

        #endregion
    }
}