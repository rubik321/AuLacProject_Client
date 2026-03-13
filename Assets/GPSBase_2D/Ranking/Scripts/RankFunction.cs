using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rubik.Ranking
{
    public class RankFunction : MonoBehaviour
    {
        public List<RankItem> rankItems;
        public RankItem rankUser;
        public void SetUpRankItem(RankingAssets rankingAssets)
        {
            List<RankingData> rankingDatas = rankingAssets.GetRankingDatas();
            for (int i = 0; i < rankItems.Count; i++)
            {
                RankingData rankingData = rankingDatas[i];
                rankItems[i].SetUpRankItem(rankingData.Stt, rankingData.Score, rankingData.UserName, rankingAssets.GetIconFlagCountry((NameFlagCountry)rankingData.IdFlag), rankingAssets);
            }
        }
        public void SetUpRankUser(RankingAssets rankingAssets)
        {
            RankingData rankingData = rankingAssets.GetUserRankingData();
            rankUser.SetUpRankItem(rankingData.Stt, rankingData.Score, rankingData.UserName, rankingAssets.GetIconFlagCountry((NameFlagCountry)rankingData.IdFlag), rankingAssets);
        }
    }
}
