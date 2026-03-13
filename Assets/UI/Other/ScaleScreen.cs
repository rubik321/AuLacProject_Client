using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScaleScreen : MonoBehaviour
{
    private float scaleValue;
    CanvasScaler ss;
    void Start()
    {
        ss = this.GetComponent<CanvasScaler>();
        ScaleScr();
    }
    void Update()
    {
        ScaleScr();
    }
    public void ScaleScr()
    {
        var x = Screen.width;
        var y = Screen.height;
        scaleValue=((float)x/y)/((float)1920/1080);
        ss.matchWidthOrHeight = (int)(scaleValue);
    }
}
