using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rubik.Common.AudioHelper{
    public class AudioStart : MonoBehaviour
    {
        public AudioSource audioSource;
        public string NameSound;

        public float TimeStart = 0f;

        void OnEnable()
        {
            StartCoroutine(this.PlaySound());
        }

        IEnumerator PlaySound(){
            yield return new WaitForSeconds(this.TimeStart);
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