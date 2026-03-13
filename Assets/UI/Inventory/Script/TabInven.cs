using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabInven : MonoBehaviour
{
    [SerializeField] GameObject OnGo, OffGo;
    void Start()
    {
        
    }

    public void TabOn(bool isOn = false)
    {
        OnGo.SetActive(isOn);
        OffGo.SetActive(!isOn);
    }
}
