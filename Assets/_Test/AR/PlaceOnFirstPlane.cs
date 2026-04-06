using NTPackage.Functions;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace Rubik.AR
{
    public class PlaceOnFirstPlane : MonoBehaviour
    {
        [SerializeField] private CharVR objectToPlace;   // Assign your prefab in Inspector
        [SerializeField] private bool placeOnlyOnce = true;

        public ARPlaneManager planeManager;

        public void GenerateObject(){
            // Get nearest plane
            var nearestPlane = GetNearestPlane(transform.position);
            if (nearestPlane != null)
            {
                PlaceObjectOnPlane(nearestPlane);
            }
        }

        public ARPlane GetNearestPlane(Vector3 position){
            var planes = planeManager.trackables;
            double nearestDistance = double.MaxValue;
            ARPlane nearestPlane = null;
            foreach (var plane in planes)
            {
                double distance = Vector3.Distance(plane.pose.position, position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestPlane = plane;
                }
            }
            return nearestPlane;
        }


        private void PlaceObjectOnPlane(ARPlane plane)
        {
            // Get the center pose of the plane
            Pose centerPose = plane.pose;

            // Randomly position in a Plane
            Vector3 randomPosition = centerPose.position + new Vector3(
                Random.Range(-0.1f, 0.1f),
                Random.Range(-0.1f, 0.1f),
                Random.Range(0, 0)
            );
            // Instantiate the object at that position
            CharVR placedObj = Instantiate(objectToPlace, randomPosition, centerPose.rotation);
            placedObj.placeOnFirstPlane = this;
            // Anchor it so it stays fixed in the real world
            placedObj.gameObject.AddComponent<ARAnchor>();

            // Optionally, disable plane visualization after placement
            // planeManager.SetTrackablesActive(false);
            // planeManager.enabled = false;

            Debug.Log("Object placed on first horizontal plane.");
        }

        public void DestroyObject(CharVR charVR){
            Destroy(charVR.gameObject);
            StartCoroutine(NTFunction.WaitSecond(1, () => {
                this.GenerateObject();
            }));
        }
    }
}