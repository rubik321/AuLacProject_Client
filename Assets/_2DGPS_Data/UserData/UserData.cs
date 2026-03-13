using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rubik._2DGPS.UserData
{
    [System.Serializable]
    public class UserData
    {
        public string _id;

        public string AccountID;

        public string DisplayName;
        public int Avatar;
        public int Server;

        public int Stage;
        public int LastLogin;

        public int Coin;
        public int GoldBar;
        public int SummonToken;
        public int PromotionGem;
        public int Iron;
        public int NightmareCrystal;

        public int WoodChest;
        public int BronzeChest;
        public int GoldChest;
        public int PlatinumChest;
        public int DiamondChest;
        public int ScoreChest;

        public double IdleLoot_LastTime;
        public int IdleLoot_Lv;
        public int IdleLoot_RewardUnlocked;

        public int ArenaTicket;

        public int RandEpicShard;
        public int RandLegendaryShard;
        public int RandMythicShard;
        public int OmniEpicShard;
        public int OmniLegendaryShard;
        public int OmniMythicShard;

        public int Prism;

        public int TowerStage;
        public int TowerEnergy;
        public double TowerEnergy_Recover;

        public int CommonRod;
        public int GoldenRod;
        public int Jade;

        public int Wrench;

        public string[] CardTeam;

        public int CampaignLv;
        public SkinData SkinData;
        public ShortUserData GetShortData()
        {
            var shortData = new ShortUserData();
            shortData._id = this._id;
            shortData.DisplayName = this.DisplayName;
            shortData.Avatar = this.Avatar;
            shortData.Server = this.Server;
            shortData.Stage = this.Stage;
            shortData.LastLogin = this.LastLogin;
            return shortData;
        }
    }

    [System.Serializable]
    public class ShortUserData
    {
        public string _id;
        public string DisplayName;
        public int Avatar;
        public int Server;
        public int Stage;
        public int LastLogin;
    }
    [System.Serializable]
    public class SkinData
    {
        public string Weapon = "Weapons/sword";
        public string Hair = "Hair/hair 0";
        public string Scar = "Scar/scar 1";
        public string Eye = "Eye/eye 2"; 
        public string Suit = "Suit/suit 3";
    }
    [System.Serializable]
    public class ListShortUserData
    {
        public List<ShortUserData> ShortUserDatas = new();
    }
}