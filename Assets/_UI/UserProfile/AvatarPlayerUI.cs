using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NTPackage.Functions;

namespace Rubik.UserProfile
{
    public enum StatusType{
        None = 0,
        Equipped = 1,
        Locked = 2,
    }

    public class AvatarPlayerUI : MonoBehaviour
    {
        public Image AvatarImage;
        public Image AvatarBorderImage;

        public Coroutine CoroutineLoadAvatar;

        public void SetData(int avatarIndex, int avatarBorderIndex, string avatarCustom = "", int level = 1)
        {
            this.AvatarImage.sprite = UserProfileManager.Instance.GetAvatarSprite(avatarIndex);
            this.AvatarBorderImage.sprite = UserProfileManager.Instance.GetAvatarBorderSprite(avatarBorderIndex);
        }


        public void SetAvatarCustomData(string avatarCustomData)
        {
            Texture2D texture = NTFunction.LoadTextureFromBase64(avatarCustomData);
            this.AvatarImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
    }
}