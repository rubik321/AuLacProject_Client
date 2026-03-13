using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NTPackage.UI;

namespace NTPackage.Functions
{
#if UNITY_EDITOR
    // [CustomEditor(typeof(PopupUI), true)]
    // public class PopupUIEditor : Editor
    // {
    //     // private PopupUI popupUI;

    //     // SerializedProperty showProp;
    //     // SerializedProperty hideProp;
    //     // SerializedProperty transMoveProp;
    //     // SerializedProperty showPosProp;
    //     // SerializedProperty durationMoveShowProp;
    //     // SerializedProperty hidePosProp;
    //     // SerializedProperty durationMoveHideProp;

    //     // public override void OnInspectorGUI()
    //     // {
    //     //     base.OnInspectorGUI();
    //     //     popupUI = (PopupUI)target;

    //     //     if (GUILayout.Button("Show"))
    //     //     {
    //     //         this.popupUI.ShowNone();
    //     //     }
    //     //     if (GUILayout.Button("Hide"))
    //     //     {
    //     //         this.popupUI.HideNone();
    //     //     }
    //     //     if (GUILayout.Button("LoadComponents"))
    //     //     {
    //     //         this.popupUI.LoadComponents();
    //     //     }

    //     //     serializedObject.Update();

    //     //     showProp = serializedObject.FindProperty("show");
    //     //     hideProp = serializedObject.FindProperty("hide");
    //     //     transMoveProp = serializedObject.FindProperty("TransMove");
    //     //     showPosProp = serializedObject.FindProperty("ShowPos");
    //     //     durationMoveShowProp = serializedObject.FindProperty("DurationMoveShow");
    //     //     hidePosProp = serializedObject.FindProperty("HidePos");
    //     //     durationMoveHideProp = serializedObject.FindProperty("DurationMoveHide");

    //     //     bool showMoveFields = (KindPopup)showProp.enumValueIndex == KindPopup.moveTo ||
    //     //                           (KindPopup)hideProp.enumValueIndex == KindPopup.moveTo;

    //     //     if (showMoveFields)
    //     //     {
    //     //         EditorGUILayout.PropertyField(transMoveProp);
    //     //     }

    //     //     if ((KindPopup)showProp.enumValueIndex == KindPopup.moveTo)
    //     //     {
    //     //         EditorGUILayout.PropertyField(showPosProp);
    //     //         EditorGUILayout.PropertyField(durationMoveShowProp);
    //     //     }

    //     //     if ((KindPopup)hideProp.enumValueIndex == KindPopup.moveTo)
    //     //     {
    //     //         EditorGUILayout.PropertyField(hidePosProp);
    //     //         EditorGUILayout.PropertyField(durationMoveHideProp);
    //     //     }

    //     //     serializedObject.ApplyModifiedProperties();
    //     // }
    // };
#endif
}