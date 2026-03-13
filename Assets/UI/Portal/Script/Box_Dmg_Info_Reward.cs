using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GOA.WorldMap;
using TMPro;
using Rubik.Format;

namespace GOA.Portal{
    public class Box_Dmg_Info_Reward : MonoBehaviour
    {
        public RewardData RewardPortalData;

        public TextMeshProUGUI TextName;

        public List<Sprite> IconRarityGear;
        public List<Sprite> IconRarityCompanion;
        public List<Sprite> IconRarityOrb;

        public Image IconGear;
        public Image IconOrb;
        public Image IconCompanion;
        public TextMeshProUGUI Coin;
        public TextMeshProUGUI Gin;

        public void InitData(RewardData rewardPortalData){
            this.RewardPortalData = rewardPortalData;
            this.TextName.text = Lean.Localization.LeanLocalization.GetTranslationText(rewardPortalData.Type, rewardPortalData.Type);
            if(rewardPortalData.RarityGear == -1){
                this.IconGear.transform.parent.gameObject.SetActive(false);
            }else{
                this.IconGear.sprite = this.IconRarityGear[rewardPortalData.RarityGear % this.IconRarityGear.Count];
            }
            if(rewardPortalData.RarityOrb == -1){
                this.IconOrb.transform.parent.gameObject.SetActive(false);
            }else{
                this.IconOrb.sprite = this.IconRarityOrb[rewardPortalData.RarityOrb % this.IconRarityOrb.Count];
            }
            if(rewardPortalData.RarityCompanion == -1){
                this.IconCompanion.transform.parent.gameObject.SetActive(false);
            }else{
                this.IconCompanion.sprite = this.IconRarityCompanion[rewardPortalData.RarityCompanion % this.IconRarityCompanion.Count];
            }
            if(rewardPortalData.Coin > 0){
                this.Coin.text = FormatData.GetFriendlyShortNumber(rewardPortalData.Coin);
            }else{
                this.Coin.gameObject.SetActive(false);
            }
            if(rewardPortalData.Gin > 0){
                this.Gin.text = FormatData.GetFriendlyShortNumber(rewardPortalData.Gin);
            }else{
                this.Gin.gameObject.SetActive(false);
            }
        }
    }
}
