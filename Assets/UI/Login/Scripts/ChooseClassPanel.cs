using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseClassPanel : MonoBehaviour
{
    [SerializeField] GameObject maleObj, femaleObj;
    [SerializeField] GameObject[] covers;
    // Start is called before the first frame update
    void Start()
    {
        ChooseClass(0);
    }
    int indexOfClass = 0;
    public void ChooseGender(int indexGender)
    {
        switch (indexGender)
        {
            case 1:
                maleObj.SetActive(true);
                femaleObj.SetActive(false);
                break;
            case 2:
                maleObj.SetActive(false);
                femaleObj.SetActive(true);
                break;
        }
    }
     public void ChooseClass(int indexClass)
    {
        for(int i = 0; i < covers.Length; i++)
        {
            if (indexClass == i)
            {
                covers[i].SetActive(false);
            }
            else
            {
                covers[i].SetActive(true);
            }
        }
        indexOfClass = indexClass;
    }
    public void OnButtonChooseClass()
    {
        FindObjectOfType<LoginController>().ChooseClass(indexOfClass);
    }
}
