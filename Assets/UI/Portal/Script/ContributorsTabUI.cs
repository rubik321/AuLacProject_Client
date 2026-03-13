using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NTFunctions_old;
using GOA.WorldMap;

namespace GOA.Portal{
    public class ContributorsTabUI : TabUI
    {
        public List<UserAttackDataItemUI> UserAttackDataItemUIs;

        public void Init(List<UserAttackData> userAttackDatas){
            for (int i = 0; i < UserAttackDataItemUIs.Count; i++)
            {
                if(i >= userAttackDatas.Count){
                    UserAttackDataItemUIs[i].gameObject.SetActive(false);
                }else{
                    UserAttackDataItemUIs[i].Init(userAttackDatas[i], i+1);
                    UserAttackDataItemUIs[i].gameObject.SetActive(true);
                }
            }
        }
    }
}
