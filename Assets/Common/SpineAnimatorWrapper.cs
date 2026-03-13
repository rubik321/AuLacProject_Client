//using DG.Tweening;
//using DG.Tweening.Core;
//using DG.Tweening.Plugins.Options;
//using Spine;
//using Spine.Unity;
//using UnityEngine;
//using Animation = Spine.Animation;


//namespace Rubik.Common
//{
//    public class SpineAnimatorWrapper : MonoBehaviour
//    {
//        private TrackEntry currentTrack;
        
//        private string animationName;
//        public string AnimationName
//        {
//            get => animationName;
//            set => SetAnimation(value);
//        }

//        private float duration;
        
//        private float position;
//        public float Position
//        {
//            set => SetPosition(value);
//            get => position;
//        }

//        public float Speed
//        {
//            get => skeletonAnimation.AnimationState.TimeScale;
//            set => skeletonAnimation.AnimationState.TimeScale = value;
//        }
        
//        private SkeletonAnimation skeletonAnimation => GetComponent<SkeletonAnimation>();

//        public void Play()
//        {
//            if (!string.IsNullOrEmpty(AnimationName))
//                Speed = 1;
//        }
        
//        public void Pause()
//        {
//            if (!string.IsNullOrEmpty(AnimationName))
//                Speed = 0;
//        }
        
//        public void Play(string animName)
//        {
//            animationName = animName;
//            currentTrack = skeletonAnimation.AnimationState.SetAnimation(0, animName, false);
//            Speed = 1;
//        }

//        public void Play(string animName, float positionAnim)
//        {
//            animationName = animName;
//            position = positionAnim;
            
//            currentTrack = skeletonAnimation.AnimationState.SetAnimation(0, animName, false);
//            Speed = 1;
//        }

//        private void SetAnimation(string animName)
//        {
//            animationName = animName;
//            currentTrack = skeletonAnimation.AnimationState.SetAnimation(0, animationName, false);
            
//            Animation myAnimation = skeletonAnimation.Skeleton.Data.FindAnimation(animationName);
//            duration = myAnimation.Duration;

//            SetPosition(0);
//        }
        
//        private void SetPosition(float v)
//        {
//            position = v * duration;
//            Debug.Log(position);
//            currentTrack.TrackTime = position;
//            Speed = 0;
//        }
//    }
    
//    public static class SpineAnimationExtension
//    {
//        public static TweenerCore<float, float, FloatOptions> DOPosition(this SpineAnimatorWrapper target, float endValue, float duration)
//        {
//            if (endValue < 0) endValue = 0;
//            else if (endValue > 1) endValue = 1;
//            TweenerCore<float, float, FloatOptions> t = DOTween.To(() => target.Position, x => target.Position = x, endValue, duration);
//            t.SetTarget(target);
//            return t;
//        }
        
//        public static TweenerCore<float, float, FloatOptions> DOSpeed(this SpineAnimatorWrapper target, float endValue, float duration)
//        {
//            if (endValue < 0) endValue = 0;

//            TweenerCore<float, float, FloatOptions> t = DOTween.To(() => target.Speed, x => target.Speed = x, endValue, duration);
//            t.SetTarget(target);
//            return t;
//        }
//    }
    
//}
