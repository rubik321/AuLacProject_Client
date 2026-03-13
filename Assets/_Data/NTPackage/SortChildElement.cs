using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NTPackage.Functions;

namespace NTPackage
{
    public class SortChildElement : NTBehaviour
    {
        [NTButton]
        public void SortChildElementByName()
        {
            List<Transform> children = new List<Transform>();
            foreach (Transform child in transform)
            {
                children.Add(child);
            }
            children.Sort((x, y) => string.Compare(x.name, y.name));
            foreach (Transform child in children)
            {
                child.SetSiblingIndex(children.IndexOf(child));
            }
        }
    }
}