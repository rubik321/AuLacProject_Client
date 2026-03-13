using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class RankDungeonItem : MonoBehaviour
{
    public TextMeshProUGUI nameTxt, rankTxt,scoreTxt;
    public void SetUp(string userName,string rank,string score)
    {
        nameTxt.text = userName;
        rankTxt.text = rank;
        scoreTxt.text = score;
    }
}
