using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace NTPackage.Functions{
    #if UNITY_EDITOR
    [CustomEditor(typeof(AlignFit), true)]
    public class AlignFitEditor : Editor
    {
        private AlignFit alignFit;
        public override void OnInspectorGUI() 
        {
            base.OnInspectorGUI();
            alignFit = (AlignFit) target;

            if(GUILayout.Button("Fit")){
                this.alignFit.Fit();
            }
            if(GUILayout.Button("FitSelf")){
                this.alignFit.FitSelf();
            }
            if(GUILayout.Button("FitChilds")){
                this.alignFit.FitChilds();
            }
            if(GUILayout.Button("FitParent")){
                this.alignFit.FitParent();
            }
            if(GUILayout.Button("LoadComponents")){
                this.alignFit.LoadComponents();
            }
        }
    }
    #endif
}
