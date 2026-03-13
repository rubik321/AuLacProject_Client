using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Rubik._2DGPS.DataCenter
{
    public enum DataName{
        DataVersion,
        ServerGame,
        CardData,
        WorldMapLvData,
        CampaignLvData,
        GearData,
        GearLvCost,
        CharacterLvCost,
        CardEvolveData,
    }

    [System.Serializable]
    public class DataVersion
    {
        public int ServerGame = 0;
        public int CardData = 0;
        public int WorldMapLvData = 0;
        public int CampaignLvData = 0;
        public int GearData = 0;
        public int GearLvCost = 0;
        public int CharacterLvCost = 0;
        public int CardEvolveData = 0;
    }
}