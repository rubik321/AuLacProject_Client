using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rubik._2DGPS.WorldMap
{
    using Card;
    
    [System.Serializable]
    public class WorldMapLvData{
        public int Difficult;
        public List<CardSlot> Cards;
    }
}