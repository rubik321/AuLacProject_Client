using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rubik.Combat
{
    [Serializable]
    public struct TurnStateData
    {
        public string characterInstanceId;
        public int tick;

        public TurnStateData(string characterInstanceId, int tick)
        {
            this.characterInstanceId = characterInstanceId;
            this.tick = tick;
        }
    }
}
