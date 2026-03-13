using System.Collections;
using System.Collections.Generic;
using Lean.Localization;
using NTPackage.Functions;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Rubik.Tool
{

    public class FindingText : NTBehaviour
    {
        [Button]
        public void CheckTextWitoutLocalizationFont()
        {
            Transform[] allChild = transform.GetComponentsInChildren<Transform>(true);
            foreach (Transform child in allChild)
            {
                if (child.GetComponent<TextMeshProUGUI>() != null)
                {
                    if (child.GetComponent<LocalizationFont.LocalizationFont>() == null)
                    {
                        Debug.LogWarning(child.name, child.gameObject);
                    }
                }
            }
            Debug.Log("Load : " + allChild.Length);
        }

        [Button]
        public void CheckTextWitoutLocalizationLanguage()
        {
            Transform[] allChild = transform.GetComponentsInChildren<Transform>(true);
            foreach (Transform child in allChild)
            {
                if (child.GetComponent<TextMeshProUGUI>() != null)
                {
                    //if(child.GetComponent<LeanLocalizedText>() == null){
                    //    Debug.LogWarning(child.name,child.gameObject);
                    //}
                }
            }
        }

        [NTButton]

        public void FindUsage()
        {
            TextMeshProUGUI[] texts = FindObjectsOfType<TextMeshProUGUI>(true); // include inactive
            foreach (var tmp in texts)
            {
                if (tmp.spriteAsset != null)
                {
                    if(tmp.spriteAsset.name == "_icon") continue;
                    Debug.LogWarning(tmp.spriteAsset.name, tmp.gameObject);
                }
            }
        }
    }
}