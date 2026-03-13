using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rubik.UI
{
    public class StarIcon : MonoBehaviour
    {
        public List<GameObject> Stars;

        public void SetData(int numb)
        {
            for (int i = 0; i < Stars.Count; i++)
            {
                if (i < numb) this.Stars[i].SetActive(true);
                else this.Stars[i].SetActive(false);
            }
        }
    }

}