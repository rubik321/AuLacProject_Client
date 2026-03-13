using Rubik.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rubik.Combat
{
    public class CharacterManager : Pixelplacement.Singleton<CharacterManager>
    {
        [SerializeField] Transform healthBarHolder;
        [SerializeField] CharacterHealthBar healthBarPrefab;


        public BaseCharacter Player { get; private set; }
        Dictionary<string, BaseCharacter> characters = new Dictionary<string, BaseCharacter>();

        public void SpawnCharacters(List<CharacterCombatState> characterStates, StageObject stage)
        {
            characters = new Dictionary<string, BaseCharacter>();
            int enemyIndex = 0;
            int allyIndex = 0;
            foreach (CharacterCombatState characterState in characterStates)
            {
                Transform holder;
                if (characterState.data.isEnemy)
                {
                    if (string.IsNullOrEmpty(characterState.data.ownerInstanceId))
                    {
                        holder = stage.enemyHolders[enemyIndex].characterHolder;
                        enemyIndex++;
                    }
                    else
                    {
                        holder = stage.enemyHolders.First(e => e.characterHolder == GetCharacterObject(characterState.data.ownerInstanceId).transform.parent).companionHolder;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(characterState.data.ownerInstanceId))
                    {
                        holder = stage.allyHolders[enemyIndex].characterHolder;
                        allyIndex++;
                    }
                    else
                    {
                        holder = stage.allyHolders.First(e => e.characterHolder == GetCharacterObject(characterState.data.ownerInstanceId).transform.parent).companionHolder;
                    }
                }
                Debug.Log(characterState.data.characterId);
                BaseCharacter character = Instantiate(AssetLoader.Instance.GetAsset(characterState.data.characterId), holder).GetComponent<BaseCharacter>();
                character.transform.localPosition = Vector3.zero;
                character.transform.localRotation = Quaternion.identity;
               
                character.SetupData(characterState);
                
                characters.Add(characterState.data.characterInstanceId, character);
                if (characterState.data.isPlayer)
                {
                    Player = character;
                    MainCombatUI.Instance.UpdatePlayerStatus();
                }
            }
        }

        public BaseCharacter GetCharacterObject(string instanceID)
        {
            if (characters.TryGetValue(instanceID, out BaseCharacter character))
                return character;
            return null;
        }
    }
}