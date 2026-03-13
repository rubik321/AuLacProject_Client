using System;
using System.Collections;
using System.Collections.Generic;
using NTPackage.Functions;
using NTPackage.UI;
using Rubik.UI;
using UnityEngine;

namespace Rubik.Localization
{
    using Lean.Localization;
    using Rubik.AddressablesLoader;

    public class LocalizationConfig
    {
        public const string English = "English";
        public const string Vietnamese = "Vietnamese";
        public const string Chinese = "Chinese";
        public const string German = "German";

        public const string English_utd = "English_utd";
        public const string Vietnamese_utd = "Vietnamese_utd";
        public const string Chinese_utd = "Chinese_utd";
        public const string German_utd = "German_utd";


        public const string KeyLanguage = "Language";
    }

    public class LocalizationManager : NTBehaviour
    {

        public string Language;

        public ListTransformAddressable GameLanguage;
        public Transform LocalizationTrans;
        public List<LeanLanguageCSV> LeanLanguageCSVList;

        public static LocalizationManager Instance;
        protected override void Awake()
        {
            base.Awake();
            if (LocalizationManager.Instance != null)
            {
                NTLog.LogError("Only 1 Instance allow");
                return;
            }
            LocalizationManager.Instance = this;
        }

        protected override void Start()
        {
            this.Language = PlayerPrefs.GetString(LocalizationConfig.KeyLanguage, "");
            if (this.Language.Length == 0)
            {
                switch (Application.systemLanguage)
                {
                    case SystemLanguage.English:
                        this.Language = LocalizationConfig.English;
                        break;
                    case SystemLanguage.Vietnamese:
                        this.Language = LocalizationConfig.Vietnamese;
                        break;
                    case SystemLanguage.Chinese:
                        this.Language = LocalizationConfig.Chinese;
                        break;
                    case SystemLanguage.German:
                        this.Language = LocalizationConfig.German;
                        break;
                    default:
                        this.Language = LocalizationConfig.English;
                        break;
                }
            }
            LeanLocalization.SetCurrentLanguageAll(this.Language);
            LocalizationFont.LocalizationFontManager.Instance.ChangeLanguage(this.Language);
        }

        public IEnumerator IELoadGameLanguage()
        {
            int count = 0;
            AddressablesLoader.Instance.LoadAssetsFromLocal(AddressablesAssetsConfig.GameLanguage, (result) =>
            {
                this.GameLanguage = result.GetComponent<ListTransformAddressable>();
                foreach (var item in this.GameLanguage.ListTransform)
                {
                    item.SetParent(this.LocalizationTrans);
                }
                count++;
            });
            yield return new WaitUntil(() => count == 1);
            foreach (Transform item in this.LocalizationTrans)
            {
                LeanLanguageCSV leanLanguageCSV = item.GetComponent<LeanLanguageCSV>();
                if (leanLanguageCSV != null)
                {
                    this.LeanLanguageCSVList.Add(leanLanguageCSV);
                }
            }
            this.ChangeLanguage(this.Language);
        }

        public void SelectLanguage(string language, Action onClick)
        {
            string textTitle = LeanLocalization.GetTranslationText("change_language_title", "Language");
            string textConfirm = LeanLocalization.GetTranslationText("change_language_confirm", "Restart the game to change the Language to {0} ?");
            textConfirm = string.Format(textConfirm, language);
            string textCancel = LeanLocalization.GetTranslationText("btn_cancel", "Cancel");
            string textYes = LeanLocalization.GetTranslationText("btn_agree", "Agree");
            PopupManager.Instance.OnUI(PopupCode.MessageOptionPanel, null, (popup) =>
            {
                MessageOptionPanel messageOptionPanel = popup as MessageOptionPanel;
                messageOptionPanel.SetData(textTitle, textConfirm);
                messageOptionPanel.SetActionConfirm(() =>
                {
                    this.ChangeLanguage(language);
                    onClick?.Invoke();
                    messageOptionPanel.OffUI();
                }, textYes);
                messageOptionPanel.SetActionReject(() =>
                {
                    messageOptionPanel.OffUI();
                    onClick?.Invoke();
                }, textCancel);
            });


        }

        public void ChangeLanguage(string language)
        {
            this.Language = language;
            PlayerPrefs.SetString(LocalizationConfig.KeyLanguage, this.Language);
            string language_utd = this.Language + "_utd";
            LeanLanguageCSV leanLanguageCSV = this.LeanLanguageCSVList.Find(x => x.Language == language_utd);
            if (leanLanguageCSV != null)
            {
                LeanLocalization.SetCurrentLanguageAll(language_utd);
            }else{
                NTLog.LogWarning("Not found: " + language_utd, gameObject);
                LeanLocalization.SetCurrentLanguageAll(this.Language);
            }
            LocalizationFont.LocalizationFontManager.Instance.ChangeLanguage(this.Language);
        }
    }
}

