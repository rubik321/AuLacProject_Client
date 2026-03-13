using Rubik.Battle;
using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rubik
{
    public class ZfxCharacter : MonoBehaviour
    {
        [SerializeField]public List<SkeletonAnimation> listSkeleton;
        public List<EffectData> effectAsset;
       [SerializeField] bool isEnemy = false;
        public void Begin(bool isEnemy =false)
        {
            this.isEnemy = isEnemy;
            foreach(SkeletonAnimation ske in listSkeleton)
            {
                if (!isEnemy)
                {
                    ske.transform.localScale = new Vector2(-ske.transform.localScale.x, ske.transform.localScale.y);
                }
            }
        }
        //public void PlayZfxSkill(int id)
        //{
        //    listSkeleton.ForEach(fx => fx.gameObject.SetActive(false));
        //    listSkeleton[id].gameObject.SetActive(true);
        //    listSkeleton[id].AnimationState.SetAnimation(0,"animation", false);
        //    var animTrackEntry = listSkeleton[id].AnimationState.SetAnimation(0, "animation", false); ;
        //    animTrackEntry.TimeScale = LevelController.Instance.levelConfig.speedGame;
        //}
        public void PlayZfxSkill(EffectType type)
        {
            int index = 0;
            foreach (EffectData data in effectAsset)
            {
                if(type == data.effectID)
                {
                    if (data.sketetonEffect != null)
                    {
                        //var anim = ObjectPool.Spawn(listSkeleton[(int)type], listSkeleton[(int)type].transform.position);
                        // listSkeleton[(int)type].transform.position = pos;
                        listSkeleton[index].gameObject.SetActive(true);
                        var animTrackEntry = listSkeleton[index].AnimationState.SetAnimation(0, "animation", false);
                       
                        animTrackEntry.TimeScale = LevelController.Instance.levelConfig.speedGame; 
                        animTrackEntry.Delay = data.delayEffect;
                    }
                    else if (data.particalEffect != null)
                    {
                        Debug.Log(data.particalEffect);
                        var anim = ObjectPool.Spawn(data.particalEffect, listSkeleton[(int)type].transform.position);
                        if (type == EffectType.HIT_DAME_EFFECT)
                            anim.transform.position = new Vector3(anim.transform.position.x, anim.transform.position.y + 1.5f);
                        anim.GetComponent<ParticleSystem>().Play();
                    }
                    //return;
                }
                index++;
            }
           
        }
        public EffectData GetActionEffect(EffectType type)
        {
            foreach (EffectData data in effectAsset)
            {
                if (type == data.effectID)
                {
                    if (data.sketetonEffect != null)
                    {
                        return data;
                    }
                    else if (data.particalEffect != null)
                    {
                        return data;
                    }
                    
                }
            }
            return null;
        }
        public GameObject GetHitDameEffect(EffectType type)
        {
            foreach (EffectData data in effectAsset)
            {
                if (type == data.effectID)
                {
                    if (data.hitDameEffect != null)
                    {
                        return data.hitDameEffect;
                    }
                   
                }
            }
            return null;
        }
        public GameObject GetParticalEffect(EffectType type)
        {
            foreach (EffectData data in effectAsset)
            {
                if (type == data.effectID)
                {
                    if (data.particalEffect != null)
                    {
                        return data.particalEffect;
                    }

                }
            }
            return null;
        }
        public SkeletonDataAsset GetSkeletonEffect(EffectType type)
        {
            foreach (EffectData data in effectAsset)
            {
                if (type == data.effectID)
                {
                    if (data.sketetonEffect != null)
                    {
                        return data.sketetonEffect;
                    }

                }
            }
            return null;
        }
        //public void PlayZfxSkillToPosition(int id, Vector2 pos, bool isEnemy)
        //{
        //    listSkeleton.ForEach(fx => fx.gameObject.SetActive(false));
        //    listSkeleton[id].gameObject.SetActive(true);
        //    listSkeleton[id].transform.position = pos;
        //    // listSkeleton[id].transform.localScale = isEnemy ? new Vector2(-listSkeleton[id].transform.localScale.x, listSkeleton[id].transform.localScale.y) : listSkeleton[id].transform.localScale;
        //    var animTrackEntry = listSkeleton[id].AnimationState.SetAnimation(0, "animation", false); ;
        //    animTrackEntry.TimeScale = LevelController.Instance.levelConfig.speedGame;

        //}
        public void PlayZfxSkillToPosition(EffectType type,Vector2 pos,bool isEnemy)
        {
            int index = 0;
            foreach (EffectData data in effectAsset)
            {
                if (type == data.effectID)
                {
                    if (data.sketetonEffect != null)
                    {
                        // var anim = ObjectPool.Spawn(listSkeleton[(int)type]);
                        listSkeleton[index].transform.position = pos;
                        listSkeleton[index].gameObject.SetActive(true);
                        TrackEntry animTrackEntry;
                        if (CheckIfAnimationExists(listSkeleton[index].AnimationState, "animation"))
                        {
                            animTrackEntry = listSkeleton[index].AnimationState.SetAnimation(0, "animation", false);
                        }
                        else
                        {
                            animTrackEntry = listSkeleton[index].AnimationState.SetAnimation(0, "VFX-skill", false);
                        }
                        animTrackEntry.TrackTime = data.startEffect;
                        

                        animTrackEntry.TimeScale = LevelController.Instance.levelConfig.speedGame;
                        //listSkeleton[index].transform.position = new Vector2(pos.x + 1, pos.y);
                        if (!isEnemy)
                        {
                            //hero position spawn effect
                            if (!data.isStartPos)
                                listSkeleton[index].transform.position = new Vector2(pos.x + 1, pos.y);
                            else
                                listSkeleton[index].transform.position = GameController.Instance.skillStartHero.position;
                            if (data.isSpwanOnTarget)
                            {
                                listSkeleton[index].transform.position = GameController.Instance.skillRangePosition.position;
                            }

                            listSkeleton[index].transform.localScale = new Vector2(data.Scale.x, data.Scale.y); 
                        }
                        else
                        {
                            //enemy position spawn effect
                            if (!data.isStartPos)
                                listSkeleton[index].transform.position = new Vector2(pos.x-1, pos.y);
                            else
                                listSkeleton[index].transform.position = GameController.Instance.skillStartEnemy.position;
                            if (data.isSpwanOnTarget)
                            {
                                listSkeleton[index].transform.position = GameController.Instance.skillEnemyRange.position;
                            }
                            listSkeleton[index].transform.localScale = new Vector2(-data.Scale.x, data.Scale.y); 
                        }
                       
                    }
                    else if (data.particalEffect != null)
                    {
                        GameObject anim;
                        if (!isEnemy)
                        {
                            anim = ObjectPool.Spawn(data.particalEffect);
                        }
                        else
                        {
                            anim = ObjectPool.Spawn(data.particalEffect);
                        }
                        anim.transform.position = pos;

                        if(type == EffectType.HIT_DAME_EFFECT)
                            anim.transform.position = new Vector3(pos.x,pos.y+1.5f);
                        if (anim.GetComponent<ParticleSystem>()!=null)
                            anim.GetComponent<ParticleSystem>().Play();
                    }
                    //return;
                }
                index++;
            }
        }
        public void PlayZfxSkillMultiPosition(EffectType type, List<HeroController> lshits, bool isEnemy)
        {
            int index = 0;
            foreach (EffectData data in effectAsset)
            {
                if (type == data.effectID)
                {
                    if (data.sketetonEffect != null)
                    {
                        // var anim = ObjectPool.Spawn(listSkeleton[(int)type]);
                        int indexHit = 0;
                        foreach (HeroController hit in lshits)
                        {
                            if (hit.typeDame[0] == BattleEngine.TypeDmg.Normal|| hit.typeDame[0] == BattleEngine.TypeDmg.Crit)
                            {
                                SkeletonAnimation ske = ObjectPool.Spawn(listSkeleton[(int)type]);

                                ske.transform.position = hit.transform.position;
                                ske.gameObject.SetActive(true);
                                TrackEntry animTrackEntry;
                                if (CheckIfAnimationExists(ske.AnimationState, "animation"))
                                {
                                    animTrackEntry = ske.AnimationState.SetAnimation(0, "animation", false);
                                }
                                else
                                {
                                    animTrackEntry = ske.AnimationState.SetAnimation(0, "skill", false);
                                }
                                animTrackEntry.TrackTime = data.startEffect;

                                ske.transform.position = new Vector2(hit.transform.position.x, hit.transform.position.y+0.5f);
                                ske.transform.localScale = new Vector2(data.Scale.x, data.Scale.y);
                                animTrackEntry.TimeScale = LevelController.Instance.levelConfig.speedGame;
                                //listSkeleton[index].transform.position = new Vector2(pos.x + 1, pos.y);
                                indexHit++;
                                Debug.Log("hit dame pos : " + hit.transform.position);
                            }
                          

                        }
                        

                    }
                    else if (data.particalEffect != null)
                    {
                        //GameObject anim;
                        //if (!isEnemy)
                        //{
                        //    anim = ObjectPool.Spawn(data.particalEffect);
                        //}
                        //else
                        //{
                        //    anim = ObjectPool.Spawn(data.particalEffect);
                        //}
                        //anim.transform.position = pos;

                        //if (type == EffectType.HIT_DAME_EFFECT)
                        //    anim.transform.position = new Vector3(pos.x, pos.y + 1.5f);
                        //if (anim.GetComponent<ParticleSystem>() != null)
                        //    anim.GetComponent<ParticleSystem>().Play();
                    }
                    //return;
                }
                index++;
            }
        }
        public static bool CheckIfAnimationExists(Spine.AnimationState animationState, string animationToUse)
        {
            if (animationState == null)
            {
                return false;
            }

            if (animationState.Data.SkeletonData.FindAnimation(animationToUse) != null)
            {
                return true;
            }
            return false;
        }
        public void StopZfx()
        {
            listSkeleton.ForEach(fx => fx.gameObject.SetActive(false));
        }
        public void SetEffects(List<EffectData> assets)
        {
            //listSkeleton = new List<SkeletonAnimation>();
            effectAsset = assets;
            for (int i = 0; i < assets.Count; i++)
            {
                
                if (assets[i].sketetonEffect != null)
                {
                    listSkeleton[i].skeletonDataAsset = assets[i].sketetonEffect;
                    listSkeleton[i].Initialize(true);
                    ObjectPool.CreatePool(listSkeleton[i].gameObject, 5);
                }
                if (assets[i].particalEffect != null)
                {
                    ObjectPool.CreatePool(listSkeleton[i].gameObject, 5);
                }
            }
        }
    }
}