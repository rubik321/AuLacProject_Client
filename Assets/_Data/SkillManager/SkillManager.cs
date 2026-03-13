using System.Collections;
using NTPackage;
using NTPackage.Functions;
using UnityEngine;

namespace Rubik.Myrk.Skill
{
    using System.Collections.Generic;
    using AddressablesLoader;
    using Rubik.CardPlayer;

    public enum SkillType{
        ActiveCard,
        PassiveCard,
        ActiveGear,
        PassiveGear,
    }

    public class SkillManager : NTBehaviour
    {

        [SerializeField] private ListSpriteAddressable IconSkillAddressable;
        [SerializeField] private NTDictionary<string, Sprite> SkillSpriteDic;

        public List<Sprite> ListBorderSkill;
        public List<Sprite> ListMaskSkill;

        public static SkillManager Instance;
        protected override void Awake()
        {
            base.Awake();
            if (SkillManager.Instance != null)
            {
                NTLog.LogError("Only 1 Instance allow");
                return;
            }
            SkillManager.Instance = this;
        }

        public IEnumerator LoadData()
        {
            int count = 0;
            AddressablesLoader.Instance.LoadAssetsFromLocal(AddressablesAssetsConfig.IconSkill, (result) =>
            {
                this.IconSkillAddressable = result.GetComponent<ListSpriteAddressable>();
                this.IconSkillAddressable.transform.SetParent(transform);
                this.SkillSpriteDic = new NTDictionary<string, Sprite>();
                foreach (var item in this.IconSkillAddressable.ListSprite)
                {
                    this.SkillSpriteDic.Add(item.name, item);
                }
                count++;
            });
            yield return new WaitUntil(() => count >= 1);
        }


        #region Getter
        public Sprite GetSkillImage(string spriteName)
        {
            return this.SkillSpriteDic.Get(spriteName);
        }

        public Sprite GetBorderSkill(SkillType skillType)
        {
            return this.ListBorderSkill[((int)skillType)];
        }

        public Sprite GetMaskSkill(SkillType skillType)
        {
            return this.ListMaskSkill[((int)skillType)];
        }

        #endregion

    }
}