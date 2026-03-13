using System;
using System.Collections.Generic;
using UnityEngine;
using GOA.UserData;
using GOA.WorldMap;
namespace Rubik.Combat
{
    [Serializable]
    public class CharacterCombatData
    {
        public string characterId;
        public string characterName;
        public string characterClass = "";
        public string characterInstanceId;
        public string ownerInstanceId;
        public bool isEnemy;
        public bool isAI;
        public bool isPlayer;
        public bool isCompanion;
        public bool isBoss;
        public int level;
        public MobTypeCode typeMob;
        /// <summary>
        /// Contain base stats
        /// </summary>
        public StatCollection baseStat;

        public List<GearState> gears = new List<GearState>();
        /// <summary>
        /// First skills is always normal attack id. Second skills is always defend skill. Skills displayed will be from 2
        /// </summary>
        public List<string> skills = new List<string>();
        public List<float> skillsRate = new List<float>();
        public string AttackId => skills[0];
        public List<CountObject> items = new List<CountObject>();
        public List<string> effects = new List<string>();
    }

    [Serializable]
    public class CharacterCombatState
    {
        public CharacterCombatData data;
        // base/max stats are stats after being altered with gears
        // non base/max stats are stats after being modified with buff/debuff
        public int maxHp;
        public int hp;
        public int maxMp;
        public int mp;
        public int baseStrength;
        public int strength;
        public int baseVitality;
        public int vitality;
        public int baseMind;
        public int mind;
        public int baseSpirit;
        public int spirit;
        public int baseDexterity;
        public int dexterity;
        public int baseSpeed;
        public int speed;
        public int baseEvasion;
        public int evasion;
        public float baseCritRate;
        public float critRate;
        public float baseCritDamage;
        public float critDamage;
        public float baseHit;
        public float hit;
        public List<EffectState> effects = new List<EffectState>();
        public int baseDamage;
        public CharacterCombatState(CharacterCombatData data)
        {
            this.data = data;
            if(data.isPlayer)
                baseDamage = UserData.Instance.characterData.BaseDame;

            StatCollection gearPlus = new StatCollection();
            StatCollection gearMultiplier = new StatCollection()
            {
                hp = 1,
                mp = 1,
                strength = 1,
                vitality = 1,
                mind = 1,
                spirit = 1,
                dexterity = 1,
                speed = 1,
                evasion = 1,
                critRate = 1,
                critDamage = 1,
                hit = 1
            };
            foreach (GearState gear in data.gears)
            {
                //  Gear stats are calculated outside combat. No need to recalculate it
                // But keep these, in case design change. It won't make anychange anyway

                //gearPlus.hp += gear.statMultiplier.hp;
                //gearPlus.mp += gear.statMultiplier.mp;
                //gearPlus.strength += gear.statMultiplier.strength;
                //gearPlus.vitality += gear.statMultiplier.vitality;
                //gearPlus.mind += gear.statMultiplier.mind;
                //gearPlus.spirit += gear.statMultiplier.spirit;
                //gearPlus.dexterity += gear.statMultiplier.dexterity;
                //gearPlus.speed += gear.statMultiplier.speed;
                //gearPlus.evasion += gear.statMultiplier.evasion;
                //gearPlus.critRate += gear.statMultiplier.critRate;
                //gearPlus.critDamage += gear.statMultiplier.critDamage;
                //gearPlus.hit += gear.statMultiplier.hit;

                //gearMultiplier.hp += gear.statMultiplier.hp;
                //gearMultiplier.mp += gear.statMultiplier.mp;
                //gearMultiplier.strength += gear.statMultiplier.strength;
                //gearMultiplier.vitality += gear.statMultiplier.vitality;
                //gearMultiplier.mind += gear.statMultiplier.mind;
                //gearMultiplier.spirit += gear.statMultiplier.spirit;
                //gearMultiplier.dexterity += gear.statMultiplier.dexterity;
                //gearMultiplier.speed += gear.statMultiplier.speed;
                //gearMultiplier.evasion += gear.statMultiplier.evasion;
                //gearMultiplier.critRate += gear.statMultiplier.critRate;
                //gearMultiplier.critDamage += gear.statMultiplier.critDamage;
                //gearMultiplier.hit += gear.statMultiplier.hit;
                
                foreach (string effect in gear.effects)
                {
                    // Effect in gears don't need attacker to affect target
                    effects.Add(new EffectState(EffectPool.Instance.GetEffectSO(effect), null));
                }
            }


            this.maxHp = (int)(data.baseStat.hp * gearMultiplier.hp + gearPlus.hp);
            this.hp = (int)data.baseStat.curHp;
            this.maxMp = (int)(data.baseStat.mp * gearMultiplier.mp + gearPlus.mp);
            this.mp = (int)data.baseStat.curMp; 
            this.baseStrength = (int)(data.baseStat.strength * gearMultiplier.strength + gearPlus.strength);
            this.strength = baseStrength;
            this.baseVitality = (int)(data.baseStat.vitality * gearMultiplier.vitality + gearPlus.vitality);
            this.vitality = baseVitality;
            this.baseMind = (int)(data.baseStat.mind * gearMultiplier.mind + gearPlus.mind);
            this.mind = baseMind;
            this.baseSpirit = (int)(data.baseStat.spirit * gearMultiplier.spirit + gearPlus.spirit);
            this.spirit = baseSpirit;
            this.baseDexterity = (int)(data.baseStat.dexterity * gearMultiplier.dexterity + gearPlus.dexterity);
            this.dexterity = baseDexterity;
            this.baseSpeed = (int)(data.baseStat.speed * gearMultiplier.speed + gearPlus.speed);
            this.speed = baseSpeed;
            this.baseEvasion = (int)(data.baseStat.evasion * gearMultiplier.evasion + gearPlus.evasion);
            this.evasion = baseEvasion;
            this.baseCritRate = data.baseStat.critRate * gearMultiplier.critRate + gearPlus.critRate;
            this.critRate = baseCritRate;
            this.baseCritDamage = data.baseStat.critDamage * gearMultiplier.critDamage + gearPlus.critDamage;
            this.critDamage = baseCritDamage;
            this.baseHit = data.baseStat.hit * gearMultiplier.hit + gearPlus.hit;
            this.hit = baseHit;
        }

        public void AddItem(string id, int count)
        {
            foreach (CountObject item in data.items)
            {
                if (item.id == id)
                {
                    item.count = Mathf.Clamp(item.count + count, 0, 999);
                    UserData.Instance.Inventory.AddInventoryByID(item.id, count);
                }
                    
            }
        }
    }

    // TODO: Should fetch from config file
    [Serializable]
    public class GearState
    {
        public string id;
        public int level;
        /// <summary>
        /// Helmet = 0,
        /// Body = 1,
        /// Leg = 2,
        /// MainHand = 3,
        /// OffHand = 4,
        /// Accessory = 5
        /// </summary>
        public int gearType;
        public StatCollection statPlus = new StatCollection();

        /// <summary>
        /// On scale of 1, not %. 0 means no changes. Negative means scaling down. 
        /// </summary>
        public StatCollection statMultiplier = new StatCollection();
        public List<string> effects = new List<string>();
    }

    [Serializable]
    public class StatCollection
    {
        public float hp;
        public float curHp;
        public float curMp;
        public float mp;
        public float strength;
        public float vitality;
        public float mind;
        public float spirit;
        public float dexterity;
        public float critRate;
        public float critDamage;
        public float speed;
        public float evasion;
        public float hit;

        public StatCollection Clone
        {
            get
            {
                return new StatCollection()
                {
                    hp = this.hp,
                    curHp = this.curHp,
                    mp = this.mp,
                    strength = this.strength,
                    vitality = this.vitality,
                    mind = this.mind,
                    spirit = this.spirit,
                    dexterity = this.dexterity,
                    critRate = this.critRate,
                    critDamage = this.critDamage,
                    speed = this.speed,
                    evasion = this.evasion,
                    hit = this.hit
                };
            }
        }
    }

    public class CountObject
    {
        public string id;
        public int count;

        public CountObject(string id, int count)
        {
            this.id = id;
            this.count = count;
        }
    }
}
