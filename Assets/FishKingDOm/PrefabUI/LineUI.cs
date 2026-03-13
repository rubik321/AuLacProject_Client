using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineUI : MonoBehaviour
{
    public LineItem[] lsLine;
    public RectTransform content;
    int current = 0;
    float countLeght;
    public void ShowLine(int numberLine,float xLength)
    {
        countLeght = xLength;

        for (int i = 0; i < numberLine; i++)
        {
            lsLine[i].gameObject.SetActive(true);

        }
        lsLine[current].lineOn.SetActive(true);
    }
    public void UpdateLine()
    {
        int index = (int)(Mathf.Abs(content.anchoredPosition.x) / countLeght);
        //Debug.Log("Content currnet : " + Mathf.Abs(content.anchoredPosition.x) + " lenght : "+ countLeght + "    kq   : "+index);
        if (index != current)
        {
            lsLine[index].lineOn.SetActive(true);
            lsLine[current].lineOn.SetActive(false);
            current = index;
        }
        
    }
    public void Update()
    {
        if (content != null)
        {
            UpdateLine();
        }
    }
}
