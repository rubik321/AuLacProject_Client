using System.Collections;
using System.Collections.Generic;
using Rubik.ItemPlayer;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ItemInventory : MonoBehaviour
{
    public ItemDataUI itemData;
    public Button btnClick;
    public TextMeshProUGUI txtCoin;

    public Image Bg;
    public Image Border;
    // public Sprite HightLight;
    // public Sprite HightLightBg;

    // public Sprite Normal;
    // public Sprite NormalBg;

    public void Chose()
    {
        this.Border.gameObject.SetActive(true);
        // Bg.sprite = HightLightBg;
        // Border.sprite = HightLight;
        Border.gameObject.SetActive(true);
    }

    public void UnChose()
    {
        this.Border.gameObject.SetActive(false);
        // Bg.sprite = NormalBg;
        // Border.sprite = Normal;
    }

}
