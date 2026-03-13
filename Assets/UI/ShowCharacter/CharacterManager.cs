using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NTFunctions_old;
using Rubik.Combat;

namespace NTShowCharacter{
    public class CharacterManager : LoadBehaviour
    {
        public static CharacterManager instance;
        protected override void Awake()
        {
            base.Awake();
            if (CharacterManager.instance != null) Debug.LogError("Only 1 CharacterManager allow");
            CharacterManager.instance = this;
        }
        public SampleCharacter sampleCharacter;
        public CharacterCombatState characterCombatState;

        [ContextMenu("LoadGearToCharacter")]
        public void LoadGearToCharacter(){
            this.sampleCharacter.SetupData(characterCombatState);
        }
    }
}
