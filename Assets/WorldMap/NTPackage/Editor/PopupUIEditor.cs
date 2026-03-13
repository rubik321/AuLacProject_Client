using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace NTFunctions_old
{
#if UNITY_EDITOR
    [CustomEditor(typeof(PopupUI), true)]
    public class PopupUIEditor : Editor
    {
        // private PopupUI popupUI;
        // public override void OnInspectorGUI()
        // {
        //     base.OnInspectorGUI();
        //     popupUI = (PopupUI)target;

        //     if (GUILayout.Button("Show"))
        //     {
        //         this.popupUI.ShowNone();
        //     }
        //     if (GUILayout.Button("Hide"))
        //     {
        //         this.popupUI.HideNone();
        //     }
        //     if (GUILayout.Button("LoadComponents"))
        //     {
        //         this.popupUI.LoadComponents();
        //     }
        // }

        public override void OnInspectorGUI()
        {
            // Draw default inspector fields
            DrawDefaultInspector();

            // Add a button
            if (GUILayout.Button("Click Me"))
            {
                Debug.Log("Button Clicked!");
            }
        }
    };
#endif
}