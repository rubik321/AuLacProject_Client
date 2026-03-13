using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIResize : MonoBehaviour
{
    float Ratio;
    // Start is called before the first frame update
    void Start()
    {
        float x = (float)1920 / 1080;
        float y = (float)Screen.width / Screen.height ;
        if(x>y)
            Ratio = (y / x);
        else
           Ratio = 1;
       // Debug.Log("tem : " + Ratio);
        transform.localScale = new Vector2(transform.localScale.x * Ratio, transform.localScale.y * Ratio);
    }


}
