using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Rubik
{
    [Serializable]
    public class CharacterModel
    {
        public CharacterModel(int _id, int _displayId, int _race, string _name, float _hp, float _dame, float _speedAttack, float _exp, int _slot)
        {
            id = _id; displayId = _displayId; race = _race; name = _name; hp = _hp; dame = _dame; speedAttack = _speedAttack; exp = _exp; slot = _slot; 
        }
        public int id;
        public int displayId; // show graphic character;
        public int race;
        public string name;
        public float hp;
        public float dame;
        public float speedAttack;
        public float exp;
        public int slot; // position in map ;
        public float ratioCrit; // 0 ->1 : ratio crit on attack turn
        public float ratioMiss; // 0 ->1 : ratio miss damage on hit attack turn
    }
    [Serializable]
    public class CharacterInfoInGame
    {
        public int id;
        public int race;
        public string name;
        public float curHp;
        public float curDame;
        public float curSpeedAttack;
        //public float curExp;
    }
}