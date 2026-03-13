using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ScriptGenerator {
internal sealed class PromptHistoryWindow : EditorWindow {
    private Vector2 _scrollPosition;
    private readonly HashSet<string> _expandedFoldouts = new HashSet<string>();

    public static void ShowWindow() {
        var window = GetWindow<PromptHistoryWindow>(true, "ChatGPT Script Generator History");
        window.Initialize();
        window.Show();
    }

    private void Initialize() { }

    private void OnGUI() {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.Space();
        EditorGUILayout.BeginVertical();

        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, false, false, GUILayout.ExpandWidth(true),
                                                          GUILayout.ExpandHeight(true));

        var prompts = PromptHistory.Get();
        var i = 0;
        foreach (var prompt in prompts) {
            ++i;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"{i}", EditorStyles.boldLabel, GUILayout.Width(100));
            EditorGUILayout.LabelField(prompt.date, EditorStyles.miniLabel);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("GameObject", GUILayout.Width(100));
            EditorGUILayout.LabelField(string.IsNullOrEmpty(prompt.gameObjectName)
                                           ? "N/A"
                                           : $"{prompt.gameObjectName}");
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Scene", GUILayout.Width(100));
            EditorGUILayout.LabelField(string.IsNullOrEmpty(prompt.sceneName) ? "N/A" : $"{prompt.sceneName}");
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Guiding Prompts", GUILayout.Width(100));
            EditorGUILayout.BeginVertical();
            var wasExpanded = _expandedFoldouts.Contains(prompt.date);
            var isExpanded =
                EditorGUILayout.Foldout(wasExpanded, $"{prompt.guidingPrompts.Length} guiding prompts", true);
            if (isExpanded) {
                _expandedFoldouts.Add(prompt.date);

                foreach (var guidingPrompt in prompt.guidingPrompts) {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("", GUILayout.Width(18));
                    EditorGUILayout.SelectableLabel(guidingPrompt,
                                                    new GUIStyle(EditorStyles.textField) { wordWrap = true },
                                                    GUILayout.ExpandWidth(true),
                                                    GUILayout.Height(EditorGUIUtility.singleLineHeight));
                    if (GUILayout.Button("Copy", EditorStyles.miniButton, GUILayout.Width(50))) {
                        EditorGUIUtility.systemCopyBuffer = guidingPrompt;
                    }

                    EditorGUILayout.EndHorizontal();
                }
            } else {
                if (wasExpanded) {
                    _expandedFoldouts.Remove(prompt.date);
                }
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Prompt", GUILayout.Width(100));
            EditorGUILayout.SelectableLabel(prompt.text, new GUIStyle(EditorStyles.textField) { wordWrap = true },
                                            GUILayout.Height(60), GUILayout.ExpandWidth(true));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Copy", EditorStyles.miniButton, GUILayout.Width(50))) {
                EditorGUIUtility.systemCopyBuffer = prompt.text;
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }

        // If there is no history, show a message.
        if (prompts.Length == 0) {
            EditorGUILayout.HelpBox("No history yet. Generate some scripts to see them here.", MessageType.Info);
        }

        // If there is history, show a button to clear it.
        if (prompts.Length > 0) {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Clear History")) {
                PromptHistory.Clear();
            }

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();

        EditorGUILayout.EndVertical();
        EditorGUILayout.Space();
        EditorGUILayout.EndHorizontal();
    }
}
}