using System.Collections;
using System.Collections.Generic;
using NTPackage.Functions;
using UnityEngine;

namespace Rubik.PlayerMail
{
    using ItemPlayer;
    public class PlayerMailAttachItemUI : NTBehaviour
    {
        public ItemDataUI ItemDataUI;

        public void SetData(ItemData itemData)
        {
            this.ItemDataUI.SetData(itemData);
        }
    }
}