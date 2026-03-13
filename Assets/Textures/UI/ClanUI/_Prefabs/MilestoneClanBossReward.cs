using Rubik.ItemPlayer;
using UnityEngine;
using UnityEngine.UI;

namespace Rubik.Myrk.Clan
{
    using Rubik.ItemPlayer;
    using TMPro;

    public class MilestoneClanBossReward : MonoBehaviour
    {
        public Image Background;

        public ItemDataUI ItemReward;
        public TextMeshProUGUI AmountReward;

        public void SetData(ItemData itemData){
            this.ItemReward.SetData(itemData, true, true);
            AmountReward.text = itemData.Amount.ToString();
        }
    }
}