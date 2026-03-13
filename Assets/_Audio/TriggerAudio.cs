using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using NTPackage_old.EventDispatcher;
using UnityEngine;

namespace Rubik.Common.AudioHelper{
    public class TriggerAudio : MonoBehaviour
    {
        public AudioSource audioSource;
        public string NameSound;

        public bool IsOff = false;

        private void Start() {
            this.IsOff = false;
            EventListenerManager.instance.Register(EventCode.OffAttackSound,"TriggerAudio"+transform.parent.name,(data)=>{
                StartCoroutine(this.OffSound());
            });
        }

        IEnumerator OffSound(){
            this.IsOff = true;
            yield return new WaitForSeconds(2);
            this.IsOff = false;
        }

        public void PlaySoundWithName(string Name){
            try
            {
                AudioCtrl.Instance.Play(this.NameSound); 
            }
            catch (System.Exception){}
        }
        public void PlaySound(){
            if(this.IsOff) return;
            try
            {
                AudioCtrl.Instance.Play(this.NameSound); 
            }
            catch (System.Exception){}
            try
            {
                audioSource.Play();
            }
            catch (System.Exception){}
        }
    }
}
