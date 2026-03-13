using System.Collections.Generic;
using Rubik.ItemPlayer;
using TMPro;
using UnityEngine;

namespace Rubik.Myrk.Ranking
{
    using NTPackage.Functions;
    using Rubik.ItemPlayer;
    using Rubik.UserProfile;

    public class RankingPlayerItem : MonoBehaviour
    {
        public List<Transform> BarBGs;

        public TextMeshProUGUI TextRank;
        public AvatarPlayerUI AvatarPlayerUI;
        public TextMeshProUGUI TextDisplayName;
        public TextMeshProUGUI TextLevel;
        public TextMeshProUGUI TextScore;

        public void SetData(string displayName, int level, int avatarIndex, int borderIndex, int rank, long score, bool isGreen = false)
        {
            this.TextRank.gameObject.SetActive(false);
            switch (rank)
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

            if (rank > 99)
            {
                this.TextRank.text = (99) + "+";
            }
            else
            {
                this.TextRank.text = (rank+1) + "";
            }

            this.TextDisplayName.text = displayName;
            if (isGreen){
                this.TextDisplayName.color = new Color(0.15f, 0.8f, 0.15f);
            }else{
                this.TextDisplayName.color = NTFunction.StringHexToColor("#7F5B29");
            }
            this.TextLevel.text = "Lv. " + level;
            this.AvatarPlayerUI.SetData(avatarIndex, borderIndex);
            this.TextScore.text = score.ToString();
        }

        public void Clear()
        {
            foreach (Transform barBG in this.BarBGs)
            {
                barBG.gameObject.SetActive(false);
            }
        }
    }
}
