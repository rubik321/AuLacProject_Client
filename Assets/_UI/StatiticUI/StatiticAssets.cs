using System.Collections.Generic;
using NTPackage.Functions;
using UnityEngine;

namespace Rubik.UI.Statitic
{
    public enum TypeStat
    {
        ATK,
        DEF,
        SPD,
        HP,
        Mind,
    }

    public class StatData
    {
        public TypeStat TypeStat;
        public long Value;

        public StatData(){
            this.TypeStat = TypeStat.ATK;
            this.Value = 0;
        }

        public StatData(TypeStat typeStat, long value){
            this.TypeStat = typeStat;
            this.Value = value;
        }
    }

    public class StatiticAssets : NTBehaviour
    {
        public List<Sprite> IconStatsColorful;
        public List<Sprite> IconStatsBlack;

        public static StatiticAssets Instance;
        protected override void Awake()
        {
            base.Awake();
            if (StatiticAssets.Instance != null)
            {
                NTLog.LogError("Only 1 Instance allow");
                return;
            }
            StatiticAssets.Instance = this;
        }

        #region Getter
        public Sprite GetIconStatsColorful(TypeStat typeStat)
        {
            return this.IconStatsColorful[(int)typeStat];
        }

        public Sprite GetIconStatsBlack(TypeStat typeStat)
        {
            return this.IconStatsBlack[(int)typeStat];
        }

        public string GetNameStat(TypeStat typeStat)
        {
            return Lean.Localization.LeanLocalization.GetTranslationText("stat_name_" + (int)typeStat, "Stat");
        }

        public string GetDescriptionStat(TypeStat typeStat)
        {
            return Lean.Localization.LeanLocalization.GetTranslationText("stat_des_" + (int)typeStat, "Stat");
        }
        
        #endregion
    }
}