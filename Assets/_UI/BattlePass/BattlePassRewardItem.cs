using UnityEngine;

namespace Rubik.Myrk.BattlePass
{
    using NTPackage.UI;
    using Rubik.ItemPlayer;
    using TMPro;

    public class BattlePassRewardItem : NTButtonEffect
    {
        public ItemDataUI ItemData;
        public TextMeshProUGUI TextAmount;
        public Transform Claimed;
        public Transform Claim;
        public Transform Locked;

        public void SetData(ItemData itemData, BattlePassStatus status){
            this.ItemData.SetData(itemData, true, false);
            this.TextAmount.text = "x" + itemData.Amount.ToString();

            this.Claimed.gameObject.SetActive(false);
            this.Claim.gameObject.SetActive(false);
            this.Locked.gameObject.SetActive(false);

            if(status == BattlePassStatus.Claimed){
                this.Claimed.gameObject.SetActive(true);
            }else if(status == BattlePassStatus.Claim){
                this.Claim.gameObject.SetActive(true);
            }else if(status == BattlePassStatus.Locked){
                this.Locked.gameObject.SetActive(true);
            }

        }
    }
}