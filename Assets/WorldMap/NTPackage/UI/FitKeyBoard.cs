using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NTFunctions_old{
    public class FitKeyBoard : LoadBehaviour
    {
        protected override void OnEnable() {
            // Debug.LogWarning(GetKeyboardSize());
        }

        public int GetKeyboardSize()
        {
            using(AndroidJavaClass UnityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                AndroidJavaObject View = UnityClass.GetStatic<AndroidJavaObject>("currentActivity").Get<AndroidJavaObject>("mUnityPlayer").Call<AndroidJavaObject>("getView");

                using(AndroidJavaObject Rct = new AndroidJavaObject("android.graphics.Rect"))
                {
                    View.Call("getWindowVisibleDisplayFrame", Rct);

                    return Screen.height - Rct.Call<int>("height");
                }
            }
        }
    }
}