using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOA.UserData;
using System;
public class CompanionAdventure : MonoBehaviour
{
    public Transform compContent;
    [SerializeField] GameObject compPrefab;
    List<CompanionItem> lsGearIteam;
    void OnEnable()
    {
        SetUpCompItem();
        
        //Debug.Log(((DateTimeOffset)(DateTime.UtcNow)).ToUnixTimeSeconds());
    }
    bool iSHow = false;
    private void OnDisable()
    {
      
    }
    public void SetUpCompItem()
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
            lsGearIteam = new List<CompanionItem>();
        }

        foreach (CompanionData comp in UserData.Instance.companions.Companions)
        {
            var temp = UserData.Instance.DictionaryCompanionData[comp.CompCode];
            if (temp .CompType<= 0)
               continue;
            var goComp= Instantiate(compPrefab).GetComponent<CompanionItem>();
            goComp.transform.SetParent(compContent, false);
            lsGearIteam.Add(goComp);
            comp.CompName = temp.CompName;
            goComp.SetUp(comp,()=> {
                APIManager.Instance.SendAdventure(comp._id);
            });

        }
    }
    public void SentAdventure()
    {
        
    }

}
