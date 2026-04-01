using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaceOnPlane : MonoBehaviour
{
    [SerializeField] private GameObject objectToPlace;          // Assign your prefab in Inspector
    [SerializeField] private bool disablePlanesAfterPlacement = true;  // Hide planes after first placement

    private ARRaycastManager arRaycastManager;
    private ARPlaneManager arPlaneManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Awake()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
        arPlaneManager = GetComponent<ARPlaneManager>();

        if (arRaycastManager == null)
            Debug.LogError("ARRaycastManager is missing on XR Origin.");
        if (arPlaneManager == null)
            Debug.LogError("ARPlaneManager is missing on XR Origin.");
    }

    void Update()
    {
        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began)
        {
            // Raycast against planes only
            if (arRaycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = hits[0].pose;

                // Place the object
                GameObject placedObject = Instantiate(objectToPlace, hitPose.position, hitPose.rotation);

                // Anchor it to stay fixed in the real world
                placedObject.AddComponent<ARAnchor>();

                // Optional: hide the plane visualizations after first placement
                if (disablePlanesAfterPlacement)
                {
                    arPlaneManager.SetTrackablesActive(false);
                    arPlaneManager.enabled = false;
                }
            }
        }
    }
}