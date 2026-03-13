using UnityEngine;
using System.Collections;

public class TestLocationService : MonoBehaviour
{
    private void Start() {
        int count = 1;
        Debug.LogWarning(count);
        for (int i = 0; i < 100000; i++)
        {
            count += Random.Range(1,3);
        }
        Debug.LogWarning(count);
    }
}