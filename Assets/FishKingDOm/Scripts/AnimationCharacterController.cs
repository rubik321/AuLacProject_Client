using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using Sirenix.OdinInspector;
using Rubik.Config;
using UnityEngine.Rendering;
namespace Rubik.Battle
{
    public enum AnimationCharacterState
    {
        Idle, Run, Attack, AttackUp, AttackDown, Hit, Die, SkillBuff, SkillAttack, Jump,Win,Box, Box_Open
    }
    public class AnimationCharacterController : SpineController
    {
        public AnimationCharacterState state;
        [SerializeField]HeroController heroCtr;
        Spine.EventData eventData;
        
        private void Start()
        {
        }
        public override void SetSkeletonData(SkeletonDataAsset skeletonData)
        {
            base.SetSkeletonData(skeletonData);
            ChangeAnimation(AnimationCharacterState.Idle);
        }
        public void SetLayer(int _sortingOrder)
        {

            skeletonAnimation.GetComponent<MeshRenderer>().sortingOrder = _sortingOrder;
            skeletonAnimation.GetComponent<SortingGroup>().sortingOrder = _sortingOrder;
        }
        public override void Begin()
        {
            base.Begin();
           
            skeletonAnimation.transform.SetParent(transform);
            skeletonAnimation.transform.localPosition = Vector2.zero;
            //skeletonAnimation.GetComponent<MeshRenderer>().sortingOrder = 1;
            skeletonAnimation.GetComponent<MeshRenderer>().sortingLayerName = "Character";
            //if(CheckIfAnimationExists(skeletonAnimation.AnimationState, AnimationConfigs.IDLE))
                ChangeAnimation(AnimationCharacterState.Idle);
            //skeletonAnimation.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(1, "action/idle/normal", true);
            //heroCtr = GetComponent<HeroController>();
            if (skeletonAnimation == null) return;

            // This is how you subscribe via a declared method.
            // The method needs the correct signature.
            skeletonAnimation.AnimationState.Event += HandleEvent;

            skeletonAnimation.AnimationState.Start += delegate (TrackEntry trackEntry) {
                // You can also use an anonymous delegate.
                //Debug.Log(string.Format("track {0} started a new animation.", trackEntry.TrackIndex));
            };
            var eventData = skeletonAnimation.Skeleton.Data.FindEvent("attack");
           
             
            skeletonAnimation.AnimationState.End += delegate {
                // ... or choose to ignore its parameters.
               // Debug.Log("An animation ended!");
            };
            skeletonAnimation.AnimationState.Complete += delegate {
                if (animTrackEntry != null && animTrackEntry.IsComplete)
                    CompleteAnimation();
               // Debug.Log("An animation Complete!");
            };
        }
        [Button]
        public void ChangeAnimation(AnimationCharacterState animationState)
        {
            //Debug.Log("animationState : " + animationState);
            switch (animationState)
            {
                case AnimationCharacterState.Idle:
                    if(SpineController.CheckIfAnimationExists(skeletonAnimation.AnimationState, AnimationConfigs.IDLE))
                    {
                        SetAnimationState(0, AnimationConfigs.IDLE, true);
                    }
                    else
                    {
                        string anim = SpineController.GetAnimationExists(skeletonAnimation.AnimationState, AnimationConfigs.ListIdles);
                        
                        SetAnimationState(0, anim, true);
                    }
                    state = AnimationCharacterState.Idle;
                    break;
                case AnimationCharacterState.Run:
                    if (SpineController.CheckIfAnimationExists(skeletonAnimation.AnimationState, AnimationConfigs.RUN))
                    {
                        SetAnimationState(0, AnimationConfigs.RUN, true);
                    }
                    else
                    {
                        string anim = SpineController.GetAnimationExists(skeletonAnimation.AnimationState, AnimationConfigs.ListAttacks);
                        SetAnimationState(0, SpineController.GetAnimationExists(skeletonAnimation.AnimationState, AnimationConfigs.ListIdles), true);
                    }
                    state = AnimationCharacterState.Run;
                    break;
                case AnimationCharacterState.Attack:
                    if (SpineController.CheckIfAnimationExists(skeletonAnimation.AnimationState, AnimationConfigs.ATTACK))
                    {
                        SetAnimationState(0, AnimationConfigs.ATTACK, false);
                    }
                    else
                    {
                        string anim = SpineController.GetAnimationExists(skeletonAnimation.AnimationState, AnimationConfigs.ListAttacks);
                       // Debug.Log(anim);
                        SetAnimationState(0, anim, false);
                    }

                    state = AnimationCharacterState.Attack;
                    break;
                case AnimationCharacterState.AttackUp:
                    if (SpineController.CheckIfAnimationExists(skeletonAnimation.AnimationState, AnimationConfigs.ATTACK))
                    {
                        SetAnimationState(0, AnimationConfigs.ATTACK, false);
                    }
                    else
                    {
                        string anim = SpineController.GetAnimationExists(skeletonAnimation.AnimationState, AnimationConfigs.ListAttacks);
                        Debug.Log(anim);
                        SetAnimationState(0, anim, false);
                    }

                    state = AnimationCharacterState.Attack;
                    break;
                case AnimationCharacterState.AttackDown:
                    if (SpineController.CheckIfAnimationExists(skeletonAnimation.AnimationState, AnimationConfigs.ATTACK))
                    {
                        SetAnimationState(0, AnimationConfigs.ATTACK, false);
                    }
                    else
                    {
                        string anim = SpineController.GetAnimationExists(skeletonAnimation.AnimationState, AnimationConfigs.ListAttacks);
                        Debug.Log(anim);
                        SetAnimationState(0, anim, false);
                    }

                    state = AnimationCharacterState.Attack;
                    break;
                case AnimationCharacterState.Hit:
                    if (SpineController.CheckIfAnimationExists(skeletonAnimation.AnimationState, AnimationConfigs.HIT))
                    {
                        SetAnimationState(0, AnimationConfigs.HIT, false);
                    }
                    else
                    {
                        string anim = SpineController.GetAnimationExists(skeletonAnimation.AnimationState, AnimationConfigs.ListHits);
                        SetAnimationState(0, anim, false);
                    }
                    state = AnimationCharacterState.Hit;
                    break;
                case AnimationCharacterState.Die:
                    if (SpineController.CheckIfAnimationExists(skeletonAnimation.AnimationState, AnimationConfigs.DIE))
                    {
                        SetAnimationState(0, AnimationConfigs.DIE, false);
                    }
                    else
                    {
                        string anim = SpineController.GetAnimationExists(skeletonAnimation.AnimationState, AnimationConfigs.ListDies);
                        SetAnimationState(0, anim, false);
                    }
                    state = AnimationCharacterState.Die;
                    break;
                case AnimationCharacterState.SkillBuff:
                    if (SpineController.CheckIfAnimationExists(skeletonAnimation.AnimationState, AnimationConfigs.SKILL_ATTACK))
                    {
                        SetAnimationState(0, AnimationConfigs.DIE, false);
                    }
                    else
                    {
                        SetAnimationState(0, AnimationConfigs.IDLE, false);
                    }
                    state = AnimationCharacterState.SkillBuff;
                    break;
                case AnimationCharacterState.SkillAttack:
                    if (SpineController.CheckIfAnimationExists(skeletonAnimation.AnimationState, AnimationConfigs.SKILL_ATTACK))
                    {
                        SetAnimationState(0, AnimationConfigs.SKILL_ATTACK, false);
                    }
                    else
                    {
                        string anim = SpineController.GetAnimationExists(skeletonAnimation.AnimationState, AnimationConfigs.ListSkills);
                        //Debug.Log("Anim name : " + anim);
                        SetAnimationState(0, anim, false);
                    }
                    // state = AnimationCharacterState.SkillBuff;
                    break; 
                case AnimationCharacterState.Jump:
                    if (SpineController.CheckIfAnimationExists(skeletonAnimation.AnimationState, AnimationConfigs.JUMP))
                    {
                        SetAnimationState(0, AnimationConfigs.JUMP, false);
                    }
                    else
                    {
                        string anim = SpineController.GetAnimationExists(skeletonAnimation.AnimationState, AnimationConfigs.ListIdles);
                        SetAnimationState(0, anim, false);
                    }
                    state = AnimationCharacterState.SkillBuff;
                    break;
                case AnimationCharacterState.Win:
                    if (SpineController.CheckIfAnimationExists(skeletonAnimation.AnimationState, AnimationConfigs.WIN))
                    {
                        SetAnimationState(0, AnimationConfigs.WIN, true);
                    }
                    else
                    {
                        string anim = SpineController.GetAnimationExists(skeletonAnimation.AnimationState, AnimationConfigs.ListIdles);
                        SetAnimationState(0, anim, true);
                    }
                    // state = AnimationCharacterState.SkillBuff;
                    break;
                case AnimationCharacterState.Box:
                    if (SpineController.CheckIfAnimationExists(skeletonAnimation.AnimationState, AnimationConfigs.BOX))
                    {
                        SetAnimationState(0, AnimationConfigs.BOX, false);
                    }
                    else
                    {
                        SetAnimationState(0, AnimationConfigs.IDLE, false);
                    }
                    state = AnimationCharacterState.SkillBuff;
                    break; 
                case AnimationCharacterState.Box_Open:
                    if (SpineController.CheckIfAnimationExists(skeletonAnimation.AnimationState, AnimationConfigs.BOX_OPEN))
                    {
                        SetAnimationState(0, AnimationConfigs.BOX_OPEN, false);
                    }
                    else
                    {
                        string anim = SpineController.GetAnimationExists(skeletonAnimation.AnimationState, AnimationConfigs.ListIdles);
                        SetAnimationState(0, anim, false);
                    }
                    state = AnimationCharacterState.SkillBuff;
                    break;
                default:
                    if (SpineController.CheckIfAnimationExists(skeletonAnimation.AnimationState, AnimationConfigs.IDLE))
                    {
                        SetAnimationState(0, AnimationConfigs.SKILL_BUFF, false);
                    }
                    else
                    {
                        string anim = SpineController.GetAnimationExists(skeletonAnimation.AnimationState, AnimationConfigs.ListIdles);
                        SetAnimationState(0, anim, false);
                    }
                    state = AnimationCharacterState.Idle;
                    break;
            }
        }

        void HandleEvent(TrackEntry trackEntry, Spine.Event e)
        {
            // Play some sound if the event named "footstep" fired.
            if (e.Data.Name == EventConfigs.ATTACK)
            {
                //Debug.Log("Play a footstep sound!");
                HitDame();
            } 
            if (e.Data.Name == EventConfigs.MOVE_TO_ATTACK || e.Data.Name == EventConfigs.MOVE_TO_ATTACK_1)
            {
               // Debug.Log("Play UPPPPPPP !");
                heroCtr.SetEvenCharacterMoveUp();
            }

        }

        void CompleteAnimation()
        {
            switch (skeletonAnimation.AnimationName)
            {
               // case AnimationConfigs.DIE:
               //     //GetComponentInParent<HeroController>().HideGraphic();
               //     //ObjectPool.Recycle(GetComponentInParent<HeroController>().gameObject);
               //     break;
               //case AnimationConfigs.DIE_DEFAULT:
               //    // GetComponentInParent<HeroController>().HideGraphic();
               //     //ObjectPool.Recycle(GetComponentInParent<HeroController>().gameObject);
               //     break;
               // case AnimationConfigs.ATTACK:
               //     heroCtr.SetEventAttack();
               //     break;
               // case AnimationConfigs.HIT:
               //     Debug.Log("Complete Hit Dame");
               //     ChangeAnimation(AnimationCharacterState.Idle);
               //     break;
               // case AnimationConfigs.HIT_DEFAULT:
               //     ChangeAnimation(AnimationCharacterState.Idle);
               //     break;
               // case AnimationConfigs.SKILL_ATTACK:
               //     heroCtr.SetEventSkillAttack();
               //     break;
               // case AnimationConfigs.JUMP:
               //     ChangeAnimation(AnimationCharacterState.Idle);
               //     break;
               // case AnimationConfigs.BOX_OPEN:
               //     //ChangeAnimation(AnimationCharacterState.Idle);
               //     break;
                default:
                    ChangeAnimation(AnimationCharacterState.Idle);
                    break;
            }
        }
        public void SetOrderIndex(int index)
        {

        }
       
        public void HitDame()
        {
            //Debug.Log("Hit dameeeeeee");
            heroCtr.SetEventHit();

        }
    }

}