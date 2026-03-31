using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasyARGeoSpatialAnchors
{
    public class CameraLook : MonoBehaviour
    {
        void Update()
        {
            transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
        }
    }
}