using System.Collections;
using System.Collections.Generic;
using NTFunctions_old;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GOA.Mat
{
public class MatItemUI : LoadBehaviour
{
    public Image Icon;
    public TextMeshProUGUI TextDetail;

    public void SetData(Sprite icon, string detail, Color color){
        this.Icon.sprite = icon;
        this.TextDetail.text = detail;
        this.TextDetail.color = color;
    }
}
}
