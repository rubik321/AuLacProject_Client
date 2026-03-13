using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabButtonUI : MonoBehaviour
{
    public GameObject hightlight;
    public Sprite onSpr, offSpr;
   
    public void ButtonOn()
    {
        hightlight.SetActive(true);
        GetComponent<Image>().sprite = onSpr;
    }
    public void ButtonOff()
    {
        hightlight.SetActive(false);
        GetComponent<Image>().sprite = offSpr;
    }
}
