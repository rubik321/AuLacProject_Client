using System.Collections;
using System.Collections.Generic;
using Rubik.CardPlayer;
using UnityEngine;

namespace Rubik.BattleEngine
{
    [System.Serializable]
    public class CardBattleShordData
    {
        public string _id;
        public CardPlayerIndex Index;
        public int Star = 0;
        public int Level = 0;
        public string TeamID;
        public int Slot;
        public long ATK;
        public long DEF;
        public long SPD;
        public long MaxHP;
        public long CurHP;
        public long AP;
    }

    [System.Serializable]
    public class BattleStats
    {
        public long ATK = 0;
        public long HP = 0; //Health
        public long DEF = 0; //Defense
        public long SPD = 0; //Speed
        public float DMG = 0; //Damage Increase
        public float DR = 0; //Damage Reduction
        public float SkDMG = 0; //Damage Skill Increase
        public float SkDR = 0; //Damage Skill Reduction
        public float Pier = 0; //Piercing
        public float PierR = 0; //Piercing Resistance
        public float CRT = 0; //Crit Rate
        public float CRT_R = 0; //Crit Resistance
        public float CRD = 0; //Crit Damage
        public float CRD_R = 0; //Crit Damage Reduction
        public float Control = 0; //Control Hit Rate
        public float Control_R = 0;//Control Reduction
        public float Block = 0; //Block
        public float Block_R = 0; //Block Break
        public float VIP_DMG = 0; //Vip Damage
        public float VIP_R = 0; //Vip Damage Reduction

        public float DealToBleed = 0; //Deal more dmg to bleed
        public float DealToFrozen = 0; //Deal more dmg to Frozen
        public float DealToStun = 0; //Deal more dmg to Stun
        public float DealToPetrify = 0; //Deal more dmg to Petrify
        public float DealToSilence = 0; //Deal more dmg to Silence

        public float FreezeRes = 0; //Freeze Resistance
        public float StunRes = 0; //Stun Resistance
        public float PetrifyRes = 0; //Petrify Resistance
        public float SilenceRes = 0; //Silence Resistance


        public static void Add(BattleStats stats_0, BattleStats stats_1)
        {
            stats_0.ATK += stats_1.ATK;
            stats_0.HP += stats_1.HP;
            stats_0.DEF += stats_1.DEF;
            stats_0.SPD += stats_1.SPD;
            stats_0.DMG += stats_1.DMG;
            stats_0.DR += stats_1.DR;
            stats_0.SkDMG += stats_1.SkDMG;
            stats_0.SkDR += stats_1.SkDR;
            stats_0.Pier += stats_1.Pier;
            stats_0.PierR += stats_1.PierR;
            stats_0.CRT += stats_1.CRT;
            stats_0.CRT_R += stats_1.CRT_R;
            stats_0.CRD += stats_1.CRD;
            stats_0.CRD_R += stats_1.CRD_R;
            stats_0.Control += stats_1.Control;
            stats_0.Control_R += stats_1.Control_R;
            stats_0.Block += stats_1.Block;
            stats_0.Block_R += stats_1.Block_R;
            stats_0.VIP_DMG += stats_1.VIP_DMG;
            stats_0.VIP_R += stats_1.VIP_R;
            stats_0.DealToBleed += stats_1.DealToBleed;
            stats_0.DealToFrozen += stats_1.DealToFrozen;
            stats_0.DealToStun += stats_1.DealToStun;
            stats_0.DealToPetrify += stats_1.DealToPetrify;
            stats_0.DealToSilence += stats_1.DealToSilence;
            stats_0.FreezeRes += stats_1.FreezeRes;
            stats_0.StunRes += stats_1.StunRes;
            stats_0.PetrifyRes += stats_1.PetrifyRes;
            stats_0.SilenceRes += stats_1.SilenceRes;
        }

        public static void Floor(BattleStats stats)
        {
            stats.ATK = (long)Mathf.Floor(stats.ATK);
            stats.HP = (long)Mathf.Floor(stats.HP);
            stats.DEF = (long)Mathf.Floor(stats.DEF);
            stats.SPD = (long)Mathf.Floor(stats.SPD);
        }
    }
}