using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Collections.Generic;

namespace NTPackage.Functions
{
    [CustomEditor(typeof(NTBehaviour), true)]
    public class NTBehaviourEditor : Editor
    {

        private Dictionary<string, bool> showIfCache = new();
        public override void OnInspectorGUI()
        {
            // DrawDefaultInspector();
            SerializedProperty property = serializedObject.GetIterator();
        bool expanded = true;
        property.NextVisible(expanded); // Skip script field
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.PropertyField(property, true);
        EditorGUI.EndDisabledGroup();


        while (property.NextVisible(expanded))
        {
            bool shouldShow = true;

            var field = target.GetType().GetField(property.name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (field != null)
            {
                var attr = field.GetCustomAttribute<NTShowIfAttribute>();
                if (attr != null)
                {
                    SerializedProperty condition = serializedObject.FindProperty(attr.ConditionFieldNameBool);
                    if (condition != null && condition.propertyType == SerializedPropertyType.Boolean)
                    {
                        shouldShow = condition.boolValue == attr.ExpectedValueBool;
                    }else{
                        SerializedProperty conditionInt = serializedObject.FindProperty(attr.ConditionFieldNameInt);
                        if (conditionInt != null && (conditionInt.propertyType == SerializedPropertyType.Integer || conditionInt.propertyType == SerializedPropertyType.Enum))
                        {
                            shouldShow = conditionInt.intValue == attr.ExpectedValueInt;
                        }
                    }
                }
            }

            if (shouldShow)
            {
                EditorGUILayout.PropertyField(property, true);
            }

            expanded = false;
        }

        serializedObject.ApplyModifiedProperties();

        var targetObject = target;

            var methods = target.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var method in methods)
            {
                var attributes = method.GetCustomAttributes(typeof(NTButtonAttribute), true);
                if (attributes.Length > 0)
                {
                    if (GUILayout.Button(method.Name))
                    {
                        method.Invoke(targetObject, null);
                    }
                }
            }
        }

        
    }
}
