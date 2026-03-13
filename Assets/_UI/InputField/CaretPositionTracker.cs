using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CaretPositionTracker : MonoBehaviour
{
    public TMP_InputField tmpInputField;
    public RectTransform customCaretImage;

    void Update()
    {
        int caretIndex = tmpInputField.text.Length;
        TMP_Text textComponent = tmpInputField.textComponent;

        // Get the position of the character at the caret index
        if (caretIndex < textComponent.textInfo.characterCount)
        {
            TMP_CharacterInfo charInfo = textComponent.textInfo.characterInfo[caretIndex];
            Vector3 worldPos = textComponent.transform.TransformPoint(charInfo.bottomLeft);

            // Move your custom caret (e.g., an image)
            customCaretImage.position = worldPos;
        }
    }
}
