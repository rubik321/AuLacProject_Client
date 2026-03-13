using Rubik.Myrk.Ranking;
using Rubik.UserDataPlayer;
using Rubik.UserProfile;
using UnityEngine;

namespace Rubik.Myrk.Clan
{
    public class ClanBossRankItem : MonoBehaviour
    {
        public RankingPlayerItem RankingPlayerItem;

        public void SetData(string displayName, int level, int avatarIndex, int borderIndex, int rank, long score)
        {
            this.RankingPlayerItem.Clear();
            bool isGreen = false;
            if (UserDataManager.Instance.GetUserID() == UserDataManager.Instance.GetUserID()) isGreen = true;
            this.RankingPlayerItem.SetData(displayName, level + 1, avatarIndex, borderIndex, rank, score, isGreen);
        }

        public void SetPlayerRank(int rank, long damage)
        {
            this.RankingPlayerItem.Clear();
            if (rank < 0 || damage < 1)
            {
                this.RankingPlayerItem.TextRank.text = "--";
                this.RankingPlayerItem.TextScore.text = "--";
                this.RankingPlayerItem.BarBGs[this.RankingPlayerItem.BarBGs.Count - 1].gameObject.SetActive(true);
            }
            else
            {
                this.RankingPlayerItem.TextRank.text = rank.ToString();
                this.RankingPlayerItem.TextScore.text = damage.ToString();
                switch (rank)
                {
                    case 0:
                        this.RankingPlayerItem.BarBGs[0].gameObject.SetActive(true);
                        break;
                    case 1:
                        this.RankingPlayerItem.BarBGs[1].gameObject.SetActive(true);
                        break;
                    case 2:
                        this.RankingPlayerItem.BarBGs[2].gameObject.SetActive(true);
                        break;
                    default:
                        this.RankingPlayerItem.TextRank.gameObject.SetActive(true);
                        this.RankingPlayerItem.BarBGs[this.RankingPlayerItem.BarBGs.Count - 1].gameObject.SetActive(true);
                        break;
                }

                if (rank > 99)
                {
                    this.RankingPlayerItem.TextRank.text = (99) + "+";
                }
                else
                {
                    this.RankingPlayerItem.TextRank.text = (rank + 1) + "";
                }
                ;
            }
            this.RankingPlayerItem.TextLevel.text = "Lv." + (UserDataManager.Instance.GetLevel() + 1);
            this.RankingPlayerItem.TextDisplayName.text = UserDataManager.Instance.GetDisplayName();
            this.RankingPlayerItem.AvatarPlayerUI.SetData(UserProfileManager.Instance.GetAvatarUsedIndex(), UserProfileManager.Instance.GetAvatarBorderUsedIndex());
        }


    }
}