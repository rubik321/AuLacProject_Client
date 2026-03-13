using System.Collections;
using System.Collections.Generic;
using NTFunctions_old;
using TMPro;
using UnityEngine;

namespace GOA.LandEvent
{
    public class LandEventDescUI : PopupUI
    {
        public EventType EventType;
        public TextMeshProUGUI TextTitle;
        public TextMeshProUGUI TextDes;
        public void OnUI(EventType eventType)
        {
            switch (eventType)
            {
                case EventType.Hunter:
                    this.TextTitle.text = Lean.Localization.LeanLocalization.GetTranslationText("hunter_event", "Hunter Event");
                    this.TextDes.text = Lean.Localization.LeanLocalization.GetTranslationText("hunter_event_des", "Hunter Event");
                    break;
                default:
                    this.TextTitle.text = Lean.Localization.LeanLocalization.GetTranslationText("land_event", "Land Event");
                    this.TextDes.text = Lean.Localization.LeanLocalization.GetTranslationText("land_event_des", "Land Event");
                    break;
            }
            this.Show();
        }
    }
}
