using Lean.Localization;
using NTPackage.Functions;
using NTPackage.UI;
using Rubik.BattleEngine;
using Rubik.CharacterGear;
using Rubik.Combat;
using Rubik.Common.AudioHelper;
using Rubik.Config;
using Rubik.Myrk.Arena;
using Rubik.Myrk.Battle;
using Rubik.Myrk.BattleTeam;
using Rubik.Myrk.Monster;
using Rubik.Myrk.Skill;
using Rubik.UI;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LineupHeroUI : PopupUI
{
    public List<MonsterOnMapCardItem> monsterOnMapCardItems;
    public TextMeshProUGUI TextPower;
    public Animator Anim;
    [SerializeField] SkeletonGraphic characterSkin;
    public SlotInventory[] lsSlotGears;
    OpponentData memberData;
    public GameObject attackGo;
    public int indexAttack = -1;
    public GearStats gearstats;
    public ListSkillItemUI listSkillItemUI;

    protected override void Start()
    {
        base.Start();
    }
    public override void OnUI(object data = null, bool isDefaultSound = true)
    {
        base.OnUI(data, isDefaultSound);
        attackGo.SetActive(indexAttack != -1 ? true : false);
        memberData = (OpponentData)data;
        BattleEngineController.Instance.enemyData = memberData;
        AssetLoader.Instance.MixSkinWithGearsUI(characterSkin, memberData.UserData.BattleTeam.Gears.ToList().Select(gear => (int)gear.Index).ToArray());
        SetUpData(memberData);
    }
    public override void UpdateData(object data = null)
    {
        base.UpdateData(data);
    }
    public override void ScriptOffUI()
    {
        this.Anim.Play(MonsterOnMapUIConfig.OffUI);
        StartCoroutine(this.OffPlayerMailAnim());
    }
    public void SetUpData(OpponentData data)
    {
        foreach(var monster in monsterOnMapCardItems)
        {
            MonsterData monsterData = null;
            monster.SetData(monsterData);
        }
        foreach(var temp in data.UserData.BattleTeam.Cards)
        {
            if (!string.IsNullOrEmpty(temp._id))
            {
                MonsterData monsterData = new MonsterData(temp.Index, temp.Level, temp.Star, 1);
                this.monsterOnMapCardItems[temp.Slot].SetData(monsterData);
            }
        }
        foreach (var gear in lsSlotGears)
        {
            gear.item.gameObject.SetActive(false);
        }
        foreach (var gear in data.UserData.BattleTeam.Gears)
        {
            CharacterGear charGear = new CharacterGear();
            charGear.Index = gear.Index;
            charGear._id = gear._id;
            charGear.Lv = gear.Level;
            charGear.Rarity = gear.Rarity;
            var gearData = CharacterGearManager.Instance.GetGearDataByIndex(gear.Index);
            lsSlotGears[gearData.Slot].item.SetData(charGear);
            lsSlotGears[gearData.Slot].gameObject.SetActive(true);
        }
        TextPower.text = BattleTeamManager.Instance.GetPower(data.UserData.BattleTeam).ToString();
        var temp1 = BattleTeamManager.Instance.GetGearStatsBattle(data.UserData.BattleTeam);
        Debug.Log("Stast  : " + temp1.HeroAtk + "   " + temp1.TeamHp);
        gearstats.SetInfo(temp1.TeamHp,temp1.HeroAtk,temp1.TeamSpeed);
        this.listSkillItemUI.SetData(CharacterGearManager.Instance.GetGearSkillLv(data.UserData.BattleTeam.Gears));
       OnButtonTabs();
    }
    public IEnumerator OffPlayerMailAnim()
    {
        AnimationClip[] clips = this.Anim.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == MonsterOnMapUIConfig.OffUI)
            {
                yield return new WaitForSeconds(clip.length);
                break;
            }
        }
        base.ScriptOffUI();
    }
    public void Attack()
    {
        if (memberData.IsWin)
        {
            HUDCanvas.Instance.ShowNotification(LeanLocalization.GetTranslationText("opponent_mess"));
            return;
        }
        else if (ArenaManager.Instance.GetUserArenaTicketItem().ticket <= 0)
        {
            HUDCanvas.Instance.ShowNotification(LeanLocalization.GetTranslationText("not_enough_arena_tickets"));
            return;
        }

        StartCoroutine(ArenaManager.Instance.IEAttackOpponent(memberData.UserID,false, () =>
        {
            // SceneController.Instance.LoadScene(SceneConfig.Battle_Screen);
            PopupManager.Instance.OnUI(PopupCode.BattleLoadingUI, null, (popup) =>
            {
                BattleLoadingUI battleLoadingUI = popup as BattleLoadingUI;
                battleLoadingUI.SetData(BattleType.Map, memberData.UserData, null);
                OffUI();
                StartCoroutine(NTFunction.WaitSecond(1f, () =>
                {
                    SceneController.Instance.LoadScene(SceneConfig.Battle_Screen);
                }));
            });
            //UpdateData(null);
            // PopupManager.Instance.OffAllPopupUI();
        }));
    }
    public void OnButtonTabs()
    {
        
        foreach (SlotInventory slot in lsSlotGears)
        {
            slot.item.gameObject.SetActive(false);
        }
        foreach (GearShortTeam temp in memberData.UserData.BattleTeam.Gears)
        {
            CharacterGear gear = new CharacterGear();
            gear._id = temp._id;
            gear.Index = temp.Index;
            gear.Rarity = temp.Rarity;
            gear.Lv = temp.Level;
            gear.GearData = CharacterGearManager.Instance.GetGearDataByIndex(temp.Index);
            Debug.Log(JsonUtility.ToJson(gear));
            lsSlotGears[gear.Slot()].item.gameObject.SetActive(true);
            lsSlotGears[gear.Slot()].item.SetData(gear);
           // gearIDs[gear.Index] = gear._id;
            lsSlotGears[gear.Slot()].item.GetComponent<Button>().onClick.RemoveAllListeners();
            lsSlotGears[gear.Slot()].item.GetComponent<Button>().onClick.AddListener(() =>
            {
                //AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
                //PopupManager.Instance.OnUI(PopupCode.GearInfo_UI, gear, (popupUI) =>
                //{
                //    GearInfo_UI gearInfoUI = popupUI as GearInfo_UI;
                //    gearInfoUI.ShowItem(gear._id);
                //});
            });
        }
       

    }
}
