using System.Collections;
using System.Collections.Generic;
using GOA.UserData;
using UnityEngine;

public class RankDungeon : SimplePopup
{
    public GameObject rankPre;
    public Transform rankContent;
    public RankDungeonItem userRank;
    List<GameObject> lsItems = new List<GameObject>();
    protected override void Start()
    {
        base.Start();
    }
    public override void ShowUp(AnimationPopupType type = AnimationPopupType.OnTopDown)
    {
        base.ShowUp(type);
    }
    public void ShowRank()
    {
        if (lsItems.Count > 0)
        {
            for(int i = 0; i < lsItems.Count; i++) {
                Destroy(lsItems[i]);
            }
            lsItems.Clear();
        }
        ShowUp(AnimationPopupType.OnFade);
        try
        {
            int index = 1;
            for (int i = 0; i < UserData.Instance.DungeonInfo.Rankers.Length; i++)
            {
                if (index > 10)
                    break;
                GameObject go = Instantiate(rankPre);
                go.transform.SetParent(rankContent, false);
                go.GetComponent<RankDungeonItem>().SetUp(UserData.Instance.DungeonInfo.Rankers[i].data.UserName, index.ToString(), (UserData.Instance.DungeonInfo.Rankers[i].score+1).ToString());
                index++;
                lsItems.Add(go);
            }
        }
        catch
        {

        }
        userRank.SetUp(UserData.Instance.data.UserName, ((UserData.Instance.DungeonInfo.Rank+1).ToString()), ((UserData.Instance.DungeonInfo.Stage+1).ToString()));
    }
}
