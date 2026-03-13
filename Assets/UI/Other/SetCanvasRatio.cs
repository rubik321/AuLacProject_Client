using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetCanvasRatio : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
       
        float x = (float)1080 / 1920;
        float y = (float)Screen.width / Screen.height;
        Debug.Log(" Screen size : " + x + " + " + y + " KQ : " + (y/x));
        GetComponent<CanvasScaler>().matchWidthOrHeight = (int)(y/x);
    }
    private string GetAspectRatio(int aScreenWidth, int aScreenHeight)
    {
        float r = (float)aScreenWidth / (float)aScreenHeight;
        string _r = r.ToString("F2");
        string ratio = _r.Substring(0, 4);
        switch (ratio)
        {
            case "2.37":
            case "2.39":
                return ("  21:9");
            case "1.25":
                return ("  5:4");
            case "1.33":
                return ("  4:3");
            case "1.50":
                return ("  3:2");
            case "1.60":
            case "1.56":
                return ("  16:10");
            case "1.67":
            case "1.78":
            case "1.77":
                return ("  16:9");
            case "0.67":
                return ("  2:3");
            case "0.56":
                return ("  9:16");
            default:
                return (string.Empty);
        }
    }
}
   

