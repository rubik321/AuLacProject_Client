using System.Collections;
using System.Collections.Generic;
using NTPackage.Functions;
using UnityEngine;

namespace Rubik.Myrk.BattleTeam
{
    using Rubik.CardPlayer;
    using Rubik.CharacterGear;
    using Rubik.CharacterPlayer;
    using Rubik.DataType;
    using Rubik.UserDataPlayer;
    using SimpleJSON;

    [System.Serializable]
    public class BattleTeamData
    {
        public int Index;
        public string CharacterID;
        public string[] GearIDs;
        public string[] CardTeam;

        // Cache
        public CharacterPlayer CharacterPlayer;
        public List<CharacterGear> CharacterGears;
        public List<CardPlayer> CardPlayers;
        public DataType.GearStats HeroStats;
        public DataType.GearStats GearStats;

        public BattleTeamData()
        {

        }

        public BattleTeamData(int index)
        {
            this.Index = index;
        }


        public void Update(BattleTeamData data)
        {
            this.Index = data.Index;
            this.CharacterID = data.CharacterID;
            this.CardTeam = data.CardTeam;
            this.GearIDs = data.GearIDs;
        }

        public BattleShortTeam ToShortTeam()
        {
            BattleShortTeam shortTeam = new BattleShortTeam();
            shortTeam.UserID = UserDataManager.Instance.GetUserID();
            CharacterShortTeam characterShortTeam = new CharacterShortTeam();
            if (this.CharacterPlayer == null || this.CharacterPlayer._id == null || this.CharacterPlayer._id == "")
            {
                characterShortTeam = null;
            }
            else
            {
                characterShortTeam._id = this.CharacterPlayer._id;
                characterShortTeam.Index = this.CharacterPlayer.Index;
            }
            List<GearShortTeam> gearShortTeams = new List<GearShortTeam>();
            foreach (CharacterGear gear in this.CharacterGears)
            {
                if (gear == null || gear._id == null || gear._id == "")
                {
                    continue;
                }
                GearShortTeam gearShortTeam = new GearShortTeam();
                gearShortTeam._id = gear._id;
                gearShortTeam.Index = gear.Index;
                gearShortTeam.Level = gear.Lv;
                gearShortTeam.Rarity = gear.Rarity;
                gearShortTeams.Add(gearShortTeam);
            }
            shortTeam.Gears = gearShortTeams.ToArray();
            shortTeam.Hero = characterShortTeam;

            List<CardShortTeam> cardShortTeams = new List<CardShortTeam>();
            for (int i = 0; i < this.CardPlayers.Count; i++)
            {
                if (this.CardPlayers[i] == null || this.CardPlayers[i]._id == null || this.CardPlayers[i]._id == "") continue;
                CardShortTeam cardShortTeam = this.CardPlayers[i].ToShortTeam();
                cardShortTeam.Slot = i;
                cardShortTeams.Add(cardShortTeam);
            }

            shortTeam.Cards = cardShortTeams.ToArray();
            shortTeam.Power = BattleTeamManager.Instance.GetPower(shortTeam);
            return shortTeam;
        }

        public JSONNode ToJson()
        {
            JSONNode json = new JSONObject();
            json["Index"] = this.Index;
            json["CharacterID"] = this.CharacterID;
            json["CardTeam"] = this.CardTeam;
            json["GearIDs"] = this.GearIDs;
            return json;
        }

        public bool Compare(BattleTeamData data)
        {
            if (this.CharacterID != data.CharacterID)
            {
                return false;
            }
            if(this.CardTeam.Length != data.CardTeam.Length)
            {
                NTLog.LogMessage("BattleTeamData Compare: this.CardTeam.Length != data.CardTeam.Length");
                return false;
            }
            for (int i = 0; i < this.CardTeam.Length; i++)
            {
                NTLog.LogMessage("BattleTeamData Compare: this.CardTeam[i]:" + this.CardTeam[i] + " data.CardTeam[i]:" + data.CardTeam[i]);
                if (this.CardTeam[i] != data.CardTeam[i])
                {
                    return false;
                }
            }
            if(this.GearIDs.Length != data.GearIDs.Length)
            {
                NTLog.LogMessage("BattleTeamData Compare: this.GearIDs.Length != data.GearIDs.Length");
                return false;
            }
            for (int i = 0; i < this.GearIDs.Length; i++)
            {
                NTLog.LogMessage("BattleTeamData Compare: this.GearIDs[i]:" + this.GearIDs[i] + " data.GearIDs[i]:" + data.GearIDs[i]);
                if (this.GearIDs[i] != data.GearIDs[i])
                {
                    return false;
                }
            }
            return true;
        }
    }

    [System.Serializable]
    public class BattleShortTeam
    {
        public string UserID;
        public CardShortTeam[] Cards;
        public CharacterShortTeam Hero;
        public GearShortTeam[] Gears;
        public double Power;
    }

    [System.Serializable]
    public class CardShortTeam
    {
        public string _id;
        public CardPlayerIndex Index;
        public int Slot;
        public int Level;
        public int Star;
    }

    [System.Serializable]
    public class CharacterShortTeam
    {
        public string _id;
        public int Index;
    }

    [System.Serializable]
    public class GearShortTeam
    {
        public string _id;
        public int Level;
        public CharacterGearIndex Index;
        public RarityType Rarity;
    }

    [System.Serializable]
    public class BattleTeamSelected
    {
        public string _id;
        public int BattleSelected;
        public int ArenaDefendSelected;
    }

    [System.Serializable]
    public class UpdateBattleShortTeam
    {
        public BattleShortTeam BattleTeamShort;
        public BattleShortTeam ArenaDefendShortTeam;
    }
}