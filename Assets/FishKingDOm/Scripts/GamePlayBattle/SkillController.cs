using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Rubik.Battle
{
    public enum SkillType
    {
        Buff,
        Attack
    }
    public class SkillController : MonoBehaviour
    {
        [SerializeField] SkillType skillType;
        [SerializeField] ParticleSystem particle;
        Skill skillData;
        Coroutine startSkill;

        public void Begin()
        {
        }
        public void SetSkill(Skill _skill)
        {
            if(startSkill != null)
            {
                StopCoroutine(startSkill);
            }
            skillData = _skill;
            startSkill = StartCoroutine(StartSkill());
        }
        IEnumerator StartSkill()
        {
            yield return new WaitForSeconds(Time.deltaTime);
        }
        public void StopSkill()
        {
            this.StopAllCoroutines();
        }
        public void PlayFxSkill()
        {
            particle.Play();
        }
    }
    [Serializable]
    public class Skill
    {
        public SkillType type;
        public int idSkill;
        public HeroController heroAction;
        public List<HeroHit> listHeroTargetHit;
        public List<HeroBuff> listHeroTargetBuff;
    }
    
}