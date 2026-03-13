using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NTPackage_old.NTFunction
{
    public class FollowTarget : MonoBehaviour
    {
        public Transform Target;
        private void FixedUpdate() {
            if(Target == null) return;
            transform.position = this.Target.position;
        }
    }
}
