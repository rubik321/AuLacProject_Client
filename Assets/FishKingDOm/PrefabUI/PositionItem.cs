using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PositionItem : MonoBehaviour
{
    public Image posIgm;
    public GameObject hightlight1, hightlight2;
    public Sprite posSprOn, posSprOff;
    public ParticleSystem spawnEffect,destroyEffect;
   
    public void SetHightOn(bool isOn = false)
    {
        if(isOn)
            posIgm.sprite = posSprOff;
        else
            posIgm.sprite = posSprOn;
        SetOverHightlight(false);
    }
    public void SetOverHightlight(bool isOn)
    {
        hightlight1.SetActive(isOn);
        hightlight2.SetActive(isOn);
      
    }
}
