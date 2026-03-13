using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rubik.UserDataPlayer
{
    using System.Linq;
    using Rubik._2DGPS.UserData;
    using Rubik.ItemPlayer;

    [System.Serializable]
    public class DisplayNameData
    {
        public string DisplayeName = "";
        public bool FirstChange = false;
    }

    public enum Role
    {
        Player = 0,
        Tester = 1,
        Developer = 2,
    }

    [System.Serializable]
    public class UserDataResponse
    {
        public string _id;
        public long PlayerID;
        public string AccountID;
        public string DisplayName;
        public bool FirstChangeName;
        public int Server;
        public long LastLogin;
        public int Level;
        public long Exp;
        public Role Role;
        public bool RemoveAds;
        public bool IsNewDay;
        public bool IsNewWeek;
        public bool IsNewMonth;
    }


    [System.Serializable]
    public class UserInventoryBag
    {
        public string _id;
        public int TimeExpand;
    }


    [System.Serializable]
    public class InventoryBagData
    {
        public ItemData PriceExpand;
        public int MaxExpand;
        public int SlotIncrease;
        public int StartSlot;
    }

    [System.Serializable]
    public class EnergyData
    {
        public long EnergyMax;
        public long EnergyRecoverTime; // Time in second
        public int IncreaseEveryLevel;
    }

    [System.Serializable]
    public class EnergyResponse
    {
        public long Energy;
        public long EnergyRecover;
    }

    [System.Serializable]
    public class AdvLimitData
    {
        public string _id;
        public int Cap;
    }

    public static class AdvLimitDataConfig{
        public static string BattleX2Reward = "battle_x2_reward";
        public static string BattleAddEnergy = "battle_add_energy";
        public static string ArenaReset = "arena_reset";
        public static string ArenaAttack = "arena_attack";
    }

    [System.Serializable]
    public class UserAdvLimit {
        public string _id;
        public int Amount;
    }
}