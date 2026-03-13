using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using NTPackage.UI;
using Rubik.CharacterPlayer;
public class GearStats : NTButtonEffect
{
    public TextMeshProUGUI hpTxt;
    public TextMeshProUGUI heroAtkTxt;
    public TextMeshProUGUI teamSpeedTxt;
    public void SetInfo(float hp = -1,float heroAtk = -1,float teamSpeed = -1)
    {
        if (hp >= 0)
        {
            hpTxt.gameObject.SetActive(true);
            hpTxt.text = hp.ToString();
        }
        else
        {
            hpTxt.gameObject.SetActive(false);
        }
        if (heroAtk >= 0)
        {
            heroAtkTxt.gameObject.SetActive(true);
            heroAtkTxt.text = heroAtk.ToString();
        }
        else
        {
            heroAtkTxt.gameObject.SetActive(false);
        }
        if (teamSpeed >= 0)
        {
            teamSpeedTxt.gameObject.SetActive(true);
            teamSpeedTxt.text = teamSpeed.ToString();
        }
        else
        {
            teamSpeedTxt.gameObject.SetActive(false);
        }
    }

    public void OnTooltipStatsGuide()
    {
        CharacterPlayerManager.Instance.OnTooltipStatsGuide();
    }
}
