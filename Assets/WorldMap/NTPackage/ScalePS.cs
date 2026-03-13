using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NTFunctions_old{
    public class ScalePS : LoadBehaviour
    {
        public Vector3 scale = new Vector3(1,1,1);

        public ParticleSystem[] particleSystems;

        public override void LoadComponents()
        {
            base.LoadComponents();
            this.LoadAllChild();
        }

        protected void LoadAllChild(){
            this.particleSystems = transform.GetComponentsInChildren<ParticleSystem>();
        }

        [ContextMenu("Scale ParticleSystem")]
        public void ScaleParticleSystem(){
            foreach (ParticleSystem item in this.particleSystems)
            {
                item.transform.localScale = this.scale;
            }
        }
    }
}
