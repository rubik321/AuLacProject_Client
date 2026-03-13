using System.Collections;
using System.Collections.Generic;
using NTPackage.Functions;
using UnityEngine;
using UnityEngine.UI;

namespace Rubik.LocalizationFont
{
    // Create ScriptableObject for Localization Font
    [CreateAssetMenu(fileName = "LocalizationFont", menuName = "ScriptableObjects/LocalizationFont")]
    public class LocalizationFontSO : ScriptableObject
    {
        public string Language;
        public List<FontData> FontDatas;
    }
}
