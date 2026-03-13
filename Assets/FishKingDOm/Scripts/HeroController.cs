using DG.Tweening;
using Rubik.BattleEngine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rubik.Battle
{
    public class HeroController : CharacterBase
    {
       
        public HeroController curHeroHit;
        Coroutine AttackToHeroCoroutine;
        Action actionComplete,hitDameAction,moveUpAction,skillAActionCompleted;
        public int slotID;
        public float ap;
        public int[] atk;
        public TypeDmg[] typeDame;
       // public TypeDmg typeDame = TypeDmg.Normal;
        bool isAttack = false;
        public int heroIndex;
        public int star;
        public int lv;
        public override void Begin( bool isHeroSelf)
        {
            base.Begin( isHeroSelf);
        }
        public override void UpdateInfo()
        {
            base.UpdateInfo();
        }
        public void SetStat()
        {

        }
        public void TurnEffectBuff(TypeBuff[] buffs)
        {
            foreach(GameObject idBuff in efffectBuff)
            {
                idBuff.SetActive(false);
            }
            foreach (TypeBuff idBuff in buffs)
            {
                efffectBuff[(int)idBuff].SetActive(true);
            }
        }
        public void ResetHero(BaseCharacterData c)
        {
            RessetCharacter(c);
        }

        public void MoveToPos(Vector2 newPos, Action action = null ,float time = 0.1f)
        {
           // Debug.Log("Time move : " + time);
            transform.DOMove(newPos, time)
           
                .SetEase(Ease.Linear)
                .SetDelay(0.3f)
                .OnComplete(() => {
                if(action != null)
                action();
            });
        }

        public void FinishAttack()
        {
            MoveToPos(positionStart.position(), () => {
                ChangeState(CharacterState.Idle);
                SetLayer(slotID);
            });
        }
        
        public void AttackOneHit( List<HeroController> _herosHit, Vector2 posTartget, Action callback = null)
        {
            StopAllCoroutines();
            isAttack = true;
            Vector2 targetPos = _herosHit.Count > 1 ? posTartget : _herosHit[0].positionStart.positionHit();
            switch (model.CardType)
            {
                case AttackType.Melee:
                    
                    SetLayer(40);
                    GameObject hitEffect = zfx.GetHitDameEffect(EffectType.ATTACK_EFFECT);
                    if (model.MoveAction.type == MoveActionType.MOVE_WITH_EVENT)
                    {
                        ChangeState(CharacterState.Attack);

                        //if (model.CharacterType == CharacterType.Hero)
                        zfx.PlayZfxSkill(0);
                        moveUpAction = null;
                        moveUpAction = () =>
                        {
                            OffBar();
                            MoveToPos(targetPos, () =>
                            {
                                moveUpAction = null;
                                moveUpAction = () =>
                                {
                                    zfx.StopZfx();
                                    MoveToPos(positionStart.position(), () =>
                                        {
                                            OffBar(true);
                                            ChangeState(CharacterState.Idle);
                                            //FlipCharacter();
                                            SetLayer(slotID);
                                            // Debug.Log("Attack Completed");
                                            isAttack = false;
                                            if (callback != null)
                                                callback();
                                        });
                                };
                                switch (model.AttackAction.type)
                                {
                                    case AttackActionType.ATTACK_WITH_EVENT:

                                        hitDameAction = null;
                                        hitDameAction = () =>
                                        {
                                            foreach (HeroController hero in _herosHit)
                                            {
                                                hero.HitDame(false, hitEffect);

                                            }

                                        };
                                        break;
                                    case AttackActionType.ATTACK_WITH_TIME_DELAY:
                                        hitDameAction = null;
                                        StartCoroutine(StartActionDelayTime(model.AttackAction.hitdame_delay_time, () =>
                                        {
                                            //if (model.CharacterType == CharacterType.Hero)
                                            zfx.PlayZfxSkill(EffectType.HIT_DAME_EFFECT);
                                            foreach (HeroController hero in _herosHit)
                                            {
                                                hero.HitDame(false, hitEffect);

                                            }
                                            UpdateAP(20);
                                        }));
                                        break;
                                }
                            }, 10f);

                        };
                    }
                    else
                    {
                        // Debug.Log("Attack " + _curHeroHit.name);
                        OffBar();


                    MoveToPos(targetPos, () =>
                   {

                       ChangeState(CharacterState.Attack);
                       zfx.PlayZfxSkill(EffectType.ATTACK_EFFECT);
                       // FndObjectOfType<AttackShake>().TriggerAttackShake();
                       //UpdateAP(20);
                       StartCoroutine(StartActionDelayTime(model.AttackAction.completed_delay_time, () =>
                       {

                           MoveToPos(positionStart.position(), () =>
                           {
                               OffBar(true);
                               ChangeState(CharacterState.Idle);
                                    //FlipCharacter();
                                    SetLayer(slotID);
                                    // Debug.Log("Attack Completed");
                                    isAttack = false;
                               if (callback != null)
                                   callback();
                           });

                       }));
                       switch (model.AttackAction.type)
                       {
                           case AttackActionType.ATTACK_WITH_EVENT:
                               hitDameAction = null;
                               hitDameAction = () =>
                               {
                                   foreach (HeroController hero in _herosHit)
                                   {
                                       hero.HitDame(false, hitEffect);

                                   }

                               };
                               break;
                           case AttackActionType.ATTACK_WITH_TIME_DELAY:
                               hitDameAction = null;
                               StartCoroutine(StartActionDelayTime(model.AttackAction.hitdame_delay_time, () =>
                               {
                                   foreach (HeroController hero in _herosHit)
                                   {
                                       hero.HitDame(false, hitEffect);

                                   }

                               }));
                               break;
                       }
                   }, model.AttackAction.effectDelay);


                    }
                  
                    break;
                case AttackType.Ranged:
                    ChangeState(CharacterState.Attack);
                    UpdateAP(20);
                    SetLayer(slotID);
                    Action hitdame = () =>
                    {

                        string bulletName , flashName, hitName;
                        var skeEffect = zfx.GetSkeletonEffect(EffectType.ATTACK_EFFECT);
                        GameObject HitDameEffect = zfx.GetParticalEffect(EffectType.ATTACK_EFFECT);
                      
                        //{
                          
                        //    foreach (HeroController hero in _herosHit)
                        //    {
                        //        GameObject bullet;
                        //        bullet = ObjectPool.Spawn("BulletSkeleton");
                        //        bullet.transform.position = positionStart.positionHitRange();
                        //        var animTrackEntry = bullet.GetComponent<Spine.Unity.SkeletonAnimation>().AnimationState.SetAnimation(0, "animation", false); 
                        //        animTrackEntry.TimeScale = LevelController.Instance.levelConfig.speedGame;
                        //        hero.HitDame(model.ATK / model.SkillAction.attackCount, false, HitDameEffect);
                        //    }
                           
                        //}
                      
                        {
                            GameObject effect = zfx.GetParticalEffect(EffectType.ATTACK_EFFECT);

                            bulletName = "FireBall";
                            flashName = "";
                            hitName = "";

                            foreach (HeroController hero in _herosHit)
                            {
                                
                                GameObject bullet;
                                if (skeEffect != null)
                                {
                                    bulletName = "BulletSkeleton";
                                    bullet = ObjectPool.Spawn(bulletName);
                                    bullet.GetComponent<SkeletonAnimation>().skeletonDataAsset = skeEffect;
                                    bullet.GetComponent<SkeletonAnimation>().Initialize(true);
                                    bullet.transform.localScale = new Vector2(1.5f, 1.5f);
                                    bullet.transform.eulerAngles = new Vector3(0, 0, Angle(hero.positionStart.positionHitRange(), positionStart.positionHitRange()));
                                    var animTrackEntry = bullet.GetComponent<Spine.Unity.SkeletonAnimation>().AnimationState.SetAnimation(0, "animation", false);
                                    animTrackEntry.TimeScale = LevelController.Instance.levelConfig.speedGame;
                                }
                                else if (effect != null)
                                {
                                    bullet = Instantiate(effect);
                                    bullet.transform.localScale = new Vector3(1f ,1f);
                                    bullet.transform.eulerAngles = new Vector3(0, -90, Angle(hero.positionStart.positionHitRange(), positionStart.positionHitRange()));
                                }

                                else
                                {
                                    bullet = ObjectPool.Spawn(bulletName);
                                    bullet.transform.localScale = new Vector3(1, 1);
                                    bullet.transform.eulerAngles = new Vector3(0, 90, Angle(hero.positionStart.positionHitRange(), positionStart.positionHitRange()));
                                }

                                //bullet.transform.LookAt(_curHeroHit.positionStart.positionHit());
                                GameObject hit;
                                GameObject HitDame = zfx.GetHitDameEffect(EffectType.ATTACK_EFFECT);
                                if (HitDame != null)
                                {
                                    hit = ObjectPool.Spawn(HitDame);
                                    hit.transform.localScale = new Vector2(0.25f,0.25f);
                                    hit.transform.position =new Vector2( hero.transform.position.x, hero.transform.position.y+0.5f);
                                }
                                bullet.transform.position = positionStart.positionHitRange() + new Vector2(0,1.5f);
                                bullet.transform.DOKill();
                                bullet.transform.DOMove(hero.positionStart.positionHitRange(),12)
                                .SetSpeedBased()
                                .OnStart(() => {
                                    if (skeEffect != null||flashName=="")
                                    {
                                        return;
                                    }
                                    GameObject flash;

                                    flash = ObjectPool.Spawn(flashName);
                                    flash.transform.localScale = new Vector2(1, 1);
                                    flash.transform.position = transform.position;
                                })
                                .SetEase(Ease.Linear)
                                .OnComplete(() => {
                                    isAttack = false;
                                    bullet.gameObject.SetActive(false);
                                    //ObjectPool.Recycle(bullet);
                                    hero.HitDame();
                                    GameObject hit;

                                    //if (HitDameEffect != null)
                                    //{
                                    //    hit = ObjectPool.Spawn(HitDameEffect);
                                    //   // hit.transform.localScale = new Vector2(1, 1);
                                    //    hit.transform.position = hero.transform.position;
                                    //}

                                    //else
                                    //{
                                    //    if (skeEffect == null||hitName=="")
                                    //    {
                                    //        hit = ObjectPool.Spawn(hitName);
                                    //        hit.transform.localScale = new Vector2(1, 1);
                                    //        hit.transform.position = hero.transform.position;
                                    //    }
                                    //}
                                });
                            }
                        }
                       
                      
                    };
                    switch (model.AttackAction.type)
                    {
                        case AttackActionType.ATTACK_WITH_EVENT:
                            hitDameAction = null;
                            hitDameAction = hitdame;
                            actionComplete = null;
                            actionComplete = () =>
                            {
                                ChangeState(CharacterState.Idle);
                                isAttack = false;
                                if (callback != null)
                                    callback();
                            };
                            break;
                        case AttackActionType.ATTACK_WITH_TIME_DELAY:
                            hitDameAction = null;
                            StartCoroutine(StartActionDelayTime(model.AttackAction.hitdame_delay_time, () => {
                                hitdame();
                            }));
                            StartCoroutine(StartActionDelayTime(model.AttackAction.completed_delay_time, () => {
                                ChangeState(CharacterState.Idle);
                                if (callback != null)
                                    callback();
                            }));
                            break;
                    }

                    
                    break;
                case AttackType.Arrow:
                    ChangeState(CharacterState.Attack);
                    UpdateAP(20);
                    hitDameAction = null;
                    Action hitDameArrow = () =>
                    {
                        foreach (HeroController hero in _herosHit)
                        {
                            
                            GameObject bullet = ObjectPool.Spawn("arrow");
                            bullet.transform.eulerAngles = new Vector3(0, 0, Angle(hero.positionStart.positionHitRange(), positionStart.positionHitRange()));
                            //bullet.transform.LookAt(_curHeroHit.positionStart.positionHit());
                            bullet.transform.localScale = new Vector2(.6f, .6f);
                            bullet.transform.position = positionStart.positionHitRange();
                            bullet.GetComponent<Arrow>().MoveToPosition(positionStart.positionHitRange(), hero.positionStart.positionHitRange(), LevelController.Instance.levelConfig.speedGame * 10, () => {
                                ObjectPool.Recycle(bullet);
                                hero.HitDame();
                                UpdateAP(20);
                                GameObject hit = ObjectPool.Spawn("Hit 1");
                                hit.transform.localScale = new Vector2(1, 1);
                                hit.transform.position = hero.transform.position;
                                isAttack = false;
                               
                            });
                        }

                    };
                    switch (model.AttackAction.type)
                    {
                        case AttackActionType.ATTACK_WITH_EVENT:
                            hitDameAction = null;
                            hitDameAction =hitDameArrow;
                            actionComplete = null;
                            actionComplete = () =>
                            {
                                ChangeState(CharacterState.Idle);
                                if (callback != null)
                                    callback();
                            };
                            break;
                        case AttackActionType.ATTACK_WITH_TIME_DELAY:
                            hitDameAction = null;
                            StartCoroutine(StartActionDelayTime(model.AttackAction.hitdame_delay_time, () => {
                                hitDameArrow();
                            }));
                            StartCoroutine(StartActionDelayTime(model.AttackAction.completed_delay_time, () => {
                                ChangeState(CharacterState.Idle);
                                if (callback != null)
                                    callback();
                            }));
                            break;
                    }

                    break;
            }
           
        }
       
        IEnumerator StartActionDelayTime(float delay_time, Action callback = null)
        {
            yield return new WaitForSeconds(delay_time);
            if (callback != null)
            {
                callback();
            }
        }
        float Angle(Vector2 pointA, Vector2 pointB)
        {
            Vector2 direction = pointA - pointB;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            return angle;
        }
        public void AttackSkill(List<HeroController> _herosHit,Vector2 posTartget, Action callback = null)
        {
            int apUpdate = 0;
            StopAllCoroutines();
            isAttack = true;
            GameObject hitEffect = zfx.GetHitDameEffect(EffectType.SKILL_EFFECT);
            UpdateAP(0);
            switch (model.CardType)
            {
                case AttackType.Melee:
                    if(model.SkillType == SkillAction.SKILL_STAND)
                    {
                        ChangeState(CharacterState.SkillAtk);
                       
                        
                        switch (model.SkillAction.type)
                        {
                            case AttackActionType.ATTACK_WITH_EVENT:
                                Debug.Log("Skill 2 Action hit dame");
                                hitDameAction = null;
                                foreach (HeroController hero in _herosHit)
                                {

                                    hero.UpdateAP(20);
                                }
                                hitDameAction = () => {
                                    foreach (HeroController hero in _herosHit)
                                    {
                                        hero.HitDame(false, hitEffect);

                                    }

                                };
                                break;
                            case AttackActionType.ATTACK_WITH_TIME_DELAY:
                                hitDameAction = null;
                               
                                StartCoroutine(StartActionDelayTime(model.SkillAction.hitdame_delay_time, () => {

                                    foreach (HeroController hero in _herosHit)
                                    {
                                        hero.HitDame(false, hitEffect);
                                        hero.UpdateAP(20);
                                    }
                                }));
                                StartCoroutine(StartActionDelayTime(model.SkillAction.effectDelay, () => {
                                    Debug.Log("Skill 2 Action hit dame "+ model.isMultiTarget);
                                    if (model.isMultiTarget)
                                    {
                                         zfx.PlayZfxSkillMultiPosition(EffectType.SKILL_EFFECT, _herosHit, isEnemy);
                                       
                                    }
                                    else
                                    {
                                        Vector2 pos = transform.position;
                                        zfx.PlayZfxSkillToPosition(EffectType.SKILL_EFFECT, pos, isEnemy);
                                    }
                                    
                                   
                                }));
                                StartCoroutine(StartActionDelayTime(model.SkillAction.completed_delay_time, () => {
                                    ChangeState(CharacterState.Idle);

                                    isAttack = false;
                                    if (callback != null)
                                        callback();
                                }));
                               
                                break;
                        }






                    }
                    else if (model.SkillType == SkillAction.SKILL_MOVE)
                    {
                        
                      
                        UpdateAP(0);
                        if(model.MoveAction.type == MoveActionType.MOVE_WITH_EVENT)
                        {
                            ChangeState(CharacterState.SkillAtk);
                            Debug.Log("Skill 2 with deaaaa ");
                            if (model.SkillAction.type== AttackActionType.ATTACK_WITH_EVENT)
                            {
                                zfx.PlayZfxSkillToPosition(EffectType.SKILL_EFFECT,posTartget,isEnemy);
                            }
                            if (model.CharacterType == CharacterType.Boss)
                            {
                                ChangeState(CharacterState.AttackUp);

                                StartCoroutine(StartActionDelayTime(.1f, () =>
                                {

                                    ChangeState(CharacterState.AttackDown);

                                }));
                                OffBar();
                                GetComponent<HeroJumpController>().JumpToTarget(transform.position, posTartget, 10, () =>
                                {
                                    AttackShake.instance.ShakeCineCamera();
                                    moveUpAction = null;
                                    SetLayer(slotID + 50);
                                    moveUpAction = () =>
                                    {
                                        zfx.StopZfx();
                                        MoveToPos(positionStart.position(), () =>
                                        {
                                            OffBar(true);
                                            ChangeState(CharacterState.Idle);
                                            //FlipCharacter();
                                            SetLayer(slotID);
                                            // Debug.Log("Attack Completed");
                                            isAttack = false;
                                            if (callback != null)
                                                callback();
                                        });
                                    };
                                    switch (model.SkillAction.type)
                                    {
                                        case AttackActionType.ATTACK_WITH_EVENT:

                                            foreach (HeroController hero in _herosHit)
                                            {

                                                hero.UpdateAP(20);
                                            }
                                            hitDameAction = null;
                                            hitDameAction = () => {
                                                foreach (HeroController hero in _herosHit)
                                                {
                                                    hero.HitDame(false, hitEffect);

                                                }
                                            };
                                            break;
                                        case AttackActionType.ATTACK_WITH_TIME_DELAY:
                                            hitDameAction = null;
                                            StartCoroutine(StartActionDelayTime(model.SkillAction.hitdame_delay_time, () => {

                                                foreach (HeroController hero in _herosHit)
                                                {
                                                    hero.HitDame(false, hitEffect);
                                                    hero.UpdateAP(20);
                                                }
                                            }));
                                            StartCoroutine(StartActionDelayTime(model.SkillAction.effectDelay, () => {
                                                Debug.Log("Attack Skill");
                                                zfx.PlayZfxSkillToPosition(EffectType.SKILL_EFFECT, GameController.Instance.skillPosition.position, isEnemy);
                                            }));
                                            break;
                                    }

                                }, .4f);
                            }
                            else
                            {
                                moveUpAction = null;
                                moveUpAction = () =>
                                {
                                    OffBar();
                                    MoveToPos(posTartget, () =>
                                    {
                                        moveUpAction = null;
                                        SetLayer(slotID + 50);
                                        moveUpAction = () =>
                                        {
                                            zfx.StopZfx();
                                            MoveToPos(positionStart.position(), () =>
                                            {
                                                OffBar(true);
                                                ChangeState(CharacterState.Idle);
                                                //FlipCharacter();
                                                SetLayer(slotID );
                                                // Debug.Log("Attack Completed");
                                                isAttack = false;
                                                if (callback != null)
                                                    callback();
                                            });
                                        };
                                        

                                    });

                                };
                            }
                           
                        }
                        else
                        {
                           
                            if (model.CharacterType == CharacterType.Boss)
                            {
                                ChangeState(CharacterState.AttackUp);

                                StartCoroutine(StartActionDelayTime(.07f, () =>
                                {

                                    ChangeState(CharacterState.AttackDown);

                                }));
                                OffBar();
                                GameController.Instance.CamZoomIn(true);
                                GetComponent<HeroJumpController>().JumpToTarget(transform.position, posTartget, 10, () =>
                                {
                                    AttackShake.instance.ShakeCineCamera();
                                    StartCoroutine(StartActionDelayTime(model.SkillAction.completed_delay_time, () => {
                                        GameController.Instance.CamZoomOut();
                                        MoveToPos(positionStart.position(), () =>
                                        {

                                            OffBar(true);
                                            ChangeState(CharacterState.Idle);
                                            //FlipCharacter();
                                            SetLayer(slotID + 50);
                                            // Debug.Log("Attack Completed");
                                            isAttack = false;
                                            if (callback != null)
                                                callback();
                                        });
                                    }));

                                    switch (model.SkillAction.type)
                                    {
                                        case AttackActionType.ATTACK_WITH_EVENT:

                                            hitDameAction = null;
                                            foreach (HeroController hero in _herosHit)
                                            {

                                                hero.UpdateAP(20);
                                            }
                                            hitDameAction = () => {
                                                foreach (HeroController hero in _herosHit)
                                                {
                                                    hero.HitDame(false, hitEffect);

                                                }

                                            };
                                            break;
                                        case AttackActionType.ATTACK_WITH_TIME_DELAY:
                                            hitDameAction = null;
                                            StartCoroutine(StartActionDelayTime(model.SkillAction.hitdame_delay_time, () => {

                                                foreach (HeroController hero in _herosHit)
                                                {
                                                    hero.HitDame(false, hitEffect);
                                                    hero.UpdateAP(20);
                                                }
                                            }));
                                            StartCoroutine(StartActionDelayTime(model.SkillAction.effectDelay, () => {

                                               

                                                Vector2 pos;
                                                if (!model.isMoveToEnemyPosition)
                                                    pos = isEnemy ? GameController.Instance.skillPosition.position : GameController.Instance.skillPosition.position;
                                                else
                                                    pos = posTartget;
                                                zfx.PlayZfxSkillToPosition(EffectType.SKILL_EFFECT, pos, isEnemy);
                                            }));
                                            break;
                                    }

                                }, .4f);
                            }
                            else
                            {
                                moveUpAction = null;
                                if (model.SkillAction.type == AttackActionType.ATTACK_WITH_EVENT)
                                {
                                    zfx.PlayZfxSkill(EffectType.SKILL_EFFECT);
                                }
                                OffBar();
                                if (model.CharacterType == CharacterType.Boss)
                                {

                                }
                                GameController.Instance.CamZoomIn(isEnemy);
                                MoveToPos(posTartget, () =>
                                {
                                    Debug.Log("Skill 2 with delay ");
                                    ChangeState(CharacterState.SkillAtk);
                                  
                                    AttackShake.instance.ShakeCineCamera();
                                    StartCoroutine(StartActionDelayTime(model.SkillAction.completed_delay_time, () => {
                                        Debug.Log("Move go Skill 2 with delay ");
                                        GameController.Instance.CamZoomOut();
                                        SetLayer(slotID + 50);
                                        MoveToPos(positionStart.position(), () =>
                                        {
                                            OffBar(true);
                                            ChangeState(CharacterState.Idle);
                                            //FlipCharacter();
                                            SetLayer(slotID);
                                            // Debug.Log("Attack Completed");
                                            isAttack = false;
                                            if (callback != null)
                                                callback();
                                        });
                                    }));

                                    switch (model.SkillAction.type)
                                    {
                                        case AttackActionType.ATTACK_WITH_EVENT:
                                            Debug.Log("Skill 2 Action hit dame");
                                            hitDameAction = null;
                                            foreach (HeroController hero in _herosHit)
                                            {

                                                hero.UpdateAP(20);
                                            }
                                            hitDameAction = () => {
                                                foreach (HeroController hero in _herosHit)
                                                {
                                                    hero.HitDame(false, hitEffect);

                                                }

                                            };
                                            break;
                                        case AttackActionType.ATTACK_WITH_TIME_DELAY:
                                            hitDameAction = null;
                                            Debug.Log("Skill 2 Action hit dame");
                                            StartCoroutine(StartActionDelayTime(model.SkillAction.hitdame_delay_time, () => {

                                                foreach (HeroController hero in _herosHit)
                                                {
                                                    hero.HitDame(false, hitEffect);
                                                    hero.UpdateAP(20);
                                                }
                                            }));
                                            StartCoroutine(StartActionDelayTime(model.SkillAction.effectDelay, () => {
                                                Vector2 pos;
                                                if (!model.isMoveToEnemyPosition)
                                                    pos = isEnemy ? GameController.Instance.skillPosition.position : GameController.Instance.skillPosition.position;
                                                else
                                                    pos = posTartget;
                                                zfx.PlayZfxSkillToPosition(EffectType.SKILL_EFFECT, pos, isEnemy);
                                            }));
                                            break;
                                    }

                                });
                            }
                           
                        }
                        
                    }
                    break;
                case AttackType.Ranged:
                    ChangeState(CharacterState.SkillAtk);
                   
                    UpdateAP(0);
                    //Debug.LogError("Skill attack " + posTartget);
                    zfx.PlayZfxSkillToPosition(EffectType.SKILL_EFFECT, posTartget, isEnemy);
                    switch (model.SkillAction.type)
                    {
                        case AttackActionType.ATTACK_WITH_EVENT:
                            hitDameAction = null;
                            foreach (HeroController hero in _herosHit)
                            {
                                
                                hero.UpdateAP(20);
                            }
                            hitDameAction = () => {

                                foreach (HeroController hero in _herosHit)
                                {
                                    hero.HitDame( false, hitEffect);
                                }

                            };
                            skillAActionCompleted = null;
                            skillAActionCompleted = () =>
                            {
                                ChangeState(CharacterState.Idle);

                                isAttack = false;
                                if (callback != null)
                                    callback();
                            };
                            break;
                        case AttackActionType.ATTACK_WITH_TIME_DELAY:
                            hitDameAction = null;
                            StartCoroutine(StartActionDelayTime(model.SkillAction.hitdame_delay_time, () => {

                                foreach (HeroController hero in _herosHit)
                                {
                                    hero.HitDame( false, hitEffect);
                                    UpdateAP(20);
                                }
                            }));
                            StartCoroutine(StartActionDelayTime(model.SkillAction.completed_delay_time, () => {
                                ChangeState(CharacterState.Idle);

                                isAttack = false;
                                if (callback != null)
                                    callback();
                            }));
                            break;
                    }
                    break;
                case AttackType.Arrow:
                    ChangeState(CharacterState.SkillAtk);

                    UpdateAP(0);
                    zfx.PlayZfxSkillToPosition(EffectType.SKILL_EFFECT,posTartget, isEnemy);
                    switch (model.SkillAction.type)
                    {
                        case AttackActionType.ATTACK_WITH_EVENT:
                            hitDameAction = null;
                            foreach (HeroController hero in _herosHit)
                            {
                               
                                hero.UpdateAP(20);
                            }
                            hitDameAction = () => {

                                foreach (HeroController hero in _herosHit)
                                {
                                    hero.HitDame(false, hitEffect);
                                }

                            };
                            skillAActionCompleted = null;
                            skillAActionCompleted = () =>
                            {
                                ChangeState(CharacterState.Idle);

                                isAttack = false;
                                if (callback != null)
                                    callback();
                            };
                            break;
                        case AttackActionType.ATTACK_WITH_TIME_DELAY:
                            hitDameAction = null;
                            StartCoroutine(StartActionDelayTime(model.SkillAction.hitdame_delay_time, () => {
                                foreach (HeroController hero in _herosHit)
                                {
                                    hero.HitDame( false, hitEffect);
                                    UpdateAP(20);
                                }
                            }));
                            StartCoroutine(StartActionDelayTime(model.SkillAction.completed_delay_time, () => {
                                ChangeState(CharacterState.Idle);

                                isAttack = false;
                                if (callback != null)
                                    callback();
                            }));
                            break;
                    }
                    break;
             
            }
        }
        IEnumerator AttackToHero (HeroController _heroHit, float _dameHit)
        {
            yield return new WaitForSeconds(0.8f/LevelController.Instance.levelConfig.speedGame);
            _heroHit.HitDame();

        }

        IEnumerator AttackToHeros(List<HeroHit> _herosHit)
        {
            yield return new WaitForSeconds(0.5f);
            //for (int i = 0; i < _herosHit.Count; i++)
            //{
            //    _herosHit[i].hero.HitDame(_herosHit[i].dameHit, _herosHit[i].crt, 0);
            //}

        }
        public void SetEventSkillAttack()
        {
            if (skillAActionCompleted != null)
                skillAActionCompleted();
        }
        public void SetEventAttack()
        {
            if(actionComplete!= null)
                actionComplete();
        }
        public void SetEvenCharacterMoveUp()
        {
            if(moveUpAction!=null)
                moveUpAction();
        }
       
        public void GetEventAttack()
        {
            //if (actionComplete != null)
               
        }
        public void SetEventHit()
        {
            if(hitDameAction!=null)
            hitDameAction();
        }
        public override void ChangeState(CharacterState _state)
        {
            base.ChangeState(_state);
        }
        public override void UpdateHP(float deltaHP)
        {
            base.UpdateHP(deltaHP);
            SetHPBar();
            
            
        }
        public override void SetHP(float deltaHP, float maxHP)
        {
            base.SetHP(deltaHP, maxHP);
            // SetHPBar();
            
           // CheckDie();


        }
        public override void UpdateAP(float deltaAP)
        {
            base.UpdateAP(deltaAP);
            SetAPBar();

        }
        public override void SetAP(float deltaAP)
        {
            base.SetAP(deltaAP);
           // SetAPBar();

        }

        public void HitDame( bool _crtDame = false, GameObject hitdameEffect = null)
        {
            if (isDie)
                return;
           
            //Debug.Log("dame hit: " + _dame);
            //UpdateHP(-atk);
            ChangeState(CharacterState.Hit);
            SetHPBar();
            SetAPBar();
            CheckDie();
            //string sDame = _crtDame == true ? "Crit " : "" + Math.Round();
            for(int i = 0; i < atk.Length; i++)
            {
                if (transform != null)
                    ZfxGameplayController.Instance.DamageNumberSpawn(new Vector2(transform.position.x, transform.position.y + 1), atk[i], typeDame[i]);
                if (hitdameEffect != null)
                {
                    ObjectPool.Spawn(hitdameEffect, new Vector2(transform.position.x,transform.position.y+0.5f));
                }
                if (isEnemy && typeDame[i] != TypeDmg.Heal)
                {
                    GameController.Instance.SetTotalDame((int)atk[i]);
                }
                atk[i]= 0;
            }
            
        }
        public void HitDame(float dame,TypeDmg typeDmg)
        {
            if (isDie)
                return;

            //Debug.Log("dame hit: " + _dame);
            //UpdateHP(-dame);
            ChangeState(CharacterState.Hit);
            SetHPBar();
            SetAPBar();
            CheckDie();
            if (isEnemy&&typeDmg!= TypeDmg.Heal)
            {
                GameController.Instance.SetTotalDame((int)dame);
            }
            //string sDame = _crtDame == true ? "Crit " : "" + Math.Round();
            if (transform != null)
                ZfxGameplayController.Instance.DamageNumberSpawn(new Vector2(transform.position.x, transform.position.y + 1), dame,typeDmg);
            //if (hitdameEffect != null)
            //{
            //    ObjectPool.Spawn(hitdameEffect, transform.position);
            //}
            //atk = 0;
        }
        public void HitDameInShoot(float _dame, bool _crtDame = false)
        {
            if (isDie)
                return;
            try
            {
                if (!isAttack)
                {
                    if(model.CharacterType!= CharacterType.Box)
                        ChangeState(CharacterState.Hit);
                    else
                        ChangeState(CharacterState.Box);
                }
               // UpdateHP(-_dame);
                SetHPBar();
               // UpdateAP(20);
               // CheckDie();
                string sDame = _crtDame == true ? "Crit " : "" + Math.Round(_dame);
                if (transform != null)
                    ZfxGameplayController.Instance.DamageNumberSpawn(transform.position, _dame);
            }
            catch
            {

            }
        }
        bool isDie = false;
        void CheckDie()
        {
            
            if (curInfo.hp <= 0&&!isDie)
            {
                
                isDie = true;
                //Die();

                Invoke("HideGraphic", 2);  
            
                if (isAttack)
                {
                    //if (StaticData.GameMode == GameMode.Campaign)
                        GameController.Instance.isAttack = false;
                    
                }
          
            }
        }
        public override void Die()
        {
            base.Die();

        }
        public void Run()
        {
            ChangeState(CharacterState.Run);
        }
    }
}
