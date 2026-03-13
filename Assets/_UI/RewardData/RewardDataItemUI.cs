using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rubik.RewardData
{
    using NTPackage.Functions;
    using NTPackage.UI;
    using Rubik.CardPlayer;
    using Rubik.CharacterGear;
    using Rubik.ItemPlayer;
    public class RewardDataItemUI : NTBehaviour
    {
        public ItemDataUI ItemDataUI;
        public ItemData ItemData;
        public CharacterGear CharacterGear;
        public void SetData(ItemData itemData)
        {
            ItemDataUI.SetData(itemData, true, true);
            ItemData = itemData;
        }

        public void SetData(CharacterGear characterGear)
        {
            ItemDataUI.SetData(characterGear, true);
            CharacterGear = characterGear;
        }

        public void SetData(CardPlayer cardPlayer)
        {
            ItemDataUI.SetData(cardPlayer, true);
        }
    }
}