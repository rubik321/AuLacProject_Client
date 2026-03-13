using Lean.Localization;
using NTPackage;
using NTPackage.Functions;
using Rubik.Myrk.Arena;
using Rubik.Myrk.Battle;
using Rubik.Myrk.Clan;
using Rubik.Myrk.Monster;
using Rubik.Myrk.Outpost;
using Rubik.Myrk.Portal;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rubik.BattleEngine
{
    public enum BattleType
    {
        Map,
        Clan,
        Portal,
        Outpost,
        Arena,
    }

    public class BattleEngineController : NTBehaviour
    {
        public TextAsset BattleData;

        public BattleResult BattleResult;
        public ClanBossBattleResult ClanBattleResult;
        public OutpostBattleResult OutpostBattleResult;
        public PortalBossBattleResult PortalBossBattleResult;
        public ArenaBattleResult ArenaBattleResult;
        public BattleShortData Result; 

        public MonsterAttackData LastMonsterAttackData;
        public ResultAttackMonsterResponse ResultAttackMonsterResponse;

        public BattleType BattleType = BattleType.Map;
        public List<Sprite> ListEffectIcon;
        public NTDictionary<string, Sprite> EffectIconDic;
        public EffectIcon EffectIconPrefab;
        public OpponentData enemyData;
        public static BattleEngineController Instance;
        protected override void Awake()
        {
            base.Awake();
            if (BattleEngineController.Instance != null)
            {
                NTLog.LogError("Only 1 Instance allow");
                return;
            }
            BattleEngineController.Instance = this;
        }

        public void LoadData()
        {
            this.EffectIconDic = new NTDictionary<string, Sprite>();
            foreach (Sprite sprite in this.ListEffectIcon)
            {
                this.EffectIconDic.Add(sprite.name, sprite);
            }
        }

        [Button]
        public void LoadBattleData()
        {
            this.BattleResult = new BattleResult();
            this.BattleResult.Result = JsonUtility.FromJson<BattleShortData>(BattleData.text);
        }

        [Button]
        public void LogBattleData()
        {
            NTLog.LogMessage(JsonUtility.ToJson(this.BattleResult));
        }

        #region Getter
        public EffectIcon GetEffectIcon(TypeBuff typeBuff, int amount = 0)
        {
            Sprite sprite = null;
            switch (typeBuff)
            {
                case TypeBuff.Buff_ATK:
                    sprite = this.EffectIconDic.Get(TypeBuff.Buff_ATK.ToString());
                    break;
                case TypeBuff.Debuff_ATK:
                    sprite = this.EffectIconDic.Get(TypeBuff.Debuff_ATK.ToString());
                    break;
                case TypeBuff.Buff_DEF:
                    sprite = this.EffectIconDic.Get(TypeBuff.Buff_DEF.ToString());
                    break;
                case TypeBuff.Debuff_DEF:
                    sprite = this.EffectIconDic.Get(TypeBuff.Debuff_DEF.ToString());
                    break;
                case TypeBuff.Buff_SPD:
                    sprite = this.EffectIconDic.Get(TypeBuff.Buff_SPD.ToString());
                    break;
                case TypeBuff.Debuff_SPD:
                    sprite = this.EffectIconDic.Get(TypeBuff.Debuff_SPD.ToString());
                    break;
                case TypeBuff.Buff_CRT:
                    sprite = this.EffectIconDic.Get(TypeBuff.Buff_CRT.ToString());
                    break;
                case TypeBuff.Debuff_CRT:
                    sprite = this.EffectIconDic.Get(TypeBuff.Debuff_CRT.ToString());
                    break;
                case TypeBuff.Buff_CRD:
                    sprite = this.EffectIconDic.Get(TypeBuff.Buff_CRD.ToString());
                    break;
                case TypeBuff.Debuff_CRD:
                    sprite = this.EffectIconDic.Get(TypeBuff.Debuff_CRD.ToString());
                    break;
                case TypeBuff.Buff_DR:
                    sprite = this.EffectIconDic.Get(TypeBuff.Buff_DR.ToString());
                    break;
                case TypeBuff.Debuff_DR:
                    break;
                case TypeBuff.Burn:
                    sprite = this.EffectIconDic.Get(TypeBuff.Burn.ToString());
                    break;
                case TypeBuff.Bleed:
                    sprite = this.EffectIconDic.Get(TypeBuff.Bleed.ToString());
                    break;
                case TypeBuff.Poison:
                    sprite = this.EffectIconDic.Get(TypeBuff.Poison.ToString());
                    break;
            }
            if (sprite == null) return null;
            EffectIcon effectIcon = ObjectPoolingManager.Instance.InstantiateObject<EffectIcon>(ObjectPoolingConfig.EffectIcon, this.EffectIconPrefab.transform);
            effectIcon.Icon.sprite = sprite;
            effectIcon.Amount.text = amount.ToString();
            return effectIcon;
        }
        
        public string GetEffectName(TypeBuff typeBuff){
            switch (typeBuff)
            {
                case TypeBuff.Buff_ATK:
                    return LeanLocalization.GetTranslationText("buff_name_atk","Attack");
                case TypeBuff.Debuff_ATK:
                    return LeanLocalization.GetTranslationText("debuff_des_atk","Attack");
                case TypeBuff.Buff_DEF:
                    return LeanLocalization.GetTranslationText("buff_name_def","Defense");
                case TypeBuff.Debuff_DEF:
                    return LeanLocalization.GetTranslationText("debuff_des_def","Defense");
                case TypeBuff.Buff_SPD:
                    return LeanLocalization.GetTranslationText("buff_name_spd","Speed");
                case TypeBuff.Debuff_SPD:
                    return LeanLocalization.GetTranslationText("debuff_des_spd","Speed");
                case TypeBuff.Buff_CRT:
                    return LeanLocalization.GetTranslationText("buff_name_crt","Critical");
                case TypeBuff.Debuff_CRT:
                    return LeanLocalization.GetTranslationText("debuff_des_crt","Critical");
                case TypeBuff.Buff_CRD:
                    return LeanLocalization.GetTranslationText("buff_name_crd","Critical Damage");
                case TypeBuff.Debuff_CRD:
                    return LeanLocalization.GetTranslationText("debuff_des_crd","Critical Damage");
                case TypeBuff.Buff_DR:
                    return LeanLocalization.GetTranslationText("buff_name_dr","Damage Reduction");
                case TypeBuff.Debuff_DR:
                    return LeanLocalization.GetTranslationText("debuff_des_dr","Damage Reduction");
                case TypeBuff.Burn:
                    return LeanLocalization.GetTranslationText("effect_name_burn","Burn");
                case TypeBuff.Bleed:
                    return LeanLocalization.GetTranslationText("effect_name_bleed","Bleed");
                case TypeBuff.Poison:
                    return LeanLocalization.GetTranslationText("effect_name_poison","Poison");
                default:
                    return "";
            }
        }

        public string GetEffectDescription(TypeBuff typeBuff){
            switch (typeBuff)
            {
                case TypeBuff.Buff_ATK:
                    return LeanLocalization.GetTranslationText("buff_des_atk","Attack");
                case TypeBuff.Debuff_ATK:
                    return LeanLocalization.GetTranslationText("debuff_des_atk","Attack");
                case TypeBuff.Buff_DEF:
                    return LeanLocalization.GetTranslationText("buff_des_def","Defense");
                case TypeBuff.Debuff_DEF:
                    return LeanLocalization.GetTranslationText("debuff_des_def","Defense");
                case TypeBuff.Buff_SPD:
                    return LeanLocalization.GetTranslationText("buff_des_spd","Speed");
                case TypeBuff.Debuff_SPD:
                    return LeanLocalization.GetTranslationText("debuff_des_spd","Speed");
                case TypeBuff.Buff_CRT:
                    return LeanLocalization.GetTranslationText("buff_des_crt","Critical");
                case TypeBuff.Debuff_CRT:
                    return LeanLocalization.GetTranslationText("debuff_des_crt","Critical");
                case TypeBuff.Buff_CRD:
                    return LeanLocalization.GetTranslationText("buff_des_crd","Critical Damage");
                case TypeBuff.Debuff_CRD:
                    return LeanLocalization.GetTranslationText("debuff_des_crd","Critical Damage");
                case TypeBuff.Buff_DR:
                    return LeanLocalization.GetTranslationText("buff_des_dr","Damage Reduction");
                case TypeBuff.Debuff_DR:
                    return LeanLocalization.GetTranslationText("debuff_des_dr","Damage Reduction");
                case TypeBuff.Burn:
                    return LeanLocalization.GetTranslationText("effect_des_burn","Burn");
                case TypeBuff.Bleed:
                    return LeanLocalization.GetTranslationText("effect_des_bleed","Bleed");
                case TypeBuff.Poison:
                    return LeanLocalization.GetTranslationText("effect_des_poison","Poison");
                default:
                    return "";
            }
        }
        #endregion

        #region Setter
        public void SetBattleResult(BattleResult result){
            if(result == null || result._id == null || result._id == "") return;
            this.ResetBattleResult();
            this.BattleResult = result;
            this.Result = result.Result;
            this.BattleType = BattleType.Map;
        }
        public void SetClanBattleResult(ClanBossBattleResult result){
            if(result == null || result._id == null || result._id == "") return;
            this.ResetBattleResult();
            this.ClanBattleResult = result;
            this.Result = result.BattleResult;
            this.BattleType = BattleType.Clan;
        }

        public void SetOutpostBattleResult(OutpostBattleResult result){
            if(result == null || result._id == null || result._id == "") return;
            this.ResetBattleResult();
            this.OutpostBattleResult = result;
            this.Result = result.Result;
            this.BattleType = BattleType.Outpost;
        }

        public void SetPortalBossBattleResult(PortalBossBattleResult result){
            if(result == null || result._id == null || result._id == "") return;
            this.ResetBattleResult();
            this.PortalBossBattleResult = result;
            this.Result = result.Result;
            this.BattleType = BattleType.Portal;
        }
        public void SetArenaBattleResult(ArenaBattleResult result){
            if(result == null || result._id == null || result._id == "") return;
            this.ResetBattleResult();
            this.ArenaBattleResult = result;
            this.Result = result.Result;
            this.BattleType = BattleType.Arena;
        }

        public void ResetBattleResult(){
            this.BattleResult = null;
            this.ClanBattleResult = null;
            this.OutpostBattleResult = null;
            this.Result = null;
        }
        #endregion
    }
}