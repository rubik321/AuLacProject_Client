using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NTPackage.UI;

namespace Rubik.Friend
{
    public class BtnOnFriendUI : MonoBehaviour
    {
        public void _Onclick(){
            PopupManager.Instance.OnUI(PopupCode.FriendUI);
        }
    }
}