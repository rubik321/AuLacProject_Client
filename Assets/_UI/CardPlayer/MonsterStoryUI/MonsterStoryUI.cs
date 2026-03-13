using System.Collections;
using System.Collections.Generic;
using NTPackage.UI;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using NTPackage.Functions;
using Rubik.Myrk.Skill;
using Rubik.UI.Statitic;
using Rubik._2DGPS.Card;
using Rubik.BattleEngine;

namespace Rubik.CardPlayer
{
    public class MonsterStoryAnim
    {
        public const string Anim_OnUI = "OnUI";
        public const string Anim_OffUI = "OffUI";
    }

    public class MonsterStoryUI : PopupUI
    {
        public TextMeshProUGUI Title;
        public TextMeshProUGUI Description;
        public Transform ModelHolder;
        public Image Element;
        public Animator Anim;
        public ListSkillItemUI ListSkillItemUI;
        public ListStatUI ListStatUI;
        public StarUI StarUI;

        
        public override void OnUI(object data = null, bool isDefaultSound = true)
        {
            base.OnUI(data, isDefaultSound);
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.ModelHolder);
            this.Title.text = "";
            this.Description.text = "";
            this.Element.sprite = null;
            this.Anim.Play(MonsterStoryAnim.Anim_OnUI);
        }

        public override void OffUI()
        {
            if(this.ScreenDim != null) this.ScreenDim.gameObject.SetActive(false);
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.ModelHolder);
            this.Anim.Play(MonsterStoryAnim.Anim_OffUI);
            StartCoroutine(this.IEOffUI());
        }

        public void SetData(CardPlayerIndex index, int star = 0, int level = 0, float scale = 1)
        {
            CardPlayerData cardPlayerData = CardPlayerManager.Instance.GetCardPlayerDataByIndex(index);
            this.Title.text = CardPlayerManager.Instance.GetCardName(index) + " Lv. " + (level + 1);
            this.Description.text = CardPlayerManager.Instance.GetCardDescription(index);
            this.Element.sprite = CardPlayerManager.Instance.GetOriginSpriteCircle(cardPlayerData.Origin);
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.ModelHolder);
            Transform model = CardPlayerManager.Instance.InstantiatePlayerSkeletonGraphic(index);
            model.SetParent(this.ModelHolder);
            NTFunction.ResetPosition(model);

            (long atk, long def, long hp, long spd) = CardPlayerManager.Instance.GetCardStatBaseLv_Star(index, level, star);
            atk = (long)(atk * scale);
            def = (long)(def * scale);
            hp = (long)(hp * scale);
            spd = (long)(spd * scale);

            BattleStats baseStats = new BattleEngine.BattleStats();
            baseStats.ATK = atk;
            baseStats.DEF = def;
            baseStats.HP = hp;
            baseStats.SPD = spd;

            // Skill
            CardSkillLv cardSkillLv = CardPlayerManager.Instance.GetCardSkill(index, star);
            this.ListSkillItemUI.SetData(cardSkillLv);

            BattleStats skillStats = new BattleEngine.BattleStats();
            foreach (CardSkillPassiveLv item in cardSkillLv.Passive)
            {
                if(item.IsLock) continue;
                BattleEngine.BattleStats.Add(skillStats, CardPlayerManager.Instance.GetCardPlayerPassiveSkillStats(item, baseStats));
            }

            BattleStats totalStats = new BattleEngine.BattleStats();

            BattleEngine.BattleStats.Add(totalStats, baseStats);
            BattleEngine.BattleStats.Add(totalStats, skillStats);


            this.ListStatUI.SetData(totalStats.ATK, totalStats.DEF, totalStats.SPD, totalStats.HP);
            this.StarUI.SetStar(star);
        }

        public IEnumerator IEOffUI(){
            AnimationClip[] clips = this.Anim.runtimeAnimatorController.animationClips;
            foreach (AnimationClip clip in clips)
            {
                if (clip.name == MonsterStoryAnim.Anim_OffUI)
                {
                    yield return new WaitForSeconds(clip.length);
                    break;
                }
            }
            base.OffUI();
        }
    }
}