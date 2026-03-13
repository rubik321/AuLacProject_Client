using System.Collections;
using System.Collections.Generic;
using NTPackage.Functions;
using Rubik.UI;
using Rubik.UI.Statitic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rubik.CardPlayer
{
    public class CardAvatarUI : MonoBehaviour
    {
        public Transform AvatarHolder;
        public StarUI StarUI;
        public TextMeshProUGUI LvText;
        public Image OrImage;
        public CardPlayerData CardPlayerData;

        public void SetData(CardPlayerIndex index, int star, int lv)
        {
            this.CardPlayerData = CardPlayerManager.Instance.GetCardPlayerDataByIndex(index);
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.AvatarHolder);
            Transform avatar = CardPlayerManager.Instance.InstantiatePlayerAvatar(index);
            avatar.SetParent(this.AvatarHolder);
            NTFunction.ResetPosition(avatar);

            this.StarUI.SetStar(star);
            this.LvText.text = "Lv. " + (lv + 1).ToString();
            this.OrImage.sprite = CardPlayerManager.Instance.GetOriginSpriteCircle(this.CardPlayerData.Origin);
        }
    }
}