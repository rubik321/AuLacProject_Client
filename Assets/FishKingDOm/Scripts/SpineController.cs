using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using System;
using static Spine.AnimationState;

public class SpineController : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;
    public TrackEntry animTrackEntry;
    public virtual void SetSkeletonData(SkeletonDataAsset skeletonDataAsset)
    {
        if(skeletonAnimation != null && skeletonDataAsset != null )
        skeletonAnimation.skeletonDataAsset = skeletonDataAsset;
        skeletonAnimation.Initialize(true);
    }
    //public void SetAnimationState(int trackIndex, string animationName, bool loop = false)
    //{
    //    //skeletonAnimation.timeScale = LevelController.Instance.levelConfig.speedGame;
    //    skeletonAnimation.AnimationState.SetAnimation(trackIndex, animationName, loop ).TimeScale = LevelController.Instance.levelConfig.speedGame;
        
    //}
    public void SetAnimationState(int trackIndex, string animationName, bool loop = false)
    {
        //skeletonAnimation.timeScale = LevelController.Instance.levelConfig.speedGame;
        animTrackEntry =  skeletonAnimation.AnimationState.SetAnimation(trackIndex, animationName, loop);
        animTrackEntry.TimeScale = LevelController.Instance.levelConfig.speedGame;
       // return trackEntry;
    }
    public virtual void Begin()
    {
        
    }

    public static bool CheckIfAnimationExists( Spine.AnimationState animationState, string animationToUse)
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

    public static bool CheckIfAnimationExists(Spine.AnimationState animationState, Spine.Animation animationToUse)
    {
        if (animationState == null)
        {
            return false;
        }

        if (animationState.Data.SkeletonData.FindAnimation(animationToUse.Name) != null)
        {
            return true;
        }
        return false;
    }

    public static bool CheckIfSkinExists( Spine.AnimationState animationState, string skinToUse)
    {
        if (animationState == null)
        {
            return false;
        }

        if (animationState.Data.SkeletonData.FindSkin(skinToUse) != null)
        {
            return true;
        }
        return false;
    }

    public static bool CheckIfSkinExists(Spine.AnimationState animationState, Spine.Skin skin)
    {
        if (animationState == null)
        {
            return false;
        }

        if (animationState.Data.SkeletonData.FindSkin(skin.Name) != null)
        {
            return true;
        }
        return false;
    }
    public static String GetAnimationExists(Spine.AnimationState animationState, List<string> animNames)
    {
        if (animationState == null)
        {
            return null;
        }
        foreach(string anim in animNames)
        {
            
            if (animationState.Data.SkeletonData.FindAnimation(anim) != null)
            {
               
                return anim;
            }
        }
        
        return null;
    }
}
