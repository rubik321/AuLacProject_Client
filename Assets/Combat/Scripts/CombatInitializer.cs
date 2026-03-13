using GOA.UserData;
using GOA.WorldMap;
using Rubik.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatInitializer : MonoBehaviour
{
    const bool testSkill = false;
    // Start is called before the first frame update
    void Start()
    {
        List<CharacterCombatData> list = new List<CharacterCombatData>();
        CharacterCombatData player = new CharacterCombatData()
        {
            characterId = "character_player",
            characterInstanceId = UserData.Instance.characterData.CharacterID,
            isAI = false,
            isEnemy = false,
            isPlayer = true,
            level = UserData.Instance.characterData.Level,

            baseStat = new StatCollection()
            {
                hp = UserData.Instance.characterData.HP,
                curHp = UserData.Instance.characterData.CurrentHP,
                mp = UserData.Instance.characterData.MP,
                curMp = UserData.Instance.characterData.CurrentMP,
                strength = UserData.Instance.characterData.Str,
                vitality = UserData.Instance.characterData.Vit,
                mind = UserData.Instance.characterData.Mind,
                speed = UserData.Instance.characterData.Speed,
                spirit = 0,
                dexterity = UserData.Instance.characterData.Dex,
                critRate = UserData.Instance.characterData.CritRate,
                critDamage = UserData.Instance.characterData.CritDame,
                evasion = UserData.Instance.characterData.Evade,
                hit = UserData.Instance.characterData.HitRate,
              
            }
        };
        if(player.baseStat.curHp < 1){
            player.baseStat.curHp = player.baseStat.hp;
        }
        if(player.baseStat.curHp < 1){
            player.baseStat.curHp = 1;
        }
        if (UserData.Instance.Inventory.Potion > 0)
        {
             player.items.Add(new CountObject("I010001", UserData.Instance.Inventory.Potion));
        }
           
        if (UserData.Instance.Inventory.HiPotion > 0)
            player.items.Add(new CountObject("I010002", UserData.Instance.Inventory.HiPotion));
       
        if (UserData.Instance.Inventory.Ether > 0)
            player.items.Add(new CountObject("I010003", UserData.Instance.Inventory.Ether));
        if (UserData.Instance.Inventory.HiEther > 0)
            player.items.Add(new CountObject("I010004", UserData.Instance.Inventory.HiEther));
        if (UserData.Instance.Inventory.Elixir > 0)
            player.items.Add(new CountObject("I010005", UserData.Instance.Inventory.Elixir));
        bool isHasWeapon = false;
        foreach (GearData gearData in UserData.Instance.gearData.Data.GearData)
        {
            if (!gearData.Equiped)
                continue;

                player.gears.Add(new GearState()
                {
                    id = gearData.GearCode,
                    gearType = gearData.Slot
                });
                if (gearData.Slot == 3)
                {
                    player.characterClass = AssetLoader.Instance.GetWeaponType(gearData.GearCode).ToString();
                isHasWeapon = true;
                }
            
        }
        if (!isHasWeapon)
        {
            UserData.Instance.characterData.BaseDame = 20;
            
        }
        if (new List<string>() { "Knife", "", "Sword", "Mace" }.Contains(player.characterClass))
        {
            player.skills = new List<string>() { "normal_attack_M", Constants.ID.DEFEND_ID };
        }
        else
        {
            player.skills = new List<string>() { "normal_attack_R", Constants.ID.DEFEND_ID };
        }
        if (testSkill)
        {
            var skills = ActionPool.Instance.actionDatas;
            for (int i = 0; i < skills.Count; i++)
            {
                if (skills[i].actionId.StartsWith("SK_00"))
                    player.skills.Add(skills[i].actionId);
            }
        }
        else
        {
            foreach (string skill in UserData.Instance.skills)
            {
                BaseActionSO actionSO = ActionPool.Instance.GetActionSO(skill);
                if (actionSO == null)
                {
                    Debug.LogError(skill);
                    continue;
                }
                if (skill.StartsWith("SK_00"))
                    player.skills.Add(skill);
            }
        }
        list.Add(player);

        if (UserData.Instance.companion != null && !string.IsNullOrEmpty(UserData.Instance.companion.Index))
        {
            MobInfo companionInfo = UserData.Instance.companion;
            var companionSkillsRate = new List<float>();
            CharacterCombatData companion = new CharacterCombatData()
            {
                characterId = companionInfo.Index,
                characterInstanceId = companionInfo.Index,
                ownerInstanceId = player.characterInstanceId,
                isAI = true,
                isEnemy = false,
                isPlayer = false,
                isCompanion = true,
                level = companionInfo.Lv,
                baseStat = new StatCollection()
                {
                    hp = companionInfo.HP,
                    curHp = companionInfo.HP,
                    mp = companionInfo.MP,
                    strength = companionInfo.Strength,
                    vitality = companionInfo.Vitality,
                    mind = companionInfo.Mind,
                    speed = companionInfo.Speed,
                    spirit = companionInfo.Spirit,
                    dexterity = companionInfo.Dexterity,
                    critRate = companionInfo.CritRate,
                    critDamage = companionInfo.CritDmg,
                    evasion = companionInfo.Evade,

                    hit = companionInfo.HitRate
                },
                skills = new List<string>() { "companion_attack" },
                skillsRate = new List<float> { companionInfo.Atk_Rate }
            };
            foreach (string skill in companionInfo.skills)
            {
                companion.skills.Add(skill);
            }
            foreach (float skill in companionInfo.skillsRate)
            {
                companion.skillsRate.Add(skill);

            }
            list.Add(companion);
        }

        int index = 0;
        foreach (MobInfo mobInfo in UserData.Instance.DataInCombat.mobsInCombat)
        {
            CharacterCombatData mob = new CharacterCombatData()
            {
                characterId = mobInfo.Index,
                characterInstanceId = mobInfo.Index + index,
                isAI = true,
                isEnemy = true,
                isPlayer = false,
                level = mobInfo.Lv,
                typeMob = mobInfo.Type,
                baseStat = new StatCollection()
                {
                    hp = mobInfo.HP,
                    curHp = mobInfo.CurHp,
                    mp = mobInfo.MP,
                    strength = mobInfo.Strength,
                    vitality = mobInfo.Vitality,
                    mind = mobInfo.Mind,
                    speed = mobInfo.Speed,
                    spirit = mobInfo.Spirit,
                    dexterity = mobInfo.Dexterity,
                    critRate = mobInfo.CritRate,
                    critDamage = mobInfo.CritDmg,
                    evasion = mobInfo.Evade,
                    
                    hit = mobInfo.HitRate
                },
                skills = new List<string>() { "mob_attack" },
                skillsRate = new List<float> { mobInfo.Atk_Rate }
            };
            foreach (string skill in mobInfo.skills)
            {
                mob.skills.Add(skill);
              
            }
            foreach (float skill in mobInfo.skillsRate)
            {
                mob.skillsRate.Add(skill);

            }

            // mob.skills.Add("SK_88001");
            //mob.skillsRate.Add(.5f);
            if (index == 0 && UserData.Instance.DataInCombat.TypeCombat == GOA.WorldMap.MobTypeCode.MobReaperPortal)
            {
                mob.isBoss = true;
                QuestManager.Instance.UpdateQuest(QuestType.Reaper, 1);
            }
            list.Add(mob);
            index++;
        }
        CombatManager.Instance.StartCombat(list, "stage_" + (int)UserData.Instance.DataInCombat.TypeCombat);
    }
}
