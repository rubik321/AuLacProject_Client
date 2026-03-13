using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NTFunctions_old;
using TMPro;


namespace GOA.UIFriends{
    public class NoticAddFriendUI : PopupUI
    {
        public float timeShow = 1f;

        //public override void LoadComponents()
        //{
        //    base.LoadComponents();
        //    this.lvUI = new PopupLv().GetValue(transform.name);
        //}

        public void OnUI(){
            this.Show();
            StartCoroutine(this.AutoHide());
        }

        IEnumerator AutoHide(){
            yield return new WaitForSeconds(this.timeShow);
            this.OffUI();
        }
    }
}
