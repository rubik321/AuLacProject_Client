using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHold : MonoBehaviour
{
    private DateTime _mouseDownTime;

    public void OnMouseDown()
    {
        _mouseDownTime = DateTime.Now;
    }

    public void OnMouseUp()
    {
        if ((DateTime.Now - _mouseDownTime).TotalSeconds > 1)
        {
            // Xử lý sự kiện "hold" của button
        }
        else
        {
            // Xử lý sự kiện "click" của button
        }
    }
}
