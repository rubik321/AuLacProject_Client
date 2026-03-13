using DG.Tweening;
using GOA.Item;
using NTPackage.Functions;
using NTPackage.UI;
using Rubik.CharacterGear;
using Rubik.Common.AudioHelper;
using Rubik.DataType;
using Rubik.Format;
using Rubik.ItemPlayer;
using Rubik.Myrk.Skill;
using Rubik.UI.Statitic;
using Rubik.UserDataPlayer;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace Rubik.CharacterGear
{
    using Rubik.ItemPlayer;
    using Rubik.Myrk.BattleTeam;

    public class GearShortInfoUI : PopupUI
    {
        public CharacterGearIndex Index;
        public RarityType Rarity;
        public int Level;
        public CharacterGearData gearData;
        public TextMeshProUGUI txtTitle;
        public TextMeshProUGUI txtDescription;
        public CharacterGearItemUI CharacterGearItemUI;
        public BarStatUI MainStat, SubStat;
        public TextMeshProUGUI TxtSkillName;
        public TextMeshProUGUI TxtSkillDescription;
        public SkillItemUI SkillItemUI;

        public void SetData(CharacterGearIndex index, RarityType rarity, int level)
        {
            this.Index = index;
            this.Rarity = rarity;
            this.Level = level;
            this.UpdateData();
        }

        public override void UpdateData(object data = null)
        {
            base.UpdateData(data);
            this.gearData = CharacterGearManager.Instance.GetGearDataByIndex(this.Index);
            this.txtTitle.text = CharacterGearManager.Instance.GetGearNameByIndex(this.Index);
            this.txtDescription.text = CharacterGearManager.Instance.GetGearDesByIndex(this.Index);
            CharacterGear characterGear = new CharacterGear();
            characterGear.Index = this.Index;
            characterGear.Rarity = this.Rarity;
            characterGear.Lv = this.Level;
            this.CharacterGearItemUI.SetData(characterGear);
            this.MainStat.SetData(CharacterGearManager.Instance.GetMainStat(this.Index, this.Rarity, this.Level));
            this.SubStat.SetData(CharacterGearManager.Instance.GetSubStats(this.Rarity));
            if (CharacterGearManager.Instance.IsActiveSkillGear(this.Index))
            {
                GearSkillActiveLv gearSkillActiveLv = CharacterGearManager.Instance.GetGearSkillActiveLv(CharacterGearManager.Instance.GetTypeActiveGearByIndex(this.Index), this.Rarity);
                this.SkillItemUI.SetActiveGearSkill(gearSkillActiveLv);
                this.TxtSkillName.text = CharacterGearManager.Instance.GetGearSkillName(this.Index);
                this.TxtSkillDescription.text = CharacterGearManager.Instance.GetGearSkillDetail(this.Index, this.Rarity);
            }
            else
            {
                GearSkillPassiveLv gearSkillPassiveLv = CharacterGearManager.Instance.GetGearSkillPassiveLv(CharacterGearManager.Instance.GetTypePassiveGearByIndex(this.Index), this.Rarity);
                this.SkillItemUI.SetPassiveGearSkill(gearSkillPassiveLv);
                this.TxtSkillName.text = CharacterGearManager.Instance.GetGearSkillName(this.Index);
                this.TxtSkillDescription.text = CharacterGearManager.Instance.GetGearSkillDetail(this.Index, this.Rarity);
            }
        }
    }
}