using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Lean.Localization;
public class CharacterGearItemDetailUI : MonoBehaviour
{
    public int gearIndex;
    public TextMeshProUGUI desTxt;
    void Start()
    {
        desTxt.text = LeanLocalization.GetTranslationText("Gear_OH_00_085");
    }

  
}
