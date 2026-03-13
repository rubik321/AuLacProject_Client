using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

namespace NTPackage.Functions{
    public class NTLog
    {
        public static void LogMessage(string message, GameObject gameObject = null)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.Log("<color=green>" + "Message: </color>" + message, gameObject);
#endif
        }
        public static void LogWarning(string message, GameObject gameObject = null)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.LogWarning("<color=yellow>" + "Warning: </color>" + message, gameObject);
#endif
        }
        public static void LogError(string message, GameObject gameObject = null)
        {
            // #if UNITY_EDITOR
                UnityEngine.Debug.LogError("<color=red>"+ "Error: </color>" + message, gameObject);
            // #endif
        }

        private static string GetLogPrefix()
        {
            // Skip 2 frames: current method + LogMessage() itself
            var stackTrace = new StackTrace(2, true);
            var frame = stackTrace.GetFrame(0);
            var method = frame.GetMethod();
            string className = method.DeclaringType?.Name ?? "UnknownClass";
            string methodName = method.Name;
            string fileName = frame.GetFileName();
            int line = frame.GetFileLineNumber();

            return $"[{className}.{methodName}({fileName}:{line})]";
        }
    }
}
