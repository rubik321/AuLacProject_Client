import { OriginType } from "../../DataType/OriginType"
import { TierType } from "../../DataType/RarityData"
import { CardTierData } from "../Model/CardPlayerData"

export enum CardPlayerIndex{
    Card_0 = 0,
    Card_1 = 1,
    Card_2 = 2,
    Card_3 = 3,
    Card_4 = 4,
    Card_5 = 5,
    Card_6 = 6,
    Card_7 = 7,
    Card_8 = 8,
    Card_9 = 9,
    Card_10 = 10,
    Card_11 = 11,
    Card_12 = 12,
    Card_13 = 13,
    Card_14 = 14,
    Card_15 = 15,
    Card_16 = 16,
    Card_17 = 17,
    Card_18 = 18,
    Card_19 = 19,
    Card_20 = 20,
    Card_21 = 21,
    Card_22 = 22,
    Card_23 = 23,
    Card_24 = 24,
    Card_25 = 25,
    Card_26 = 26,
    Card_27 = 27,
    Card_28 = 28,
    Card_29 = 29,

    // Boss
    Boss_0 = 1000,

    Portal_Boss_0 = 1100,

    Creep_0 = 2000,
    Creep_1 = 2001,
    Creep_2 = 2002,
    Creep_3 = 2003,
    Creep_4 = 2004,
    Creep_5 = 2005,
    Creep_6 = 2006,
    Creep_7 = 2007,
    Creep_8 = 2008,
    Creep_9 = 2009,
}

export enum CardPlayerType{
    Type_0 = 0,
    Type_1 = 1,
    Type_2 = 2,
    Type_3 = 3,
    Type_4 = 4,
    Type_5 = 5,
    Type_6 = 6,
    Type_7 = 7,
    Type_8 = 8,
    Type_9 = 9,

    Creep_0 = 1000,
    Creep_1 = 1001,
    Creep_2 = 1002,
    Creep_3 = 1003,
    Creep_4 = 1004,
    Creep_5 = 1005,
    Creep_6 = 1006,
    Creep_7 = 1007,
    Creep_8 = 1008,
    Creep_9 = 1009,

    Boss_0 = 2000,
    Portal_Boss_0 = 2100,
}

export const rateSommon = {
    Mythic : 0.01,
    Legendary : 0.37,
    Epic : 0.62
}

export const cardTierOriginData : CardTierOriginData[] = [
    {
        Origin : OriginType.Metal,
        Card: [
            {
                Tier : TierType.Tier_0,
                CardIndexes: [CardPlayerIndex.Card_0]
            },
            {
                Tier : TierType.Tier_1,
                CardIndexes: [CardPlayerIndex.Card_1]
            },
            {
                Tier : TierType.Tier_2,
                CardIndexes: [CardPlayerIndex.Card_2]
            }
        ]
    },
    {
        Origin : OriginType.Wood,
        Card: [
            {
                Tier : TierType.Tier_0,
                CardIndexes: [CardPlayerIndex.Card_3, CardPlayerIndex.Card_15]
            },
            {
                Tier : TierType.Tier_1,
                CardIndexes: [CardPlayerIndex.Card_4, CardPlayerIndex.Card_16]
            },
            {
                Tier : TierType.Tier_2,
                CardIndexes: [CardPlayerIndex.Card_5, CardPlayerIndex.Card_17]
            }
        ]
    },
    {
        Origin : OriginType.Water,
        Card: [
            {
                Tier : TierType.Tier_0,
                CardIndexes: [CardPlayerIndex.Card_12, CardPlayerIndex.Card_18]
            },
            {
                Tier : TierType.Tier_1,
                CardIndexes: [CardPlayerIndex.Card_13, CardPlayerIndex.Card_19]
            },
            {
                Tier : TierType.Tier_2,
                CardIndexes: [CardPlayerIndex.Card_14, CardPlayerIndex.Card_20]
            }
        ]
    },
    {
        Origin : OriginType.Fire,
        Card: [
            {
                Tier : TierType.Tier_0,
                CardIndexes: [CardPlayerIndex.Card_6]
            },
            {
                Tier : TierType.Tier_1,
                CardIndexes: [CardPlayerIndex.Card_7]
            },
            {
                Tier : TierType.Tier_2,
                CardIndexes: [CardPlayerIndex.Card_8]
            }
        ]
    },
    {
        Origin : OriginType.Earth,
        Card: [
            {
                Tier : TierType.Tier_0,
                CardIndexes: [CardPlayerIndex.Card_9]
            },
            {
                Tier : TierType.Tier_1,
                CardIndexes: [CardPlayerIndex.Card_10]
            },
            {
                Tier : TierType.Tier_2,
                CardIndexes: [CardPlayerIndex.Card_11]
            }
        ]
    },
]