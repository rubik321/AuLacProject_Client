using System.Collections;
using System.Collections.Generic;
using NTPackage.Functions;
using Rubik.CharacterPlayer;
using Rubik.Myrk.BattleTeam;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace Rubik.CharacterCloth
{
    public class CharacterClothResourceConfig{
        public const string DEFAULT_ANIMATION = "idle";
        public const string IDLE_ANIMATION = "idle";
        public const string JUMP_ANIMATION = "jump";
        public const string JUMP_X_ANIMATION = "jump_x";
        public const string RUN_ANIMATION = "run";
        public const string WALK_ANIMATION = "walk";
    }

    public class CharacterClothResource : NTBehaviour
    {
        [SerializeField] private SkeletonGraphic CharacterClothGraphic;
        [SerializeField] private SkeletonAnimation CharacterClothAnimation;

        public static CharacterClothResource Instance;
        protected override void Awake()
        {
            base.Awake();
            if (CharacterClothResource.Instance != null){
               NTLog.LogError("Only 1 Instance allow");
               return;
             }
            CharacterClothResource.Instance = this;
        }

        #region Getter

        public SkeletonAnimation GetCharacterClothModel(string[] spineNames)
        {
            SkeletonAnimation skeletonAnimation = ObjectPoolingManager.Instance.PullObjectFromPooling<SkeletonAnimation>(ObjectPoolingConfig.CharacterClothSkeletonAnimation);
            if(skeletonAnimation == null){
                skeletonAnimation = Instantiate(this.CharacterClothAnimation);
            }
            skeletonAnimation.gameObject.SetActive(true);
            NTFunction.ResetPosition(skeletonAnimation.transform);
            Skeleton skeleton = skeletonAnimation.skeleton;
            Skin skin = new Skin("custom");
            foreach (string item in spineNames)
            {
                Skin skinCustom = skeleton.Data.FindSkin(item);
                if(skinCustom != null){
                    Debug.Log(item);
                    skin.AddSkin(skinCustom);
                }else{
                    NTLog.LogError("cloth not found: " + item);
                }
            }
            skeleton.SetSkin(skin);
            skeleton.SetToSetupPose();
            skeletonAnimation.AnimationState.Apply(skeleton);
            skeletonAnimation.AnimationState.SetAnimation(0, CharacterClothResourceConfig.DEFAULT_ANIMATION, true);
            skeletonAnimation.transform.name = ObjectPoolingConfig.CharacterClothSkeletonAnimation;
            return skeletonAnimation;
        }

        public SkeletonGraphic GetCharacterClothUI(string[] spineNames){
            SkeletonGraphic skeletonGraphic = ObjectPoolingManager.Instance.PullObjectFromPooling<SkeletonGraphic>(ObjectPoolingConfig.CharacterClothSkeletonGraphic);
            if(skeletonGraphic == null){
                skeletonGraphic = Instantiate(this.CharacterClothGraphic);
            }
            skeletonGraphic.gameObject.SetActive(true);
            NTFunction.ResetPosition(skeletonGraphic.transform);
            Skeleton skeleton = skeletonGraphic.Skeleton;
            Skin combinedSkin = new Skin("mixed-skin");
            foreach (string item in spineNames)
            {
                Skin skinCustom = skeleton.Data.FindSkin(item);
                if(skinCustom != null){
                    Debug.Log("cloth found: " + skinCustom.Name);
                    combinedSkin.AddSkin(skinCustom);
                }else{
                    NTLog.LogError("cloth not found: " + item);
                }
            }
            skeleton.SetSkin(combinedSkin);
            skeleton.SetToSetupPose();
            skeletonGraphic.AnimationState.Apply(skeleton);
            skeletonGraphic.AnimationState.SetAnimation(0, CharacterClothResourceConfig.DEFAULT_ANIMATION, true);
            skeletonGraphic.transform.name = ObjectPoolingConfig.CharacterClothSkeletonGraphic;
            return skeletonGraphic;
        }
        #endregion
    }
}
