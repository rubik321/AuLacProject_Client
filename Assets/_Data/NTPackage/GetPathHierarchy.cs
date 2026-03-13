using System.Collections;
using System.Collections.Generic;
using NTPackage.Functions;
using UnityEngine;

namespace NTPackage.NTFunction
{
    public class GetPathHierarchy : NTBehaviour
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
