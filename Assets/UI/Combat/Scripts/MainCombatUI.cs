using CodeHelper;
using DG.Tweening;
using Rubik.Combat;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pixelplacement;
using GOA.UserData;

namespace Rubik.UI
{
    public class MainCombatUI : Pixelplacement.Singleton<MainCombatUI>, IMessageHandle
    {
        [SerializeField] GameObject panelAction;
        [SerializeField] StateMachine panelSM;
        [SerializeField] Transform itemElementHolder;
        [SerializeField] ItemElement itemElementPrefab;
        [SerializeField] Transform effectElementHolder;
        [SerializeField] EffectElement effectElementPrefab;
        [SerializeField] Transform skillElementHolder;
        [SerializeField] SkillElement skillElementPrefab;
        //[SerializeField] CharacterInfoElement[] enemyInfoElements;
        [SerializeField] TurnElement[] turnElements;
        [SerializeField] ButtonGroup[] actionButtons;
        [SerializeField] GameObject bossHealthBG;
        [SerializeField] Image bossHealthBar;
        [SerializeField] TMPro.TMP_Text textBossHealth;
        CharacterCombatState playerState;
        List<ItemElement> itemElements = new List<ItemElement>();
        List<EffectElement> effectElements = new List<EffectElement>();
        List<SkillElement> skillElements = new List<SkillElement>();
        int maxHP;
        int maxMP;
        DG.Tweening.Core.TweenerCore<Vector2, Vector2, DG.Tweening.Plugins.Options.VectorOptions> turnChangeTween = null;
        public static ChooseActionState CurrentState { get; private set; }
        public static string CurrentActionId { get; set; }
        public static string CurrentItemId { get; set; }

      
        protected override void OnRegistration()
        {
            bossHealthBG.SetActive(false);
        }

        private void OnEnable()
        {
            MessageManager.AddSubcriber<CodeHelper.MessageCollection.OnCharacterStartTurn>(this);
            MessageManager.AddSubcriber<CodeHelper.MessageCollection.OnGameStart>(this);
            MessageManager.AddSubcriber<CodeHelper.MessageCollection.OnTurnStateActivate>(this);
            MessageManager.AddSubcriber<CodeHelper.MessageCollection.OnGameLose>(this);
            MessageManager.AddSubcriber<CodeHelper.MessageCollection.OnGameWin>(this);
            if (UserData.Instance.movespeed == 1)
            {
                //UserData.Instance.movespeed = 2;
               
                Time.timeScale = 1;
                btnSpeed.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "X2";

            }
            else
            {
                //UserData.Instance.movespeed = 1;
                
                Time.timeScale = 2;
                btnSpeed.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "X1";
            }
        }

        private void OnDisable()
        {
            MessageManager.RemoveSubcriber<CodeHelper.MessageCollection.OnCharacterStartTurn>(this);
            MessageManager.RemoveSubcriber<CodeHelper.MessageCollection.OnGameStart>(this);
            MessageManager.RemoveSubcriber<CodeHelper.MessageCollection.OnTurnStateActivate>(this);
            MessageManager.RemoveSubcriber<CodeHelper.MessageCollection.OnGameLose>(this);
            MessageManager.RemoveSubcriber<CodeHelper.MessageCollection.OnGameWin>(this);
        }

        public void Handle(Message message)
        {
            switch (message.type)
            {
                case nameof(CodeHelper.MessageCollection.OnCharacterStartTurn):
                    {
                        //foreach (CharacterInfoElement characterInfo in enemyInfoElements)
                        //{
                        //    characterInfo.SetNextTurnActive(characterInfo.instanceId == CombatManager.Instance.GetNextTurn(1).characterInstanceId);
                        //    // This may override line above if a character get 2 turns back to back
                        //    characterInfo.SetTurnActive(characterInfo.instanceId == CombatManager.Instance.CurrentCharacterState.data.characterInstanceId);
                        //}
                        if (turnChangeTween == null || !turnChangeTween.IsPlaying())
                        {
                            SetTurnElements();
                            break;
                        }
                        else
                        {
                            turnChangeTween.OnComplete(() =>
                            {
                                SetTurnElements();
                            });
                        }
                        break;
                    }
                case nameof(CodeHelper.MessageCollection.OnGameStart):
                    {
                        var allies = CombatManager.Instance.GetAliveAllies();
                        // TODO: Make this real data
                        SetupData(null, allies[0].maxHp, allies[0].maxMp);
                        break;
                    }
                case nameof(CodeHelper.MessageCollection.OnTurnStateActivate):
                    {
                        if ((Type)message.data[0] == typeof(StateWait))
                        {
                            turnElements[0].GetComponent<RectTransform>().DOKill();
                            turnElements[0].transform.DOScale(Vector3.zero, 0.3f);
                            turnChangeTween = turnElements[0].GetComponent<RectTransform>().DOSizeDelta(new Vector2(0, 0), 0.33f);
                            break;
                        }

                        panelSM.ChangeState(0);
                        panelAction.SetActive((Type)message.data[0] == typeof(StateChooseAction) && CombatManager.Instance.CurrentCharacterState.data.isPlayer);
                        if (panelAction.activeSelf)
                        {
                            OnClickAttack();
                            ActivateButton("attack");
                        }
                        break;
                    }
                case nameof(CodeHelper.MessageCollection.OnGameLose):
                case nameof(CodeHelper.MessageCollection.OnGameWin):
                    gameObject.SetActive(false);
                    break;

            }
        }

        public void SetupData(Sprite avatar, int maxHP, int maxMP)
        {
            panelSM.ChangeState(0);
            //this.avatar.sprite = avatar;
            this.maxHP = maxHP;
            this.maxMP = maxMP;
            var items = CharacterManager.Instance.Player.CharacterState.data.items;
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].count <= 0)
                    continue;
                if (itemElements.Count >= i)
                {
                    itemElements.Add(Instantiate(itemElementPrefab, itemElementHolder));
                }
                itemElements[i].SetupData(items[i].id, items[i].count, OnChooseItem);
            }
            for (int i = items.Count; i < itemElements.Count; i++)
            {
                itemElements[i].gameObject.SetActive(false);
            }

            CharacterCombatState player = CharacterManager.Instance.Player.CharacterState;
            for (int i = 2; i < player.data.skills.Count; i++)
            {
                skillElements.Add(Instantiate(skillElementPrefab, skillElementHolder));
                skillElements[^1].SetupData(player.data.skills[i], OnChooseSkill);
            }

            //List<CharacterCombatState> chars = CombatManager.Instance.GetAliveEnemies();
            //for (int i = 0; i < chars.Count; i++)
            //{
            //    enemyInfoElements[i].SetupData(chars[i].data.characterId, chars[i].data.characterInstanceId);
            //}
            playerState = CharacterManager.Instance.Player.CharacterState;
        }

        public void SetTurnElements()
        {
            turnElements[0].GetComponent<RectTransform>().sizeDelta = new Vector2(140, 140);
            turnElements[0].transform.localScale = Vector3.one;
            int clamp = 0;
            for (int i = 0; i < turnElements.Length; i++)
            {
                CharacterCombatState state;
                // On get to the game, current turn index = -1, cause Out of range exception
                try
                {
                    state = CombatManager.Instance.GetCharacterCombatState(CombatManager.Instance.GetNextTurn(i + clamp).characterInstanceId);
                }
                catch
                {
                    clamp = 1;
                    state = CombatManager.Instance.GetCharacterCombatState(CombatManager.Instance.GetNextTurn(i + clamp).characterInstanceId);
                }
                //state.data.characterId
                //turnElements[i].SetCharacterAva();
                //if (i > 0)
                //{
                //    turnElements[i].SetSide(state.data.isEnemy);
                //}
            }
        }

        public void UpdateItem(string id)
        {
            var items = CharacterManager.Instance.Player.CharacterState.data.items;
            int count = 0;
            foreach (var item in items)
            {
                if (item.id == id)
                {
                    count = item.count;
                    break;
                }
            }

            foreach (ItemElement itemElement in itemElements)
            {
                if (itemElement.ID == id)
                {
                    itemElement.UpdateQuantity(count);
                    return;
                }
            }

            if (count <= 0)
                return;
            // When item not exist yet, create it and update change
            itemElements.Add(Instantiate(itemElementPrefab, itemElementHolder));
            itemElements[^1].SetupData(id, count, OnChooseItem);
        }

        public void OnClickAttack()
        {
            if (CurrentState != ChooseActionState.Attack)
            {
                foreach (CharacterCombatState character in CombatManager.Instance.GetAliveCharacters())
                {
                    CharacterManager.Instance.GetCharacterObject(character.data.characterInstanceId).ResetArrow();
                }
            }

            CurrentState = ChooseActionState.Attack;
            CurrentActionId = CombatManager.Instance.CurrentCharacterState.data.AttackId;
            panelSM.ChangeState("PanelAttack");
        }

        public void OnClickSkill()
        {
            panelSM.ChangeState("PanelSkill");
            CurrentState = ChooseActionState.ChooseSkill;
            CurrentActionId = "";
        }

        public void OnChooseSkill(string id)
        {
            if (id != CurrentActionId)
            {
                foreach (CharacterCombatState character in CombatManager.Instance.GetAliveCharacters())
                {
                    CharacterManager.Instance.GetCharacterObject(character.data.characterInstanceId).ResetArrow();
                }
            }
            CurrentActionId = id;
        }

        public void OnClickDefense()
        {
            if (CurrentState != ChooseActionState.Attack)
            {
                foreach (CharacterCombatState character in CombatManager.Instance.GetAliveCharacters())
                {
                    CharacterManager.Instance.GetCharacterObject(character.data.characterInstanceId).ResetArrow();
                }
            }

            CurrentState = ChooseActionState.Defend;
            CurrentActionId = Constants.ID.DEFEND_ID;
            panelSM.ChangeState("PanelDefend");
        }

        public void OnClickItems()
        {
            CurrentState = ChooseActionState.ChooseItem;
            CurrentActionId = "";
            panelSM.ChangeState("PanelItem");
        }

        public void OnChooseItem(string id)
        {
            if (id != CurrentItemId)
            {
                foreach (CharacterCombatState character in CombatManager.Instance.GetAliveCharacters())
                {
                    CharacterManager.Instance.GetCharacterObject(character.data.characterInstanceId).ResetArrow();
                }
            }
            CurrentItemId = id;
        }

        public void OnClickFlee()
        {
            panelSM.ChangeState(0);
            CurrentState = ChooseActionState.Flee;
            UI.CombatUIController.Instance.ChangeState("FleeUI");
        }

        public void EnableBossHealthBar(int current, int maxHP)
        {
            bossHealthBG.SetActive(true);
            bossHealthBar.fillAmount = current * 1f / maxHP;
            textBossHealth.text = current + "/" + maxHP;
        }

        public void UpdateBossHealthBar(int current, int max)
        {
            bossHealthBar.fillAmount = current * 1f / max;
            textBossHealth.text = current + "/" + max;
        }


        // Assign to action buttons unityevent
        public void ActivateButton(string btnName)
        {
            foreach (ButtonGroup buttonGroup in actionButtons)
            {
                buttonGroup.activeBtn.SetActive(btnName == buttonGroup.name);
                buttonGroup.inactiveButton.SetActive(btnName != buttonGroup.name);
            }
        }

        public void UpdatePlayerStatus()
        {
            CharacterCombatState player = CharacterManager.Instance.Player.CharacterState;

            Dictionary<string, (int turn, int stack)> effectStack = new Dictionary<string, (int turn, int stack)>();
            foreach (EffectState effect in player.effects)
            {
                if (!effectStack.ContainsKey(effect.effectId))
                {
                    effectStack.Add(effect.effectId, (0, 0));
                }
                effectStack[effect.effectId] = (Mathf.Max(effectStack[effect.effectId].turn, effect.turnLeft), effectStack[effect.effectId].stack + 1);
            }

            int index = 0;
            foreach (string effectId in effectStack.Keys)
            {
                if (effectElements.Count <= index)
                {
                    effectElements.Add(Instantiate(effectElementPrefab, effectElementHolder));
                }
                effectElements[index].gameObject.SetActive(true);
                effectElements[index].SetupData(effectId, effectStack[effectId].turn, effectStack[effectId].stack);
                index++;
            }
            for (int i = effectStack.Count; i < effectElements.Count; i++)
            {
                effectElements[i].gameObject.SetActive(false);
            }
        }
        public GameObject btnSpeed;
        public void BtnSpeed()
        {
            if (UserData.Instance.movespeed == 1)
            {
                //UserData.Instance.movespeed = 2;
                UserData.Instance.movespeed = 2;
                Time.timeScale = 2;
                btnSpeed.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "X1";
                
            }
            else
            {
                //UserData.Instance.movespeed = 1;
                UserData.Instance.movespeed = 1;
                Time.timeScale = 1;
                btnSpeed.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "X2";
            }
        }
        [Serializable]
        public class ButtonGroup
        {
            public string name;
            public GameObject activeBtn;
            public GameObject inactiveButton;
        }
    }
}
