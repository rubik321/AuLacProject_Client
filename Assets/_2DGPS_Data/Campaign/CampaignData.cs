using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rubik._2DGPS.Campaign
{
    using Card;
    using Rubik._2DGPS.UserData;

    [System.Serializable]
    public class CampaignLvData{
        public int Level;
        public int Difficult;
        public List<CardSlot> Cards;
        public List<CurrencyData> Rewards;
    }
}