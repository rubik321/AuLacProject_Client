using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Spine.Unity;
namespace Rubik.Battle
{
    public class ZfxGameplayItem : MonoBehaviour
    {
        [SerializeField] ParticleSystem partical;
        [SerializeField] SkeletonAnimation skeleton;
        Coroutine play;
        public void Begin()
        {
            
        }
        void SetZfxSpine(SkeletonDataAsset _skeletonData)
        {
            skeleton.skeletonDataAsset = _skeletonData;
            skeleton.Initialize(true);

        }
        public void Play(Vector2 _position,  float _loop)
        {
            if(play != null)
            {
                StopCoroutine(play);
            }
            transform.position = _position;
            play = StartCoroutine(Play());
        }
        IEnumerator Play()
        {
            while (partical.isPlaying)
            {
                yield return new WaitForSeconds(Time.deltaTime);
            }
            Disable();
        }
        void MoveToPos(Vector3 from, Vector3 to, float v, System.Action evtComplete = null)
        {
            transform.position = from;
            float t = Vector3.Distance(to, from) / v;
            transform.DOMove(to, t).OnComplete(()=> {
                if(evtComplete != null)
                {
                    evtComplete();
                }
            });
        }
        private void Disable()
        {
            
        }

        private void ResetZfx()
        {
            this.StopAllCoroutines();
        }
    }
}