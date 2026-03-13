using DG.Tweening;
using Rubik.ItemPlayer;
using Rubik.Myrk.Clan;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class DameBossGamePlayUI : MonoBehaviour
{
    public Image slider;
    public TextMeshProUGUI processTxt, numberChessTxt;
    public List<RateItemDataItem> lsRawardChess;
    public GameObject rewardGo;
    float lastSlider = 0;
    public void Setup(int currentDame,int AmountReward, long MaxDamage)
    {
        processTxt.text = currentDame + "/" + MaxDamage;
        numberChessTxt.text ="x"+ AmountReward;
        // slider.fillAmount =  (float)currentDame /MaxDamage;
        float currentSlider = (float)currentDame / MaxDamage;
        if(AmountReward <= lastSlider)
            slider.DOFillAmount(currentSlider,2f).SetSpeedBased(true);
        else
        {
           
            
            slider.DOFillAmount(1,2).SetSpeedBased(true).OnComplete(() => {
                slider.fillAmount = 0;
                slider.DOFillAmount(currentSlider, 2).SetSpeedBased(true);
            });
        }
        lastSlider = AmountReward;
    }
    public void ShowRewardRate()
    {
        rewardGo.SetActive(true);


        foreach (RateItemDataItem item in lsRawardChess)
        {
            item.gameObject.SetActive(false);
        }
        var reward = ClanManager.Instance.GetItemRewardClanBoss();
        int index = 0;
        foreach (ItemRate item in reward)
        {
            Rubik.ItemPlayer.ItemData data = new Rubik.ItemPlayer.ItemData();
            data.Type = item.Type;
            data.Amount = item.Amount;
            lsRawardChess[index].SetData(data, item.Rate, true, true);
            lsRawardChess[index].gameObject.SetActive(true);
            index++;
        }
        // ticketTxt.text = reward.rest + "/" + reward.max;

   
    }
}
