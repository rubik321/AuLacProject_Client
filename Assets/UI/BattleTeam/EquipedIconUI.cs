using System.Collections.Generic;
using System.Linq;
using NTPackage.Functions;
using UnityEngine;

namespace Rubik.Myrk.BattleTeam
{
    public class EquipedIconUI : MonoBehaviour
    {
        public Sprite PentagonSpace;
        public Sprite PentagonArena;

        public List<EquipedIconUIItem> lsEquipedIcon;
        public Transform HolderEquipedIcon;
        public EquipedIconUIItem EquipedIconUIItemPrefab;

        public void SetData(List<int> teams){
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.HolderEquipedIcon);
            this.lsEquipedIcon.Clear();
            foreach (int team in teams){
                EquipedIconUIItem item = ObjectPoolingManager.Instance.InstantiateObject<EquipedIconUIItem>(ObjectPoolingConfig.EquipedIconUIItem, this.EquipedIconUIItemPrefab.transform);
                item.transform.SetParent(this.HolderEquipedIcon);
                NTFunction.ResetPosition(item.transform);
                if(BattleTeamConfig.ArenaDefendTeamIndex == team){
                    item.SetData(this.PentagonArena, "");
                }
                else if(BattleTeamConfig.BattleTeamIndex.ToList().Contains(team)){
                    item.SetData(this.PentagonSpace, team.ToString());
                }
                else{
                    item.SetData(this.PentagonSpace, "");
                }
                this.lsEquipedIcon.Add(item);
            }
        }

        public void Clear(){
            ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.HolderEquipedIcon);
            this.lsEquipedIcon.Clear();
        }
    }
}