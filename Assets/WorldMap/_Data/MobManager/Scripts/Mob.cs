using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoShared;
using GOA.WorldMap;
using NTFunctions_old;
using DG.Tweening;
using Rubik.Myrk;
using Rubik.Myrk.Monster;
using Rubik.CardPlayer;
using NTPackage.Functions;
using TMPro;

namespace GOA.WorldMap
{
    [System.Serializable]
    public class IconAvaMob{
        public Sprite bg;
        public Sprite border;
    }
    public class Mob : LoadBehaviour
    {
        public MobData mobData;

        public bool isDestroy = false;

        public IconAvaMob regular;
        public IconAvaMob greater;

        public MobCtrl mobCtrl;

        public bool isRote = false;

        // New Feature
        public MonsterOnMapData MonsterOnMapData;
        public TextMeshProUGUI TextLevel;

        public override void LoadComponents()
        {
            base.LoadComponents();
            this.LoadMobCtrl();
        }

        protected void LoadMobCtrl(){
            if(mobCtrl != null) return;
            this.mobCtrl = transform.GetComponent<MobCtrl>();
        }

        //Function

        public void ResetData(){
            this.isDestroy = false;
            if(this.mobCtrl.prefab != null) return;
            this.UpdateData();
        }

        public void UpdateData(){
            if(this.mobData.coordinates == null){
                this.isDestroy = true;
                return;
            }
            this.UpdateDis();
            this.UpdatePos();
            this.UpdateSkin();
        }

        public double Dis;
        protected void UpdateDis(){
            double dis = this.mobData.coordinates.DistanceFromOtherGPSCoordinate(GameMaster.instance.player.Coordinates)*NTConst.toMetter;
            // if(dis < GameMaster.instance.player.LimitDis) this.OnLock();
            // else this.OffLock();
            Dis = dis;
            if(dis > MobManager.instance.DisRegion){
                this.isDestroy = true;
            }
            if(dis <= MobManager.instance.DisVisible){
                gameObject.SetActive(true);
            }else{
                gameObject.SetActive(false);
            }
            if(dis < MobManager.instance.DisFollow){
                if(this.isRote) return;
                transform.DOLookAt(GameMaster.instance.player.transform.position, 1).OnComplete(()=>{
                    this.isRote = false;
                });

            }

        }

        protected void UpdatePos(){
            Vector3 pos = this.mobData.coordinates.convertCoordinateToVector(0);
            transform.position = pos;
        }

        protected void UpdateSkin(){
            if(this.mobCtrl.prefab != null){
                ObjectPoolingManager.Instance.PushObjectIntoPooling(this.mobCtrl.prefab.transform);
            }
            CardPlayerIndex index = this.MonsterOnMapData.Index;
            Transform avatar = CardPlayerManager.Instance.InstantiatePlayerAvatar(index);
            this.mobCtrl.prefab = avatar.gameObject;
            this.mobCtrl.prefab.transform.SetParent(this.mobCtrl.skin);
            NTPackage.Functions.NTFunction.ResetPosition(this.mobCtrl.prefab.transform);
            this.TextLevel.text = "Lv. " + this.MonsterOnMapData.Lv.ToString();
        }

        public void ParserFromData(MobData mobData){
            transform.name = this.mobData.Index;
            this.mobData = mobData;
            this.mobData.coordinates = mobData.coordinates;
            this.MonsterOnMapData = MonsterManager.Instance.GetMonsterOnMapData(AttackType.Grinding);
            this.ResetData();
        }

        public bool Equals(Mob mob)
        {
            if(!mob.mobData.coordinates.Equals(this.mobData.coordinates)) return false;
            if(mob.mobData.Index != this.mobData.Index) return false;
            return true;
        }

        public bool Equals(MobData mobData){
            if(mobData.Index != this.mobData.Index) return false;
            return true;
        }

        public void OnLock(){
            this.mobCtrl.mobLock.gameObject.SetActive(true);
        }
        public void OffLock(){
            this.mobCtrl.mobLock.gameObject.SetActive(false);
        }
    }
}
