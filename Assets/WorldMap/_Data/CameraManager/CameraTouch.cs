using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOA.WorldMap;
using NTFunctions_old;

namespace GOA.WorldMap
{
    public class CameraTouch : LoadBehaviour
    {
        public float panSpeed = 20f;
        public float zoomSpeed = 30f;

        public float minHeigh = 200f;
        public float maxheigh = 1000f;

        public float maxDistanceView = 1000;

        private Touch touch;
        private float zoomModifier = 0.5f;

        protected override void Update()
        {
            // Move camera with touch input
            if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                touch = Input.GetTouch(0);
                Vector3 cameraPosition = transform.position;
                cameraPosition.x -= touch.deltaPosition.x * panSpeed * Time.deltaTime;
                cameraPosition.z -= touch.deltaPosition.y * panSpeed * Time.deltaTime;
                if(Vector3.Distance(new Vector3(cameraPosition.x, 0, cameraPosition.z), new Vector3(GameMaster.instance.player.transform.position.x, 0, GameMaster.instance.player.transform.position.z)) > this.maxDistanceView) return;
                transform.position = cameraPosition;
            }

            // Zoom camera with pinch gesture
            if (Input.touchCount == 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

                float difference = currentMagnitude - prevMagnitude;

                Vector3 cameraPosition = transform.position;
                cameraPosition.y -= difference * zoomSpeed * zoomModifier * Time.deltaTime;
                if(cameraPosition.y > this.maxheigh) cameraPosition.y = this.maxheigh;
                if(cameraPosition.y < this.minHeigh) cameraPosition.y = this.minHeigh;
                transform.position = cameraPosition;
            }
        }
    }
}    
