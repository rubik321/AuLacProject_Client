using System.Collections;
using System.Collections.Generic;
using NTPackage.UI;
using Rubik.CardPlayer;
using Rubik.CharacterGear;
using UnityEngine;
using TMPro;

namespace Rubik.Myrk.Skill
{
    public class SkillInfoUI : PopupUI
    {
        public TextMeshProUGUI TextName, TextDescription;
        public SkillItemUI SkillItemUI;

        public CardSkillActiveLv ActiveSkill;
        public CardSkillPassiveLv PassiveSkill;
        public GearSkillPassiveLv PassiveGearSkill;
        public GearSkillActiveLv ActiveGearSkill;
        public bool IsMax = false;
        public bool IsActiveSkill = false;

        public TextMeshProUGUI DetailSkill;

        public NTButtonEffect ButtonMax;

        public override void OnUI(object data = null, bool isDefaultSound = true)
        {
            base.OnUI(data, isDefaultSound);
            this.IsMax = false;
        }

        public override void UpdateData(object data = null)
        {
            base.UpdateData(data);
        }

        public void SetActiveSkill(CardSkillActiveLv activeSkill)
        {
            this.ActiveSkill = activeSkill;
            this.IsActiveSkill = true;
            int lv = this.IsMax ? CardPlayerManager.Instance.GetMaxLevelSkillActive(activeSkill.Index) : activeSkill.Level;
            this.SkillItemUI.SetActiveCardSkill(activeSkill);
            this.TextName.text = CardPlayerManager.Instance.GetSkillActiveName(activeSkill.Index);
            this.TextDescription.text = CardPlayerManager.Instance.GetSkillActiveDetail(activeSkill.Index, lv);
            if (CardPlayerManager.Instance.IsMaxSkillActive(activeSkill.Index, lv)){
                this.TextName.text += Lean.Localization.LeanLocalization.GetTranslationText("max", "(Max)");
            }
            if(activeSkill.IsLock){
                string str_lock = Lean.Localization.LeanLocalization.GetTranslationText("unlock_at_star", "Unlock at star {0}");
                this.TextName.text +="\n" + string.Format(str_lock, activeSkill.StarUnlock+1);
            }
            this.DetailSkill.text = Lean.Localization.LeanLocalization.GetTranslationText("active_skill_detail", "*Active skills trigger when energy is full and turn coming.");
            if (this.IsMax) this.ButtonMax.Chose();
            else this.ButtonMax.Unchose();
        }

        public void SetPassiveSkill(CardSkillPassiveLv passiveSkill)
        {
            this.PassiveSkill = passiveSkill;
            this.IsActiveSkill = false;
            int lv = this.IsMax ? CardPlayerManager.Instance.GetMaxLevelSkillPassive(passiveSkill.Index) : passiveSkill.Level;
            this.SkillItemUI.SetPassiveCardSkill(passiveSkill);
            this.TextName.text = CardPlayerManager.Instance.GetSkillPassiveName(passiveSkill.Index);
            if (CardPlayerManager.Instance.IsMaxSkillPassive(passiveSkill.Index, lv)){
                this.TextName.text += Lean.Localization.LeanLocalization.GetTranslationText("max", "(Max)");
            }
            if(passiveSkill.IsLock){
                string str_lock = Lean.Localization.LeanLocalization.GetTranslationText("unlock_at_star", "Unlock at star {0}");
                this.TextName.text +="\n" + string.Format(str_lock, passiveSkill.StarUnlock+1);
            }
            this.TextDescription.text = CardPlayerManager.Instance.GetSkillPassiveDetail(passiveSkill.Index, lv);
            this.DetailSkill.text = Lean.Localization.LeanLocalization.GetTranslationText("passive_skill_detail", "*Passive skills that are always triggered automatically based on conditions.");
            if (this.IsMax) this.ButtonMax.Chose();
            else this.ButtonMax.Unchose();
        }

        public void SetGearSkill(GearSkillActiveLv gearSkillActiveLv){
            this.ActiveGearSkill = gearSkillActiveLv;
            this.IsActiveSkill = true;
            this.SkillItemUI.SetActiveGearSkill(gearSkillActiveLv);
            this.TextName.text = CharacterGearManager.Instance.GetGearSkillName(gearSkillActiveLv.Index);
            this.TextDescription.text = CharacterGearManager.Instance.GetGearSkillDetail(gearSkillActiveLv.Index, gearSkillActiveLv.Rarity);
            this.DetailSkill.text = Lean.Localization.LeanLocalization.GetTranslationText("active_skill_detail", "*Active skills trigger when energy is full and turn coming.");
        }

        public void SetPassiveGearSkill(GearSkillPassiveLv passiveGearSkill){
            this.PassiveGearSkill = passiveGearSkill;
            this.IsActiveSkill = false;
            this.SkillItemUI.SetPassiveGearSkill(passiveGearSkill);
            this.TextName.text = CharacterGearManager.Instance.GetGearSkillName(passiveGearSkill.Index);
            this.TextDescription.text = CharacterGearManager.Instance.GetGearSkillDetail(passiveGearSkill.Index, passiveGearSkill.Rarity);
            this.DetailSkill.text = Lean.Localization.LeanLocalization.GetTranslationText("passive_skill_detail", "*Passive skills that are always triggered automatically based on conditions.");
        }

        public void OnClickMax(){
            this.IsMax = !this.IsMax;
            if (this.IsActiveSkill){
                this.SetActiveSkill(this.ActiveSkill);
            }
            else{
                this.SetPassiveSkill(this.PassiveSkill);
            }
        }
    }
}
