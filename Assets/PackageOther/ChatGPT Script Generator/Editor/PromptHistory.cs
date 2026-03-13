using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ScriptGenerator {
[Serializable]
internal struct History {
    public Prompt[] prompts;
}

[Serializable]
internal struct Prompt {
    public string text;
    public string[] guidingPrompts;
    public string gameObjectId;
    public string gameObjectName;
    public string sceneName;
    public string date;
}

internal static class PromptHistory {
    public static void Add(string fullPrompt, string corePrompt, GameObject targetGameObject) {
        var history = GetHistory();
        var prompt = new Prompt {
            text = corePrompt,
            guidingPrompts = Settings.instance.GetEnabledGuidingPrompts().ToArray(),
            gameObjectId = targetGameObject != null ? targetGameObject.GetInstanceID().ToString() : null,
            gameObjectName = targetGameObject != null ? targetGameObject.name : null,
            sceneName = targetGameObject != null ? targetGameObject.scene.name : null,
            date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };
        history.prompts = history.prompts.Prepend(prompt).ToArray();
        SetHistory(history);
    }

    public static Prompt[] Get() {
        return GetHistory().prompts;
    }

    private static History GetHistory() {
        var json = EditorPrefs.GetString("ChatGptScriptGeneratorHistory", "");
        var result = string.IsNullOrEmpty(json) ? new History() : JsonUtility.FromJson<History>(json);
        if (result.prompts == null) {
            result.prompts = Array.Empty<Prompt>();
        }

        return result;
    }

    private static void SetHistory(History history) {
        var json = JsonUtility.ToJson(history);
        EditorPrefs.SetString("ChatGptScriptGeneratorHistory", json);
    }

    public static void Clear() {
        EditorPrefs.DeleteKey("ChatGptScriptGeneratorHistory");
    }
}
}