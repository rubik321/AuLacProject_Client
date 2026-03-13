using System.Collections;
using System.Collections.Generic;
using NTPackage.Functions;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rubik.LocalizationFont
{
    [System.Serializable]
    public class FontData
    {
        public TypeFont TypeFont;
        public TMP_FontAsset Font;
    }

    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LocalizationFont : NTBehaviour
    {
        private TextMeshProUGUI Text;
        public TypeFont TypeFont;
        public string ID;

        protected override void Awake()
        {
            this.ID = "";
        }

        protected override void Start()
        {
            base.Start();
            this.Init();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            this.Init();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            this.UnInit();
        }

        protected override void OnDestroy()
        {
            this.UnInit();
        }

        [Button]
        public void Init()
        {
            if (this.Text == null)
            {
                this.Text = this.GetComponent<TextMeshProUGUI>();
            }
            if (!string.IsNullOrEmpty(ID))
            {
                this.UnInit();
            }
            ID = NTFunction.GenerateId();

            LocalizationFontManager.Instance.RegisterChangeLanguage(ID, OnChangeLanguage);
            this.OnChangeLanguage();
        }

        private void UnInit()
        {
            LocalizationFontManager.Instance?.OnChangeLanguage.Remove(ID);
        }

        private void OnChangeLanguage()
        {
            if (this.TypeFont == TypeFont.None)
            {
                return;
            }
            if (this.Text == null)
            {
                this.Text = this.GetComponent<TextMeshProUGUI>();
            }
            TMP_FontAsset font = LocalizationFontManager.Instance.GetFont(this.TypeFont, LocalizationFontManager.Instance.Language);
            if (font != null) this.Text.font = font;
        }
    }

}