using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NTPackage.Functions
{
    public class MissingScriptChecker : MonoBehaviour
    {
        [ContextMenu("Check Missing Scripts")]
        public void CheckMissingScripts()
        {
            Component[] allObjects = GetComponentsInChildren(typeof(Component), true);

            foreach (Component obj in allObjects)
            {
                Component[] components = obj.GetComponents<Component>();

                for (int i = 0; i < components.Length; i++)
                {
                    if (components[i] == null)
                    {
                        Debug.LogWarning($"Missing script found on GameObject: {obj.name}", obj);
                    }
                    if (components[i].GetType() == typeof(MonoBehaviour))
                    {
                        MonoBehaviour monoBehaviour = components[i] as MonoBehaviour;
                        if (monoBehaviour != null)
                        {
                            Debug.LogWarning($"Missing script found on GameObject: {obj.name}", obj);
                        }
                    }
                }
                Debug.Log(allObjects.Length + " Missing script check completed.");
            }
        }
    }
}