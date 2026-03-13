using Rubik.Combat;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerTriggerHit : MonoBehaviour
{
    [SerializeField] float delay;
    Action onHit;

    public void SetupData(Action onHit)
    {
        this.onHit = onHit;
        StartCoroutine(IEDelayHit());
    }

    IEnumerator IEDelayHit()
    {
        yield return new WaitForSeconds(delay);
        onHit?.Invoke();
    }
}
