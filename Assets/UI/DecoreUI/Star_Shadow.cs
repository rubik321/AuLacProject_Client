using System.Collections;
using System.Collections.Generic;
using NTPackage_old.UI;
using UnityEngine;

namespace Rubik.UI
{
    public class Star_Shadow : MonoBehaviour
    {
        public List<NTButtonEffect> Stars;

        public void SetData(int numb)
        {
            for (int i = 0; i < Stars.Count; i++)
            {
                if (i < numb) this.Stars[i].Chose();
                else this.Stars[i].UnChose();
            }
        }
    }
}
