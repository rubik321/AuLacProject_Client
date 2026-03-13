using CodeHelper;
using Rubik.Combat;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rubik.UI
{
    public class SkillElement : MonoBehaviour, IMessageHandle
    {
        [SerializeField] GameObject highlight, hpCost, manaCost;
        [SerializeField] Image icon;
        [SerializeField] TMP_Text textCost;
        string skillId = "";
        Action<string> onClick;

        private void OnEnable()
        {
            if (string.IsNullOrEmpty(skillId))
            {
                return;
            }
            SkillStat cost = ActionPool.Instance.GetActionSO(skillId).cost;
            switch (cost.statType)
            {
                case StatType.HP:
                    {
                        if (cost.isFixed)
                        {
                            icon.GetComponent<Button>().interactable = CharacterManager.Instance.Player.CharacterState.hp >= cost.rate;
                        }
                        else
                        {
                            icon.GetComponent<Button>().interactable = CharacterManager.Instance.Player.CharacterState.hp > cost.rate * CharacterManager.Instance.Player.CharacterState.maxHp;
                        }
                        hpCost.SetActive(true);
                        manaCost.SetActive(false);
                        break;
                    }
                case StatType.MP:
                    {
                        if (cost.isFixed)
                        {
                            icon.GetComponent<Button>().interactable = CharacterManager.Instance.Player.CharacterState.mp >= cost.rate;
                        }
                        else
                        {
                            icon.GetComponent<Button>().interactable = CharacterManager.Instance.Player.CharacterState.mp > cost.rate * CharacterManager.Instance.Player.CharacterState.maxHp;
                        }
                        hpCost.SetActive(false);
                        manaCost.SetActive(true);
                        break;
                    }
            }
            MessageManager.AddSubcriber<CodeHelper.MessageCollection.OnClickSkill>(this);
            highlight.SetActive(false);
        }

        private void OnDisable()
        {
            MessageManager.RemoveSubcriber<CodeHelper.MessageCollection.OnClickSkill>(this);
        }

        public void SetupData(string skillId, Action<string> onClick)
        {
            BaseActionSO skill = ActionPool.Instance.GetActionSO(skillId);
            if (skill == null)
                Debug.LogError(skillId);
            icon.sprite = SpriteHelper.Instance.GetSprite(skillId);
            if (skill.cost.isFixed)
            {
                textCost.text = skill.cost.rate.ToString();
            }
            else
            {
                textCost.text = skill.cost.rate * 100 + "% ";
            }
            this.onClick = onClick;
            this.skillId = skillId;
        }

        public void OnClick(int index)
        {
            //onClick?.Invoke(skillId);
            //MessageManager.SendMessage(new Message(nameof(CodeHelper.MessageCollection.OnClickSkill)));
           // highlight.SetActive(true);
            UIGamePlayBattle.Instance.ShowPanelButton();
            UIGamePlayBattle.Instance.ShowSkill(index);
            //GameController.Instance.ChangeState();
            GameController.Instance.indexSkill = index;
            GameController.Instance.SkillPlayer();
        }

        public void Handle(Message message)
        {
            highlight.SetActive(false);
        }
    }
}
