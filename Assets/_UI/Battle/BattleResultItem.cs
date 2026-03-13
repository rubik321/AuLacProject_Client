using NTPackage.Functions;
using Rubik.CardPlayer;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rubik.Myrk.Battle{
    public class BattleResultItem : MonoBehaviour{
        public CardAvatarUI AvatarUI;
        public TextMeshProUGUI TextDamage;
        public TextMeshProUGUI TextSlot;
        

        public Sprite DamageSprite;
        public Sprite HealSprite;
        public Sprite DamagedImage;

        public Image FillImage;

        public void SetData(CardPlayerIndex index, int star, int lv, int slot){
            this.AvatarUI.SetData(index, star, lv);
            this.TextSlot.text = (slot + 1).ToString();
        }

        public void SetDamage(long damage, long maxDamage){
            this.TextDamage.text = NTFunction.FormatNumberWithComa(damage);
            this.FillImage.sprite = this.DamageSprite;
            this.FillImage.fillAmount = (float)damage / maxDamage;
        }

        public void SetHeal(long heal, long maxHeal){
            this.TextDamage.text = NTFunction.FormatNumberWithComa(heal);
            this.FillImage.sprite = this.HealSprite;
            this.FillImage.fillAmount = (float)heal / maxHeal;
        }

        public void SetDamaged(long damage, long maxDamage){
            this.TextDamage.text = NTFunction.FormatNumberWithComa(damage);
            this.FillImage.sprite = this.DamagedImage;
            this.FillImage.fillAmount = (float)damage / maxDamage;
        }
    }
}
