using System;
using System.Collections;
using System.Collections.Generic;
using NTPackage;
using NTPackage.Functions;
using TMPro;
using UnityEngine;

namespace Rubik.LocalizationFont
{
    public enum TypeFont
    {
        None = 0,
        // Base
        Title,
        Content,

        // Special
        TitleBlackOutline,
        ContentBlackOutline,
    }

    public class LocalizationFontManager : NTBehaviour
    {
        public List<LocalizationFontSO> LocalizationFonts;
        public LocalizationFontSO DefaultLocalizationFont;
        public string Language;
        public TMP_FontAsset FontDefault;
        public NTDictionary<string, Action> OnChangeLanguage = new NTDictionary<string, Action>();

        public static LocalizationFontManager Instance;
        protected override void Awake()
        {
            base.Awake();
            if (LocalizationFontManager.Instance != null)
            {
                NTLog.LogWarning("Only 1 Instance allow");
                return;
            }
            LocalizationFontManager.Instance = this;
        }

        public void ChangeLanguage(string language){
            this.Language = language;
            this.OnChangeLanguage.ToList().ForEach(x => x?.Invoke());
        }

        public void RegisterChangeLanguage(string id, Action action)
        {
            if (this.OnChangeLanguage == null || this.OnChangeLanguage.Count == 0)
            {
                this.OnChangeLanguage = new NTDictionary<string, Action>();
            }
            this.OnChangeLanguage.Add(id, action);
        }

        public void UnRegisterChangeLanguage(string id){
            if(this.OnChangeLanguage == null || this.OnChangeLanguage.Count == 0) return;
            this.OnChangeLanguage.Remove(id);
        }

        public TMP_FontAsset GetFont(TypeFont typeFont, string language){
            if(this.LocalizationFonts == null || this.LocalizationFonts.Count == 0) return this.FontDefault;
            LocalizationFontSO localizationFontSO = this.LocalizationFonts.Find(x => x.Language == language);
            if(localizationFontSO == null && this.DefaultLocalizationFont != null){
                localizationFontSO = this.DefaultLocalizationFont;
            }
            if(localizationFontSO == null) return this.FontDefault;
            FontData fontData = localizationFontSO.FontDatas.Find(x => x.TypeFont == typeFont);
            if(fontData == null){
                return this.FontDefault;
            }
            return fontData.Font;
        }
    }
}