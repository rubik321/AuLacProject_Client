using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rubik.UI
{
    public class RarityShadow : MonoBehaviour
    {
        public List<GameObject> Rarity;

        public void SetData(int numb)
        {
            foreach (GameObject item in Rarity)
            {
                item.SetActive(false);
            }
            if (numb >= this.Rarity.Count) numb = this.Rarity.Count - 1;
            if (numb < 0) numb = 0;
            this.Rarity[numb].SetActive(true);
        }
    }

}
