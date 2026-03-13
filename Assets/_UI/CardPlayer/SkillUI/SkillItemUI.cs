using System.Collections;
using System.Collections.Generic;
using NTPackage.UI;
using Rubik.CardPlayer;
using Rubik.CharacterGear;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rubik.Myrk.Skill
{
    public class SkillItemUI : NTButtonEffect
    {
        public TextMeshProUGUI Level;
        public Image IconSkill;
        public Image BorderSkill;
        public Image MaskSkill;

        public SkillType SkillType;
        public CardSkillActiveLv ActiveSkill;
        public CardSkillPassiveLv PassiveSkill;
        public GearSkillActiveLv ActiveGearSkill;
        public GearSkillPassiveLv PassiveGearSkill;

        public Transform LockStatus;
        public int StarUnlock;
        public bool IsLock;

        public void SetActiveCardSkill(CardSkillActiveLv active)
        {
            this.SkillType = SkillType.ActiveCard;
            this.ActiveSkill = active;
            this.Level.text = (active.Level + 1)+"";
            this.IconSkill.sprite = CardPlayerManager.Instance.GetSkillActiveImage(active.Index);
            if (active.IsLock)
            {
                this.LockStatus.gameObject.SetActive(true);
                this.StarUnlock = active.StarUnlock;
                this.IsLock = active.IsLock;
            }
            else
            {
                this.LockStatus.gameObject.SetActive(false);
                this.IsLock = false;
            }
            this.BorderSkill.sprite = SkillManager.Instance.GetBorderSkill(this.SkillType);
            this.MaskSkill.sprite = SkillManager.Instance.GetMaskSkill(this.SkillType);
        }

        public void SetPassiveCardSkill(CardSkillPassiveLv passive)
        {
            this.SkillType = SkillType.PassiveCard;
            this.PassiveSkill = passive;
            this.Level.text = (passive.Level + 1)+"";
            this.IconSkill.sprite = CardPlayerManager.Instance.GetSkillPassiveImage(passive.Index);
            if (passive.IsLock)
            {
                this.LockStatus.gameObject.SetActive(true);
                this.StarUnlock = passive.StarUnlock;
                this.IsLock = passive.IsLock;
            }
            else
            {
                this.LockStatus.gameObject.SetActive(false);
                this.IsLock = false;
            }
            this.BorderSkill.sprite = SkillManager.Instance.GetBorderSkill(this.SkillType);
            this.MaskSkill.sprite = SkillManager.Instance.GetMaskSkill(this.SkillType);
        }

        public void SetActiveGearSkill(GearSkillActiveLv active){
            this.SkillType = SkillType.ActiveGear;
            this.ActiveGearSkill = active;
            this.Level.text = (active.Level + 1)+"";
            this.IconSkill.sprite = CharacterGearManager.Instance.GetSkillActiveImage(active.Index);
            this.BorderSkill.sprite = SkillManager.Instance.GetBorderSkill(this.SkillType);
            this.MaskSkill.sprite = SkillManager.Instance.GetMaskSkill(this.SkillType);
            this.LockStatus.gameObject.SetActive(false);
        }

        public void SetPassiveGearSkill(GearSkillPassiveLv passive){

            this.SkillType = SkillType.PassiveGear;
            this.PassiveGearSkill = passive;
            this.Level.text = (passive.Level + 1)+"";
            this.IconSkill.sprite = CharacterGearManager.Instance.GetSkillPassiveImage(passive.Index);
            this.BorderSkill.sprite = SkillManager.Instance.GetBorderSkill(this.SkillType);
            this.MaskSkill.sprite = SkillManager.Instance.GetMaskSkill(this.SkillType);
            this.LockStatus.gameObject.SetActive(false);
        }

        public void _OnClick()
        {
            switch (this.SkillType)
            {
                case SkillType.ActiveCard:
                    PopupManager.Instance.OnUI(PopupCode.SkillInfoUI, null, (PopupUI popupUI) =>
                    {
                        SkillInfoUI skillInfoUI = (SkillInfoUI)popupUI;
                        skillInfoUI.SetActiveSkill(this.ActiveSkill);
                    });
                    break;
                case SkillType.PassiveCard:
                    PopupManager.Instance.OnUI(PopupCode.SkillInfoUI, null, (PopupUI popupUI) =>
                    {
                        SkillInfoUI skillInfoUI = (SkillInfoUI)popupUI;
                        skillInfoUI.SetPassiveSkill(this.PassiveSkill);
                    });
                    break;
                case SkillType.ActiveGear:
                    PopupManager.Instance.OnUI(PopupCode.SkillInfoUI, null, (PopupUI popupUI) =>
                    {
                        SkillInfoUI skillInfoUI = (SkillInfoUI)popupUI;
                        skillInfoUI.SetGearSkill(this.ActiveGearSkill);
                    });
                    break;
                case SkillType.PassiveGear:
                    PopupManager.Instance.OnUI(PopupCode.SkillInfoUI, null, (PopupUI popupUI) =>
                    {
                        SkillInfoUI skillInfoUI = (SkillInfoUI)popupUI;
                        skillInfoUI.SetPassiveGearSkill(this.PassiveGearSkill);
                    });
                    break;
                default:
                    break;
            }
        }
    }
}