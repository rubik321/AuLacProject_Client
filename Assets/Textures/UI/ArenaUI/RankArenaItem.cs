using Rubik.ItemPlayer;
using Rubik.Myrk.Arena;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankArenaItem : MonoBehaviour
{
    public Image rankAva,iconRank;
    public TextMeshProUGUI rankNameTxt,pointRankTxt;
    public ItemDataUI[] itemDatas;
     
    public void SetUp(RankingData data,Sprite ava, Sprite iconSpr)
    {
        GetIndexRank(data.Type.ToString());
        rankNameTxt.text = data.Type.ToString();
        pointRankTxt.text = data.MinPoint + "-" +data.MaxPoint;
        int index = 0;
        foreach(var item in data.RewardItems)
        {
            itemDatas[index].SetData(item);
            index++;
        }
        rankAva.sprite = ava; iconRank.sprite = iconSpr;
        
    }
    int GetIndexRank(string rank)
    {
        if (rank.Contains("Bronze"))
        {
            rankNameTxt.color = Color.brown;
            return 0;
        }
        if (rank.Contains("Silver"))
        {
            rankNameTxt.color = Color.white;
            return 1;
        }
        if (rank.Contains("Gold"))
        {
            rankNameTxt.color = Color.yellow;
            return 2;
        }
        if (rank.Contains("Platinum"))
        {
            rankNameTxt.color = Color.pink;
            return 3;
        }
        return 0;
    }
}
