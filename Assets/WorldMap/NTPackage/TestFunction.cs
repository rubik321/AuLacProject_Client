using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFunction : MonoBehaviour
{
    [ContextMenu("Test")]

    public void Test(){
        Debug.LogWarning("Now:"+DateTime.Now);
        Debug.LogWarning("Now:"+DateTime.UtcNow);
        double tsU = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        double tsl = (DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        Debug.LogWarning(tsU);
        Debug.LogWarning(tsl);
        Debug.LogWarning(tsl -tsU);
    }
}
