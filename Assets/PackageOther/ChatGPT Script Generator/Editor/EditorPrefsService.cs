using UnityEditor;
using UnityEngine;

namespace ScriptGenerator {
internal static class EditorPrefsService {
    public static void SetClassName(string className) {
        EditorPrefs.SetString("ChatGptScriptGeneratorClassName", className);
    }

    public static string GetClassName() {
        return EditorPrefs.GetString("ChatGptScriptGeneratorClassName", "");
    }

    public static void SetPrompt(string prompt, GameObject targetGameObject) {
        var key = targetGameObject != null
            ? $"ChatGptScriptGeneratorPrompt-{targetGameObject.GetInstanceID()}"
            : "ChatGptScriptGeneratorPrompt-Generic";
        EditorPrefs.SetString(key, prompt);
    }

    public static string GetPrompt(GameObject targetGameObject) {
        var key = targetGameObject != null
            ? $"ChatGptScriptGeneratorPrompt-{targetGameObject.GetInstanceID()}"
            : "ChatGptScriptGeneratorPrompt-Generic";
        return EditorPrefs.GetString(key, "");
    }

    public static bool GetDeletePreviousFile() {
        return EditorPrefs.GetBool("ChatGptScriptGeneratorDeletePreviousFile", true);
    }

    public static void SetDeletePreviousFile(bool value) {
        EditorPrefs.SetBool("ChatGptScriptGeneratorDeletePreviousFile", value);
    }
}
}