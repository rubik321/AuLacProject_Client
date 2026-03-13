using UnityEngine;
using UnityEditor;
using JetBrains.Annotations;

namespace ScriptGenerator {
internal sealed class PromptWindow : EditorWindow {
    private const float AdvancedOptionsHeight = 44;

    private string _prompt = "";
    private bool _deletePreviousFile;
    private bool _advancedOptionsExpanded;

    [CanBeNull] private GameObject _targetGameObject;
    [CanBeNull] private DefaultAsset _targetFolder;
    [CanBeNull] private MonoScript _targetScript;

    public static PromptWindow ShowWindow(GameObject targetGameObject, DefaultAsset targetFolder,
                                          MonoScript targetScript, string title) {
        var window = GetWindow<PromptWindow>(true, title);
        window.Initialize(targetGameObject, targetFolder, targetScript);
        window.position = new Rect(Screen.currentResolution.width / 2f - window.position.width / 2f,
                                   Screen.currentResolution.height / 2f - window.position.height / 2f,
                                   window.position.width, 140);
        window.Show();
        return window;
    }

    private void Initialize([CanBeNull] GameObject targetGameObject, [CanBeNull] DefaultAsset targetFolder,
                            [CanBeNull] MonoScript targetScript) {
        _targetGameObject = targetGameObject;
        _targetFolder = targetFolder;
        _targetScript = targetScript;
        _deletePreviousFile = EditorPrefsService.GetDeletePreviousFile();
    }

    private bool IsApiKeyOk => !string.IsNullOrEmpty(Settings.instance.apiKey);

    private void OnGUI() {
        if (!IsApiKeyOk) {
            EditorGUILayout.HelpBox("API Key hasn't been set. Please check the project settings.", MessageType.Error);
            if (GUILayout.Button("Open Project Settings")) {
                SettingsService.OpenProjectSettings("Project/ChatGPT Script Generator");
                Close();
            }

            return;
        }

        if (string.IsNullOrEmpty(_prompt)) {
            _prompt = EditorPrefsService.GetPrompt(_targetGameObject);
        }

        _prompt = EditorGUILayout.TextArea(_prompt, new GUIStyle(EditorStyles.textArea) { wordWrap = true },
                                           GUILayout.ExpandHeight(true));

        var previousClassName = EditorPrefsService.GetClassName();

        // Delete previous file
        {
            var deletePreviousFileContent = new GUIContent($"Overwrite {previousClassName}",
                                                           $"Enable this option to overwrite the previous script named " +
                                                           $"{previousClassName}.");
            _deletePreviousFile =
                !(string.IsNullOrEmpty(previousClassName) || !Selection.activeGameObject ||
                  !Selection.activeGameObject.GetComponent(previousClassName)) &&
                EditorGUILayout.Toggle(deletePreviousFileContent, _deletePreviousFile);
            EditorPrefsService.SetDeletePreviousFile(_deletePreviousFile);
        }

        // Advanced options
        var temperature = Settings.instance.temperature;
        {
            var advancedOptionsContent = new GUIContent("Advanced Options", "Overriding the default settings.");
            var expanded = EditorGUILayout.Foldout(_advancedOptionsExpanded, advancedOptionsContent);
            if (expanded) {
                EditorGUI.indentLevel++;
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                {
                    // Temperature
                    {
                        const string tooltip = "The higher the temperature, the more surprising the results. " +
                                               "The lower the temperature, the more repetitive the results.";
                        var temperatureContent = new GUIContent("Temperature", tooltip);
                        temperature = EditorGUILayout.Slider(temperatureContent, Settings.instance.temperature, 0f, 1f);
                    }
                }
                EditorGUILayout.EndVertical();
                EditorGUI.indentLevel--;

                if (!_advancedOptionsExpanded) {
                    position = new Rect(position.x, position.y - AdvancedOptionsHeight, position.width,
                                        position.height + AdvancedOptionsHeight);
                }

                // Buttons
                {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Settings", EditorStyles.miniButton)) {
                        SettingsService.OpenProjectSettings("Project/ChatGPT Script Generator");
                    }

                    if (GUILayout.Button("History", EditorStyles.miniButton)) {
                        PromptHistoryWindow.ShowWindow();
                    }

                    EditorGUILayout.EndHorizontal();
                }
            } else {
                if (_advancedOptionsExpanded) {
                    position = new Rect(position.x, position.y + AdvancedOptionsHeight, position.width,
                                        position.height - AdvancedOptionsHeight);
                }
            }

            _advancedOptionsExpanded = expanded;
        }

        // Info
        {
            if (_targetFolder != null) {
                EditorGUILayout.LabelField($"Adding to folder: {_targetFolder.name}", EditorStyles.miniLabel);
            }

            if (_targetScript != null) {
                EditorGUILayout.LabelField($"Editing script: {_targetScript.name}", EditorStyles.miniLabel);
            }
        }

        // Submit
        {
            var submitContent = new GUIContent(_targetScript == null ? "Generate and Add" : "Generate and Replace",
                                               "Starts the script generation process. " +
                                               "You can also use Ctrl+Enter / Cmd+Enter.");
            // Listen to Ctrl+Enter or Cmd+Enter
            var keyboardTrigger = Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return &&
                                  (Event.current.modifiers & (EventModifiers.Control | EventModifiers.Command)) != 0;
            if (GUILayout.Button(submitContent) || keyboardTrigger) {
                HandleSubmit(temperature, previousClassName);
            }
        }
    }

    private void HandleSubmit(float temperature, string previousClassName) {
        EditorPrefsService.SetPrompt(_prompt, _targetGameObject);

        if (_deletePreviousFile) {
            var gameObject = Selection.activeGameObject;
            if (gameObject != null) {
                var component = gameObject.GetComponent(previousClassName);
                if (component != null) {
                    DestroyImmediate(component);
                }
            }

            var settings = Settings.instance;
            var file = AssetDatabase.LoadAssetAtPath<MonoScript>($"{settings.path}{previousClassName}.cs");
            if (file != null) {
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(file));
            }
        }

        try {
            RunGenerator(temperature);
        }
        catch (System.Exception e) {
            Debug.LogError(e);
        }

        Close();
    }

    private void RunGenerator(float temperature) {
        var wrappedPrompt = PromptBuilder.Compose(_prompt, _targetScript);
        var streamingResultsWindow = GetWindow<StreamingResultsWindow>(true, "ChatGPT is generating your script");
        streamingResultsWindow.Show();

        void OnScriptGenerationCompleted(string resultText) {
            if (string.IsNullOrEmpty(resultText)) {
                Close();
                return;
            }

            ScriptAsset.Create(resultText, _targetScript, streamingResultsWindow);
        }

        var cancelCallback = ChatGptService.GenerateScript(wrappedPrompt, temperature,
                                                           delta => streamingResultsWindow.AddDelta(delta),
                                                           OnScriptGenerationCompleted,
                                                           failureCallback: () => streamingResultsWindow.Close());
        streamingResultsWindow.Initialize(cancelCallback, _targetGameObject, _targetFolder, _targetScript);

        PromptHistory.Add(wrappedPrompt, _prompt, _targetGameObject);
    }

    void OnEnable() => AssemblyReloadEvents.afterAssemblyReload += Close;
    void OnDisable() => AssemblyReloadEvents.afterAssemblyReload -= Close;
}
}