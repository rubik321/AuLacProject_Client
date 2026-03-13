using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace NTFunctions_old{
    public abstract class BaseButton : LoadBehaviour
    {
        public bool DoTwice = false;
        Button button;

        public override void LoadComponents()
        {
            base.LoadComponents();
            this.LoadButtonAction();
        }

        protected void LoadButtonAction(){
            if(this.button == null) this.button = transform.GetComponent<Button>();
            this.button.onClick.RemoveAllListeners();
            this.button.onClick.AddListener(this.OnClick);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            if(this.DoTwice){
                transform.DOComplete();
                transform.localScale = Vector3.zero;
                transform.DOScale(new Vector3(1.1f,1.1f,1.1f), 0.2f).OnComplete(()=>{
                transform.DOScale(new Vector3(1f,1f,1f), 0.1f);
            });
            }
            
        }

        protected virtual void OnClick(){
            this.OnSound();
        }

        protected virtual void OnSound(){
            Rubik.Common.AudioHelper.AudioCtrl.Instance.Play(Rubik.Common.AudioHelper.AudioName.UI_Button_Default);
        }
    }
}