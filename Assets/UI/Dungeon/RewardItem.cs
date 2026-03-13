using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
public class RewardItem : MonoBehaviour
{
    public Image itemAva,boxAva;
    public TextMeshProUGUI valueTxt,nameTxt;
    public void SetUp(Sprite spr,int value,string name)
    {
        itemAva.sprite = spr;
        valueTxt.text = "x"+value.ToString();
        nameTxt.text = name;
        transform.localScale = Vector2.zero;
        transform.DOScale(new Vector2(1.2f, 1.2f), .2f)
            .OnComplete(() => {
                transform.DOScale(new Vector2(1f, 1f), .1f);
            });
    }
}
