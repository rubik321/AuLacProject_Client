
using System;
namespace Rubik._2DGPS.Chest
{
    using Card;
    using UserData;

    [Serializable]
    public class ChestReward
    {
        public CurrencyData[] CurrencyDatas;
        public Card[] Cards;
    }
}