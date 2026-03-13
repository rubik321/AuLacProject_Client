using DamageNumbersPro;
using Lean.Localization;
using NTFunctions_old;
using Rubik.BattleEngine;
using Rubik.Common.AudioHelper;
using Rubik.LocalizationFont;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Rubik.Battle
{
    public class ZfxGameplayController : MonoBehaviour
    {

        public TMPro.VertexGradient DAMAGE_COLOR = new TMPro.VertexGradient(NTFunction.StringHexToColor("fff11e"), NTFunction.StringHexToColor("fff11e"), NTFunction.StringHexToColor("ffba1e"), NTFunction.StringHexToColor("ffba1e"));
        public TMPro.VertexGradient HEAL_COLOR = new TMPro.VertexGradient(NTFunction.StringHexToColor("e1ed75"), NTFunction.StringHexToColor("e1ed75"), NTFunction.StringHexToColor("288c18"), NTFunction.StringHexToColor("288c18"));
        public TMPro.VertexGradient CRIT_COLOR = new TMPro.VertexGradient(NTFunction.StringHexToColor("ffba1e"), NTFunction.StringHexToColor("ffba1e"), NTFunction.StringHexToColor("ff6d01"), NTFunction.StringHexToColor("ff6d01"));
        public TMPro.VertexGradient BLOCK_COLOR = new TMPro.VertexGradient(NTFunction.StringHexToColor("f3ffff"), NTFunction.StringHexToColor("f3ffff"), NTFunction.StringHexToColor("2cc6fb"), NTFunction.StringHexToColor("2cc6fb"));
        public TMPro.VertexGradient POSION_COLOR = new TMPro.VertexGradient(NTFunction.StringHexToColor("7af709"), NTFunction.StringHexToColor("7af709"), NTFunction.StringHexToColor("29d800"), NTFunction.StringHexToColor("29d800"));
        public TMPro.VertexGradient BLEED_COLOR = new TMPro.VertexGradient(NTFunction.StringHexToColor("de61c5"), NTFunction.StringHexToColor("de61c5"), NTFunction.StringHexToColor("d91afb"), NTFunction.StringHexToColor("d91afb"));
        public TMPro.VertexGradient BURN_COLOR = new TMPro.VertexGradient(NTFunction.StringHexToColor("f8802a"), NTFunction.StringHexToColor("f8802a"), NTFunction.StringHexToColor("c00000"), NTFunction.StringHexToColor("c00000"));

        public static ZfxGameplayController Instance;
        [SerializeField] DamageNumber damageNumber;
        [SerializeField] DamageNumber notiText;
        [SerializeField] DamageNumber HealingText;
        [SerializeField] ZfxGameplayItem zfxItem;
        [SerializeField] GameObject heal;

        // Start is called before the first frame update
        void Start()
        {
            Instance = this;
        }

        public void ShowFx()
        {
            
        }

        public void DamageNumberSpawn(Vector2 pos, float damage)
        {
            Debug.Log("Damege " + damage);
            damageNumber.Spawn(pos, damage);
        }
        public void DamageNumberSpawn(Vector2 pos, float damage,TypeDmg typeDame)
        {
          //  Debug.Log("Damege " + damage);
            Color colorDamage = Color.white;
            string dameTxt = damage + " HP";
            HealingText.SetFontMaterial(LocalizationFont.LocalizationFontManager.Instance.GetFont(TypeFont.Content, LocalizationFontManager.Instance.Language));
            switch (typeDame)
            {
                case TypeDmg.Normal:
                    // colorDamage = Color.red;
                    dameTxt =  " -"+ dameTxt;
                    HealingText.SetGradientColor(DAMAGE_COLOR);
                    HealingText.Spawn(pos, dameTxt, colorDamage);
                    AudioCtrl.Instance.Play(AudioName.Normal_Hit_Sound);
                    break;
                case TypeDmg.Block:
                    dameTxt =LeanLocalization.GetTranslationText("dmg_block","Block") + " -" + dameTxt;
                    HealingText.SetGradientColor(BLOCK_COLOR);
                    HealingText.Spawn(pos, dameTxt, colorDamage);
                    AudioCtrl.Instance.Play(AudioName.Block_Sound);
                    break;   
                case TypeDmg.Bleed:
                    dameTxt =LeanLocalization.GetTranslationText("dmg_bleed","Bleed") + " -" + dameTxt;
                    HealingText.SetGradientColor(BLEED_COLOR);
                    HealingText.Spawn(pos, dameTxt, colorDamage);
                    AudioCtrl.Instance.Play(AudioName.Bleed_Sound);
                    break;
                case TypeDmg.Burn:
                    dameTxt = LeanLocalization.GetTranslationText("dmg_burn","Burn") + " -" + dameTxt;
                    HealingText.SetGradientColor(BURN_COLOR);
                    HealingText.Spawn(pos, dameTxt, colorDamage);
                    AudioCtrl.Instance.Play(AudioName.Burn_Sound);
                    break;
                case TypeDmg.Poison:
                    dameTxt = LeanLocalization.GetTranslationText("dmg_poison","Poison") + " -" + dameTxt;
                    HealingText.SetGradientColor(POSION_COLOR);
                    HealingText.Spawn(pos, dameTxt, colorDamage);
                    AudioCtrl.Instance.Play(AudioName.Poison_Sound);
                    break;
                case TypeDmg.Crit:
                    dameTxt = LeanLocalization.GetTranslationText("dmg_crit","Crit") + " -" + dameTxt;
                    HealingText.SetGradientColor(CRIT_COLOR);
                    HealingText.Spawn(pos, dameTxt, colorDamage);
                    AudioCtrl.Instance.Play(AudioName.Normal_Crit_Sound);
                    break;
                case TypeDmg.Heal:
                    dameTxt = LeanLocalization.GetTranslationText("dmg_heal","Heal") + " +"+ dameTxt;
                    GameObject effect =ObjectPool.Spawn(heal);
                    effect.transform.position = new Vector3(pos.x, pos.y - 1.5f);
                    effect.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                    HealingText.SetGradientColor(HEAL_COLOR);
                    HealingText.Spawn(pos, dameTxt, colorDamage);
                    AudioCtrl.Instance.Play(AudioName.Heal_Sound);
                    break;
            }
            
        }
        
        void RycleObj(GameObject effect)
        {
            effect.Recycle();
        }
        [Button]
        public void ShowNotiText(string _text)
        {
            notiText.SetFontMaterial(LocalizationFont.LocalizationFontManager.Instance.GetFont(TypeFont.Content, LocalizationFontManager.Instance.Language));
            notiText.Spawn(new Vector3(0,0,0), _text );

            //notiText.GetComponent<RectTransform>().position = new Vector3(0, 0, 0);
        }
        public void HealingSpawn(Vector2 pos, float damage)
        {
            Debug.Log("Damege " + damage);
            HealingText.Spawn(pos, damage);
        }
    }
}