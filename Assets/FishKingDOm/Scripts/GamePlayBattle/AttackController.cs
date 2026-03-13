using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
namespace Rubik.Battle
{
    public class AttackController : MonoBehaviour
    {
        [SerializeField] HeroController heroAttack;
        [SerializeField] List<HeroHit> herosHit;
        SkillController skillCtr;
        HeroHit curHeroHit;
        int indexCurHeroHit = 0;

        Coroutine attackCoroutine;

        // Start is called before the first frame update

        public void Begin()
        {
            heroAttack = null;
            herosHit = new List<HeroHit>();
        }

        public void SetAttack(HeroController _heroAttack, List<HeroHit> _herosHit)
        {
            heroAttack = _heroAttack;
            herosHit = _herosHit;
            if(attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
            }
            attackCoroutine = StartCoroutine(Attack());
        }
        public void SetSkillAttack(HeroController _heroAttack, List<HeroHit> _herosHit)
        {
            heroAttack = _heroAttack;
            herosHit = _herosHit;
            StartCoroutine(AttackSkill());

        }
        public void SetSkillBuff(HeroController _heroAttack)
        {

        }
        float timeAttack = 1;
        IEnumerator Attack()
        {
            yield return new WaitForSeconds(1);
            indexCurHeroHit = 0;
            for(int i=0; i< herosHit.Count; i++)
            {
                NextAttack(herosHit[i]);
                yield return new WaitForSeconds(timeAttack);
            }
            StopAttack();
        }
        IEnumerator AttackSkill() {
            yield return new WaitForSeconds(1);
            //heroAttack.AttackSkill(herosHit);
            yield return new WaitForSeconds(2);
            StopAttack();

        }

        public void StopAttack()
        {
            this.StopAllCoroutines();
            heroAttack.FinishAttack();

            if (skillCtr != null)
                skillCtr.StopSkill();

            //GamePlayBattleController.Instance.NextProcess();

            //if (GamePlayBattleController.Instance != null)
            //    GamePlayBattleController.Instance.NextProcess();
            //else
            //    Rubik.AutoChess.GamePlayBattleController.Instance.NextProcess();
        }
        public void ResetHero()
        {
            heroAttack = null;
            herosHit = new List<HeroHit>();
        }
        public void NextAttack(HeroHit _heroHit)
        {
                //curHeroHit = _heroHit;
                //heroAttack.AttackOneHit(curHeroHit.hero);
                //indexCurHeroHit += 1;
        }

        //public void SkillAction(Skill _skill)
        //{
        //    if (skillCtr == null) return;
        //    skillCtr.SetSkill(_skill);
        //}
    }
    [Serializable]
    public class HeroHit{
        public HeroController hero;
        public float dameHit;
        public bool crt;
        public HeroHit(HeroController _hero, float _dameHit, bool _crt )
        {
            hero = _hero;
            dameHit = _dameHit;
            crt = _crt;
        }
    }
    [Serializable]
    public class HeroBuff
    {
        public HeroController hero;
        public float dameBuff;
        public float hpBuff;
        public HeroBuff(HeroController _hero, float _hpBuff, float _dameBuff)
        {
            hero = _hero;
            dameBuff = _dameBuff;
            hpBuff = _hpBuff;
        }
    }
}