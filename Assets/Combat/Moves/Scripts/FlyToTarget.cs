using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using GOA.UserData;

public class FlyToTarget : MonoBehaviour
{
    public Ease ease;
    public float speed = 1;

    public void SetTarget(Transform target, Action onReachTarget)
    {
        transform.LookAt(target, Vector3.up);
        transform.DOMove(target.position, speed * UserData.Instance.movespeed).SetEase(ease).SetSpeedBased().OnComplete(() =>
        {
            try
            {
                onReachTarget?.Invoke();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        });        
    }    
}
