using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rubik.Ranking
{
    public class RankItem : MonoBehaviour
    {
        public Image iconCoverNumber, iconFlagCountry;
        public TextMeshProUGUI txtNumericalOrder, txtNamePlayer, txtScore;

        public void SetUpRankItem(int numericalOrder, int score, string namePlayer, Sprite iconFlagCountry, RankingAssets rankingAssets)
        {
            txtNamePlayer.text = namePlayer;
            txtScore.text = score.ToString();
            this.iconFlagCountry.sprite = iconFlagCountry;

            if (numericalOrder == 1 || numericalOrder == 2 || numericalOrder == 3)
            {
                iconCoverNumber.gameObject.SetActive(true);
                txtNumericalOrder.gameObject.SetActive(false);
                iconCoverNumber.sprite = rankingAssets.GetIconTopRankIcon(numericalOrder);
            }
            else
            {
                iconCoverNumber.gameObject.SetActive(false);
                txtNumericalOrder.gameObject.SetActive(true);
                txtNumericalOrder.text = numericalOrder.ToString();
            }
        }
    }
}
