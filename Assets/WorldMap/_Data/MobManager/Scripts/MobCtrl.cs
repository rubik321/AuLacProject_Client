using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOA.WorldMap;
using NTFunctions_old;
using UnityEngine.UI;

namespace GOA.WorldMap
{
    public class MobCtrl : LoadBehaviour
    {
        public Mob mob;
        public MobLock mobLock;
        public Transform skin;
        public GameObject prefab;

        public override void LoadComponents()
        {
            base.LoadComponents();
            this.LoadMob();
            this.LoadMobLock();
        }

        protected void LoadMob(){
            if(mob != null) return;
            this.mob = transform.GetComponent<Mob>();
        }

        protected void LoadMobLock(){
            if(mobLock != null) return;
            this.mobLock = transform.Find("MobLock").GetComponent<MobLock>();
        }
    }
}
