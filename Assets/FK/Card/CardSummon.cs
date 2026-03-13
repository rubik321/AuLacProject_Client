using System;
namespace Rubik.KingFish.Card
{
    [Serializable]
    public class CardSummon
    {
        public string _id;
        public int Index;
        public int Shard;
        public bool IsNew;
    }
}