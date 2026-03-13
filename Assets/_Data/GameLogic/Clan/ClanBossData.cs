using System.Collections;
using System.Collections.Generic;
using Rubik.BattleEngine;
using Rubik.ItemPlayer;
using UnityEngine;

namespace Rubik.Myrk.Clan
{
    [System.Serializable]
    public class ClanBossData
    {
        public int MaxTimeAttack;
        public int MaxTimeAttackAdv;
        public TimeAttackReward[] TimeAttackReward;
        public DamageMilestone[] DamageMilestone;
        public ItemRate[] MilestoneReward;
    }

    [System.Serializable]
    public class TimeAttackReward
    {
        public int Amount;
        public Rubik.ItemPlayer.ItemData[] Items;
    }

    [System.Serializable]
    public class DamageMilestone
    {
        public int Index;
        public long Damage;
    }

    [System.Serializable]
    public class ClanBossResponse
    {
        public int TotalTimeAttack = -1;
        public PlayerClanBossDamage[] MemberDamage;
    }

    [System.Serializable]
    public class PlayerClanBossDamage
    {
        public string UserID;
        public long Damage;
    }

    [System.Serializable]
    public class PlayerClanBossResponse
    {
        public string _id;
        public int TimeAttack = 0;
        public int TimeAttackAdv = 0;
        public int[] RecieveReward;
    }

    [System.Serializable]
    public class ClanBossBattleResult
    {
        public string _id;
        public long TotalDamage = -1;
        public Rubik.ItemPlayer.ItemData[] ItemReward;
        public BattleShortData BattleResult;
    }
}