#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace ScriptGenerator.Demo {
[ExecuteInEditMode]
public class ChatGptScriptGeneratorDemo : MonoBehaviour {
    public Text test1;
    public Text testDescription1;

    public Text test2;
    public Text testDescription2;

    public Button debugButton;

    [Space] public Color successColor = Color.green;
    public Color failureColor = Color.red;
    public Color warningColor = Color.yellow;

    [Space] public MeshRenderer meshRenderer;

    private bool IsApiKeyOk => !string.IsNullOrEmpty(Settings.instance.apiKey);

    private bool _testQuerySent;

    private void OnEnable() {
        debugButton.gameObject.SetActive(false);

        if (meshRenderer != null) {
            GameObject primitive = GameObject.CreatePrimitive(PrimitiveType.Plane);
            Material defaultMaterial = primitive.GetComponent<MeshRenderer>().sharedMaterial;
            DestroyImmediate(primitive);
            meshRenderer.sharedMaterial = defaultMaterial;
        }
    }

    void Update() {
        {
            test1.text = IsApiKeyOk ? "API Key OK" : "API Key Missing";
            test1.color = IsApiKeyOk ? successColor : failureColor;
            testDescription1.text = IsApiKeyOk
                ? ""
                : "Please set your OpenAI API key in the ChatGPT Script Generator settings.";
            testDescription1.color = IsApiKeyOk ? successColor : failureColor;

            if (!IsApiKeyOk) {
                debugButton.gameObject.SetActive(false);
            }
        }

        if (EditorApplication.isCompiling) {
            test2.text = "Unity is compiling...";
            test2.color = warningColor;
            testDescription2.text = "";
            testDescription2.color = warningColor;
            debugButton.gameObject.SetActive(false);
            return;
        }

        if (string.IsNullOrEmpty(CloudProjectSettings.userName) || CloudProjectSettings.userName == "anonymous") {
            test2.text = "Unity account not matching OpenAI API key. Log in with previous Unity account or re-enter " +
                         "your API key.";
            test2.color = warningColor;
            testDescription2.text = "";
            testDescription2.color = warningColor;
            debugButton.gameObject.SetActive(false);
            return;
        }

        if (!_testQuerySent && IsApiKeyOk) {
            RunTestQuery();
        }
    }

    [ContextMenu("Run Test Query")]
    private void RunTestQuery() {
        _testQuerySent = true;

        test2.text = "Testing API connection...";
        test2.color = warningColor;

        ChatGptService.GenerateScript("Debug log 'Hello, world!'", 0, null, _ => {
            test2.text = "API Connection OK";
            test2.color = successColor;
            testDescription2.text = "";
            testDescription2.color = successColor;
        }, () => {
            test2.text = "API Connection Failed (Play/Stop to retry)";
            test2.color = failureColor;
            testDescription2.text = "Please follow the suggestions in the error, check the ChatGPT Script Generator " +
                                    "settings or reach out to us on Discord (button below)";
            testDescription2.color = failureColor;
            debugButton.gameObject.SetActive(true);
        });
    }

    public void OpenDocumentation() {
        Application.OpenURL("https://discord.gg/GBAeuWC9qS");
    }

    public void OpenSettings() {
        SettingsService.OpenProjectSettings("Project/ChatGPT Script Generator");
    }
}
}

#endif