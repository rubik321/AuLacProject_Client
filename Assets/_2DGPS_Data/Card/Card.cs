using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rubik._2DGPS.Card
{
    public enum CardIndex
    {
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
    }

    public enum CardRarity
    {
        Epic = 0,
        Legendary = 1,
        Mythic = 2,
    }

    [System.Serializable]
    public class Gear
    {
        public int Lv = 1;
    }



    [System.Serializable]
    public class CardData
    {
        public CardIndex Index;
        public string Name;
        public CardRarity Rarity;
        public int Origin;

        public string ModelPath;
        public string Avatapath;
    }

    [System.Serializable]
    public class CardSlot
    {
        public CardIndex Index;
        public int Level;
        public int Slot;
    }

    [System.Serializable]
    public class Card
    {
        public string _id;
        public string UserID;
        public CardIndex Index;
        public int Lv;
        public int LvEnhance;

        public Gear[] Gears;

        public int AscendLv;
        public int rarity;

        public Card()
        {
            List<Gear> gears = new List<Gear>();
            for (int i = 0; i < 4; i++)
            {
                gears.Add(new Gear());
            }
            this.Gears = gears.ToArray();
        }

        public void UpdateData(Card card)
        {
            this._id = card._id;
            this.UserID = card.UserID;
            this.Index = card.Index;
            this.Lv = card.Lv;
            this.LvEnhance = card.LvEnhance;
            this.Gears = card.Gears;
            this.AscendLv = card.AscendLv;
            this.rarity = card.rarity;
        }
    }
}
