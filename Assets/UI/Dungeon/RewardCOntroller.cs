using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class RewardCOntroller : SimplePopup
{
    public List<RewardItem> lsItems;
    public RewardItem itemPrefab;
    [SerializeField] Transform contentItem;
         
    protected override void Start()
    {
        base.Start();
    }
    public override void ShowUp(AnimationPopupType type = AnimationPopupType.OnTopDown)
    {
        base.ShowUp(type);
    }
    [Button]
    public void ShowReward()
    {
        for(int i = 0; i < 10; i++)
        {
            var go = Instantiate(itemPrefab);
            lsItems.Add(go);
            go.transform.SetParent(contentItem, false);
        }
    }
      
}
