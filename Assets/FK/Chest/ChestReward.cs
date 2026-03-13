using Rubik.KingFish.Card;
using System;
using GOA.Item;

namespace Rubik.KingFish.Chest
{
    [Serializable]
    public class ChestReward
    {
        public ItemCode ItemType;
        public int CurrencyAmount;
        public CardSummon CardSummon;
    }
}