using System.Threading;
using UnityEngine;
using Sirenix.OdinInspector;
public class ThreadTesting : MonoBehaviour
{
    private Thread _thread;

    [Button("DoThread")]
    public void DoThread()
    {
        _thread = new Thread(DoWork);
        _thread.Start();
    }

    [Button("DoWithoutThread")]
    public void DoWithoutThread()
    {
        DoWork();
    }

    void DoWork()
    {
        // Heavy computation or data processing
        Debug.Log("Doing work in background thread"); // ⚠️ This will cause issues in Unity Editor
        double result = 0;
        for (double i = 0; i < 1000000000; i++)
        {
            result += i;
        }

        // Safely pass data back to main thread (see below)
        _mainThreadResult = result;
        _isResultReady = true;
    }

    public double _mainThreadResult;
    public bool _isResultReady;

    void Update()
    {
        // Safely read result from main thread
        if (_isResultReady)
        {
            Debug.Log($"Result: {_mainThreadResult}");
            _isResultReady = false;
        }
    }

    void OnDestroy()
    {
        _thread?.Abort(); // Not ideal – consider using flags for graceful shutdown
    }
}
