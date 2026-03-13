using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class StageItem : MonoBehaviour
{

    public Image floorAva, stageDisplayAva;
    public TextMeshProUGUI valueTxt;
    public void SetUp( int level,Sprite floorSpr,Sprite stageDisplaySpr)
    {
       // floorAva.sprite = spr;
        valueTxt.text =  (level+1).ToString();
        floorAva.sprite = floorSpr;
        stageDisplayAva.sprite = stageDisplaySpr;


    }
}
