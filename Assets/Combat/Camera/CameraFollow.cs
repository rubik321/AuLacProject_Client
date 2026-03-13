using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public CameraType cameraType;

    public void SetFollowTransform(Transform target)
    {
        transform.SetPositionAndRotation(target.position, target.rotation);
    }

    public enum CameraType
    {
        Strategic,
        Focus
    }
}
