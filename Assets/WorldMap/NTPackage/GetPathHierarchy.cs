using System.Collections;
using System.Collections.Generic;
using NTFunctions_old;
using UnityEngine;

namespace NTPackage_old.NTFunction
{
    public class GetPathHierarchy : LoadBehaviour
    {
        public string Path;

        public override void LoadComponents()
        {
            base.LoadComponents();
            this.Path = transform.name;
            Transform parent = transform;
            for (int i = 0; i < 100; i++)
            {
                try
                {
                    parent = parent.parent;
                    this.Path = parent.name+"/"+this.Path;
                }
                catch (System.Exception)
                {
                    return;
                }
            }
        }
    }

}
