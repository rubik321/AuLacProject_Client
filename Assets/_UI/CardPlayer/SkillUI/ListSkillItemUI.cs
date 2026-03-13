using System.Collections.Generic;
using Rubik.CardPlayer;
using Rubik.CharacterGear;
using UnityEngine;

namespace Rubik.Myrk.Skill
{
    public class ListSkillItemUI : MonoBehaviour
    {
        public List<SkillItemUI> SkillItemUIList;

        public void SetData(CardSkillLv cardSkillLv){
            for (int i = 0; i < this.SkillItemUIList.Count; i++)
            {
                this.SkillItemUIList[i].gameObject.SetActive(false);
            }
            this.SkillItemUIList[0].gameObject.SetActive(true);
            this.SkillItemUIList[0].SetActiveCardSkill(cardSkillLv.Active);

            for (int i = 0; i < cardSkillLv.Passive.Count; i++)
            {
                this.SkillItemUIList[i + 1].gameObject.SetActive(true);
                this.SkillItemUIList[i + 1].SetPassiveCardSkill(cardSkillLv.Passive[i]);
            }
        }

        public void SetData(GearSkillLv gearSkillLv){
            for (int i = 0; i < this.SkillItemUIList.Count; i++){
                this.SkillItemUIList[i].gameObject.SetActive(false);
            }
            if(gearSkillLv.Active != null && gearSkillLv.Active.Index != TypeActiveGear.None){
                this.SkillItemUIList[0].gameObject.SetActive(true);
                this.SkillItemUIList[0].SetActiveGearSkill(gearSkillLv.Active);
            }
            for (int i = 0; i < gearSkillLv.Passive.Count; i++){
                this.SkillItemUIList[i + 1].gameObject.SetActive(true);
                this.SkillItemUIList[i + 1].SetPassiveGearSkill(gearSkillLv.Passive[i]);
            }
        }
    }
}