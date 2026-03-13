using System.Collections;
using System.Collections.Generic;
using NTPackage.Functions;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Rubik.LocalizationFont
{
    public class CheckLocalizationFont : MonoBehaviour
    {
        public LocalizationFontManager LocalizationFontManager;
        [Button]
        public void Check(){
            TextMeshProUGUI[] texts = this.GetComponentsInChildren<TextMeshProUGUI>();
            foreach (TextMeshProUGUI text in texts){
                if(!text.TryGetComponent(out LocalizationFont localizationFont)){
                    NTLog.LogError("Not have LocalizationFont: " + text.name, text.gameObject);
                }
            }
        }

        [Button]
        public void UpdateFont(){
            TextMeshProUGUI[] texts = this.GetComponentsInChildren<TextMeshProUGUI>();
            foreach (TextMeshProUGUI text in texts){
                if(text.TryGetComponent(out LocalizationFont localizationFont)){
                    text.font = LocalizationFontManager.GetFont(localizationFont.TypeFont, LocalizationFontManager.Language);
                }
            }
        }
    }
}