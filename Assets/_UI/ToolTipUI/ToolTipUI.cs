using System.Collections;
using System.Collections.Generic;
using NTPackage.UI;
using TMPro;
using UnityEngine;

namespace Rubik.UI
{
    public class ToolTipUI : PopupUI
    {

        [SerializeField] private TextMeshProUGUI Title;
        [SerializeField] private TextMeshProUGUI Description;
        public void SetData(string title, string description)
        {
            this.Title.text = title;
            this.Description.text = description;
        }
    }
}

