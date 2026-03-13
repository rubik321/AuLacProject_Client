using System;
using System.Collections;
using System.Collections.Generic;
using NTPackage.Functions;
using Rubik.UI;
using SimpleJSON;
using UnityEngine;

namespace Rubik.UserProfile
{
    using NTPackage.EventDispatcher;
    using Rubik.DataCenter;
    using Rubik.Manager;
    using Rubik.Config;
    using Rubik.UserDataPlayer;
    using Sirenix.Serialization;
    using UnityEngine.U2D;
    using NTPackage;
    using NTPackage.UI;
    using Rubik.ItemPlayer;

    public class UserProfileResource : NTBehaviour
    {

        public Sprite AvatarSpriteDefault;
        public List<Sprite> AvatarSprites;
        public Sprite AvatarBorderSpriteDefault;
        public List<Sprite> AvatarBorderSprites;
        public List<GameObject> SkinUIObjects;
        public List<GameObject> EmojiObjects;
        public List<Sprite> EmojiSprites;

        public static UserProfileResource Instance;
        protected override void Awake()
        {
            base.Awake();
            if (UserProfileResource.Instance != null)
            {
                NTLog.LogWarning("Only 1 Instance allow");
                return;
            }
            UserProfileResource.Instance = this;
        }

        #region Getter

        public Sprite GetAvatarSprite(int index)
        {
            try
            {
                return this.AvatarSprites[index];
            }
            catch (System.Exception e)
            {
                return this.AvatarSpriteDefault;
            }
            ;
        }

        public Sprite GetAvatarBorderSprite(int index)
        {
            try
            {
                return this.AvatarBorderSprites[index];
            }
            catch (System.Exception e)
            {
                return this.AvatarBorderSpriteDefault;
            }
            ;
        }

        public GameObject GetSkinUIObject(int index)
        {
            return this.SkinUIObjects[index];
        }

        public GameObject GetEmojiPrefabByIndex(int index)
        {
            return this.EmojiObjects[index];
        }

        public Sprite GetEmojiSpriteByIndex(int index)
        {
            return this.EmojiSprites[index];
        }
        #endregion
    }
}
