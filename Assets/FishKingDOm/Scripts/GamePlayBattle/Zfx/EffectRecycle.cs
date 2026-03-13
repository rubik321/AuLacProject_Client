using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectRecycle : MonoBehaviour
{
    public float timeDelay = 1f;
    private void OnEnable()
    {
        Invoke("Recycle", timeDelay);
    }
    void Recycle()
    {
        gameObject.Recycle();
    }
}
