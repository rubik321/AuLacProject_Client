using NTPackage.Functions;
using Rubik.Combat;
using Rubik.Myrk.Arena;
using Rubik.UserDataPlayer;
using Rubik.UserProfile;
using Spine.Unity;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LeaderBoardArenaUI : MonoBehaviour
{
    public ArenaLeaderBoardItem itemPre;
    public ArenaLeaderBoardItem[] topRanks;
    public Transform content;
    UserRankResponse userRank;

    private void OnEnable()
    {
        ArenaManager.Instance.GetArenaRankAll((userRank) => { 
            this.userRank = userRank;
            List<string> userIDs = new List<string>();
            foreach (UserRank user in userRank.UserRanks)
            {
                userIDs.Add(user.UserID);
            }
            UserProfileManager.Instance.GetUserDataShorts(userIDs, (userDataShorts) =>
            {
                UpdateData(userDataShorts);
            });
        });
    }

    public void UpdateData(UserDataShort[] users)
    {
        int index = 0;
        ObjectPoolingManager.Instance.PushChildObjectIntoPooling(this.content);
        foreach(ArenaLeaderBoardItem item in topRanks)
        {
            item.gameObject.SetActive(false);
        }
        foreach (UserRank user in userRank.UserRanks)
        {
            if(index < 3)
            {
                topRanks[index].SetUp(user, users[index]);
                topRanks[index].gameObject.SetActive(true);
            }
            else
            {
                ArenaLeaderBoardItem arenaItem = ObjectPoolingManager.Instance.InstantiateObject<ArenaLeaderBoardItem>(ObjectPoolingConfig.ArenaResultItem, this.itemPre.transform);
                arenaItem.SetUp(user, users[index]);
                arenaItem.transform.SetParent(this.content);
                NTFunction.ResetPosition(arenaItem.transform);
            }

            index++;
        }
    }


}
