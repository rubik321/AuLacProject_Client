using System.Collections;
using System.Collections.Generic;
using GOA.Config;
using UnityEngine;
using UnityEngine.UI;

public class LevelItem : MonoBehaviour
{
    public GameObject offLevel, onLevel, passlevel;
    void Start()
    {
        
    }
    public void SetUp(int index,int level)
    {
       
        if (index == 0)
        {
            offLevel.SetActive(true);
            onLevel.SetActive(false);
            passlevel.SetActive(false);
        }
        else if(index == 1)
        {
            offLevel.SetActive(false);
            onLevel.SetActive(true);
            passlevel.SetActive(false);
        }
        else
        {
            offLevel.SetActive(false);
            onLevel.SetActive(false);
            passlevel.SetActive(true);
        }
        this.index = index;
        this.level = level;
        
    }
    public int index = 0,level;
    public void OnMouseUp()
    {
        OnPlay();
    }
    public void OnPlay()
    {

        if (index > 0)
        {
            
            FindObjectOfType<LevelMapController>().levelPopup.ShowUp(index, level);
        }
            
    }
}
