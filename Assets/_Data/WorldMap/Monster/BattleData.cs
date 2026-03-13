using Rubik.BattleEngine;

namespace Rubik.Myrk.Monster
{
    using ItemPlayer;
    using Rubik.CardPlayer;
    using Rubik.DataType;

    [System.Serializable]
    public class BattleResult
    {
        public string _id;
        public BattleShortData Result;
        public bool IsWin;
    }

    [System.Serializable]
    public class BattleStatus
    {
        public long LastTime;
        public int MonsterLimit;
        public int AttackAdvLimit;
    }

    [System.Serializable]
    public class BattleMonsterWorldMapData
    {
        public ItemData ItemConsume;
        public int MonsterLimit;

        public int MonsterOnMapLimit;
        public CardPlayerIndex[] MonsterOnMap;
        public int CreepOnMapLimit;
        public CardPlayerIndex[] CreepOnMap;

        // Shard Reward
        public AmountRate[] ShardReward;
        public ItemData[] ShardItemReward;
        public ScaleMonsterByLevel[] ScaleShardByLevel;

        // Grinding Reward
        public AmountRate[] GrindingShardReward;
        public ItemRate[] GrindingReward;
        public ItemData[] GrindingItemReward;
        public ScaleMonsterByLevel[] ScaleGrindingByLevel;

        // Monster World Map Amount
        public MonsterWorldMapAmount[] MonsterWorldMapAmount;

        public ItemData OfferedAdv;
    }

    [System.Serializable]
    public enum AttackType
    {
        Grinding,
        Shard,

        // Outpost
        Outpost,
    }

    [System.Serializable]
    public class MonsterAttackData
    {
        public MonsterData[] Monsters;
        public string TeamID;
        public AttackType AttackType = AttackType.Grinding;
        public float ScaleMonster = 1;
    }

    [System.Serializable]
    public class ScaleMonsterByLevel
    {
        public int LowerLevel;
        public int Star;
        public float Scale;
    }

    [System.Serializable]
    public class MonsterWorldMapAmount
    {
        public int LowerLevel;
        public int Amount;
    }

    [System.Serializable]
    public class ResultAttackMonsterData
    {
        public string _id;
        public bool Victory;
        public AttackType AttackType;
        public CardPlayerIndex MonsterIndex;
    }


    [System.Serializable]
    public class ResultAttackMonsterResponse
    {
        public string _id;
        public bool Victory;
        public ItemData[] ItemReward;
    }
}