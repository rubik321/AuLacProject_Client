using DG.Tweening;
using GOA.Item;
using NTPackage.Functions;
using NTPackage.UI;
using Rubik.CharacterGear;
using Rubik.Common.AudioHelper;
using Rubik.DataType;
using Rubik.Format;
using Rubik.ItemPlayer;
using Rubik.Myrk.Skill;
using Rubik.UI.Statitic;
using Rubik.UserDataPlayer;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace Rubik.CharacterGear
{
    using Rubik.ItemPlayer;
    using Rubik.Myrk.BattleTeam;

    public class GearInfo_UI : PopupUI
    {
        public ParticleSystem upgradeEffect;
        Action equipAction, sellAction;
        int indexSelected;

        public string GearID;
        [SerializeField] CharacterGear gear;
        public CharacterGearData gearData;
        public TextMeshProUGUI txtTitle;
        public CharacterGearItemUI CharacterGearItemUI;
        public BarStatUI MainStat, SubStat, StatIncrease;
        public TextMeshProUGUI TxtSkillName;
        public TextMeshProUGUI TxtSkillDescription;
        public SkillItemUI SkillItemUI;
        public List<ItemDataBarUI> PriceUpgrade; // 2 item
        public ItemDataBarUI PriceSell;
        public TextMeshProUGUI TxtSub;
        public NTButtonEffect BtnUpgrade, BtnEquip, BtnSell;
        public TextMeshProUGUI TxtEquip;
        public EquipedIconUI EquipedIconUI;

        public override void ScriptOffUI()
        {
            base.ScriptOffUI();
            this.GearID = "";
            this.EquipedIconUI.Clear();
        }

        public void SetData(string gearID, int teamIndex, Action equipAction = null, Action sellAction = null)
        {
            this.GearID = gearID;
            this.indexSelected = teamIndex;
            this.equipAction = equipAction;
            this.sellAction = sellAction;
            this.UpdateData();
        }

        public override void UpdateData(object data = null)
        {
            base.UpdateData(data);
            this.gear = CharacterGearManager.Instance.GetCharacterGearByID(GearID);
            if (this.gear == null || this.gear._id == null || this.gear._id.Length == 0)
            {
                return;
            }
            CharacterGearManager.Instance.UpdateCacheCharacterGear(this.gear);
            this.TxtSub.text = Lean.Localization.LeanLocalization.GetTranslationText("gear_sub_upgrade", "**Upgrade your gear to boost your hero's power**");
            this.TxtSub.color = NTFunction.StringHexToColor(ItemDataCf.HexColorBrown);
            this.gearData = CharacterGearManager.Instance.GetGearDataByIndex(this.gear.Index);
            this.txtTitle.text = CharacterGearManager.Instance.GetGearNameByIndex(this.gear.Index);
            this.CharacterGearItemUI.SetData(this.gear);
            this.MainStat.SetData(CharacterGearManager.Instance.GetMainStat(this.gear.Index, this.gear.Rarity, this.gear.Lv));
            this.SubStat.SetData(CharacterGearManager.Instance.GetSubStats(this.gear.Rarity));
            this.StatIncrease.SetData(CharacterGearManager.Instance.GetMainStatsIncrease(this.gear.Index, this.gear.Rarity, this.gear.Lv), true);
            this.StatIncrease.TextValue.color = NTFunction.StringHexToColor(ItemDataCf.HexColorGreen);
            if (CharacterGearManager.Instance.IsActiveSkillGear(this.gear.Index))
            {
                GearSkillActiveLv gearSkillActiveLv = CharacterGearManager.Instance.GetGearSkillActiveLv(CharacterGearManager.Instance.GetTypeActiveGearByIndex(this.gear.Index), this.gear.Rarity);
                this.SkillItemUI.SetActiveGearSkill(gearSkillActiveLv);
                this.TxtSkillName.text = CharacterGearManager.Instance.GetGearSkillName(this.gear.Index);
                this.TxtSkillDescription.text = CharacterGearManager.Instance.GetGearSkillDetail(this.gear.Index, this.gear.Rarity);
            }
            else
            {
                GearSkillPassiveLv gearSkillPassiveLv = CharacterGearManager.Instance.GetGearSkillPassiveLv(CharacterGearManager.Instance.GetTypePassiveGearByIndex(this.gear.Index), this.gear.Rarity);
                this.SkillItemUI.SetPassiveGearSkill(gearSkillPassiveLv);
                this.TxtSkillName.text = CharacterGearManager.Instance.GetGearSkillName(this.gear.Index);
                this.TxtSkillDescription.text = CharacterGearManager.Instance.GetGearSkillDetail(this.gear.Index, this.gear.Rarity);
            }
            ItemData[] costDatas = CharacterGearManager.Instance.GetGearUpgradeLvCost(this.gear.Lv);
            for (int i = 0; i < this.PriceUpgrade.Count; i++)
            {
                this.PriceUpgrade[i].gameObject.SetActive(false);
            }
            this.BtnUpgrade.Chose();
            for (int i = 0; i < costDatas.Length; i++)
            {
                this.PriceUpgrade[i].gameObject.SetActive(true);
                this.PriceUpgrade[i].SetData(costDatas[i]);
                this.PriceUpgrade[i].AddMinus();
                double costAmount = costDatas[i].Amount;
                double itemAmount = ItemDataManager.Instance.GetItem(costDatas[i].Type).Amount;
                this.PriceUpgrade[i].Amount.text =FormatData.GetFriendlyShortNumber(costAmount) + " / " + FormatData.GetFriendlyShortNumber(itemAmount);;
                if (costAmount > itemAmount)
                {
                    this.PriceUpgrade[i].Amount.color = NTFunction.StringHexToColor(ItemDataCf.HexColorRed);
                    this.TxtSub.color = NTFunction.StringHexToColor(ItemDataCf.HexColorRed);
                    this.BtnUpgrade.Unchose();
                }
                else
                {
                    this.PriceUpgrade[i].Amount.color = NTFunction.StringHexToColor(ItemDataCf.HexColorGreen);
                }
            }

            this.PriceSell.SetData(CharacterGearManager.Instance.GetGearSellPrice());
            this.PriceSell.AddPlus();
            this.EquipedIconUI.SetData(this.gear.Teams);
            if (this.gear.IsEquiped)
            {
                this.BtnSell.Unchose();
                this.BtnEquip.Unchose();
            }
            else
            {
                this.BtnSell.Chose();
                this.BtnEquip.Chose();
            }
            if (this.gear.IsEquipedByIndex(this.indexSelected))
            {
                this.BtnEquip.Unchose();
                this.TxtEquip.text = Lean.Localization.LeanLocalization.GetTranslationText("equipped");
            }
            else
            {
                this.BtnEquip.Chose();
                this.TxtEquip.text = Lean.Localization.LeanLocalization.GetTranslationText("equip");
            }

            if (this.gear.Lv >= UserDataManager.Instance.GetPlayerLevel().level)
            {
                this.TxtSub.color = NTFunction.StringHexToColor(ItemDataCf.HexColorRed);
                this.TxtSub.text = Lean.Localization.LeanLocalization.GetTranslationText("gear_sub_max_level", "**Gear level cannot exceed player level**");
                this.BtnUpgrade.Unchose();
            }
        }

        public void UpgradeGear()
        {
            if (!this.BtnUpgrade.IsChose()) return;
            StartCoroutine(CharacterGearManager.Instance.IEUpgradeLvGear(gear._id, () =>
            {
                AppsFlyerManager.TrackingEvent(AppsflyerEvents.equipment_upgraded, 1, 1);
                AppsFlyerManager.TrackingEvent(AppsflyerEvents.money_spent, "upgrade_gear", 100);
                // gear = CharacterGearManager.Instance.GetCharacterGearByID(gear._id);
                this.UpdateData();
                upgradeEffect.Play();
                PopupManager.Instance.GetPopupUIByCode(NTPackage.UI.PopupCode.CharacterGear_UI).GetComponent<CharacterGearUI>().LoadAllGear();
            }));
        }

        public void SellGear()
        {
            if (!this.BtnSell.IsChose()) return;
            AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
            PopupManager.Instance.OnUI(PopupCode.ConfirmUI, gear, (popupUI) =>
            {
                ComfirmUI confirmUI = popupUI as ComfirmUI;

                confirmUI.SetAction(() =>
                {
                    StartCoroutine(CharacterGearManager.Instance.IESellGear(gear._id, () =>
                    {

                        this.BtnEquip.Unchose();
                        this.BtnSell.Unchose();
                        this.BtnUpgrade.Unchose();
                        if (sellAction != null)
                            sellAction.Invoke();
                        PopupManager.Instance.GetPopupUIByCode(NTPackage.UI.PopupCode.CharacterGear_UI).GetComponent<CharacterGearUI>().LoadAllGear();
                        confirmUI.OffUI();

                    }));
                });

            });


        }
        public void EquipGear()
        {
            if (!this.BtnEquip.IsChose()) return;
            AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
            equipAction.Invoke();
            this.UpdateData();
        }

    }
}