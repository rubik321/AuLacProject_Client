using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NTPackage_old.Functions{
    public class NTLog
    {
        public static void LogMessage(string message, GameObject gameObject = null)
        {
            #if UNITY_EDITOR
            Debug.Log(message, gameObject);
            #endif
        }
        public static void LogWarning(string message, GameObject gameObject = null)
        {
            #if UNITY_EDITOR
            Debug.LogWarning(message, gameObject);
            #endif
        }
        public static void LogError(string message, GameObject gameObject = null)
        {
            // #if UNITY_EDITOR
            Debug.LogWarning(message, gameObject);
            // #endif
        }
    }
}
