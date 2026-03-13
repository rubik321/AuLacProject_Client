using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rubik.DataType;

namespace Rubik.CharacterPlayer
{
    using CharacterGear;
    using Rubik.Myrk.BattleTeam;

    [System.Serializable]
    public class CharacterPlayer
    {
        public string _id;
        public string UserID;
        public int Index;

        // Cache
        public List<int> Teams;
        public bool IsEquiped {
            get {
                return Teams.Count > 0;
            }
        }
    }

    [System.Serializable]
    public class CharacterPlayerData
    {
        public int Index;
        public string Name;
        public CharacterPlayerType Type;
    }

    public enum CharacterPlayerType
    {
        Base = 0,
    }
}
