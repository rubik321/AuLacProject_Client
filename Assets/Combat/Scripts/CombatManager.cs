using CodeHelper;
using GOA.UserData;
using Pixelplacement;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GOA.Config;
using SimpleJSON;
using GOA.WorldMap;
using GOA.Item;
using GOA.WorldMap.Outpost;

namespace Rubik.Combat
{
    public class CombatManager : Pixelplacement.Singleton<CombatManager>, IMessageHandle
    {
        [SerializeField] StateMachine turnHighLight;
        [SerializeField, ReadOnly] List<TurnStateData> turnStateDatas = new List<TurnStateData>();
        [SerializeField, ReadOnly] int currentTurnIndex = 0;
        [SerializeField, ReadOnly] public int currentTick = 0;
        [SerializeField, ReadOnly] int aliveAllyCount = 0; // Update when one is down
        [SerializeField, ReadOnly] int aliveEnemyCount = 0; // Update when one is down
        [SerializeField, ReadOnly] int gameStateIndex = 0;
        [SerializeField, ReadOnly] List<CharacterCombatState> allyStates = new List<CharacterCombatState>();
        [SerializeField, ReadOnly] List<CharacterCombatState> enemyStates = new List<CharacterCombatState>();
        [SerializeField, ReadOnly] List<CharacterCombatState> companionStates = new List<CharacterCombatState>();
        [SerializeField, ReadOnly] List<BaseTurnState> gameStateCollection = new List<BaseTurnState>();
        [SerializeField, ReadOnly] string currentStateName;
        Dictionary<string, CharacterCombatState> characterStates = new Dictionary<string, CharacterCombatState>();
        Dictionary<Type, Action<string>> onTurnStateActivate = new Dictionary<Type, Action<string>>();
        public BaseTurnState CurrentGameState => gameStateCollection[gameStateIndex];
        public TurnStateData CurrentTurnData => turnStateDatas[currentTurnIndex];
        public CharacterCombatState CurrentCharacterState => GetCharacterCombatState(CurrentTurnData.characterInstanceId);
        public CharacterCombatState NextCharacterState => GetCharacterCombatState(turnStateDatas[currentTurnIndex + 1].characterInstanceId);

        protected override void OnRegistration()
        {
            // Fill states
            gameStateCollection = new List<BaseTurnState>
            {
                new StateStart(),
                new StateStartTurn(),
                new StateStartTurnEffect(),
                new StateChooseAction(),
                new StateAction(),
                new StateEndTurnEffect(),
                new StateEndTurn(),
                new StateWait()
            };
        }

        private void OnEnable()
        {
            MessageManager.AddSubcriber<CodeHelper.MessageCollection.OnCharacterKilled>(this);
            MessageManager.AddSubcriber<CodeHelper.MessageCollection.OnCharacterEndTurn>(this);
        }

        private void OnDisable()
        {
            MessageManager.RemoveSubcriber<CodeHelper.MessageCollection.OnCharacterKilled>(this);
            MessageManager.RemoveSubcriber<CodeHelper.MessageCollection.OnCharacterEndTurn>(this);
        }

        public void Handle(Message message)
        {
            switch (message.type)
            {
                case nameof(CodeHelper.MessageCollection.OnCharacterKilled):
                    {
                        // Companions don't count as ally or enemy
                        if (string.IsNullOrEmpty(GetCharacterCombatState((string)message.data[0]).data.ownerInstanceId))
                        {
                            if (GetCharacterCombatState((string)message.data[0]).data.isEnemy)
                            {
                                aliveEnemyCount--;
                            }
                            else
                            {
                                aliveAllyCount--;
                            }
                        }

                        // TODO: Call this at the end of turn because many character can be dead at the same time
                        // Delete turn of those just died
                        HashSet<string> aliveCharacters = new HashSet<string>();
                        foreach (var character in GetAliveCharactersIncludeCompanion())
                        {
                            aliveCharacters.Add(character.data.characterInstanceId);
                        }
                        int turnDeletedCount = 0;
                        for (int i = turnStateDatas.Count - 1; i > currentTurnIndex; i--)
                        {
                            if (!aliveCharacters.Contains(turnStateDatas[i].characterInstanceId))
                            {
                                turnStateDatas.RemoveAt(i);
                                turnDeletedCount++;
                            }
                        }
                        // Generate more turns to filled in those turn that have been deleted
                        GenerateTurnData(turnDeletedCount);

                        // When a character dies in its own turn, must skip to next turn by force Game State to change to StateWait
                        if (GetCharacterCombatState((string)message.data[0]).data.characterInstanceId == CurrentTurnData.characterInstanceId)
                        {
                            this.DelayInvoke(() =>
                            {
                                gameStateIndex = gameStateCollection.Count - 1;
                                currentStateName = CurrentGameState.GetTurnType().Name;
                                CurrentGameState.Activate(GetCharacterCombatState(CurrentTurnData.characterInstanceId));
                            }, 1);
                        }
                        break;
                    }
                case nameof(CodeHelper.MessageCollection.OnCharacterEndTurn):
                    {
                        currentTurnIndex++;
                        break;
                    }
            }
        }

        public void StartCombat(List<CharacterCombatData> characterCombatDatas, string stageId)
        {
            
            StageObject stage = Instantiate(AssetLoader.Instance.GetAsset(stageId), Vector3.zero, Quaternion.identity).GetComponent<StageObject>();

            foreach (CharacterCombatData data in characterCombatDatas)
            {
                
                CharacterCombatState state = new CharacterCombatState(data);
                characterStates.Add(state.data.characterInstanceId, state);
                if (data.isEnemy)
                {
                    enemyStates.Add(state);
                }
                else
                {
                    allyStates.Add(state);
                }
                if (data.isBoss)
                {
                    Rubik.UI.MainCombatUI.Instance.EnableBossHealthBar(state.hp ,state.maxHp);
                }
            }
            CharacterManager.Instance.SpawnCharacters(characterStates.Values.ToList(), stage);
            MessageManager.SendMessage(new Message(nameof(CodeHelper.MessageCollection.OnGameStart)));
            StartCoroutine(IEStartCombat());
        }

        // Start is called before the first frame update
        IEnumerator IEStartCombat()
        {
            yield return IEGameStart();
            yield return IEGameLoop();
            yield return IEGameEnd();
        }

        IEnumerator IEGameStart()
        {
            gameStateIndex = 0;
            // Current index = -1 so when first turn is active, that index = 0, which is actually the first turn
            currentTurnIndex = -1;
            currentTick = 0;
            aliveAllyCount = GetAliveAllies().Count;
            aliveEnemyCount = GetAliveEnemies().Count;

            GenerateTurnData(30);

            CurrentGameState.Activate(null); // StateStart don't need turn data
            currentStateName = CurrentGameState.GetTurnType().Name;

            Rubik.UI.MainCombatUI.Instance.SetTurnElements();
            yield return null;
        }

        IEnumerator IEGameLoop()
        {
            BaseCharacter currentCharacter = null;
            while (aliveAllyCount > 0 && aliveEnemyCount > 0)
            {
                // Generate more turns when turn count reaches UI turn count display to avoid blank turn element UI
                if (turnStateDatas.Count - currentTurnIndex < 10)
                {
                    GenerateTurnData(20);
                }

                // When a character has finish their turn. All characters must wait for tick counter to increase
                // When tick counter reaches next turn tick. Activate that character's turn
                if (CurrentGameState.GetTurnType() == typeof(StateStart) || CurrentGameState.GetTurnType() == typeof(StateWait))
                {
                    currentTick++;
                    TurnStateData nextTurn = turnStateDatas[currentTurnIndex + 1];
                    currentCharacter = CharacterManager.Instance.GetCharacterObject(nextTurn.characterInstanceId);
                    if (currentTick >= nextTurn.tick)
                    {
                        ActivateTurn(nextTurn);
                    }
                }

                // Loop through turn states
                if (CurrentGameState.IsComplete())
                {
                    // Exclude State Start (index = 0), this one is outside game loop
                    gameStateIndex = Mathf.Clamp((gameStateIndex + 1) % gameStateCollection.Count, 1, gameStateCollection.Count - 1);
                    currentStateName = CurrentGameState.GetTurnType().Name;
                    CurrentGameState.Activate(GetCharacterCombatState(CurrentTurnData.characterInstanceId));
                }
                yield return null;
            }

            // End game when character finish every move
            if (currentCharacter == null)
                yield break;
            while (currentCharacter.gameObject.activeSelf && !currentCharacter.IsCurrentAnimationName("idle"))
            {
                yield return null;
            }
            yield return new WaitForSeconds(0.5f);
        }

        IEnumerator IEGameEnd()
        {
            bool isWin = false;
            Time.timeScale = 1;
            if (aliveEnemyCount > 0)
            {
                isWin = false;
                MessageManager.SendMessage(new Message(nameof(CodeHelper.MessageCollection.OnGameLose)));
               
            }
            else
            {
                isWin = true;
                MessageManager.SendMessage(new Message(nameof(CodeHelper.MessageCollection.OnGameWin)));
                if(UserData.Instance.DataInCombat.TypeCombat == MobTypeCode.MobChieftainOutpost)
                    QuestManager.Instance.UpdateQuest(QuestType.Outpost, 1);
            }
            int damePorTal = 0;
            if (!String.IsNullOrEmpty(UserData.Instance.DataInCombat.PortalId))
                {
                damePorTal = enemyStates[0].maxHp- enemyStates[0].hp;
                }
            if(UserData.Instance.DataInCombat.TypeCombat == MobTypeCode.MobReaperPortal){
                PlayerPrefs.SetString("Portal:"+UserData.Instance.DataInCombat.PortalId+":"+UserData.Instance.data.UserId, System.DateTime.Now.ToString());
                JSONNode dataAttack = new JSONObject();
                dataAttack["userID"] = UserData.Instance.data.UserId;
                dataAttack["portalID"] = UserData.Instance.DataInCombat.PortalId;
                dataAttack["dmg"] = UserData.Instance.TotalDameInPortal;
                JSONNode dataUser = new JSONObject();
                dataUser["Name"] = UserData.Instance.data.DisplayName;
                dataUser["Avatar"] = 0;
                dataAttack["data"] = dataUser.ToString();
                StartCoroutine(APIManager.Instance.PostDataUrl(dataAttack.ToString() ,SeverConfigs.BASE_API_URL + SeverConfigs.AttackPortalAPI, callback=>{
                    Debug.LogWarning(callback.downloadHandler.text);
                }));
                GameObject go = UI.CombatUIController.Instance.ChangeState("EndCombatUI");
                go.GetComponent<Rubik.UI.EndCombatUI>().Show(0);
                UserData.Instance.TotalDameInPortal = 0;
            }else
            APIManager.Instance?.AttackMobAPI(isWin,allyStates[0].hp,allyStates[0].mp, (reward) =>
            {
                if (!isWin)
                {
                    GameObject goLose = UI.CombatUIController.Instance.ChangeState("EndCombatUI");
                    goLose.GetComponent<Rubik.UI.EndCombatUI>().Show(0);
                    if (!isFlee)
                    {
                        UserData.Instance.characterData.CurrentHP = UserData.Instance.characterData.HP;
                    }
                    var temp1 = new List<(string id, int amount, int rarity)>();
                    if (reward.Data.Reward.Coin != 0)
                    {
                        temp1.Add(new("icon_coin", reward.Data.Reward.Coin, -1));
                        UserData.Instance.data.Coin += reward.Data.Reward.Coin;
                    }
                    goLose.GetComponent<Rubik.UI.EndCombatUI>().Show(0, temp1);
                    return;
                }
                    
                GameObject go = UI.CombatUIController.Instance.ChangeState("EndCombatUI");
                var temp = new List<(string id, int amount, int rarity)>();

                Debug.Log("Gear drop : " + reward.Data.GearDrop.GearCode);
                if (reward.Data.GearDrop.GearCode != null && reward.Data.GearDrop.GearCode != "")
                {
                    temp.Add(new(UserData.Instance.lsGoIdToName[reward.Data.GearDrop.GearCode], 1, reward.Data.GearDrop.Rarity));
                    UserData.Instance.gearData.Data.GearData.Add(reward.Data.GearDrop);
                }
                if (reward.Data.Reward.Coin > 0)
                {
                    temp.Add(new("icon_coin", reward.Data.Reward.Coin, -1));
                    UserData.Instance.data.Coin += reward.Data.Reward.Coin;
                }
                if (reward.Data.Reward.Gin > 0)
                {
                    temp.Add(new("icon_gin", reward.Data.Reward.Gin, -1));
                    UserData.Instance.data.Gin += reward.Data.Reward.Gin;
                }
                if (reward.Data.Reward.Potion != "")
                {
                    temp.Add(new(reward.Data.Reward.Potion, 1, -1));
                    UserData.Instance.Inventory.AddInventoryByID(reward.Data.Reward.Potion,1);
                    //{
                    //    case "Potion":
                    //        UserData.Instance.data.inventory.Potion ++;
                    //        break;
                    //    case "HiPotion":
                    //        UserData.Instance.data.inventory.HiPotion++;
                    //        break;
                    //    case "Ether":
                    //        UserData.Instance.data.inventory.Ether++;
                    //        break; 
                    //    case "HiEther":
                    //        UserData.Instance.data.inventory.HiEther++;
                    //        break; 
                    //    case "Elixir":
                    //        UserData.Instance.data.inventory.Elixir++;
                    //        break; 
                    //}
                }
                if (reward.Data.Reward.PoorLeather > 0)
                {
                    temp.Add(new("PoorLeather", reward.Data.Reward.PoorLeather, -1));
                    QuestManager.Instance.UpdateQuest(QuestType.Materials, 1);
                    UserData.Instance.Inventory.AddInventoryByID("PoorLeather", reward.Data.Reward.PoorLeather);
                    //{
                    //UserData.Instance.data.inventory.PoorLeather += reward.Data.Reward.PoorLeather;
                }
                if (reward.Data.Reward.ChieftainBlood > 0)
                {
                    temp.Add(new("ChieftainBlood", reward.Data.Reward.ChieftainBlood, -1));
                    QuestManager.Instance.UpdateQuest(QuestType.Materials, 1);
                    UserData.Instance.Inventory.AddInventoryByCode(ItemCode.ChieftainBlood, reward.Data.Reward.ChieftainBlood);
                   
                }
                if (reward.Data.Reward.HardLeather > 0)
                {
                    temp.Add(new("HardLeather", reward.Data.Reward.HardLeather, -1));
                    QuestManager.Instance.UpdateQuest(QuestType.Materials, 1);
                    UserData.Instance.Inventory.AddInventoryByCode(ItemCode.HardLeather, reward.Data.Reward.HardLeather);

                }
                if (reward.Data.Reward.AbyssRoots > 0)
                {
                    temp.Add(new("AbyssRoots", reward.Data.Reward.AbyssRoots, -1));
                    QuestManager.Instance.UpdateQuest(QuestType.Materials, 1);
                    UserData.Instance.Inventory.AddInventoryByCode(ItemCode.AbyssRoots, reward.Data.Reward.AbyssRoots);

                }
                if (reward.Data.Reward.DragonFlower > 0)
                {
                    temp.Add(new("DragonFlower", reward.Data.Reward.DragonFlower, -1));
                    QuestManager.Instance.UpdateQuest(QuestType.Materials, 1);
                    UserData.Instance.Inventory.AddInventoryByCode(ItemCode.DragonFlower, reward.Data.Reward.DragonFlower);

                }
                if (reward.Data.Reward.TritinumAlloy > 0)
                {
                    temp.Add(new("TritinumAlloy", reward.Data.Reward.TritinumAlloy, -1));
                    QuestManager.Instance.UpdateQuest(QuestType.Materials, 1);
                    UserData.Instance.Inventory.AddInventoryByCode(ItemCode.TritinumAlloy, reward.Data.Reward.TritinumAlloy);

                }
                if (reward.Data.Reward.Ferrieliquid > 0)
                {
                    temp.Add(new("Ferrieliquid", reward.Data.Reward.Ferrieliquid, -1));
                    QuestManager.Instance.UpdateQuest(QuestType.Materials, 1);
                    UserData.Instance.Inventory.AddInventoryByCode(ItemCode.Ferrieliquid, reward.Data.Reward.Ferrieliquid);

                }   
                if (reward.Data.Reward.MetalPlate > 0)
                {
                    temp.Add(new("MetalPlate", reward.Data.Reward.MetalPlate, -1));
                    QuestManager.Instance.UpdateQuest(QuestType.Materials, 1);
                    UserData.Instance.Inventory.AddInventoryByCode(ItemCode.MetalPlate, reward.Data.Reward.MetalPlate);

                } 
                if (reward.Data.Reward.StonePile > 0)
                {
                    temp.Add(new("StonePile", reward.Data.Reward.StonePile, -1));
                    QuestManager.Instance.UpdateQuest(QuestType.Materials, 1);
                    UserData.Instance.Inventory.AddInventoryByCode(ItemCode.StonePile, reward.Data.Reward.StonePile);

                } 
                if (reward.Data.Reward.SilicatePowder > 0)
                {
                    temp.Add(new("SilicatePowder", reward.Data.Reward.SilicatePowder, -1));
                    QuestManager.Instance.UpdateQuest(QuestType.Materials, 1);
                    UserData.Instance.Inventory.AddInventoryByCode(ItemCode.SilicatePowder, reward.Data.Reward.SilicatePowder);

                }
                if (reward.Data.Reward.Orb != "")
                {
                    temp.Add(new(reward.Data.Reward.Orb, 1, -1));
                    //UserData.Instance.SkillDatas.SkillData.Add(temp1);
                }
                if (reward.Data.Reward.Companion != "")
                {
                    temp.Add(new(reward.Data.Reward.Companion, 1, -1));
                    //var com = new CompanionData();
                    //com.CompCode = reward.Data.Reward.Companion;
                    //UserData.Instance.companions.Companions.Add(com);
                }
                go.GetComponent<Rubik.UI.EndCombatUI>().Show(reward.Data.Reward.EXP, temp);
                if(UserData.Instance.DataInCombat.TypeCombat == MobTypeCode.Dungeon)
                {
                    APIManager.Instance.GetDungeon();
                }
                APIManager.Instance.GetUserOrb();
                UserData.Instance.DataInCombat.IsMobClean = true;
                if(UserData.Instance.DataInCombat.TypeCombat == MobTypeCode.MobChieftainOutpost){
                    UserData.Instance.OutpostsOccupied[UserData.Instance.DataInCombat.OutpostId] = 1;
                    UserData.Instance.data.DailyIncome += Outpost.GetRewardByID(UserData.Instance.DataInCombat.OutpostId);
                }
            });
            yield return null;
        }

        public void SubscribeOnActivateTurnState<T>(Action<string> action) where T : BaseTurnState
        {
            foreach (BaseTurnState turnState in gameStateCollection)
            {
                if (turnState.GetTurnType() == typeof(T))
                {
                    turnState.SubscribeOnActivate(action);
                }
            }
        }

        private void ActivateTurn(TurnStateData data)
        {
            currentTurnIndex++;
            BaseCharacter character = CharacterManager.Instance.GetCharacterObject(data.characterInstanceId);
            character.ActivateTurn();
            turnHighLight.transform.position = character.transform.position;
            turnHighLight.ChangeState(character.GetCharacterSideInfo());
        }
        bool isFlee = false;
        public void Flee(){
            isFlee = true;
            StartCoroutine(this.IEGameEnd());
        }

        private void GenerateTurnData(int totalTurn)
        {
            var aliveCharacter = GetAliveCharactersIncludeCompanion();
            // TODO: Optimize if needed
            // Note: Binary Search Tree instead of List will reduce shifting cost in InsertAndSort()

            // Note: This method has complexity of O(n * k * k)
            // n = total turn
            // k = character. k^2 cost is caused by Binary insert, by is heuristically reduced
            List<TurnStateData> newTurns = new List<TurnStateData>();

            Dictionary<string, int> lastAppearanceTick = new Dictionary<string, int>();
            for (int i = turnStateDatas.Count - 1; i > -1; i--)
            {
                if (!lastAppearanceTick.ContainsKey(turnStateDatas[i].characterInstanceId))
                {
                    lastAppearanceTick.Add(turnStateDatas[i].characterInstanceId, turnStateDatas[i].tick);
                    continue;
                }
            }

            // Turn loop outside character loop has a better chance of reducing binary insert shifting cost than character loop outside turn loop
            for (int i = 1; i <= totalTurn; i++)
            {
                foreach (var character in aliveCharacter)
                {
                    int lastTick = lastAppearanceTick.ContainsKey(character.data.characterInstanceId) ?
                                    lastAppearanceTick[character.data.characterInstanceId] : 0;
                    InsertAndSort(newTurns,
                                    character.data.characterInstanceId,
                                    lastTick + GetTick(i, character.speed));
                }
            }


            // Only take first [total turn] in the List
            for (int i = 0; i < totalTurn; i++)
            {
                turnStateDatas.Add(newTurns[i]);
            }
        }

        private void InsertAndSort(List<TurnStateData> listData, string instanceId, int tick)
        {
            int left = 0;
            int right = listData.Count - 1;
            TurnStateData turn = new TurnStateData(instanceId, tick);

            while (left <= right)
            {
                int mid = left + (right - left) / 2;

                if (listData[mid].tick == tick)
                {
                    // if the value is already in the list, insert it after the last occurrence
                    int lastOccurrence = mid;
                    while (lastOccurrence < listData.Count - 1 && listData[lastOccurrence + 1].tick == tick)
                    {
                        lastOccurrence++;
                    }
                    listData.Insert(lastOccurrence + 1, turn);
                    return;
                }

                if (listData[mid].tick < tick)
                    left = mid + 1;
                else
                    right = mid - 1;
            }

            // if the value is not found, insert it at the appropriate position
            listData.Insert(left, turn);
        }

        private int GetTick(int index, int speed)
        {
            return (int)(index * 1f * (300f /Math.Sqrt( speed)));
        }

        public List<CharacterCombatState> GetAliveAllies()
        {
            List<CharacterCombatState> result = new List<CharacterCombatState>();
            foreach (CharacterCombatState ally in allyStates)
            {
                if (ally.hp > 0 && string.IsNullOrEmpty(ally.data.ownerInstanceId))
                {
                    result.Add(ally);
                }
            }
            return result;
        }

        public List<CharacterCombatState> GetDeadAllies()
        {
            List<CharacterCombatState> result = new List<CharacterCombatState>();
            foreach (CharacterCombatState ally in allyStates)
            {
                if (ally.hp <= 0 && string.IsNullOrEmpty(ally.data.ownerInstanceId))
                {
                    result.Add(ally);
                }
            }
            return result;
        }

        public List<CharacterCombatState> GetAliveEnemies()
        {
            List<CharacterCombatState> result = new List<CharacterCombatState>();
            foreach (CharacterCombatState enemy in enemyStates)
            {
                if (enemy.hp > 0 && string.IsNullOrEmpty(enemy.data.ownerInstanceId))
                {
                    result.Add(enemy);
                }
            }
            return result;
        }

        public List<CharacterCombatState> GetDeadEnemies()
        {
            List<CharacterCombatState> result = new List<CharacterCombatState>();
            foreach (CharacterCombatState enemy in enemyStates)
            {
                if (enemy.hp <= 0 && string.IsNullOrEmpty(enemy.data.ownerInstanceId))
                {
                    result.Add(enemy);
                }
            }
            return result;
        }

        public List<CharacterCombatState> GetAliveCharacters()
        {
            List<CharacterCombatState> result = new List<CharacterCombatState>();
            foreach (CharacterCombatState character in characterStates.Values)
            {
                if (character.hp > 0 && string.IsNullOrEmpty(character.data.ownerInstanceId))
                {
                    result.Add(character);
                }
            }
            return result;
        }

        public List<CharacterCombatState> GetDeadCharacters()
        {
            List<CharacterCombatState> result = new List<CharacterCombatState>();
            foreach (CharacterCombatState character in characterStates.Values)
            {
                if (character.hp <= 0 && string.IsNullOrEmpty(character.data.ownerInstanceId))
                {
                    result.Add(character);
                }
            }
            return result;
        }

        public List<CharacterCombatState> GetAliveAdjancentCharacters(string centerCharacterId)
        {
            List<CharacterCombatState> result = new List<CharacterCombatState>();
            List<CharacterCombatState> side;
            if (GetCharacterCombatState(centerCharacterId).data.isEnemy)
            {
                side = enemyStates;
            }
            else
            {
                side = allyStates;
            }
            for (int i = 0; i < side.Count; i++)
            {
                if (side[i].data.characterInstanceId == centerCharacterId)
                {
                    switch (i)
                    {
                        case 0:
                            if (side[1].hp > 0)
                                result.Add(side[1]);
                            if (side[2].hp > 0)
                                result.Add(side[2]);
                            break;
                        case 1:
                            if (side[0].hp > 0)
                                result.Add(side[0]);
                            if (side[3].hp > 0)
                                result.Add(side[3]);
                            break;
                        case 2:
                            if (side[0].hp > 0)
                                result.Add(side[0]);
                            break;
                        case 3:
                            if (side[1].hp > 0)
                                result.Add(side[1]);
                            break;
                    }
                }
            }
            return result;
        }

        public List<CharacterCombatState> GetAliveCharactersIncludeCompanion()
        {
            List<CharacterCombatState> result = new List<CharacterCombatState>();
            foreach (CharacterCombatState character in characterStates.Values)
            {
                if (character.hp > 0)
                {
                    result.Add(character);
                }
            }
            return result;
        }

        public CharacterCombatState GetPlayer()
        {
            foreach (CharacterCombatState character in characterStates.Values)
            {
                if (character.data.isPlayer)
                {
                    return character;
                }
            }
            return allyStates[0];
        }

        public CharacterCombatState GetCharacterCombatState(string instanceId)
        {
            if (characterStates.TryGetValue(instanceId, out CharacterCombatState character))
            {
                return character;
            }
            return allyStates[0];
        }


        public TurnStateData GetNextTurn(int offset)
        {
            return turnStateDatas[currentTurnIndex + offset];
        }
    }
}
