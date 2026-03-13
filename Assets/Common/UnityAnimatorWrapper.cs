using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace Puto.Common
{
    [RequireComponent(typeof(Animator))]
    public class UnityAnimatorWrapper : MonoBehaviour
    {
        private string animationName;
        public string AnimationName
        {
            get => animationName;
            set => SetAnimation(value);
        }

        private float position;
        public float Position
        {
            set => SetPosition(value);
            get => position;
        }
        
        public float Speed
        {
            get => animator.speed;
            set => animator.speed = value;
        }
        
        public Animator animator => GetComponent<Animator>();
        
        // Start is called before the first frame update
        void Start()
        {

        }

        public void Play()
        {
            if (!string.IsNullOrEmpty(AnimationName))
                animator.speed = 1;
        }
        
        public void Pause()
        {
            if (!string.IsNullOrEmpty(AnimationName))
                animator.speed = 0;
        }
        
        public void Play(string animName)
        {
            animationName = animName;
            animator.Play(animName);
            animator.speed = 1;
        }

        public void Play(string animName, float positionAnim)
        {
            animationName = animName;
            this.position = positionAnim;
            animator.Play(animName, -1, this.position);
            animator.speed = 1;
        }

        private void SetAnimation(string animName)
        {
            animationName = animName;
            SetPosition(0);
        }
        
        private void SetPosition(float v)
        {
            position = v;
            animator.Play(animationName, -1, position);
            animator.speed = 0;
        }
    }

    public static class UnityAnimationExtension
    {
        public static TweenerCore<float, float, FloatOptions> DOPosition(this UnityAnimatorWrapper target, float endValue, float duration)
        {
            if (endValue < 0) endValue = 0;
            else if (endValue > 1) endValue = 1;
            TweenerCore<float, float, FloatOptions> t = DOTween.To(() => target.Position, x => target.Position = x, endValue, duration);
            t.SetTarget(target);
            return t;
        }
        
        public static TweenerCore<float, float, FloatOptions> DOSpeed(this UnityAnimatorWrapper target, float endValue, float duration)
        {
            if (endValue < 0) endValue = 0;

            TweenerCore<float, float, FloatOptions> t = DOTween.To(() => target.Speed, x => target.Speed = x, endValue, duration);
            t.SetTarget(target);
            return t;
        }
    }

}
