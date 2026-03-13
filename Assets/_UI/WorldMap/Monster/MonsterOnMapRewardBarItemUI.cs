using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rubik.Myrk.Monster
{
    using NTPackage.Functions;
    using Rubik.ItemPlayer;
    public class MonsterOnMapRewardBarItemUI : MonoBehaviour
    {
        public ItemDataUI ItemDataUI;

        public void SetData(ItemData itemData)
        {
            this.ItemDataUI.SetData(itemData, true, true);
        }
    }
}
