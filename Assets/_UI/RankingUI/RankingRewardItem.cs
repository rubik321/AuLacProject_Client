using System.Collections.Generic;
using Rubik.ItemPlayer;
using TMPro;
using UnityEngine;

namespace Rubik.Myrk.Ranking
{
    using Rubik.ItemPlayer;

    public class RankingRewardItem : MonoBehaviour
    {
        public List<Transform> BarBGs;

        public ListItemDataUI itemDataUI;
        public TextMeshProUGUI TextRank;

        public void SetData(int rankMax, int rankMin, List<ItemData> rewards)
        {
            foreach (Transform barBG in this.BarBGs)
            {
                barBG.gameObject.SetActive(false);
            }
            this.TextRank.gameObject.SetActive(false);
            switch (rankMin)
            {
                case 0:
                    this.BarBGs[0].gameObject.SetActive(true);
                    break;
                case 1:
                    this.BarBGs[1].gameObject.SetActive(true);
                    break;
                case 2:
                    this.BarBGs[2].gameObject.SetActive(true);
                    break;
                default:
                    this.TextRank.gameObject.SetActive(true);
                    this.BarBGs[this.BarBGs.Count - 1].gameObject.SetActive(true);
                    break;
            }

            if (rankMax < 0)
            {
                this.TextRank.text = (rankMin + 1) + "+";
            }
            else
            {
                this.TextRank.text = (rankMin + 1) + "-" + (rankMax + 1);
            }
            this.itemDataUI.SetData(rewards.ToArray(), null, true, true);
        }

        public void Clear()
        {
            this.itemDataUI.Clear();
        }
    }
}
