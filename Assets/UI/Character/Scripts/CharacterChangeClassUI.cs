using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CharacterChangeClassUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI characterNameTxt,classNameTxt,contentInfoTxt;
    [SerializeField] Image statsImg, orbsImg;
    [SerializeField] GameObject statsGo, orbsGo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

   public void OnButton_Stats_Onclick()
    {
        statsImg.enabled = true;
        orbsImg.enabled = false;
        statsGo.SetActive(true);
        orbsGo.SetActive(false);
    }
    public void OnButton_Orbs_Onclick()
    {
        statsImg.enabled = false;
        orbsImg.enabled = true;
        statsGo.SetActive(false);
        orbsGo.SetActive(true);
    }
    public void Btn_Quit()
    {
        CharacterUIController.Instance.ChangeBottomField();
    }
}
