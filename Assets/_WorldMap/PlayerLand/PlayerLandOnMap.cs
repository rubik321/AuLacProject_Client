using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Rubik.Combat;
using Rubik.Myrk.BattleTeam;
using Spine.Unity;
using UnityEngine;

namespace Rubik.PlayerLand
{
    using NTPackage.Functions;
    using Rubik.CharacterGear;
    public class PlayerLandOnMap : NTBehaviour
    {
        public SpriteRenderer SR_Land;
        public SkeletonAnimation CharacterSkin;


        protected override void Start()
        {
            base.Start();
            // this.SR_Land.sprite = PlayerLandManager.Instance.GetPlayerSprite();
            
        }

        [NTButton]
        public void LoadSkin(){
            BattleTeamData battleTeamData = BattleTeamManager.Instance.GetBattleTeamDataSelected();
            Debug.LogError("battleTeamData.CharacterGears: " + JsonUtility.ToJson(battleTeamData));
            List<int> gearIDs = new List<int>();
            foreach (CharacterGear gear in battleTeamData.CharacterGears)
            {
                gearIDs.Add((int)gear.Index);
            }
            AssetLoader.Instance.MixSkinWithGears(CharacterSkin, gearIDs.ToArray());
        }
    }
}