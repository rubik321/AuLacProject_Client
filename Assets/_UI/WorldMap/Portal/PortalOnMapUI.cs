using GOA.Portal;
using NTPackage.EventDispatcher;
using NTPackage.Functions;
using NTPackage.UI;
using Rubik.CardPlayer;
using Rubik.Common.AudioHelper;
using Rubik.Myrk.Guide;
using Rubik.Myrk.Skill;
using Rubik.UI;
using Rubik.UI.Statitic;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rubik.Myrk.Portal
{
    public class PortalOnMapUI : PopupUI
    {
        public long PointID;
        public Image Holder;
        public TextMeshProUGUI TextTitle;

        public CardPlayerIndex CardPlayerIndex;
        public CardPlayerData CardPlayerData;
        public ListSkillItemUI ListSkillItemUI;
        public StarUI StarUI;

        public PortalOnMapRankTab PortalOnMapRankTab;
        public NTButtonEffect BtnOnTabRank;

        public PortalOnMapRewardTab PortalOnMapRewardTab;
        public NTButtonEffect BtnOnTabReward;

        public PortalAttackData PortalAttackData;
        public PortalBossRank PortalBossRank;

        public TextMeshProUGUI TextAtk;
        public TextMeshProUGUI TextHp;

        public List<Sprite> ElementCardLists;
        public Image ElementCardImage;
        public TextMeshProUGUI TextPortalName;

        public TextMeshProUGUI TextDes;

        public int Level;
        public bool IsClose = false;
        public bool IsEnd = false;

        public long HP;

        public Coroutine CorUpdateData; // Every 3 Second

        [Header("Test")]
        public long TestPointID = 0;
        [NTButton]
        public void TestOnUI()
        {
            this.OnUI((object)this.TestPointID);
        }

        public override void OnUI(object data = null, bool isDefaultSound = true)
        {
            this.PointID = (long)data;
            this.IsClose = true;
            this.IsEnd = false;
            this.Level = 0;
            this.TextTitle.text = Lean.Localization.LeanLocalization.GetTranslationText("portal_title", "Portal") + " #" + this.PointID;
            if (this.CorUpdateData != null)
            {
                StopCoroutine(this.CorUpdateData);
            }
            this.SetLocation(Lean.Localization.LeanLocalization.GetTranslationText("portal_unknow_local", "Myrk"));
            this.CorUpdateData = StartCoroutine(this.IEUpdateData());
            this.SetData(this.PointID);
            base.OnUI(data, isDefaultSound);
        }

        public override void ScriptOffUI()
        {
            base.ScriptOffUI();
            if (this.CorUpdateData != null)
            {
                StopCoroutine(this.CorUpdateData);
            }
            this.PortalOnMapRewardTab.OffUI();
            this.PortalOnMapRankTab.OffUI();
            EventListenerManager.instance.RemoveListener(EventCode.UpdatePortalAttackData, "PortalOnMapUI");
            EventListenerManager.instance.RemoveListener(EventCode.UpdatePortalBossRank, "PortalOnMapUI");
        }

        public void SetData(long pointID)
        {
            this.PointID = pointID;
            this.CardPlayerIndex = PortalWorldMapManager.Instance.GetBossCardPlayerIndex(this.PointID);
            this.CardPlayerData = CardPlayerManager.Instance.GetCardPlayerDataByIndex(this.CardPlayerIndex);
            this.TextPortalName.text = CardPlayerManager.Instance.GetCardName(this.CardPlayerIndex);
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.Holder.transform);
            RectTransform skeletonGraphic = CardPlayerManager.Instance.InstantiatePlayerSkeletonGraphic(this.CardPlayerIndex);
            skeletonGraphic.SetParent(this.Holder.transform);
            NTFunction.ResetPosition(skeletonGraphic);
            this.PortalAttackData = PortalWorldMapManager.Instance.GetPortalAttackData(this.PointID);
            this.PortalBossRank = PortalWorldMapManager.Instance.GetPortalBossRank(this.PointID);
            this.ElementCardImage.sprite = this.ElementCardLists[(int)this.CardPlayerData.Origin % this.ElementCardLists.Count];
            EventListenerManager.instance.Register(EventCode.UpdatePortalAttackData, "PortalOnMapUI", (data) =>
            {
                this.OnUpdatePortalAttackData((long)data);
            });
            EventListenerManager.instance.Register(EventCode.UpdatePortalBossRank, "PortalOnMapUI", (data) =>
            {
                this.OnUpdatePortalBossRank((long)data);
            });
        }

        public void SetLocation(string location){
            string str = Lean.Localization.LeanLocalization.GetTranslationText("portal_des", "In the ancient lands of {0}, a celestial rift known as the Astral Gate awakens - a gate between realms where time itself trembles. Born of forgotten constellations and sealed by ancient guardians, it hums with the echoes of countless worlds long lost to history.\nWhen the Gate opens, the skies blaze with light, and from within emerges the Gatekeeper - a being forged of starlight and shadow, bound to test the courage of all who dare approach. Each strike against it echoes through the void, a trial not just of strength, but of resolve.");
            str = string.Format(str, location);
            this.TextDes.text = str;
        }

        public void _OnclickAttack()
        {
            // StartCoroutine(PortalWorldMapManager.Instance.IEAttack(this.PointID, (result) =>
            // {
            //     BattleEngine.BattleEngineController.Instance.BattleResult = result;
            //     BattleEngine.BattleEngineController.Instance.isCLanbattle = false;
            //     bl_SceneLoaderManager.LoadScene("Campaign");
            //     OffUI();
            //     // TODO: Show result
            // }));
        }

        public override void UpdateData(object data = null)
        {
            base.UpdateData(data);
            if (this.PortalAttackData == null || this.PortalBossRank == null)
            {
                this.PortalAttackData = new PortalAttackData();
                this.PortalAttackData.PointID = this.PointID;
            }
            long timeReset = PortalWorldMapManager.Instance.GetTimeResetPortal(this.PortalAttackData.OpenTime);
            if (timeReset > 0)
            {
                if (this.IsClose)
                {
                    this._OnTabRank();

                }
                this.IsClose = false;
                this.Level = this.PortalAttackData.Level;
                if (PortalWorldMapManager.Instance.GetRemainingTimeAttackPortal(this.PortalAttackData.OpenTime) > 0)
                {
                    this.IsEnd = false;
                }
                else
                {
                    this.IsEnd = true;
                }
            }
            else
            {
                this.IsEnd = true;
                this.IsClose = true;
                this._OnTabReward();
            }

            if (this.IsClose)
            {
                this.BtnOnTabRank.gameObject.SetActive(false);
                this.BtnOnTabReward.gameObject.SetActive(false);
                this.HP = PortalWorldMapManager.Instance.GetPortalHp(this.Level);
                this.TextHp.text = NTFunction.FormatNumber(this.HP);
            }
            else
            {
                this.BtnOnTabRank.gameObject.SetActive(true);
                this.BtnOnTabReward.gameObject.SetActive(true);
                this.HP = this.PortalAttackData.HP;
                if (this.HP <= 0) this.HP = 0;
                this.TextHp.text = NTFunction.FormatNumber(this.HP);
            }
            this.TextAtk.text = NTFunction.FormatNumber(PortalWorldMapManager.Instance.GetPortalAttack(this.Level));

            this.StarUI.SetStar(this.Level);
            CardSkillLv cardSkillLv = CardPlayerManager.Instance.GetCardSkill(this.CardPlayerIndex, this.Level);
            this.ListSkillItemUI.SetData(cardSkillLv);
            this.PortalOnMapRewardTab.UpdateData();
            this.PortalOnMapRankTab.UpdateData();
        }

        public void OnUpdatePortalAttackData(long pointID)
        {
            if (pointID != this.PointID){
                NTLog.LogMessage("OnUpdatePortalAttackData: " + pointID + " != " + this.PointID);
                return;
            }
            this.PortalAttackData = PortalWorldMapManager.Instance.GetPortalAttackData(this.PointID);
            this.UpdateData();
        }

        public void OnUpdatePortalBossRank(long pointID)
        {
            if (pointID != this.PointID){
                NTLog.LogMessage("OnUpdatePortalBossRank: " + pointID + " != " + this.PointID);
                return;
            }
            this.PortalBossRank = PortalWorldMapManager.Instance.GetPortalBossRank(this.PointID);
            this.UpdateData();
        }

        public void _OnTabReward()
        {
            AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
            this.PortalOnMapRewardTab.OnUI();
            this.BtnOnTabReward.Chose();
            this.PortalOnMapRankTab.OffUI();
            this.BtnOnTabRank.Unchose();
        }

        public void _OnTabRank()
        {
            AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
            this.PortalOnMapRankTab.OnUI();
            this.BtnOnTabRank.Chose();
            this.PortalOnMapRewardTab.OffUI();
            this.BtnOnTabReward.Unchose();
        }

        public void OnChangeLevel(int level)
        {
            AudioCtrl.Instance.Play(AudioName.UI_Button_Default);
            this.Level = level;
            this.UpdateData();
            this.PortalOnMapRewardTab.UpdateData();
        }

        public IEnumerator IEUpdateData()
        {
            while (true)
            {
                yield return new WaitForSeconds(3f);
                PortalWorldMapManager.Instance.SendMsgGetPortalAttackData(this.PointID);
                PortalWorldMapManager.Instance.SendMsgGetPortalBossRank(this.PointID);
            }
        }

        public void _ConclickInfo(){
            PopupManager.Instance.OnUI(PopupCode.GuideUI, null, (popup) =>
            {
                GuideUI guideUI = popup as GuideUI;
                string title = Lean.Localization.LeanLocalization.GetTranslationText("portal_title", "Astral Gate");
                string content = Lean.Localization.LeanLocalization.GetTranslationText("portal_guide", "A mysterious gateway connecting two worlds. Inside awaits the Gatekeeper - a mighty guardian threatening your realm. The stronger the Gatekeeper, the greater the challenge… and the rewards.\n<sprite name=\"icon_portal_brown\">To enter, you must use a <b>Astral Gate Key</b>. <b>5 Astral Gate Keys</b> are restored daily at <b>00:00 (GMT+0)</b>. Unused keys <b>cannot be carried over</b> to the next day.\n<sprite name=\"icon_portal_brown\">Once opened, the gate <b>endures for 24 hours</b>.\n<sprite name=\"icon_portal_brown\">Each challenge attempt <b>recovers every hour</b>, with a <b>maximum of 20 attempts</b> stored.\n<sprite name=\"icon_portal_brown\">The gate can be <b>reopened 1 hour</b> after it closes.\n<sprite name=\"icon_portal_brown\"><b>Rewards are sent via mail</b> after the Gatekeeper is defeated.\n<sprite name=\"icon_portal_brown\"><b>Battle rankings determine your final treasure</b> - only the most relentless will rise to the top before the gate vanishes.");
                guideUI.SetData(title, content);
            });
        }
    }
}