using System.Collections;
using System.Collections.Generic;
using Rubik.Combat;
using UnityEngine;
[CreateAssetMenu(fileName = "GearData", menuName = "ScriptableObjects/GearData", order = 1)]
public class GearDataSO : ScriptableObject
{
    public string GearID;
    public GearTypeSlot gearType;
    public WeaponType weaponType;
    public Sprite gearAva;
    public float HP;
    public float MP;
    public float ATK;
    public float Def;
    public int rarity;
    public bool isEquiped;
    public string GearName;
}
public enum GearTypeSlot
{
    Helmet = 0,
    Body = 1,
    Weapon = 2,
    Ring = 3,
    Amulet = 4,
    Leg = 5
}

