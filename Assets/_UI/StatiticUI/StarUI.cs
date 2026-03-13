using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rubik.UI.Statitic
{
    public class StarUI : MonoBehaviour
    {
        public List<Transform> StarList;

        public void SetStar(int star){
            for(int i = 0; i < this.StarList.Count; i++){
                if(i == star) this.StarList[i].gameObject.SetActive(true);
                else this.StarList[i].gameObject.SetActive(false);
            }
        }
    }
}
