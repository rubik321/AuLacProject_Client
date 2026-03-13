using System.Collections;
using System.Collections.Generic;
using NTPackage;
using NTPackage.Functions;
using SimpleJSON;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace Rubik.SpineManager
{

    public class CharacterGearAnimationNameConfig{
        public const string DEFAULT_ANIMATION = "idle";
        public const string IDLE_ANIMATION = "idle";
        public const string JUMP_ANIMATION = "jump";
        public const string JUMP_X_ANIMATION = "jump_x";
        public const string RUN_ANIMATION = "run";
        public const string WALK_ANIMATION = "walk";
    }

    [System.Serializable]
    public class SpineNameData
    {
        public int Index;
        public string SpineName;
    }

    public class SpineController : NTBehaviour
    {
        public NTDictionary<int, SpineNameData> CharacterGearSpineName;
        public TextAsset CharacterGearSpineNameAsset;
        [SerializeField] private SkeletonGraphic CharacterGearGraphic;
        [SerializeField] private SkeletonAnimation CharacterGearAnimation;

        public static SpineController Instance;
        protected override void Awake()
        {
            base.Awake();
            if (SpineController.Instance != null)
            {
                NTLog.LogError("Only 1 Instance allow");
                return;
            }
            SpineController.Instance = this;
        }

        protected override void Start()
        {
            base.Start();
            this.LoadCharacterGearSpineName();
        }

        #region Function
        public void LoadCharacterGearSpineName()
        {
            JSONNode jsonNode = JSON.Parse(this.CharacterGearSpineNameAsset.text);
            foreach (JSONNode item in jsonNode)
            {
                SpineNameData spineNameData = JsonUtility.FromJson<SpineNameData>(item.ToString());
                this.CharacterGearSpineName.Add(spineNameData.Index, spineNameData);
            }
        }
        #endregion

        #region Getter
        public string GetCharacterGearSpineName(int index)
        {
           
            SpineNameData spineNameData = this.CharacterGearSpineName.Get(index);
            if (spineNameData != null)
            {
                return spineNameData.SpineName;
            }
            return "";
        }

        public SkeletonGraphic GetCharacterGearGraphic(List<int> indexList)
        {
            List<string> spineNameList = new List<string>();
            foreach (int index in indexList)
            {
                SpineNameData spineNameData = this.CharacterGearSpineName.Get(index);
                if (spineNameData != null)
                {
                    spineNameList.Add(spineNameData.SpineName);
                }
            }

            SkeletonGraphic skeletonGraphic = ObjectPoolingManager.Instance.PullObjectFromPooling<SkeletonGraphic>(ObjectPoolingConfig.CharacterGearSkeletonGraphic);
            if (skeletonGraphic == null)
            {
                skeletonGraphic = Instantiate(this.CharacterGearGraphic);
            }
            skeletonGraphic.gameObject.SetActive(true);
            NTFunction.ResetPosition(skeletonGraphic.transform);
            Skeleton skeleton = skeletonGraphic.Skeleton;
            Skin combinedSkin = new Skin("mixed-skin");
            foreach (string item in spineNameList)
            {
                Skin skinCustom = skeleton.Data.FindSkin(item);
                if (skinCustom != null)
                {
                    Debug.Log("cloth found: " + skinCustom.Name);
                    combinedSkin.AddSkin(skinCustom);
                }
                else
                {
                    NTLog.LogError("cloth not found: " + item);
                }
            }
            skeleton.SetSkin(combinedSkin);
            skeleton.SetToSetupPose();
            skeletonGraphic.AnimationState.Apply(skeleton);
            skeletonGraphic.AnimationState.SetAnimation(0, CharacterGearAnimationNameConfig.DEFAULT_ANIMATION, true);
            skeletonGraphic.transform.name = ObjectPoolingConfig.CharacterGearSkeletonGraphic;
            return skeletonGraphic;
        }

        public SkeletonAnimation GetCharacterGearModel(List<int> indexList)
        {
            List<string> spineNameList = new List<string>();
            foreach (int index in indexList)
            {
                SpineNameData spineNameData = this.CharacterGearSpineName.Get(index);
                if (spineNameData != null)
                {
                    spineNameList.Add(spineNameData.SpineName);
                }
            }

            SkeletonAnimation skeletonAnimation = ObjectPoolingManager.Instance.PullObjectFromPooling<SkeletonAnimation>(ObjectPoolingConfig.CharacterClothSkeletonAnimation);
            if (skeletonAnimation == null)
            {
                skeletonAnimation = Instantiate(this.CharacterGearAnimation);
            }
            skeletonAnimation.gameObject.SetActive(true);
            NTFunction.ResetPosition(skeletonAnimation.transform);
            Skeleton skeleton = skeletonAnimation.skeleton;
            Skin skin = new Skin("custom");
            foreach (string item in spineNameList)
            {
                Skin skinCustom = skeleton.Data.FindSkin(item);
                if (skinCustom != null)
                {
                    Debug.Log(item);
                    skin.AddSkin(skinCustom);
                }
                else
                {
                    NTLog.LogError("cloth not found: " + item);
                }
            }
            skeleton.SetSkin(skin);
            skeleton.SetToSetupPose();
            skeletonAnimation.AnimationState.Apply(skeleton);
            skeletonAnimation.AnimationState.SetAnimation(0, CharacterGearAnimationNameConfig.DEFAULT_ANIMATION, true);
            skeletonAnimation.transform.name = ObjectPoolingConfig.CharacterGearSkeletonAnimation;
            return skeletonAnimation;
        }


        #endregion
    }
}