using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecycleOj : MonoBehaviour
{
    public float timeDelay;
    // Start is called before the first frame update
    private void OnEnable()
    {
        
            Invoke("Recycle", timeDelay);
       
    }

    void Recycle()
    {
        ObjectPool.Recycle(gameObject);
    }
}
