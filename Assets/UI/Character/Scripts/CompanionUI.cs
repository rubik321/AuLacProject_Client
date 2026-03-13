using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CompanionUI : MonoBehaviour
{
    public GameObject gearPrefab;
    public Transform content;
    [SerializeField] Image statsImg, orbsImg;
    [SerializeField] GameObject normalGo, genesisGo;
    int indexGearChoose = 0;
    // Start is called before the first frame update
    void Start()
    {
        SetUpOrbs(6);
    }
    List<GearItem> lsGearIteam;
    public void SetUpOrbs(int lenght)
    {
        if (lsGearIteam != null && lsGearIteam.Count > 0)
        {
            for (int i = 0; i < lsGearIteam.Count; i++)
            {
                Destroy(lsGearIteam[i].gameObject);
            }
            lsGearIteam.Clear();
        }
        else
        {
            lsGearIteam = new List<GearItem>();
        }
        CreateGearItem(6, "Companion Item");

    }
    void CreateGearItem(int lenght, string name)
    {
        for (int i = 0; i < lenght; i++)
        {
            GameObject go = Instantiate(gearPrefab);
            go.transform.SetParent(content);
            go.transform.localScale = Vector3.one;
            go.transform.eulerAngles = Vector3.zero;
            lsGearIteam.Add(go.GetComponent<GearItem>());
            go.GetComponent<GearItem>().itemName.text = name + (i + 1).ToString();
            int temp = i;
            go.GetComponent<GearItem>().pressBtn.onClick.AddListener(() =>
            {
                for (int j = 0; j < lsGearIteam.Count; j++)
                {
                    lsGearIteam[j].chooseObj.SetActive(false);
                }
                lsGearIteam[temp].chooseObj.SetActive(true);
                indexGearChoose = temp;
            });
        }
    }
    public void OnButton_Normal_Onclick()
    {
        statsImg.enabled = true;
        orbsImg.enabled = false;
        genesisGo.SetActive(false);
        normalGo.SetActive(true);
       
    }
    public void OnButton_Genesis_Onclick()
    {
        statsImg.enabled = false;
        orbsImg.enabled = true;
        normalGo.SetActive(false);
        genesisGo.SetActive(true);
    }



}
