using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Rubik.Ranking
{
    public class Panel_Ranking : SimplePopup
    {
        public RankFunction rankFunction;
        public RankingAssets rankingAssets;

        [Button]
        void TestSetUp()
        {
            rankFunction.SetUpRankItem(rankingAssets);
            rankFunction.SetUpRankUser(rankingAssets);
        }
    }

}
