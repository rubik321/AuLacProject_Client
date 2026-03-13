using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NTFunctions_old;

namespace GOA.UIProfile{
    public class FriendRequestUI : PopupUI
    {
        public float timeShow = 1f;

        public override void LoadComponents()
        {
            base.LoadComponents();
            this.lvUI = new PopupLv().GetValue(transform.name);
        }

        public void OnUI(){
            if(!this.CanShow()) return;
            this.Show();
            StartCoroutine(this.AutoHide());
        }

        IEnumerator AutoHide(){
            yield return new WaitForSeconds(this.timeShow);
            this.OffUI();
        }
    }
}