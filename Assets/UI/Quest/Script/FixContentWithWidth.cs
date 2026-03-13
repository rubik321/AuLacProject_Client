using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FixContentWithWidth : MonoBehaviour
{
    GridLayoutGroup gripLayout;
    float WidthContent;
    // Start is called before the first frame update
    void Start()
    {
        gripLayout = GetComponent<GridLayoutGroup>();
        WidthContent = gripLayout.cellSize.x;
        Debug.Log("width : " + WidthContent);
        //gripLayout.cellSize = new Vector2(900,396);
    }

  
}
